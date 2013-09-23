using System;

namespace Alba.Interop.ShellObjects
{
    /// <summary>Determines the types of items included in an enumeration. These values are used with the IShellFolder::EnumObjects method.</summary>
    [Flags]
    internal enum SHCONTF : uint
    {
        /// <summary>Windows 7 and later. The calling application is checking for the existence of child items in the folder.</summary>
        CHECKING_FOR_CHILDREN = 0x10,
        /// <summary>Include items that are folders in the enumeration.</summary>
        FOLDERS = 0x20,
        /// <summary>Include items that are not folders in the enumeration.</summary>
        NONFOLDERS = 0x40,
        /// <summary>Include hidden items in the enumeration. This does not include hidden system items. (To include hidden system items, use <see cref="INCLUDESUPERHIDDEN"/>.)</summary>
        INCLUDEHIDDEN = 0x80,
        /// <summary>No longer used; always assumed. IShellFolder::EnumObjects can return without validating the enumeration object. Validation can be postponed until the first call to IEnumIDList::Next. Use this flag when a user interface might be displayed prior to the first IEnumIDList::Next call. For a user interface to be presented, hwnd must be set to a valid window handle.</summary>
        INIT_ON_FIRST_NEXT = 0x100,
        /// <summary>The calling application is looking for printer objects.</summary>
        NETPRINTERSRCH = 0x200,
        /// <summary>The calling application is looking for resources that can be shared.</summary>
        SHAREABLE = 0x400,
        /// <summary>Include items with accessible storage and their ancestors, including hidden items.</summary>
        STORAGE = 0x800,
        /// <summary>Windows 7 and later. Child folders should provide a navigation enumeration.</summary>
        NAVIGATION_ENUM = 0x1000,
        /// <summary>Windows Vista and later. The calling application is looking for resources that can be enumerated quickly.</summary>
        FASTITEMS = 0x2000,
        /// <summary>Windows Vista and later. Enumerate items as a simple list even if the folder itself is not structured in that way.</summary>
        FLATLIST = 0x4000,
        /// <summary>Windows Vista and later. The calling application is monitoring for change notifications. This means that the enumerator does not have to return all results. Items can be reported through change notifications.</summary>
        ENABLE_ASYNC = 0x8000,
        /// <summary>Windows 7 and later. Include hidden system items in the enumeration. This value does not include hidden non-system items. (To include hidden non-system items, use <see cref="INCLUDEHIDDEN"/>.)</summary>
        INCLUDESUPERHIDDEN = 0x10000
    };
}