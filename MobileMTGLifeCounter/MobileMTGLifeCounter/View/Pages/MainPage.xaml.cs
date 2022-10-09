using MobileMTGLifeCounter.Control;
using MobileMTGLifeCounter.View.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileMTGLifeCounter
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = Main.Current;
            Control.Main.Current.NavigateSettinsEvent += NavigateToSettings;
        }

        private void NavigateToSettings (object sender, EventArgs e)
        {
            Navigation.PushAsync(new Settings());
        }
    }
}
