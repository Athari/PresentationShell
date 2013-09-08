using System;

namespace Alba.Interop.CommCtrl
{
    [Flags]
    internal enum ILD : uint
    {
        /// <summary>Draws the image using the background color for the image list. If the background color is the CLR_NONE value, the image is drawn transparently using the mask.</summary>
        NORMAL = 0x00000000,
        /// <summary>Draws the image transparently using the mask, regardless of the background color. This value has no effect if the image list does not contain a mask.</summary>
        TRANSPARENT = 0x00000001,
        /// <summary>Draws the image, blending 25 percent with the system highlight color. This value has no effect if the image list does not contain a mask.</summary>
        BLEND25 = 0x00000002,
        /// <summary>Same as <see cref="BLEND25"/>.</summary>
        FOCUS = 0x00000002,
        /// <summary>Draws the image, blending 50 percent with the system highlight color. This value has no effect if the image list does not contain a mask.</summary>
        BLEND50 = 0x00000004,
        /// <summary>Same as <see cref="BLEND50"/>.</summary>
        SELECTED = 0x00000004,
        /// <summary>Same as <see cref="BLEND50"/>.</summary>
        BLEND = 0x00000004,
        /// <summary>Draws the mask.</summary>
        MASK = 0x00000010,
        /// <summary>Set this flag if the overlay does not require a mask to be drawn. This flag causes ImageList_DrawEx to draw just the image, ignoring the mask.</summary>
        IMAGE = 0x00000020,
        /// <summary>Draws the image using the raster operation code specified by the dwRop member.</summary>
        ROP = 0x00000040,
        /// <summary>To extract the overlay image from the fStyle member, use the logical AND to combine fStyle with the ILD_OVERLAYMASK value.</summary>
        OVERLAYMASK = 0x00000F00,
        /// <summary>Preserves the alpha channel in the destination.</summary>
        PRESERVEALPHA = 0x00001000,
        /// <summary>Causes the image to be scaled to cx, cy instead of being clipped.</summary>
        SCALE = 0x00002000,
        /// <summary>Scales the image to the current dpi of the display.</summary>
        DPISCALE = 0x00004000,
        /// <summary>Windows Vista and later. Draw the image if it is available in the cache. Do not extract it automatically. The called draw method returns E_PENDING to the calling component, which should then take an alternative action, such as, provide another image and queue a background task to force the image to be loaded via ForceImagePresent using the ILFIP_ALWAYS flag. The ILD_ASYNC flag then prevents the extraction operation from blocking the current thread and is especially important if a draw method is called from the user interface (UI) thread.</summary>
        ASYNC = 0x00008000,
    }
}