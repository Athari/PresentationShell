using System;
using System.Runtime.InteropServices;
using Alba.Interop.WinError;

namespace Alba.Interop.ShellObjects
{
    internal class NativeShellIconOverlay : IDisposable
    {
        public const int NoOverlay = -1;
        public const int AsyncOverlay = -2;

        private IShellIconOverlay _shellIconOverlay;

        public NativeShellIconOverlay (IShellIconOverlay shellIconOverlay)
        {
            _shellIconOverlay = shellIconOverlay;
        }

        public int GetOverlayIndex (PIDLIST pidl, bool allowAsync = false)
        {
            int overlayIndex = allowAsync ? OI.ASYNC : 0;
            HRESULT hr = _shellIconOverlay.GetOverlayIndex(pidl, ref overlayIndex);
            if (hr == HRESULT.S_FALSE)
                return NoOverlay;
            if (hr == HRESULT.E_PENDING)
                return AsyncOverlay;
            hr.ThrowIfFailed();
            return overlayIndex;
        }

        public int GetOverlayIconIndex (PIDLIST pidl)
        {
            int overlayIndex = 0;
            HRESULT hr = _shellIconOverlay.GetOverlayIconIndex(pidl, ref overlayIndex);
            if (hr == HRESULT.S_FALSE)
                return NoOverlay;
            hr.ThrowIfFailed();
            return overlayIndex;
        }

        ~NativeShellIconOverlay ()
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
            if (_shellIconOverlay == null)
                return;
            Marshal.ReleaseComObject(_shellIconOverlay);
            _shellIconOverlay = null;
        }
    }
}