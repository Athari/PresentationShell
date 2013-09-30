using System.Diagnostics;

namespace Alba.Diagnostics
{
    public class AlbaPresentaionShellTraceSources
    {
        public const string BaseTraceSourceName = "Alba.PresentationShell.";
        public const string InteropTraceSourceName = BaseTraceSourceName + "Interop";

        private static TraceSource _interop;

        public static TraceSource Interop
        {
            get { return _interop ?? (_interop = new TraceSource(InteropTraceSourceName)); }
        }
    }
}