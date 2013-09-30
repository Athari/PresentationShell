using Alba.Interop.WinError;

namespace Alba.Interop.ShellObjects
{
    internal class NativeShellIconOverlay : NativeComInterface<IShellIconOverlay>
    {
        public const int NoOverlay = -1;
        public const int AsyncOverlay = -2;

        public NativeShellIconOverlay (IShellIconOverlay com, bool own = true) : base(com, own)
        {}

        public int GetOverlayIndex (PIDLIST pidl, bool allowAsync = false)
        {
            int overlayIndex = allowAsync ? OI.ASYNC : 0;
            HRESULT hr = Com.GetOverlayIndex(pidl, ref overlayIndex);
            if (hr == HRESULT.E_PENDING)
                return AsyncOverlay;
            return LogIfFailed(hr) ? overlayIndex : NoOverlay;
        }

        public int GetOverlayIconIndex (PIDLIST pidl)
        {
            int overlayIndex = 0;
            HRESULT hr = Com.GetOverlayIconIndex(pidl, ref overlayIndex);
            return LogIfFailed(hr) ? overlayIndex : NoOverlay;
        }
    }
}