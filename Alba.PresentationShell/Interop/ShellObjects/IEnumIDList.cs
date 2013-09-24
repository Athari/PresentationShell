using System.Runtime.InteropServices;
using Alba.Interop.WinError;

namespace Alba.Interop.ShellObjects
{
    /// <summary>Exposes a standard set of methods used to enumerate the pointers to item identifier lists (PIDLs) of the items in a Shell folder. When a folder's IShellFolder::EnumObjects method is called, it creates an enumeration object and passes a pointer to the object's IEnumIDList interface back to the calling application.</summary>
    [ComImport, Guid ("000214F2-0000-0000-C000-000000000046"), InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IEnumIDList
    {
        /// <summary>Retrieves the specified number of item identifiers in the enumeration sequence and advances the current position by the number of items retrieved.</summary>
        /// <param name="celt">The number of elements in the array referenced by the rgelt parameter.</param>
        /// <param name="rgelt">(PITEMIDLIST) The address of a pointer to an array of ITEMIDLIST pointers that receive the item identifiers. The implementation must allocate these item identifiers using CoTaskMemAlloc. The calling application is responsible for freeing the item identifiers using CoTaskMemFree.<br/>
        /// The ITEMIDLIST structures returned in the array are relative to the IShellFolder being enumerated.</param>
        /// <param name="pceltFetched">A pointer to a value that receives a count of the item identifiers actually returned in rgelt. The count can be smaller than the value specified in the celt parameter. This parameter can be NULL on entry only if celt = 1, because in that case the method can only retrieve one (S_OK) or zero (S_FALSE) items.</param>
        /// <returns>Returns S_OK if the method successfully retrieved the requested celt elements. This method only returns S_OK if the full count of requested items are successfully retrieved.<br/>
        /// S_FALSE indicates that more items were requested than remained in the enumeration. The value pointed to by the pceltFetched parameter specifies the actual number of items retrieved. Note that the value will be 0 if there are no more items to retrieve.<br/>
        /// Returns a COM-defined error value otherwise.</returns>
        /// <remarks>If this method returns a Component Object Model (COM) error code (as determined by the FAILED macro), then no entries in the rgelt array are valid on exit. If this method returns a success code (such as S_OK or S_FALSE), then the ULONG pointed to by the pceltFetched parameter determines how many entries in the rgelt array are valid on exit.<br/>
        /// The distinction is important in the case where celt > 1. For example, if you pass celt=10 and there are only 3 elements left, *pceltFetched will be 3 and the method will return S_FALSE meaning that you reached the end of the file. The three fetched elements will be stored into rgelt and are valid.</remarks>
        [PreserveSig]
        HRESULT Next ([In] uint celt, [Out] out PIDLIST rgelt, out uint pceltFetched);
        /// <summary>Skips the specified number of elements in the enumeration sequence.</summary>
        /// <param name="celt">The number of item identifiers to skip.</param>
        void Skip ([In] uint celt);
        /// <summary>Returns to the beginning of the enumeration sequence.</summary>
        void Reset ();
        /// <summary>Creates a new item enumeration object with the same contents and state as the current one.</summary>
        /// <param name="ppenum">The address of a pointer to the new enumeration object. The calling application must eventually free the new object by calling its Release member function.</param>
        /// <remarks>This method makes it possible to record a particular point in the enumeration sequence and then return to that point at a later time.</remarks>
        void Clone (out IEnumIDList ppenum);
    }
}