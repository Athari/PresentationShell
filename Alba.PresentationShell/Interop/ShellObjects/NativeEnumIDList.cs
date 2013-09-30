using System;
using System.Collections;
using System.Collections.Generic;
using Alba.Interop.WinError;

namespace Alba.Interop.ShellObjects
{
    internal class NativeEnumIDList : NativeComInterface<IEnumIDList>, IEnumerator<PIDLIST>, ICloneable
    {
        private PIDLIST _currentPidl;

        public NativeEnumIDList (IEnumIDList com, bool own = true) : base(com, own)
        {}

        public bool MoveNext ()
        {
            uint fetched;
            HRESULT hr = Com.Next(1, out _currentPidl, out fetched);
            if (hr == HRESULT.S_FALSE)
                return false;
            hr.ThrowIfFailed();
            return fetched == 1;
        }

        public void Reset ()
        {
            Com.Reset();
        }

        public PIDLIST Current
        {
            get { return _currentPidl; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public object Clone ()
        {
            IEnumIDList clone;
            Com.Clone(out clone);
            return clone;
        }
    }
}