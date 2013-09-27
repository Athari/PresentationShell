using System;

namespace Alba.Interop.ShellObjects
{
    /// <summary>GetIconLocation() return flags (GIL).</summary>
    [Flags]
    internal enum GILR : uint
    {
        /// <summary>The calling application should create a document icon using the specified icon.</summary>
        SIMULATEDOC = 0x0001,
        /// <summary>Each object of this class has its own icon. This flag is used internally by the Shell to handle cases like Setup.exe, where objects with identical names can have different icons. Typical implementations of IExtractIcon do not require this flag.</summary>
        PERINSTANCE = 0x0002,
        /// <summary>All objects of this class have the same icon. This flag is used internally by the Shell. Typical implementations of IExtractIcon do not require this flag because the flag implies that an icon handler is not required to resolve the icon on a per-object basis. The recommended method for implementing per-class icons is to register a DefaultIcon for the class.</summary>
        PERCLASS = 0x0004,
        /// <summary>The location is not a file name/index pair. The values in pszIconFile and piIndex cannot be passed to ExtractIcon or ExtractIconEx.<br/>
        /// When this flag is omitted, the value returned in pszIconFile is a fully-qualified path name to either a .ico file or to a file that can contain icons. Also, the value returned in piIndex is an index into that file that identifies which of its icons to use. Therefore, when the GIL_NOTFILENAME flag is omitted, these values can be passed to ExtractIcon or ExtractIconEx.</summary>
        NOTFILENAME = 0x0008,
        /// <summary>The physical image bits for this icon are not cached by the calling application.</summary>
        DONTCACHE = 0x0010,
        /// <summary>Windows Vista only. The calling application must stamp the icon with the UAC shield.</summary>
        SHIELD = 0x0200,
        /// <summary>Windows Vista only. The calling application must not stamp the icon with the UAC shield.</summary>
        FORCENOSHIELD = 0x0400,
    }
}