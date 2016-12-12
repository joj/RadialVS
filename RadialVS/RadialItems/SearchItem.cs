using EnvDTE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadialVS
{
    class SearchItem : IFileIconRadialItem
    {
        private DTE dte;
        private IServiceProvider serviceProvider;

        public SearchItem(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            dte = serviceProvider.GetService(typeof(EnvDTE.DTE)) as EnvDTE.DTE;

            Icon = Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), "Resources\\SearchIcon.png");
        }

        public string DisplayText => "Search";

        public string Icon { get; private set; }

        public void Execute()
        {
        }

        public void Next()
        {
        }

        public void Previous()
        {
        }
    }
}
