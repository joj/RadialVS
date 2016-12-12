using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Input;
using EnvDTE;

namespace RadialVS
{
    class RadialControllerFactory
    {
        private IServiceProvider serviceProvider;
        private DTE dte;

        private RadialController radialController;
        private RadialControllerItemsFactory itemsFactory;

        public RadialControllerFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void Initialize()
        {
            CreateController();
            itemsFactory = new RadialControllerItemsFactory(radialController, serviceProvider);

            RemoveDefaultItems();
            itemsFactory.AddItems();

            SubscribeToCallbacks();
        }

        private void RemoveDefaultItems()
        {
            RadialControllerConfiguration radialControllerConfig;
            IRadialControllerConfigurationInterop radialControllerConfigInterop = (IRadialControllerConfigurationInterop)System.Runtime.InteropServices.WindowsRuntime.WindowsRuntimeMarshal.GetActivationFactory(typeof(RadialControllerConfiguration));
            Guid guid = typeof(RadialControllerConfiguration).GetInterface("IRadialControllerConfiguration").GUID;

            radialControllerConfig = radialControllerConfigInterop.GetForWindow(GetWindowHwnd(), ref guid);
            radialControllerConfig.SetDefaultMenuItems(new[] { RadialControllerSystemMenuItemKind.Volume, RadialControllerSystemMenuItemKind.UndoRedo });
        }

        private void CreateController()
        {
            IRadialControllerInterop interop = (IRadialControllerInterop)System.Runtime.InteropServices.WindowsRuntime.WindowsRuntimeMarshal.GetActivationFactory(typeof(RadialController));
            Guid guid = typeof(RadialController).GetInterface("IRadialController").GUID;

            radialController = interop.CreateForWindow(GetWindowHwnd(), ref guid);
        }

        private void SubscribeToCallbacks()
        {
            radialController.ButtonClicked += (s, e) => { itemsFactory.ActiveItem.Execute();  };
            radialController.RotationChanged += (s, e) =>
            {
                if (e.RotationDeltaInDegrees > 0) itemsFactory.ActiveItem.Next();
                else itemsFactory.ActiveItem.Previous();
            };

            dte = serviceProvider.GetService(typeof(EnvDTE.DTE)) as EnvDTE.DTE;
            dte.Events.SolutionEvents.AfterClosing += () =>
            {
                radialController.Menu.Items.Clear();
            };
        }

        private IntPtr GetWindowHwnd()
        {
            var shell = serviceProvider.GetService(typeof(SVsUIShell)) as IVsUIShell;
            IntPtr hWnd;
            shell.GetDialogOwnerHwnd(out hWnd);

            return hWnd;
        }

    }
}
