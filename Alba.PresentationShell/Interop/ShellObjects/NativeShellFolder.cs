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
        private IShellFolder _shellFolder;

        public NativeShellFolder (IShellFolder shellFolder)
        {
            _shellFolder = shellFolder;
        }

        public static NativeShellFolder GetDesktopFolder ()
        {
            return new NativeShellFolder(Native.SHGetDesktopFolder());
        }

        public T QueryInterface<T> () where T : class
        {
            return _shellFolder as T;
        }

        public PIDLIST ParseDisplayName (string displayName, ref uint pchEaten, ref SFGAO attrs, HWND hwnd)
        {
            PIDLIST pidl;
            _shellFolder.ParseDisplayName(hwnd, null, displayName, ref pchEaten, out pidl, ref attrs);
            return pidl;
        }

        public PIDLIST ParseDisplayName (string displayName, HWND hwnd)
        {
            uint pchEaten = 0;
            SFGAO pdwAttributes = 0;
            return ParseDisplayName(displayName, ref pchEaten, ref pdwAttributes, hwnd);
        }

        public PIDLIST ParseDisplayName (string displayName)
        {
            return ParseDisplayName(displayName, IntPtr.Zero);
        }

        public IEnumerable<PIDLIST> EnumObjects (HWND hwnd, SHCONTF flags)
        {
            IEnumIDList enumIdList;
            HRESULT hr = _shellFolder.EnumObjects(hwnd, flags, out enumIdList);
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
            _shellFolder.BindToObject(pidl, null, typeof(T).GUID, out obj);
            return (T)obj;
        }

        public T BindToStorage<T> (PIDLIST pidl)
        {
            object obj;
            _shellFolder.BindToStorage(pidl, null, typeof(T).GUID, out obj);
            return (T)obj;
        }

        public int CompareIDs (PIDLIST pidl1, PIDLIST pidl2, int lParam = 0)
        {
            HRESULT hr = _shellFolder.CompareIDs(lParam, pidl1, pidl2);
            hr.ThrowIfFailed();
            return hr.Code;
        }

        public T CreateViewObject<T> (HWND hwndOwner)
        {
            object obj;
            _shellFolder.CreateViewObject(hwndOwner, typeof(T).GUID, out obj);
            return (T)obj;
        }

        public SFGAO GetAttributesOf (PIDLIST[] pidls, SFGAO attrs)
        {
            _shellFolder.GetAttributesOf(pidls.Length, pidls, ref attrs);
            return attrs;
        }

        public SFGAO GetAttributesOf (PIDLIST pidl, SFGAO attrs)
        {
            return GetAttributesOf(new[] { pidl }, attrs);
        }

        public T GetUIObjectOf<T> (PIDLIST[] pidls, HWND hwnd)
        {
            uint reserved = 0;
            object obj;
            _shellFolder.GetUIObjectOf(hwnd, pidls.Length, pidls, typeof(T).GUID, ref reserved, out obj);
            return (T)obj;
        }

        public T GetUIObjectOf<T> (PIDLIST pidl, HWND hwnd)
        {
            return GetUIObjectOf<T>(new[] { pidl }, hwnd);
        }

        public T GetUIObjectOf<T> (PIDLIST[] pidls)
        {
            return GetUIObjectOf<T>(pidls, IntPtr.Zero);
        }

        public T GetUIObjectOf<T> (PIDLIST pidl)
        {
            return GetUIObjectOf<T>(pidl, IntPtr.Zero);
        }

        public string GetDisplayNameOf (PIDLIST pidl, SHGDN uFlags)
        {
            STRRET name;
            _shellFolder.GetDisplayNameOf(pidl, uFlags, out name);
            return Native.StrRetToBuf(ref name, pidl);
        }

        public PIDLIST SetNameOf (HWND hwnd, PIDLIST pidl, string displayName, SHGDN flags)
        {
            PIDLIST pidlOut;
            _shellFolder.SetNameOf(hwnd, pidl, displayName, flags, out pidlOut);
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
            if (_shellFolder == null)
                return;
            Marshal.ReleaseComObject(_shellFolder);
            _shellFolder = null;
        }
    }
}