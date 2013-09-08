﻿using System;
using System.Runtime.InteropServices;
using Alba.Interop.CommonControls;
using Alba.Interop.ShellApi;

namespace Alba.Interop
{
    internal partial class Native
    {
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

        [DllImport (Dll.Shell, EntryPoint = "#727")]
        private static extern int SHGetImageList (SHIL iImageList, [MarshalAs (UnmanagedType.LPStruct)] Guid riid, [MarshalAs (UnmanagedType.IUnknown)] out object ppv);

        [DllImport (Dll.Shell, EntryPoint = "#727")]
        private static extern int SHGetImageList (SHIL iImageList, [MarshalAs (UnmanagedType.LPStruct)] Guid riid, out IImageList ppv);

        public static int SHGetFileInfo_IconIndex (string fileName)
        {
            var info = new SHFILEINFO();
            SHGetFileInfo(fileName, 0, ref info, Marshal.SizeOf(info), SHGFI.SYSICONINDEX);
            return info.iIcon;
        }

        public static IImageList SHGetImageList (SHIL iImageList)
        {
            IImageList ppv;
            SHGetImageList(iImageList, typeof(IImageList).GUID, out ppv);
            return (IImageList)ppv;
        }
    }
}