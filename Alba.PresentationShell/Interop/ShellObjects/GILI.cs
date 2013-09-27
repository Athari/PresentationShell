using System;

namespace Alba.Interop.ShellObjects
{
    /// <summary>GetIconLocation() input flags (GIL).</summary>
    [Flags]
    internal enum GILI : uint
    {
        /// <summary>The icon is in the open state if both open-state and closed-state images are available. If this flag is not specified, the icon is in the normal or closed state. This flag is typically used for folder objects.</summary>
        OPENICON = 0x0001,
        /// <summary>The icon is displayed in a Shell folder.</summary>
        FORSHELL = 0x0002,
        /// <summary>Set this flag to determine whether the icon should be extracted asynchronously. If the icon can be extracted rapidly, this flag is usually ignored. If extraction will take more time, GetIconLocation should return E_PENDING. See the Remarks for further discussion.</summary>
        ASYNC = 0x0020,
        /// <summary>Retrieve information about the fallback icon. Fallback icons are usually used while the desired icon is extracted and added to the cache.</summary>
        DEFAULTICON = 0x0040,
        /// <summary>The icon indicates a shortcut. However, the icon extractor should not apply the shortcut overlay; that will be done later. Shortcut icons are state-independent.</summary>
        FORSHORTCUT = 0x0080,
        /// <summary>Explicitly return either GIL_SHIELD or GIL_FORCENOSHIELD in pwFlags. Do not block if GIL_ASYNC is set.</summary>
        CHECKSHIELD = 0x0200,
    }
}