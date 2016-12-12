using Microsoft.VisualStudio.Imaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Storage;
using Windows.Storage.Streams;
using EnvDTE;
using Microsoft.VisualStudio.TextManager.Interop;

namespace RadialVS
{
    class BookmarksItem : IFileIconRadialItem
    {
        private DTE dte;
        private IServiceProvider serviceProvider;

        public BookmarksItem(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            dte = serviceProvider.GetService(typeof(EnvDTE.DTE)) as EnvDTE.DTE;

            Icon = Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), "Resources\\BookmarksIcon.png");
        }

        public string DisplayText => "Bookmark";

        public string Icon { get; private set; }

        public void Execute()
        {
            ((TextSelection)dte.ActiveDocument.Selection).SetBookmark();
        }

        public void Next()
        {
            ((TextSelection)dte.ActiveDocument.Selection).NextBookmark();
        }

        public void Previous()
        {
            ((TextSelection)dte.ActiveDocument.Selection).PreviousBookmark();

        }
    }
}
