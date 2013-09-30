using Alba.Interop.WinError;
using Alba.Windows.Shell;

// IShellIconOverlay fails like crazy, so just ignore errors to avoid spamming log and incurring performance cost.
namespace Alba.Interop.ShellObjects
{
    internal class NativeShellIconOverlay : NativeComInterface<IShellIconOverlay>
    {
        public NativeShellIconOverlay (IShellIconOverlay com) : base(com)
        {}

        public int GetOverlayIndex (PIDLIST pidl, bool allowAsync = false)
        {
            int overlayIndex = allowAsync ? OI.ASYNC : 0;
            HRESULT hr = Com.GetOverlayIndex(pidl, ref overlayIndex);
            if (hr == HRESULT.E_PENDING)
                return ShellItem.AsyncIconIndex;
            return hr.IsSucceeded ? overlayIndex : ShellItem.NoIconIndex;
        }

        public int GetOverlayIconIndex (PIDLIST pidl)
        {
            int overlayIndex = 0;
            HRESULT hr = Com.GetOverlayIconIndex(pidl, ref overlayIndex);
            return hr.IsSucceeded ? overlayIndex : ShellItem.NoIconIndex;
        }
    }
}