using Alba.Interop.CommonControls;
using Alba.Interop.ShellObjects;

namespace Alba.Interop
{
    internal static class NativeExts
    {
        public static NativeEnumIDList ToNative (this IEnumIDList @this, bool own = true)
        {
            return @this != null ? new NativeEnumIDList(@this, own) : null;
        }

        public static NativeExtractIcon ToNative (this IExtractIcon @this, bool own = true)
        {
            return @this != null ? new NativeExtractIcon(@this, own) : null;
        }

        public static NativeImageList ToNative (this IImageList @this, bool own = true)
        {
            return @this != null ? new NativeImageList(@this, own) : null;
        }

        public static NativeShellFolder ToNative (this IShellFolder @this, bool own = true)
        {
            return @this != null ? new NativeShellFolder(@this, own) : null;
        }

        public static NativeShellIcon ToNative (this IShellIcon @this, bool own = true)
        {
            return @this != null ? new NativeShellIcon(@this, own) : null;
        }

        public static NativeShellIconOverlay ToNative (this IShellIconOverlay @this, bool own = true)
        {
            return @this != null ? new NativeShellIconOverlay(@this, own) : null;
        }
    }
}