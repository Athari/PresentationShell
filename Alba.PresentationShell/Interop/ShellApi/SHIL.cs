﻿namespace Alba.Interop.ShellApi
{
    internal enum SHIL
    {
        /// <summary>The image size is normally 32x32 pixels. However, if the Use large icons option is selected from the Effects section of the Appearance tab in Display Properties, the image is 48x48 pixels.</summary>
        LARGE = 0x0,
        /// <summary>These images are the Shell standard small icon size of 16x16, but the size can be customized by the user.</summary>
        SMALL = 0x1,
        /// <summary>These images are the Shell standard extra-large icon size. This is typically 48x48, but the size can be customized by the user.</summary>
        EXTRALARGE = 0x2,
        /// <summary>These images are the size specified by GetSystemMetrics called with SM_CXSMICON and GetSystemMetrics called with SM_CYSMICON.</summary>
        SYSSMALL = 0x3,
        /// <summary>Windows Vista and later. The image is normally 256x256 pixels.</summary>
        JUMBO = 0x4,
        /// <summary>The largest valid flag value, for validation purposes.</summary>
        LAST = 0x4,
    }
}