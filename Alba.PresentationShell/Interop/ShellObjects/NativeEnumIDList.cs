using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Alba.Interop.WinError;

namespace Alba.Interop.ShellObjects
{
    internal class NativeEnumIDList : IEnumerator<PIDLIST>, ICloneable
    {
        private IEnumIDList _enumIdList;
        private PIDLIST _currentPidl;

        public NativeEnumIDList (IEnumIDList enumIdList)
        {
            _enumIdList = enumIdList;
        }

        public bool MoveNext ()
        {
            uint fetched;
            HRESULT hr = _enumIdList.Next(1, out _currentPidl, out fetched);
            if (hr == HRESULT.S_FALSE)
                return false;
            hr.ThrowIfFailed();
            return fetched == 1;
        }

        public void Reset ()
        {
            _enumIdList.Reset();
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
            _enumIdList.Clone(out clone);
            return clone;
        }

        ~NativeEnumIDList ()
        {
            Dispose(false);
        }

        public void Dispose ()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose (bool disposing)
        {
            if (_enumIdList == null)
                return;
            Marshal.ReleaseComObject(_enumIdList);
            _enumIdList = null;
        }
    }
}