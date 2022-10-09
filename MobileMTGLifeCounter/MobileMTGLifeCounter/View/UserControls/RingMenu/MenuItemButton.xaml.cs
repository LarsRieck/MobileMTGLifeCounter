using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileMTGLifeCounter.View.UserControls.RingMenu
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuItemButton : ContentView
    {
        private static List<MenuItemButton> _current = new List<MenuItemButton>();
        public IMenuItem MenuItemVM
        {
            get { return (IMenuItem)GetValue(MenuItemProperty); }
            set { SetValue(MenuItemProperty, value); }
        }

        public static readonly BindableProperty MenuItemProperty =
        BindableProperty.Create(nameof(MenuItemVM), typeof(IMenuItem), typeof(MenuItemButton), propertyChanged: OnMenuItemPropertyChange, defaultBindingMode:BindingMode.TwoWay);

        private static void OnMenuItemPropertyChange(BindableObject bindable, object oldValue, object newValue)
        {
            var ctrl = ((MenuItemButton)bindable);
                     

            if (_current.Contains(ctrl))
            {
                _current.First(x => x == ctrl).MenuItemVM.NotifyMe();
            }
            
        }

        public MenuItemButton()
        {
            InitializeComponent();
            _current.Add(this);            
        }

        public void SetClickEvent(Action onClick)
        {
            ActualButton.Clicked +=  (object sender, EventArgs e) => { onClick?.Invoke(); } ;
        }
    }
}