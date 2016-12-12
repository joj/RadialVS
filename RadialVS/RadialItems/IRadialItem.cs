using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Input;

namespace RadialVS
{
    interface IRadialItem
    {
        string DisplayText { get; }

        void Execute();
        void Next();
        void Previous();
    }

    interface IKnownIconRadialItem : IRadialItem
    {
        RadialControllerMenuKnownIcon Icon { get; }
    }

    interface IFileIconRadialItem : IRadialItem
    {
        string Icon { get; }
    }
}
