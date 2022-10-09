using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MobileMTGLifeCounter.View.UserControls.RingMenu
{
    public interface IMenuItem
    {
        string Name { get; set; }
        ICommand StoredCommand { get;}        

        void NotifyMe();
    }
}
