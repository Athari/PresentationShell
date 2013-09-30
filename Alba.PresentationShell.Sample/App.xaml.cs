using System.Diagnostics;
using System.Windows;
using Alba.Diagnostics;

namespace Alba.PresentationShell.Sample
{
    public partial class App
    {
        protected override void OnStartup (StartupEventArgs e)
        {
            base.OnStartup(e);

            AlbaPresentaionShellTraceSources.Interop.Switch.Level = SourceLevels.All;
            AlbaPresentaionShellTraceSources.Interop.Listeners.Add(new DebugTraceListener());
        }

        private class DebugTraceListener : TraceListener
        {
            public override void Write (string message)
            {
                Debug.Write(message);
            }

            public override void WriteLine (string message)
            {
                Debug.WriteLine(message);
            }
        }
    }
}