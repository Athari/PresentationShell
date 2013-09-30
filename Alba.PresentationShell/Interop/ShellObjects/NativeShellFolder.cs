using System;
using System.Collections.Generic;
using Alba.Interop.ShellTypes;
using Alba.Interop.WinError;

namespace Alba.Interop.ShellObjects
{
    using HWND = IntPtr;

    internal class NativeShellFolder : NativeComInterface<IShellFolder>
    {
        public NativeShellFolder (IShellFolder com) : base(com)
        {}

        public static NativeShellFolder GetDesktopFolder ()
        {
            return new NativeShellFolder(Native.SHGetDesktopFolder());
        }

        public PIDLIST ParseDisplayName (string displayName, ref uint pchEaten, ref SFGAO attrs, HWND hwnd)
        {
            PIDLIST pidl;
            Com.ParseDisplayName(hwnd, null, displayName, ref pchEaten, out pidl, ref attrs);
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
            HRESULT hr = Com.EnumObjects(hwnd, flags, out enumIdList);
            if (hr == HRESULT.S_FALSE)
                yield break;
            hr.ThrowIfFailed();
            if (enumIdList == null)
                yield break;
            using (var enumerator = new NativeEnumIDList(enumIdList))
                while (enumerator.MoveNext())
                    yield return enumerator.Current;
        }

        public T BindToObject<T> (PIDLIST pidl) where T : class
        {
            object obj;
            HRESULT hr = Com.BindToObject(pidl, null, typeof(T).GUID, out obj);
            return hr.IsSucceeded ? (T)obj : null;
        }

        public T BindToStorage<T> (PIDLIST pidl)
        {
            object obj;
            Com.BindToStorage(pidl, null, typeof(T).GUID, out obj);
            return (T)obj;
        }

        public int CompareIDs (PIDLIST pidl1, PIDLIST pidl2, int lParam = 0)
        {
            HRESULT hr = Com.CompareIDs(lParam, pidl1, pidl2);
            hr.ThrowIfFailed();
            return hr.Code;
        }

        public T CreateViewObject<T> (HWND hwndOwner)
        {
            object obj;
            Com.CreateViewObject(hwndOwner, typeof(T).GUID, out obj);
            return (T)obj;
        }

        public SFGAO GetAttributesOf (PIDLIST[] pidls, SFGAO attrs)
        {
            Com.GetAttributesOf(pidls.Length, pidls, ref attrs);
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
            Com.GetUIObjectOf(hwnd, pidls.Length, pidls, typeof(T).GUID, ref reserved, out obj);
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
            Com.GetDisplayNameOf(pidl, uFlags, out name);
            return Native.StrRetToBuf(ref name, pidl);
        }

        public PIDLIST SetNameOf (HWND hwnd, PIDLIST pidl, string displayName, SHGDN flags)
        {
            PIDLIST pidlOut;
            Com.SetNameOf(hwnd, pidl, displayName, flags, out pidlOut);
            return pidlOut;
        }
    }
}