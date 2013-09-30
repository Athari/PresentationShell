using System.Runtime.InteropServices;
using Alba.Interop.WinError;

namespace Alba.Interop.ShellObjects
{
    internal class NativeShellIcon : NativeComInterface<IShellIcon>
    {
        public NativeShellIcon (IShellIcon com) : base(com)
        {}

        public int GetIconOf ([In] PIDLIST pidl, [In] GILI flags)
        {
            int iconIndex;
            HRESULT hr = Com.GetIconOf(pidl, flags, out iconIndex);
            if (hr == HRESULT.S_FALSE)
                return -1;
            hr.ThrowIfFailed();
            return iconIndex;
        }
    }
}