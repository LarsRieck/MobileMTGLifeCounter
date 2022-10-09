using MobileMTGLifeCounter.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;


namespace MobileMTGLifeCounter.View.UserControls.RingMenu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RingControl : ContentView
    {
        private List<MenuItemButton> _buttons = new List<MenuItemButton>();

        private System.Timers.Timer _visibilityTimer;
        public IEnumerable<IMenuItem> MenuItems
        {
            get { return (IEnumerable<IMenuItem>)GetValue(MenuItemsProperty); }
            set { SetValue(MenuItemsProperty, value); }
        }

        public static readonly BindableProperty MenuItemsProperty =
        BindableProperty.Create("MenuItems", typeof(IEnumerable<IMenuItem>), typeof(RingControl), Enumerable.Empty<IMenuItem>());

        public RingControl()
        {
            InitializeComponent();
        }

        private void MenuButton_Clicked(object sender, EventArgs e)
        {
            if (_buttons is null || _buttons.Count < 1)
            {
                AddMenuButtons();
            }
            SetMenuButtonPositions();
        }


        private void SetOverlay()
        {
            var appWidth = Xamarin.Forms.Application.Current.MainPage.Width;
            var appHeight = Xamarin.Forms.Application.Current.MainPage.Height;

            var gridColumnWidth = (appWidth / 2) - (RingControlGrid.ColumnDefinitions[1].Width.Value / 2) - 7;
            var gridRowHeight = (appHeight / 2) - (RingControlGrid.ColumnDefinitions[1].Width.Value / 2) - 7;
            Grid.SetColumn(SKCanvas, 0);
            Grid.SetRow(SKCanvas, 0);
            Grid.SetColumnSpan(SKCanvas, 3);
            Grid.SetRowSpan(SKCanvas, 3);
            RingControlGrid.ColumnDefinitions[0].Width = gridColumnWidth;
            RingControlGrid.ColumnDefinitions[2].Width = gridColumnWidth;
            RingControlGrid.RowDefinitions[0].Height = gridRowHeight;
            RingControlGrid.RowDefinitions[2].Height = gridRowHeight;
            SKCanvas.Opacity = .5;
        }

        private void ResetOverlay()
        {
            RingControlGrid.ColumnDefinitions[0].Width = 0;
            RingControlGrid.ColumnDefinitions[2].Width = 0;
            RingControlGrid.RowDefinitions[0].Height = 0;
            RingControlGrid.RowDefinitions[2].Height = 0;
            Grid.SetColumn(SKCanvas, 1);
            Grid.SetRow(SKCanvas, 1);
            Grid.SetColumnSpan(SKCanvas, 1);
            Grid.SetRowSpan(SKCanvas, 1);

            SKCanvas.Opacity = 0;
        }

        private void SetMenuButtonPositions()
        {
            if (!(_visibilityTimer is null) && _visibilityTimer.Enabled)
            {
                return;
            }
            SetOverlay();
            double angle = 360 / (double)MenuItems.Count();
            double index = 0;
            var appHeight = Xamarin.Forms.Application.Current.MainPage.Height;


            foreach (var mib in _buttons)
            {            
                double radiusAddition = ((360 / angle) * Math.PI * 2)+5;

                radiusAddition = radiusAddition < 10 ? 10 : radiusAddition;
                radiusAddition = radiusAddition >= (appHeight / 2) - (RingControlGrid.ColumnDefinitions[1].Width.Value) ? (appHeight / 2) - (RingControlGrid.ColumnDefinitions[1].Width.Value) - RingControlButton.Height : radiusAddition;
                double radius = RingControlButton.Height + radiusAddition;                          
                var x = radius * Math.Sin(Math.PI * 2 * (angle * index) / 360);
                var y = radius * Math.Cos(Math.PI * 2 * (angle * index) / 360);                       
                index++;           
                mib.TranslateTo(x, y, 300);             
            }            
            //SetTimer();
            

        }
        private void ResetMenuButtonPositions()
        {
            foreach (var mib in _buttons)
            {
                mib.TranslateTo(0, 0, 300);
            }
            ResetOverlay();
        }

        private void AddMenuButtons()
        {
            foreach (var mi in MenuItems)
            {               
                MenuItemButton mib = new MenuItemButton
                {
                    MenuItemVM = mi
                };  
               
                RingControlGrid.Children.Add(mib);
                mib.SetClickEvent(ResetMenuButtonPositions);
                _buttons.Add(mib);
                //Grid.SetColumnSpan(mib, 3);
                //Grid.SetRowSpan(mib, 3);
                Grid.SetColumn(mib, 1);
                Grid.SetRow(mib, 1);
                RingControlGrid.LowerChild(mib); 
                RingControlGrid.LowerChild(SKCanvas);               
            }
        }

        //private void SetTimer()
        //{
        //    if (!(_visibilityTimer is null) && _visibilityTimer.Enabled)
        //    {
        //        return;
        //    }
        //    _visibilityTimer = new System.Timers.Timer(5000);
        //    _visibilityTimer.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) => { ResetMenuButtonPositions(); };
        //    _visibilityTimer.AutoReset = false;
        //    _visibilityTimer.Enabled = true;
        //    _visibilityTimer.Start();
        //}
        //private void InterruptTimer()
        //{
        //    _visibilityTimer.Enabled = false;
        //    _visibilityTimer.Stop();
        //}

        private void SKCanvas_Touch(object sender, SkiaSharp.Views.Forms.SKTouchEventArgs e)
        {
            ResetMenuButtonPositions(); 
            //InterruptTimer();
        }
       
    }
}