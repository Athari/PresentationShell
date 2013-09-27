using Alba.Interop.CommonControls;
using Alba.Interop.ShellObjects;

namespace Alba.Interop
{
    internal static class NativeExts
    {
        public static NativeEnumIDList ToNative (this IEnumIDList @this)
        {
            return new NativeEnumIDList(@this);
        }

        public static NativeExtractIcon ToNative (this IExtractIcon @this)
        {
            return new NativeExtractIcon(@this);
        }

        public static NativeImageList ToNative (this IImageList @this)
        {
            return new NativeImageList(@this);
        }

        public static NativeShellFolder ToNative (this IShellFolder @this)
        {
            return new NativeShellFolder(@this);
        }

        public static NativeShellIcon ToNative (this IShellIcon @this)
        {
            return new NativeShellIcon(@this);
        }
    }
}