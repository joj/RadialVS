using EnvDTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Input;

namespace RadialVS
{
    class LineScrollItem : IKnownIconRadialItem
    {
        private IServiceProvider serviceProvider;

        public string DisplayText => "Scroll";

        public RadialControllerMenuKnownIcon Icon => RadialControllerMenuKnownIcon.Scroll;

        public LineScrollItem(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void Execute()
        {
        }

        public void Next()
        {
            var dte = serviceProvider.GetService(typeof(EnvDTE.DTE)) as EnvDTE.DTE;
            var selection = ((TextSelection)dte.ActiveDocument.Selection);
            selection.MoveToLineAndOffset(selection.CurrentLine + 1, selection.CurrentColumn);
        }

        public void Previous()
        {
            var dte = serviceProvider.GetService(typeof(EnvDTE.DTE)) as EnvDTE.DTE;
            var selection = ((TextSelection)dte.ActiveDocument.Selection);
            selection.MoveToLineAndOffset(selection.CurrentLine - 1, selection.CurrentColumn);
        }
    }
}
