using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Alba.Diagnostics;
using Alba.Framework.Diagnostics;
using Alba.Framework.Text;
using Alba.Interop.WinError;

// ReSharper disable StaticFieldInGenericType
namespace Alba.Interop
{
    internal abstract class NativeComInterface<TCom> : IDisposable
        where TCom : class
    {
        protected static readonly ILog Log = new Log<NativeComInterface<TCom>>(AlbaPresentaionShellTraceSources.Interop);

        protected TCom Com { get; private set; }

        protected NativeComInterface (TCom com)
        {
            Com = com;
        }

        public TComOther QueryInterface<TComOther> () where TComOther : class
        {
            var com = Com as TComOther;
            if (com != null)
                RuntimeCallWrapperAddRef(com);
            return com;
        }

        protected static bool LogIfFailed (HRESULT hr, [CallerMemberName] string methodName = null)
        {
            Exception e = hr.GetException();
            if (e == null)
                return true;
            Log.Error("Call to {0}.{1} failed.".Fmt(typeof(TCom).Name, methodName), e);
            return false;
        }

        ~NativeComInterface ()
        {
            Dispose(false);
        }

        public void Dispose ()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose (bool disposing)
        {
            if (Com == null)
                return;
            Marshal.ReleaseComObject(Com);
            Com = null;
        }

        private static void RuntimeCallWrapperAddRef<T> (T com) where T : class
        {
            Marshal.GetObjectForIUnknown(Marshal.GetIUnknownForObject(com));
        }
    }
}