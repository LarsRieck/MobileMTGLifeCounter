using MobileMTGLifeCounter.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileMTGLifeCounter.View.UserControls
{
   
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerContent : ContentView
    {
        public PlayerStats Stats
        {
            get { return (PlayerStats)GetValue(StatsProperty); }
            set { SetValue(StatsProperty, value); }
        }
        
        public static readonly BindableProperty StatsProperty =
        BindableProperty.Create("Stats", typeof(PlayerStats), typeof(PlayerContent), new PlayerStats() );

        public Main MainVM
        {
            get { return (Main)GetValue(MainVMProperty); }
            set { SetValue(MainVMProperty, value); }
        }

        public static readonly BindableProperty MainVMProperty =
        BindableProperty.Create("MainVM", typeof(Main), typeof(PlayerContent));



        public PlayerContent()
        {
            InitializeComponent();
        }
    }
}