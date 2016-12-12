using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using System.IO;

namespace RadialVS
{
    public class DebugItem : IFileIconRadialItem
    {
        private DTE dte;
        private IServiceProvider serviceProvider;

        public DebugItem(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            dte = serviceProvider.GetService(typeof(EnvDTE.DTE)) as EnvDTE.DTE;

            Icon = Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), "Resources\\DebugIcon.png");
        }

        public string DisplayText => "Debug";

        public string Icon { get;  private set; }

        public void Execute()
        {
            if (dte.Application.Debugger.CurrentMode == dbgDebugMode.dbgRunMode)
            {
                dte.Application.Debugger.Stop();
            }
            else
            {
                dte.Application.Debugger.Go();
            }
        }

        public void Next()
        {
            dte.Debugger.StepInto();
        }

        public void Previous()
        {
            dte.Debugger.StepOut();
        }
    }
}
