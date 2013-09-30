using System;
using System.IO;
using System.Runtime.InteropServices;
using Alba.Framework.Text;
using Alba.Interop.CommonControls;
using Alba.Interop.ShellApi;
using Alba.Interop.ShellObjects;
using Alba.Interop.WinUser;

// TODO Useful functions: SHGetStockIconInfo (folder icon parts)
// TODO Useful "deprecated" functions: PickIconDlg, SHExtractIconsW, SHGetSetFolderCustomSettings, SHGetSetSettings, IColumnProvider
namespace Alba.Interop
{
    using HICON = IntPtr;

    internal partial class Native
    {
        [DllImport (Dll.User, CharSet = CharSet.Auto)]
        private static extern int PrivateExtractIcons (string lpszFile, int nIconIndex, int cxIcon, int cyIcon, out HICON phicon, IntPtr piconid, uint nIcons, LR flags);

        /// <summary>Retrieves information about an object in the file system, such as a file, folder, directory, or drive root.</summary>
        /// <param name="pszPath">A pointer to a null-terminated string of maximum length MAX_PATH that contains the path and file name. Both absolute and relative paths are valid.<br/>
        /// If the uFlags parameter includes the SHGFI_PIDL flag, this parameter must be the address of an ITEMIDLIST (PIDL) structure that contains the list of item identifiers that uniquely identifies the file within the Shell's namespace. The PIDL must be a fully qualified PIDL. Relative PIDLs are not allowed.<br/>
        /// If the uFlags parameter includes the SHGFI_USEFILEATTRIBUTES flag, this parameter does not have to be a valid file name. The function will proceed as if the file exists with the specified name and with the file attributes passed in the dwFileAttributes parameter. This allows you to obtain information about a file type by passing just the extension for pszPath and passing FILE_ATTRIBUTE_NORMAL in dwFileAttributes.</param>
        /// <param name="dwFileAttributes">A combination of one or more file attribute flags (FILE_ATTRIBUTE_ values as defined in Winnt.h). If uFlags does not include the SHGFI_USEFILEATTRIBUTES flag, this parameter is ignored.</param>
        /// <param name="psfi">Pointer to a SHFILEINFO structure to receive the file information.</param>
        /// <param name="cbFileInfo">The size, in bytes, of the SHFILEINFO structure pointed to by the psfi parameter.</param>
        /// <param name="uFlags">The flags that specify the file information to retrieve.</param>
        /// <returns>Returns a value whose meaning depends on the uFlags parameter.<br/>
        /// If uFlags does not contain SHGFI_EXETYPE or SHGFI_SYSICONINDEX, the return value is nonzero if successful, or zero otherwise.<br/>
        /// If uFlags contains the SHGFI_EXETYPE flag, the return value specifies the type of the executable file. It will be one of the following values.</returns>
        [DllImport (Dll.Shell, CharSet = CharSet.Auto)]
        private static extern IntPtr SHGetFileInfo (string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, int cbFileInfo, SHGFI uFlags);

        [DllImport (Dll.Shell, CharSet = CharSet.Auto)]
        private static extern IntPtr SHGetFileInfo (PIDLIST pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, int cbFileInfo, SHGFI uFlags);

        [DllImport (Dll.Shell)]
        private static extern int SHGetImageList (SHIL iImageList, [MarshalAs (UnmanagedType.LPStruct)] Guid riid, [MarshalAs (UnmanagedType.IUnknown)] out object ppv);

        public static void PrivateExtractIcons (string lpszFile, int nIconIndex, int cxIcon, out HICON phicon, LR flags)
        {
            int res = PrivateExtractIcons(lpszFile, nIconIndex, cxIcon, cxIcon, out phicon, IntPtr.Zero, 1, flags);
            if (res == -1)
                throw new FileNotFoundException("Could not load icon from '{0}'.".Fmt(lpszFile), lpszFile);
            if (res != 1)
                phicon = IntPtr.Zero;
        }

        public static SHFILEINFO SHGetFileInfo (PIDLIST pidl, SHGFI flags)
        {
            var info = new SHFILEINFO();
            SHGetFileInfo(pidl, 0, ref info, Marshal.SizeOf(info), SHGFI.PIDL | flags);
            return info;
        }

        public static IImageList SHGetImageList (SHIL iImageList)
        {
            object ppv;
            SHGetImageList(iImageList, typeof(IImageList).GUID, out ppv);
            return (IImageList)ppv;
        }
    }
}