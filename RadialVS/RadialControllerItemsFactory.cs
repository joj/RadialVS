using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Input;

namespace RadialVS
{
    class RadialControllerItemsFactory
    {
        private RadialController radialController;
        private IServiceProvider serviceProvider;

        public IRadialItem ActiveItem { get; private set; }

        public RadialControllerItemsFactory(RadialController radialController, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.radialController = radialController;
        }

        public void AddItems()
        {
            radialController.Menu.Items.Clear();
            AddItem(new LineScrollItem(serviceProvider));
            AddItem(new DebugItem(serviceProvider));
            //AddItem(new BookmarksItem(serviceProvider));
            AddItem(new BreakpointsItem(serviceProvider));
        }

        public async void AddItem(IRadialItem item)
        {
            RadialControllerMenuItem menuItem;
            if (item is IFileIconRadialItem)
            {
                var fileName = ((IFileIconRadialItem)item).Icon;
                var icon = await StorageFile.GetFileFromPathAsync(fileName);
                menuItem = RadialControllerMenuItem.CreateFromIcon(item.DisplayText, RandomAccessStreamReference.CreateFromFile(icon));
            }
            else
            {
                menuItem = RadialControllerMenuItem.CreateFromKnownIcon(item.DisplayText, ((IKnownIconRadialItem)item).Icon);
            }

            if (!radialController.Menu.Items.Contains(menuItem))
            {
                radialController.Menu.Items.Add(menuItem);

                menuItem.Invoked += (s, a) => { ActiveItem = item; };
            }
        }


    }
}
