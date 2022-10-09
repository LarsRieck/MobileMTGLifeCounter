using MobileMTGLifeCounter.Control.MVVMBase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileMTGLifeCounter.Control
{
    public class MenuItemVM : ViewModelBase, View.UserControls.RingMenu.IMenuItem
    {
        private string _name = "";  

        public string Name 
        { 
            get => _name;
            set
            { 
                _name = value;
                Notify();
            }
        }
      
        public ICommand StoredCommand { get; }

        public MenuItemVM(Action onClick)
        {
            StoredCommand = new Command(onClick);
            //OnClick = new Command(NotifyMe);
        }

        public void NotifyMe()
        {
            Notify(nameof(Name));
        }
    }
}
