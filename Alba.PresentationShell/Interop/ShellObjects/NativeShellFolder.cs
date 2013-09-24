using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Alba.Interop.ShellTypes;
using Alba.Interop.WinError;

namespace Alba.Interop.ShellObjects
{
    using HWND = IntPtr;

    internal class NativeShellFolder : IDisposable
    {
        private IShellFolder _iShellFolder;

        public NativeShellFolder (IShellFolder iShellFolder)
        {
            _iShellFolder = iShellFolder;
        }

        public static NativeShellFolder GetDesktopFolder ()
        {
            return new NativeShellFolder(Native.SHGetDesktopFolder());
        }

        public PIDLIST ParseDisplayName (HWND hwnd, string displayName, ref uint pchEaten, ref SFGAO attrs)
        {
            PIDLIST pidl;
            _iShellFolder.ParseDisplayName(hwnd, null, displayName, ref pchEaten, out pidl, ref attrs);
            return pidl;
        }

        public PIDLIST ParseDisplayName (HWND hwnd, string displayName)
        {
            uint pchEaten = 0;
            SFGAO pdwAttributes = 0;
            return ParseDisplayName(hwnd, displayName, ref pchEaten, ref pdwAttributes);
        }

        public IEnumerable<PIDLIST> EnumObjects (HWND hwnd, SHCONTF flags)
        {
            IEnumIDList enumIdList;
            HRESULT hr = _iShellFolder.EnumObjects(hwnd, flags, out enumIdList);
            if (hr == HRESULT.S_FALSE)
                yield break;
            hr.ThrowIfFailed();
            if (enumIdList == null)
                yield break;
            using (var enumerator = new NativeEnumIDList(enumIdList))
                while (enumerator.MoveNext())
                    yield return enumerator.Current;
        }

        public T BindToObject<T> (PIDLIST pidl)
        {
            object obj;
            _iShellFolder.BindToObject(pidl, null, typeof(T).GUID, out obj);
            return (T)obj;
        }

        public T BindToStorage<T> (PIDLIST pidl)
        {
            object obj;
            _iShellFolder.BindToStorage(pidl, null, typeof(T).GUID, out obj);
            return (T)obj;
        }

        public int CompareIDs (PIDLIST pidl1, PIDLIST pidl2, int lParam = 0)
        {
            HRESULT hr = _iShellFolder.CompareIDs(lParam, pidl1, pidl2);
            hr.ThrowIfFailed();
            return hr.Code;
        }

        public T CreateViewObject<T> (HWND hwndOwner)
        {
            object obj;
            _iShellFolder.CreateViewObject(hwndOwner, typeof(T).GUID, out obj);
            return (T)obj;
        }

        public SFGAO GetAttributesOf (PIDLIST[] pidls, SFGAO attrs)
        {
            _iShellFolder.GetAttributesOf(pidls.Length, pidls, ref attrs);
            return attrs;
        }

        public T GetUIObjectOf<T> (HWND hwndOwner, PIDLIST[] pidls)
        {
            uint reserved = 0;
            object obj;
            _iShellFolder.GetUIObjectOf(hwndOwner, pidls.Length, pidls, typeof(T).GUID, ref reserved, out obj);
            return (T)obj;
        }

        public string GetDisplayNameOf (PIDLIST pidl, SHGDN uFlags)
        {
            STRRET name;
            _iShellFolder.GetDisplayNameOf(pidl, uFlags, out name);
            return Native.StrRetToBuf(ref name, pidl);
        }

        public PIDLIST SetNameOf (HWND hwnd, PIDLIST pidl, string displayName, SHGDN flags)
        {
            PIDLIST pidlOut;
            _iShellFolder.SetNameOf(hwnd, pidl, displayName, flags, out pidlOut);
            return pidlOut;
        }

        ~NativeShellFolder ()
        {
            Dispose(false);
        }

        public void Dispose ()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose (bool disposing)
        {
            if (_iShellFolder == null)
                return;
            Marshal.ReleaseComObject(_iShellFolder);
            _iShellFolder = null;
        }
    }
}