namespace Alba.Interop.CommCtrl
{
    internal enum ILS : uint
    {
        /// <summary>The image state is not modified.</summary>
        NORMAL = 0x00000000,
        /// <summary>Not supported.</summary>
        GLOW = 0x00000001,
        /// <summary>Not supported.</summary>
        SHADOW = 0x00000002,
        /// <summary>Reduces the color saturation of the icon to grayscale. This only affects 32bpp images.</summary>
        SATURATE = 0x00000004,
        /// <summary>Alpha blends the icon. Alpha blending controls the transparency level of an icon, according to the value of its alpha channel. The value of the alpha channel is indicated by the frame member in the IMAGELISTDRAWPARAMS method. The alpha channel can be from 0 to 255, with 0 being completely transparent, and 255 being completely opaque.</summary>
        ALPHA = 0x00000008,
    }
}