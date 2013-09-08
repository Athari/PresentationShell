using System;
using System.Runtime.InteropServices;
using Alba.Interop.CommCtrl;
using Alba.Interop.WinDef;

namespace Alba.Interop.CommonControls
{
    using HBITMAP = IntPtr;
    using HICON = IntPtr;
    using HWND = IntPtr;
    using HDC = IntPtr;

    [ComImport, Guid ("46EB5926-582E-4017-9FDF-E8998DAA0950"), InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IImageList
    {
        void Add ([In] ref HBITMAP hbmImage, [In] ref HBITMAP hbmMask, out int pi);
        void ReplaceIcon (int i, [In] ref HICON hicon, out int pi);
        void SetOverlayImage (int iImage, int iOverlay);
        void Replace (int i, [In] ref HBITMAP hbmImage, [In] ref HBITMAP hbmMask);
        void AddMasked ([In] ref HBITMAP hbmImage, uint crMask, out int pi);
        void Draw ([In] ref IMAGELISTDRAWPARAMS pimldp);
        void Remove (int i);
        void GetIcon (int i, ILD flags, out IntPtr picon);
        void GetImageInfo (int i, out IMAGEINFO pImageInfo);
        void Copy (int iDst, [In, MarshalAs (UnmanagedType.IUnknown)] object punkSrc, int iSrc, uint uFlags);
        void Merge (int i1, [In, MarshalAs (UnmanagedType.IUnknown)] object punk2, int i2, int dx, int dy, [MarshalAs (UnmanagedType.LPStruct)] Guid riid, out IntPtr ppv);
        void Clone ([MarshalAs (UnmanagedType.LPStruct)] Guid riid, out IntPtr ppv);
        void GetImageRect (int i, out RECT prc);
        void GetIconSize (out int cx, out int cy);
        void SetIconSize (int cx, int cy);
        void GetImageCount (out int pi);
        void SetImageCount (uint uNewCount);
        void SetBkColor (uint clrBk, out uint pclr);
        void GetBkColor (out uint pclr);
        void BeginDrag (int iTrack, int dxHotspot, int dyHotspot);
        void EndDrag ();
        void DragEnter ([In] ref HWND hwndLock, int x, int y);
        void DragLeave ([In] ref HWND hwndLock);
        void DragMove (int x, int y);
        void SetDragCursorImage ([In, MarshalAs (UnmanagedType.IUnknown)] object punk, int iDrag, int dxHotspot, int dyHotspot);
        void DragShowNolock (int fShow);
        void GetDragImage (out POINT ppt, out POINT pptHotspot, [MarshalAs (UnmanagedType.LPStruct)] Guid riid, out IntPtr ppv);
        void GetItemFlags (int i, out uint dwFlags);
        void GetOverlayImage (int iOverlay, out int piIndex);
    }
}