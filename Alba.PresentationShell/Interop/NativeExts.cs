using Alba.Interop.CommonControls;
using Alba.Interop.ShellObjects;

namespace Alba.Interop
{
    internal static class NativeExts
    {
        public static NativeEnumIDList ToNative (this IEnumIDList @this)
        {
            return @this != null ? new NativeEnumIDList(@this) : null;
        }

        public static NativeExtractIcon ToNative (this IExtractIcon @this)
        {
            return @this != null ? new NativeExtractIcon(@this) : null;
        }

        public static NativeImageList ToNative (this IImageList @this)
        {
            return @this != null ? new NativeImageList(@this) : null;
        }

        public static NativeShellFolder ToNative (this IShellFolder @this)
        {
            return @this != null ? new NativeShellFolder(@this) : null;
        }

        public static NativeShellIcon ToNative (this IShellIcon @this)
        {
            return @this != null ? new NativeShellIcon(@this) : null;
        }
    }
}