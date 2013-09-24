using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using Alba.Framework.Text;

namespace Alba.Interop.WinError
{
    // Based on MS.Internal.Interop.Win32Error from WindowsBase
    [StructLayout (LayoutKind.Sequential)]
    internal struct HRESULT
    {
        private readonly int _value;

        public static readonly HRESULT S_OK = new HRESULT(0);
        public static readonly HRESULT S_FALSE = new HRESULT(1);
        /*public static readonly HRESULT E_NOTIMPL = new HRESULT(2147500033);
        public static readonly HRESULT E_NOINTERFACE = new HRESULT(2147500034);
        public static readonly HRESULT E_POINTER = new HRESULT(2147500035);
        public static readonly HRESULT E_ABORT = new HRESULT(2147500036);
        public static readonly HRESULT E_FAIL = new HRESULT(2147500037);
        public static readonly HRESULT E_UNEXPECTED = new HRESULT(2147549183);
        public static readonly HRESULT E_ACCESSDENIED = new HRESULT(2147942405);
        public static readonly HRESULT E_OUTOFMEMORY = new HRESULT(2147942414);
        public static readonly HRESULT E_INVALIDARG = new HRESULT(2147942487);*/

        public HRESULT (int value)
        {
            _value = value;
        }

        public HRESULT (uint value)
        {
            _value = unchecked ((int)value);
        }

        public int Code
        {
            get { return GetCode(_value); }
        }

        public FACILITY Facility
        {
            get { return GetFacility(_value); }
        }

        public bool IsFailed
        {
            get { return _value < 0; }
        }

        public bool IsSucceeded
        {
            get { return _value >= 0; }
        }

        public Exception GetException (string message = null)
        {
            if (!IsFailed)
                return null;
            Exception ehr = Marshal.GetExceptionForHR(_value, new IntPtr(-1));
            if (ehr.GetType() == typeof(COMException)) {
                if (Facility == FACILITY.WIN32)
                    return message.IsNullOrEmpty() ? new Win32Exception(Code) : new Win32Exception(Code, message);
                return new COMException(message ?? ehr.Message, _value);
            }
            if (!string.IsNullOrEmpty(message)) {
                ConstructorInfo constructor = ehr.GetType().GetConstructor(new[] { typeof(string) });
                if (constructor != null)
                    ehr = constructor.Invoke(new object[] { message }) as Exception;
            }
            return ehr;
        }


        public void ThrowIfFailed (string message = null)
        {
            Exception exception = GetException(message);
            if (exception != null)
                throw exception;
        }

        public override string ToString ()
        {
            foreach (FieldInfo field in typeof(HRESULT).GetFields(BindingFlags.Public | BindingFlags.Static))
                if (field.FieldType == typeof(HRESULT) && this == (HRESULT)field.GetValue(null))
                    return field.Name;
            /*if (Facility == FACILITY.WIN32) {
                foreach (FieldInfo info in typeof(Win32Error).GetFields(BindingFlags.Public | BindingFlags.Static))
                    if (info.FieldType == typeof(Win32Error) && this == (HRESULT)(Win32Error)info.GetValue(null))
                        return ("HRESULT_FROM_WIN32(" + info.Name + ")");
            }
            return string.Format(CultureInfo.InvariantCulture, "0x{0:X8}", _value);*/
            return "0x{0:X8}: {1}".FmtInv(_value, GetException().Message);
        }

        public bool Equals (HRESULT other)
        {
            return _value == other._value;
        }

        public override bool Equals (object obj)
        {
            return !ReferenceEquals(obj, null) && obj is HRESULT && Equals((HRESULT)obj);
        }

        public override int GetHashCode ()
        {
            return _value;
        }

        public static bool operator == (HRESULT left, HRESULT right)
        {
            return left.Equals(right);
        }

        public static bool operator != (HRESULT left, HRESULT right)
        {
            return !left.Equals(right);
        }

        public static int GetCode (int error)
        {
            return error & 0xffff;
        }

        public static FACILITY GetFacility (int error)
        {
            // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
            return (FACILITY)(error >> 16) & (FACILITY)0x1fff;
        }

        public static HRESULT Make (bool severe, FACILITY facility, int code)
        {
            return new HRESULT((severe ? int.MinValue : 0) | ((int)facility << 16) | code);
        }
    }
}