using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Alba.Interop.WinError;

namespace Alba.Interop.ShellObjects
{
    using HICON = IntPtr;

    /// <summary>Exposes methods that allow a client to retrieve the icon that is associated with one of the objects in a folder.</summary>
    [ComImport, Guid ("000214fa-0000-0000-c000-000000000046"), InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressUnmanagedCodeSecurity]
    internal interface IExtractIcon
    {
        /// <summary>Gets the location and index of an icon.</summary>
        /// <param name="uFlags">One or more of the GILI values.</param>
        /// <param name="szIconFile">A pointer to a buffer that receives the icon location. The icon location is a null-terminated string that identifies the file that contains the icon.</param>
        /// <param name="cchMax">The size of the buffer, in characters, pointed to by pszIconFile.</param>
        /// <param name="piIndex">A pointer to an int that receives the index of the icon in the file pointed to by pszIconFile.</param>
        /// <param name="pwFlags">A pointer to a UINT value that receives zero or a combination of the GILR values.</param>
        /// <returns>Returns S_OK if the function returned a valid location, or S_FALSE if the Shell should use a default icon. If the GIL_ASYNC flag is set in uFlags, the method can return E_PENDING to indicate that icon extraction will be time-consuming.</returns>
        /// <remarks>When a client sets the GIL_ASYNC flag in uFlags and receives E_PENDING as a return value, it typically creates a background thread to extract the icon. It calls GetIconLocation from that thread, without the GIL_ASYNC flag, to retrieve the icon location. It then calls IExtractIcon::Extract to extract the icon. Returning E_PENDING implies that the object is free threaded. In other words, it can safely be called concurrently by multiple threads.<br/>
        /// The GIL_DEFAULTICON flag is usually set in the case where the desired icon is found, but that icon is not present in the icon cache. Icon extraction is a low priority background process, and as such may be delayed by other processes. The default icon will be displayed in place of the final icon during the time that it takes for that final icon to be extracted, added to the cache, and made available.</remarks>
        [PreserveSig]
        HRESULT GetIconLocation ([In] GILI uFlags, [Out, MarshalAs (UnmanagedType.LPWStr, SizeParamIndex = 2)] StringBuilder szIconFile,
            [In] int cchMax, [Out] out int piIndex, [Out] out GILR pwFlags);
        /// <summary>Extracts an icon image from the specified location.</summary>
        /// <param name="pszFile">A pointer to a null-terminated string that specifies the icon location.</param>
        /// <param name="nIconIndex">The index of the icon in the file pointed to by pszFile.</param>
        /// <param name="phiconLarge">A pointer to an HICON value that receives the handle to the large icon. This parameter may be NULL.</param>
        /// <param name="phiconSmall">A pointer to an HICON value that receives the handle to the small icon. This parameter may be NULL.</param>
        /// <param name="nIconSize">The desired size of the icon, in pixels. The low word contains the size of the large icon, and the high word contains the size of the small icon. The size specified can be the width or height. The width of an icon always equals its height.</param>
        /// <returns>Returns S_OK if the function extracted the icon, or S_FALSE if the calling application should extract the icon.</returns>
        /// <remarks>The icon location and index are the same values returned by the IExtractIcon::GetIconLocation method. If IExtractIcon::Extract function returns S_FALSE, these values must specify an icon file name and index that form legal parameters for a call to ExtractIcon. If IExtractIcon::Extract does not return S_FALSE, no assumptions should be made about the meanings of the pszFile and nIconIndex parameters.</remarks>
        [PreserveSig]
        unsafe HRESULT Extract ([In, MarshalAs (UnmanagedType.LPWStr)] string pszFile, [In] int nIconIndex, [Out] IntPtr* phiconLarge, [Out] IntPtr* phiconSmall, [In] uint nIconSize);
    }
}