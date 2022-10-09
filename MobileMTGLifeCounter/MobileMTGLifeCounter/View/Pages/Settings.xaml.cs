using MobileMTGLifeCounter.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileMTGLifeCounter.View.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : ContentPage
    {
        public Settings()
        {
            InitializeComponent();
            BindingContext = Main.Current;
        }

        private async void NavigateToMain(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}