using System;

namespace Alba.Interop.WinUser
{
    /// <summary>LoadImage(), CopyImage(), CreateIconFromResource() etc. flags.</summary>
    [Flags]
    internal enum LR : uint
    {
        /// <summary>The default flag; it does nothing. All it means is "not LR_MONOCHROME".</summary>
        DEFAULTCOLOR = 0x00000000,
        /// <summary>Loads/creates the image in black and white.</summary>
        MONOCHROME = 0x00000001,
        /// <summary>Undocumented.</summary>
        COLOR = 0x00000002,
        /// <summary>CopyImage: Returns the original hImage if it satisfies the criteria for the copy—that is, correct dimensions and color depth—in which case the LR_COPYDELETEORG flag is ignored. If this flag is not specified, a new object is always created.</summary>
        COPYRETURNORG = 0x00000004,
        /// <summary>CopyImage: Deletes the original image after creating the copy.</summary>
        COPYDELETEORG = 0x00000008,
        /// <summary>LoadImage: Loads the stand-alone image from the file specified by lpszName (icon, cursor, or bitmap file).</summary>
        LOADFROMFILE = 0x00000010,
        /// <summary>LoadImage: Retrieves the color value of the first pixel in the image and replaces the corresponding entry in the color table with the default window color (COLOR_WINDOW). All pixels in the image that use that entry become the default window color. This value applies only to images that have corresponding color tables.<br/>
        /// Do not use this option if you are loading a bitmap with a color depth greater than 8bpp.<br/>
        /// If fuLoad includes both the LR_LOADTRANSPARENT and LR_LOADMAP3DCOLORS values, LR_LOADTRANSPARENT takes precedence. However, the color table entry is replaced with COLOR_3DFACE rather than COLOR_WINDOW.</summary>
        LOADTRANSPARENT = 0x00000020,
        /// <summary>LoadImage: Uses the width or height specified by the system metric values for cursors or icons, if the cxDesired or cyDesired values are set to zero. If this flag is not specified and cxDesired and cyDesired are set to zero, the function uses the actual resource size. If the resource contains multiple images, the function uses the size of the first image.</summary>
        DEFAULTSIZE = 0x00000040,
        /// <summary>LoadImage: Uses true VGA colors.</summary>
        VGACOLOR = 0x00000080,
        /// <summary>LoadImage: Searches the color table for the image and replaces the following shades of gray with the corresponding 3-D color: Dk Gray, Gray, Lt Gray. Do not use this option if you are loading a bitmap with a color depth greater than 8bpp.</summary>
        LOADMAP3DCOLORS = 0x00001000,
        /// <summary>When the uType parameter specifies IMAGE_BITMAP, causes the function to return a DIB section bitmap rather than a compatible bitmap. This flag is useful for loading a bitmap without mapping it to the colors of the display device.</summary>
        CREATEDIBSECTION = 0x00002000,
        /// <summary>CopyImage: Tries to reload an icon or cursor resource from the original resource file rather than simply copying the current image. This is useful for creating a different-sized copy when the resource file contains multiple sizes of the resource. Without this flag, CopyImage stretches the original image to the new size. If this flag is set, CopyImage uses the size in the resource file closest to the desired size. This will succeed only if hImage was loaded by LoadIcon or LoadCursor, or by LoadImage with the LR_SHARED flag.</summary>
        COPYFROMRESOURCE = 0x00004000,
        /// <summary>LoadImage: Shares the image handle if the image is loaded multiple times. If LR_SHARED is not set, a second call to LoadImage for the same resource will load the image again and return a different handle. When you use this flag, the system will destroy the resource when it is no longer needed.<br/>
        /// Do not use LR_SHARED for images that have non-standard sizes, that may change after loading, or that are loaded from a file.<br/>
        /// When loading a system icon or cursor, you must use LR_SHARED or the function will fail to load the resource.<br/>
        /// This function finds the first image in the cache with the requested resource name, regardless of the size requested.</summary>
        SHARED = 0x00008000,
    }
}