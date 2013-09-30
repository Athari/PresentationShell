using System.Runtime.InteropServices;
using System.Text;
using Alba.Interop.ShellObjects;
using Alba.Interop.ShellTypes;

namespace Alba.Interop
{
    internal partial class Native
    {
        [DllImport (Dll.ShellLight, CharSet = CharSet.Auto, PreserveSig = false)]
        private static extern void StrRetToBuf ([In, Out] ref STRRET pstr, [In] PIDLIST pidl, [Out] StringBuilder pszBuf, [In] int cchBuf);

        public static string StrRetToBuf (ref STRRET pstr, PIDLIST pidl)
        {
            var sb = new StringBuilder(MAX_PATH);
            StrRetToBuf(ref pstr, pidl, sb, sb.Capacity);
            return sb.ToString();
        }
    }
}