using System;
using System.Runtime.InteropServices;
using Alba.Interop.WinError;

namespace Alba.Interop.ShellObjects
{
    internal class NativeShellIcon : IDisposable
    {
        private IShellIcon _shellIcon;

        public NativeShellIcon (IShellIcon shellIcon)
        {
            _shellIcon = shellIcon;
        }

        public int GetIconOf ([In] PIDLIST pidl, [In] GILI flags)
        {
            int iconIndex;
            HRESULT hr = _shellIcon.GetIconOf(pidl, flags, out iconIndex);
            if (hr == HRESULT.S_FALSE)
                return -1;
            hr.ThrowIfFailed();
            return iconIndex;
        }

        ~NativeShellIcon ()
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
            if (_shellIcon == null)
                return;
            Marshal.ReleaseComObject(_shellIcon);
            _shellIcon = null;
        }
    }
}