using System;
using System.Runtime.InteropServices;
using System.Text;
using Alba.Interop.WinError;

namespace Alba.Interop.ShellObjects
{
    using HICON = IntPtr;

    internal class NativeExtractIcon : IDisposable
    {
        private IExtractIcon _extractIcon;

        public NativeExtractIcon (IExtractIcon extractIcon)
        {
            _extractIcon = extractIcon;
        }

        public bool? GetIconLocation (GILI inFlags, out string iconFile, out int iconIndex, out GILR resFlags)
        {
            var sb = new StringBuilder(Native.MAX_PATH);
            HRESULT hr = _extractIcon.GetIconLocation(inFlags, sb, sb.Capacity, out iconIndex, out resFlags);
            iconFile = sb.ToString();
            if (hr == HRESULT.S_FALSE)
                return false;
            else if (hr == HRESULT.E_PENDING)
                return null;
            hr.ThrowIfFailed();
            return true;
        }

        public unsafe bool Extract (string iconFile, int iconIndex, ushort iconSizeLarge, ushort iconSizeSmall,
            out HICON hiconLarge, out HICON hiconSmall)
        {
            fixed (HICON* piconLarge = &hiconLarge)
            fixed (HICON* piconSmall = &hiconSmall)
                return Extract(iconFile, iconIndex, iconSizeLarge, iconSizeSmall, piconLarge, piconSmall);
        }

        public unsafe bool Extract (string iconFile, int iconIndex, ushort iconSize, out HICON hicon)
        {
            fixed (HICON* picon = &hicon)
                return iconSize > 16
                    ? Extract(iconFile, iconIndex, iconSize, 0, picon, null)
                    : Extract(iconFile, iconIndex, 0, iconSize, null, picon);
        }

        private unsafe bool Extract (string iconFile, int iconIndex, ushort iconSizeLarge, ushort iconSizeSmall,
            HICON* hiconLarge, HICON* hiconSmall)
        {
            HRESULT hr = _extractIcon.Extract(iconFile, iconIndex, hiconLarge, hiconSmall,
                Native.MakeLong(iconSizeLarge, iconSizeSmall));
            if (hr == HRESULT.S_FALSE)
                return false;
            hr.ThrowIfFailed();
            return true;
        }

        ~NativeExtractIcon ()
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
            if (_extractIcon == null)
                return;
            Marshal.ReleaseComObject(_extractIcon);
            _extractIcon = null;
        }
    }
}