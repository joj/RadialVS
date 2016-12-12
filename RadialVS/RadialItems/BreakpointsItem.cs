using EnvDTE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace RadialVS
{
    class BreakpointsItem : IFileIconRadialItem
    {
        private DTE dte;
        private IServiceProvider serviceProvider;

        public BreakpointsItem(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            dte = serviceProvider.GetService(typeof(EnvDTE.DTE)) as EnvDTE.DTE;

            Icon = Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), "Resources\\BreakpointsIcon.png");
        }

        public string DisplayText => "Breakpoint";

        public string Icon { get; private set; }

        public void Execute()
        {
            var selection = ((TextSelection)dte.ActiveDocument.Selection);
            var breakpoint = (dte.Debugger.Breakpoints.OfType<Breakpoint>().FirstOrDefault(b => b.File == dte.ActiveDocument.FullName && b.FileLine == selection.CurrentLine));
            if (breakpoint == null)
            {
                dte.Debugger.Breakpoints.Add(File: dte.ActiveDocument.FullName, Line: selection.CurrentLine);
            }
            else
            {
                breakpoint.Delete();
            }
        }

        public void Next()
        {
            FindBreakpoint((s, bs) =>
            {
                var breakpoint = bs.FirstOrDefault(b =>
                    b.File == dte.ActiveDocument.FullName && b.FileLine > s.CurrentLine
                    || string.Compare(b.File, dte.ActiveDocument.FullName) > 0);

                breakpoint = breakpoint ?? bs.First();

                return breakpoint;
            });
        }

        public void Previous()
        {
            FindBreakpoint((s, bs) =>
            {
                var breakpoint = bs.LastOrDefault(b =>
                    b.File == dte.ActiveDocument.FullName && b.FileLine < s.CurrentLine
                    || string.Compare(b.File, dte.ActiveDocument.FullName) < 0);

                breakpoint = breakpoint ?? bs.Last();

                return breakpoint;
            });
        }

        private void FindBreakpoint(Func<TextSelection, IEnumerable<Breakpoint>, Breakpoint> query)
        {
            var selection = ((TextSelection)dte.ActiveDocument.Selection);

            var breakpoints = dte.Debugger.Breakpoints.OfType<Breakpoint>().OrderBy(b => b, new FileOrderer());

            var breakpoint = query(selection, breakpoints);

            var s = dte.Documents.OfType<Document>().FirstOrDefault(d => d.FullName == breakpoint.File).Selection as TextSelection;
            dte.Documents.Open(breakpoint.File);
            s.MoveToLineAndOffset(breakpoint.FileLine, breakpoint.FileColumn);
        }

    }

    class FileOrderer : IComparer<Breakpoint>
    {
        public int Compare(Breakpoint x, Breakpoint y)
        {
            if (x.File == y.File)
            {
                return x.FileLine.CompareTo(y.FileLine);
            }
            else
            {
                return string.Compare(x.File, y.File);
            }
        }
    }
}
