using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using Alba.Interop.ShellObjects;

namespace Alba.Interop
{
    internal partial class Native
    {
        /// <summary>Appends or prepends an SHITEMID structure to an ITEMIDLIST structure.</summary>
        /// <param name="pidl">(PIDLIST_RELATIVE) A pointer to an ITEMIDLIST structure. When the function returns, the SHITEMID structure specified by pmkid is appended or prepended.</param>
        /// <param name="pmkid">(PSHITEMID) A pointer to a SHITEMID structure to be appended or prepended to pidl.</param>
        /// <param name="fAppend">Value that is set to TRUE to append pmkid to pidl. Set this value to FALSE to prepend pmkid to pidl.</param>
        /// <returns>(PIDLIST_RELATIVE) Returns the ITEMIDLIST structure specified by pidl, with pmkid appended or prepended. Returns NULL on failure.</returns>
        [DllImport (Dll.Shell)]
        internal static extern PIDLIST ILAppendID ([In] PIDLIST pidl, [In] PIDLIST pmkid, [In] bool fAppend);

        /// <summary>Clones an ITEMIDLIST structure.</summary>
        /// <param name="pidl">(PCUIDLIST_RELATIVE) A pointer to the ITEMIDLIST structure to be cloned.</param>
        /// <returns>Returns a pointer to a copy of the ITEMIDLIST structure pointed to by pidl.</returns>
        [DllImport (Dll.Shell)]
        internal static extern PIDLIST ILClone ([In] PIDLIST pidl);

        /// <summary>Clones the first SHITEMID structure in an ITEMIDLIST structure.</summary>
        /// <param name="pidl">(PCUIDLIST_RELATIVE) A pointer to the ITEMIDLIST structure that you want to clone.</param>
        /// <returns>(PITEMID_CHILD) A pointer to an ITEMIDLIST structure that contains the first SHITEMID structure from the ITEMIDLIST structure specified by pidl. Returns NULL on failure.</returns>
        [DllImport (Dll.Shell)]
        internal static extern PIDLIST ILCloneFirst ([In] PIDLIST pidl);

        /// <summary>Combines two ITEMIDLIST structures.</summary>
        /// <param name="pidl1">(PCIDLIST_ABSOLUTE) A pointer to the first ITEMIDLIST structure.</param>
        /// <param name="pidl2">(PCUIDLIST_RELATIVE) A pointer to the second ITEMIDLIST structure. This structure is appended to the structure pointed to by pidl1.</param>
        /// <returns>(PIDLIST_ABSOLUTE) Returns an ITEMIDLIST containing the combined structures. If you set either pidl1 or pidl2 to NULL, the returned ITEMIDLIST structure is a clone of the non-NULL parameter. Returns NULL if pidl1 and pidl2 are both set to NULL.</returns>
        [DllImport (Dll.Shell)]
        internal static extern PIDLIST ILCombine ([In] PIDLIST pidl1, [In] PIDLIST pidl2);

        /// <summary>Returns the ITEMIDLIST structure associated with a specified file path.</summary>
        /// <param name="pszPath">A pointer to a null-terminated Unicode string that contains the path. This string should be no more than MAX_PATH characters in length, including the terminating null character.</param>
        /// <returns>(PIDLIST_ABSOLUTE) Returns a pointer to an ITEMIDLIST structure that corresponds to the path.</returns>
        [DllImport (Dll.Shell, CharSet = CharSet.Auto)]
        internal static extern PIDLIST ILCreateFromPath ([In] string pszPath);

        /// <summary>Determines whether a specified ITEMIDLIST structure is the child of another ITEMIDLIST structure.</summary>
        /// <param name="pidlParent">(PCIDLIST_ABSOLUTE) A pointer to the parent ITEMIDLIST structure.</param>
        /// <param name="pidlChild">(PCIDLIST_ABSOLUTE) A pointer to the child ITEMIDLIST structure.</param>
        /// <returns>(PUIDLIST_RELATIVE) Returns a pointer to the child's simple ITEMIDLIST structure if pidlChild is a child of pidlParent. The returned structure consists of pidlChild, minus the SHITEMID structures that make up pidlParent. Returns NULL if pidlChild is not a child of pidlParent.</returns>
        /// <remarks>The returned pointer is a pointer into the existing parent structure. It is an alias for pidlChild. No new memory is allocated in association with the returned pointer. It is not the caller's responsibility to free the returned value.</remarks>
        [DllImport (Dll.Shell)]
        internal static extern PIDLIST ILFindChild ([In] PIDLIST pidlParent, [In] PIDLIST pidlChild);

        /// <summary>Returns a pointer to the last SHITEMID structure in an ITEMIDLIST structure.</summary>
        /// <param name="pidl">(PCUIDLIST_RELATIVE) A pointer to an ITEMIDLIST structure.</param>
        /// <returns>(PUITEMID_CHILD) A pointer to the last SHITEMID structure in pidl.</returns>
        /// <remarks>This function does not clone the last item, so you do not have to call ILFree to release the returned pointer.</remarks>
        [DllImport (Dll.Shell)]
        internal static extern PIDLIST ILFindLastID ([In] PIDLIST pidl);

        /// <summary>Frees an ITEMIDLIST structure allocated by the Shell.</summary>
        /// <param name="pidl">A pointer to the ITEMIDLIST structure to be freed. This parameter can be NULL.</param>
        /// <remarks>ILFree is often used with ITEMIDLIST structures allocated by one of the other IL functions, but it can be used to free any such structure returned by the Shell—for example, the ITEMIDLIST structure returned by SHBrowseForFolder or used in a call to SHGetFolderLocation.<br/>
        /// When using Windows 2000 or later, use CoTaskMemFree rather than ILFree. ITEMIDLIST structures are always allocated with the Component Object Model (COM) task allocator on those platforms.</remarks>
        [DllImport (Dll.Shell)]
        internal static extern void ILFree ([In] PIDLIST pidl);

        /// <summary>Retrieves the next SHITEMID structure in an ITEMIDLIST structure.</summary>
        /// <param name="pidl">(PCUIDLIST_RELATIVE) A pointer to a particular SHITEMID structure in a larger ITEMIDLIST structure.</param>
        /// <returns>(PCUIDLIST_RELATIVE) Returns a pointer to the SHITEMID structure that follows the one specified by pidl. Returns NULL if pidl points to the last SHITEMID structure.</returns>
        [DllImport (Dll.Shell)]
        internal static extern PIDLIST ILGetNext ([In] PIDLIST pidl);

        /// <summary>Returns the size, in bytes, of an ITEMIDLIST structure.</summary>
        /// <param name="pidl">(PCUIDLIST_RELATIVE) A pointer to an ITEMIDLIST structure.</param>
        /// <returns>The size of the ITEMIDLIST structure specified by pidl, in bytes.</returns>
        [DllImport (Dll.Shell)]
        internal static extern int ILGetSize ([In] PIDLIST pidl);

        /// <summary>Tests whether two ITEMIDLIST structures are equal in a binary comparison.</summary>
        /// <param name="pidl1">(PCIDLIST_ABSOLUTE) The first ITEMIDLIST structure.</param>
        /// <param name="pidl2">(PCIDLIST_ABSOLUTE) The second ITEMIDLIST structure.</param>
        /// <returns>Returns TRUE if the two structures are equal, FALSE otherwise.</returns>
        /// <remarks>ILIsEqual performs a binary comparison of the item data. It is possible for two ITEMIDLIST structures to differ at the binary level while referring to the same item. IShellFolder::CompareIDs should be used to perform a non-binary comparison.</remarks>
        [DllImport (Dll.Shell)]
        internal static extern bool ILIsEqual ([In] PIDLIST pidl1, [In] PIDLIST pidl2);

        /// <summary>Tests whether an ITEMIDLIST structure is the parent of another ITEMIDLIST structure.</summary>
        /// <param name="pidl1">A pointer to an ITEMIDLIST (PIDL) structure that specifies the parent. This must be an absolute PIDL.</param>
        /// <param name="pidl2">A pointer to an ITEMIDLIST (PIDL) structure that specifies the child. This must be an absolute PIDL.</param>
        /// <param name="fImmediate">A Boolean value that is set to TRUE to test for immediate parents of pidl2, or FALSE to test for any parents of pidl2.</param>
        /// <returns>Returns TRUE if pidl1 is a parent of pidl2. If fImmediate is set to TRUE, the function only returns TRUE if pidl1 is the immediate parent of pidl2. Otherwise, the function returns FALSE.</returns>
        [DllImport (Dll.Shell)]
        internal static extern bool ILIsParent ([In] PIDLIST pidl1, [In] PIDLIST pidl2, [In] bool fImmediate);

        /// <summary>Removes the last SHITEMID structure from an ITEMIDLIST structure.</summary>
        /// <param name="pidl">A pointer to the ITEMIDLIST structure to be shortened. When the function returns, this variable points to the shortened structure.</param>
        /// <returns>Returns TRUE if successful, FALSE otherwise.</returns>
        [DllImport (Dll.Shell)]
        internal static extern bool ILRemoveLastID ([In, Out] ref PIDLIST pidl);

        [DllImport (Dll.Shell, PreserveSig = false)]
        internal static extern void ILSaveToStream ([In] IStream pstm, [In] PIDLIST pidl);

        [DllImport (Dll.Shell, PreserveSig = false)]
        private static extern void SHGetDesktopFolder ([Out] out IShellFolder ppshf);

        /// <summary>Clones a full, or absolute, ITEMIDLIST structure.</summary>
        /// <param name="pidl">(PCUIDLIST_ABSOLUTE) A pointer to the full, or absolute, ITEMIDLIST structure to be cloned.</param>
        /// <returns>(PCUIDLIST_ABSOLUTE) A pointer to a copy of the ITEMIDLIST structure pointed to by pidl.</returns>
        internal static PIDLIST ILCloneFull ([In] PIDLIST pidl)
        {
            return ILClone(pidl);
        }

        /// <summary>Clones a child ITEMIDLIST structure.</summary>
        /// <param name="pidl">(PCUITEMID_CHILD) A pointer to the child ITEMIDLIST structure to be cloned.</param>
        /// <returns>(PCUITEMID_CHILD) A pointer to a copy of the child ITEMIDLIST structure pointed to by pidl.</returns>
        internal static PIDLIST ILCloneChild ([In] PIDLIST pidl)
        {
            return ILCloneFirst(pidl);
        }

        /// <summary>Verifies whether a pointer to an item identifier list (PIDL) is a child PIDL, which is a PIDL with exactly one SHITEMID.</summary>
        /// <param name="pidl">(PCUIDLIST_RELATIVE) A constant, unaligned, relative PIDL that is being checked.</param>
        /// <returns>Returns TRUE if the given PIDL is a child PIDL; otherwise, FALSE.</returns>
        /// <remarks>This function does not guarantee that the PIDL is non-NULL or non-empty.</remarks>
        internal static bool ILIsChild ([In] PIDLIST pidl)
        {
            return ILIsEmpty(pidl) || ILIsEmpty(ILNext(pidl));
        }

        /// <summary>Verifies whether an ITEMIDLIST structure is empty.</summary>
        /// <param name="pidl">(PCUID_RELATIVE) A pointer to the ITEMIDLIST structure to be checked.</param>
        /// <returns>TRUE if the pidl parameter is NULL or the ITEMIDLIST structure pointed to by pidl is empty; otherwise FALSE.</returns>
        internal static bool ILIsEmpty ([In] PIDLIST pidl)
        {
            return pidl.Handle == IntPtr.Zero || pidl.Cb == 0;
        }

        /// <summary>Retrieves the next SHITEMID structure in an ITEMIDLIST structure.</summary>
        /// <param name="pidl">(PCUIDLIST_RELATIVE | PUIDLIST_RELATIVE) A constant, unaligned, relative PIDL for which the next SHITEMID structure is being retrieved.</param>
        /// <returns>(PCUIDLIST_RELATIVE | PUIDLIST_RELATIVE) When this function returns, contains one of three results: If pidl is valid and not the last SHITEMID in the ITEMIDLIST, then it contains a pointer to the next ITEMIDLIST structure. If the last ITEMIDLIST structure is passed, it contains NULL, which signals the end of the PIDL. For other values of pidl, the return value is meaningless.</returns>
        /// <remarks>To verify if the return value is NULL, use ILIsEmpty.</remarks>
        internal static PIDLIST ILNext ([In] PIDLIST pidl)
        {
            return ILSkip(pidl, pidl.Cb);
        }

        /// <summary>Skips a given number of bytes in a constant, unaligned, relative ITEMIDLIST structure.</summary>
        /// <param name="pidl">(PCUIDLIST_RELATIVE | PUIDLIST_RELATIVE) A constant, unaligned, relative PIDL in which bytes are to be skipped.</param>
        /// <param name="cb">The number of bytes to skip.</param>
        /// <returns>(PCUIDLIST_RELATIVE | PUIDLIST_RELATIVE) When this function returns, if pidl and cb are valid, contains a constant pointer to the ITEMIDLIST structure that results after the skip. Otherwise, the value is meaningless.</returns>
        internal static PIDLIST ILSkip ([In] PIDLIST pidl, int cb)
        {
            return new PIDLIST(IntPtr.Add(pidl.Handle, cb));
        }

        internal static IShellFolder SHGetDesktopFolder ()
        {
            IShellFolder ppshf;
            SHGetDesktopFolder(out ppshf);
            return ppshf;
        }
    }
}