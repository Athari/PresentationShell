using System;

namespace Alba.Interop.ShellObjects
{
    /// <summary>Attributes that can be retrieved on an item (file or folder) or set of items.</summary>
    [Flags]
    internal enum SFGAO : uint
    {
        /// <summary>The specified items can be copied.</summary>
        CANCOPY = 0x00000001,
        /// <summary>The specified items can be moved.</summary>
        CANMOVE = 0x00000002,
        /// <summary>Shortcuts can be created for the specified items. This attribute has the same value as DROPEFFECT_LINK.<br/>
        /// If a namespace extension returns this attribute, a Create Shortcut entry with a default handler is added to the shortcut menu that is displayed during drag-and-drop operations. The extension can also implement its own handler for the link verb in place of the default. If the extension does so, it is responsible for creating the shortcut.<br/>
        /// A Create Shortcut item is also added to the Windows Explorer File menu and to normal shortcut menus.<br/>
        /// If the item is selected, your application's IContextMenu::InvokeCommand method is invoked with the lpVerb member of the CMINVOKECOMMANDINFO structure set to link. Your application is responsible for creating the link.</summary>
        CANLINK = 0x00000004,
        /// <summary>The specified items can be bound to an IStorage object through IShellFolder::BindToObject. For more information about namespace manipulation capabilities, see IStorage.</summary>
        STORAGE = 0x00000008,
        /// <summary>The specified items can be renamed. Note that this value is essentially a suggestion; not all namespace clients allow items to be renamed. However, those that do must have this attribute set.</summary>
        CANRENAME = 0x00000010,
        /// <summary>The specified items can be deleted.</summary>
        CANDELETE = 0x00000020,
        /// <summary>The specified items have property sheets.</summary>
        HASPROPSHEET = 0x00000040,
        /// <summary>The specified items are drop targets.</summary>
        DROPTARGET = 0x00000100,
        /// <summary>This flag is a mask for the capability attributes: <see cref="CANCOPY"/>, <see cref="CANMOVE"/>, <see cref="CANLINK"/>, <see cref="CANRENAME"/>, <see cref="CANDELETE"/>, <see cref="HASPROPSHEET"/>, and <see cref="DROPTARGET"/>. Callers normally do not use this value.</summary>
        CAPABILITYMASK = 0x00000177,
        /// <summary>Windows 7 and later. The specified items are system items.</summary>
        SYSTEM = 0x00001000,
        /// <summary>The specified items are encrypted and might require special presentation.</summary>
        ENCRYPTED = 0x00002000,
        /// <summary>Accessing the item (through IStream or other storage interfaces) is expected to be a slow operation. Applications should avoid accessing items flagged with <see cref="ISSLOW"/>.</summary>
        /// <remarks>Opening a stream for an item is generally a slow operation at all times. <see cref="ISSLOW"/> indicates that it is expected to be especially slow, for example in the case of slow network connections or offline (FILE_ATTRIBUTE_OFFLINE) files. However, querying <see cref="ISSLOW"/> is itself a slow operation. Applications should query <see cref="ISSLOW"/> only on a background thread. An alternate method, such as retrieving the PKEY_FileAttributes property and testing for FILE_ATTRIBUTE_OFFLINE, could be used in place of a method call that involves <see cref="ISSLOW"/>.</remarks>
        ISSLOW = 0x00004000,
        /// <summary>The specified items are shown as dimmed and unavailable to the user.</summary>
        GHOSTED = 0x00008000,
        /// <summary>The specified items are shortcuts.</summary>
        LINK = 0x00010000,
        /// <summary>The specified objects are shared.</summary>
        SHARE = 0x00020000,
        /// <summary>The specified items are read-only. In the case of folders, this means that new items cannot be created in those folders. This should not be confused with the behavior specified by the FILE_ATTRIBUTE_READONLY flag retrieved by IColumnProvider::GetItemData in a SHCOLUMNDATA structure. FILE_ATTRIBUTE_READONLY has no meaning for Win32 file system folders.</summary>
        READONLY = 0x00040000,
        /// <summary>The item is hidden and should not be displayed unless the Show hidden files and folders option is enabled in Folder Settings.</summary>
        HIDDEN = 0x00080000,
        /// <summary>Do not use.</summary>
        DISPLAYATTRMASK = 0x000FC000,
        /// <summary>The specified folders are either file system folders or contain at least one descendant (child, grandchild, or later) that is a file system (<see cref="FILESYSTEM"/>) folder.</summary>
        FILESYSANCESTOR = 0x10000000,
        /// <summary>The specified items are folders. Some items can be flagged with both <see cref="STREAM"/> and <see cref="FOLDER"/>, such as a compressed file with a .zip file name extension. Some applications might include this flag when testing for items that are both files and containers.</summary>
        FOLDER = 0x20000000,
        /// <summary>The specified folders or files are part of the file system (that is, they are files, directories, or root directories). The parsed names of the items can be assumed to be valid Win32 file system paths. These paths can be either UNC or drive-letter based.</summary>
        FILESYSTEM = 0x40000000,
        /// <summary>The specified folders have subfolders. The <see cref="HASSUBFOLDER"/> attribute is only advisory and might be returned by Shell folder implementations even if they do not contain subfolders. Note, however, that the converse—failing to return <see cref="HASSUBFOLDER"/>—definitively states that the folder objects do not have subfolders.<br/>
        /// Returning <see cref="HASSUBFOLDER"/> is recommended whenever a significant amount of time is required to determine whether any subfolders exist. For example, the Shell always returns <see cref="HASSUBFOLDER"/> when a folder is located on a network drive.</summary>
        HASSUBFOLDER = 0x80000000,
        /// <summary>This flag is a mask for content attributes, at present only <see cref="HASSUBFOLDER"/>. Callers normally do not use this value.</summary>
        CONTENTSMASK = 0x80000000,
        /// <summary>When specified as input, <see cref="VALIDATE"/> instructs the folder to validate that the items contained in a folder or Shell item array exist. If one or more of those items do not exist, IShellFolder::GetAttributesOf and IShellItemArray::GetAttributes return a failure code. This flag is never returned as an [out] value.<br/>
        /// When used with the file system folder, <see cref="VALIDATE"/> instructs the folder to discard cached properties retrieved by clients of IShellFolder2::GetDetailsEx that might have accumulated for the specified items.</summary>
        VALIDATE = 0x01000000,
        /// <summary>The specified items are on removable media or are themselves removable devices.</summary>
        REMOVABLE = 0x02000000,
        /// <summary>The specified items are compressed.</summary>
        COMPRESSED = 0x04000000,
        /// <summary>The specified items can be hosted inside a web browser or Windows Explorer frame.</summary>
        BROWSABLE = 0x08000000,
        /// <summary>The items are nonenumerated items and should be hidden. They are not returned through an enumerator such as that created by the IShellFolder::EnumObjects method.</summary>
        NONENUMERATED = 0x00100000,
        /// <summary>The items contain new content, as defined by the particular application.</summary>
        NEWCONTENT = 0x00200000,
        /// <summary>Not supported.</summary>
        CANMONIKER = 0x00400000,
        /// <summary>Not supported.</summary>
        HASSTORAGE = 0x00400000,
        /// <summary>Indicates that the item has a stream associated with it. That stream can be accessed through a call to IShellFolder::BindToObject or IShellItem::BindToHandler with IID_IStream in the riid parameter.</summary>
        STREAM = 0x00400000,
        /// <summary>Children of this item are accessible through IStream or IStorage. Those children are flagged with <see cref="STORAGE"/> or <see cref="STREAM"/>.</summary>
        STORAGEANCESTOR = 0x00800000,
        /// <summary>This flag is a mask for the storage capability attributes: <see cref="STORAGE"/>, <see cref="LINK"/>, <see cref="READONLY"/>, <see cref="STREAM"/>, <see cref="STORAGEANCESTOR"/>, <see cref="FILESYSANCESTOR"/>, <see cref="FOLDER"/>, and <see cref="FILESYSTEM"/>. Callers normally do not use this value.</summary>
        STORAGECAPMASK = 0x70C50008,
        /// <summary>Mask used by the PKEY_SFGAOFlags property to determine attributes that are considered to cause slow calculations or lack context: <see cref="ISSLOW"/>, <see cref="READONLY"/>, <see cref="HASSUBFOLDER"/>, and <see cref="VALIDATE"/>. Callers normally do not use this value.</summary>
        PKEYSFGAOMASK = 0x81044000,
    }
}