using System.Runtime.InteropServices;
using System.Security;
using Alba.Interop.WinError;

namespace Alba.Interop.ShellObjects
{
    /// <summary>Exposes methods that are used by a namespace extension to specify icon overlays for the objects it contains.</summary>
    [ComImport, Guid ("7D688A70-C613-11D0-999B-00C04FD655E1"), InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressUnmanagedCodeSecurity]
    internal interface IShellIconOverlay
    {
        /// <summary>Gets the overlay index in the system image list.</summary>
        /// <param name="pidl">(PCUITEMID_CHILD) Pointer to an ITEMIDLIST structure that identifies the object whose icon is being displayed.</param>
        /// <param name="pIndex">Pointer to a value that states the overlay index (one-based) in the system image list. This index is equivalent to the iOverlay value that is specified when you add an overlay image to a private image list with the ImageList::SetOverlayImage function.</param>
        /// <returns>This method can return one of these values: S_OK (The index of an overlay was found.); S_FALSE	(No overlay exists for this file.); E_FAIL(The PIDL is invalid.); E_INVALIDARG(The argument is invalid, for example, if pIndex is NULL.); E_PENDING(The calling application passed OI_ASYNC to signify that the operation of calculating the overlay index will take some time.).</returns>
        /// <remarks>To retrieve the overlay index in the system image list, call SHGetIconOverlayIndex.<br/>
        /// If you set pIndex to point to OI_ASYNC when you call this method, the Shell icon overlay handler might return E_PENDING instead of storing the overlay index in pIndex. This return value indicates that computing the overlay is a slow operation and should be handled in the background. When an IShellIconOverlay implementation returns E_PENDING, it is called back on a background worker thread without the OI_ASYNC flag. If you do not use OI_ASYNC when you call GetOverlayIndex, the overlay handler must compute the overlay index and store the value in pIndex before returning.</remarks>
        [PreserveSig]
        HRESULT GetOverlayIndex ([In] PIDLIST pidl, [In, Out] ref int pIndex);
        /// <summary>Gets the index of the icon overlay in the system image list.</summary>
        /// <param name="pidl">(PCUITEMID_CHILD) Pointer to an ITEMIDLIST structure that identifies the object whose icon is being displayed.</param>
        /// <param name="pIconIndex">Pointer to the index of the icon overlay's image in the system image list. This index is equivalent to the iImage value that is specified when you add an overlay image to a private image list with the ImageList::SetOverlayImage function.</param>
        /// <remarks>This method can return one of these values: S_OK(The index of an overlay was found.); S_FALSE(No overlay exists for this file.); E_FAIL(The PIDL is invalid.).</remarks>
        /// <remarks>To retrieve the overlay's image index in the system image list, you must first call SHGetIconOverlayIndex to retrieve the overlay index. Then use the INDEXTOOVERLAYMASK macro to convert the overlay index into the equivalent image index.</remarks>
        [PreserveSig]
        HRESULT GetOverlayIconIndex ([In] PIDLIST pidl, [In, Out] ref int pIconIndex);
    }
}