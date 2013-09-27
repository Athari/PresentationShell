using System.Runtime.InteropServices;
using Alba.Interop.WinError;

namespace Alba.Interop.ShellObjects
{
    /// <summary>Exposes a method that obtains an icon index for an IShellFolder object.</summary>
    [ComImport, Guid ("000214E5-0000-0000-C000-000000000046"), InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IShellIcon
    {
        /// <summary>Gets an icon for an object inside a specific folder.</summary>
        /// <param name="pidl">(LPCITEMIDLIST) The address of the ITEMIDLIST structure that specifies the relative location of the folder.</param>
        /// <param name="flags">Flags specifying how the icon is to display. This parameter can be zero or one of the following values: GIL_FORSHELL, GIL_OPENICON.</param>
        /// <param name="pIconIndex">The address of the index of the icon in the system image list.</param>
        [PreserveSig]
        HRESULT GetIconOf ([In] PIDLIST pidl, [In] GILI flags, [Out] out int pIconIndex);
    }
}