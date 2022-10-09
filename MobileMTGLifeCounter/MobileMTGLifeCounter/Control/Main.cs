using MobileMTGLifeCounter.Control.MVVMBase;
using MobileMTGLifeCounter.View.UserControls.RingMenu;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Timers;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace MobileMTGLifeCounter.Control
{
    public class Main: ViewModelBase
    {
        public static Main Current;

        private const int _maxLifeTotal = 40;
        private TimeSpan _maxTime = new TimeSpan(1,0,0);

        private PlayerStats _currentPlayerStats = new PlayerStats();
        public PlayerStats CurrentPlayerStats { get => _currentPlayerStats; internal set { _currentPlayerStats = value;Notify(); } }

        private static System.Timers.Timer CountdownTimer;
        private List<Tuple<string, Color>> _allColors;

        public ObservableCollection<PlayerStats> PlayerStatsList { get; } = new ObservableCollection<PlayerStats>();
        public List<View.UserControls.RingMenu.IMenuItem> MenuItems { get; }

        public event EventHandler NavigateSettinsEvent;

        public List<Tuple<string, Color,Color>> AllColors
        { get; }
        = new List<Tuple<string, Color, Color>>()
        {
            new Tuple<string, Color, Color>( "MTGLightGreen", Color.FromRgb(196,211,202), Color.Black ),
            new Tuple<string, Color, Color>( "MTGGreen", Color.FromRgb(0,115,62), Color.White ),
            new Tuple<string, Color, Color>( "MTGLightBlue", Color.FromRgb(175,206,234), Color.Black ),
            new Tuple<string, Color, Color>( "MTGBlue", Color.FromRgb(14,104,171), Color.White ),
            new Tuple<string, Color, Color>( "MTGLightRed", Color.FromRgb(235,159,130), Color.Black ),
            new Tuple<string, Color, Color>( "MTGRed", Color.FromRgb(211,32,42), Color.Black ),
            new Tuple<string, Color, Color>( "MTGLightWhite", Color.FromRgb(249,250,244), Color.Black ),
            new Tuple<string, Color, Color>( "MTGWhite", Color.FromRgb(248,231,185), Color.Black ),
            new Tuple<string, Color, Color>( "MTGLightBlack", Color.FromRgb(166,159,157), Color.White ),
            new Tuple<string, Color, Color>( "MTGBlack", Color.FromRgb(21,11,0), Color.White ),
            new Tuple<string, Color, Color>( "Aqua", Color.Aqua, Color.Black ), 
            new Tuple<string, Color, Color>( "Black", Color.Black, Color.White ),
            new Tuple<string, Color, Color>( "Blue", Color.LightBlue, Color.Black ), 
            new Tuple<string, Color, Color>( "Fuchsia", Color.Fuchsia, Color.Black ),
            new Tuple<string, Color, Color>( "Gray", Color.Gray , Color.Black),
            new Tuple<string, Color, Color>( "Green", Color.LightGreen, Color.Black ),
            new Tuple<string, Color, Color>( "Lime", Color.Lime , Color.Black), 
            new Tuple<string, Color, Color>( "Maroon", Color.Maroon, Color.Black ),
            new Tuple<string, Color, Color>( "Navy", Color.Navy, Color.Black), 
            new Tuple<string, Color, Color>( "Olive", Color.Olive , Color.Black),
            new Tuple<string, Color, Color>( "Purple", Color.Purple, Color.Black ), 
            new Tuple<string, Color, Color>( "Red", Color.Coral, Color.Black ),
            new Tuple<string, Color, Color>( "Silver", Color.Silver, Color.Black ), 
            new Tuple<string, Color, Color>( "Teal", Color.Teal, Color.Black ),
            new Tuple<string, Color, Color>( "White", Color.White, Color.Black ), 
            new Tuple<string, Color, Color>( "Yellow", Color.LightYellow, Color.Black),
            new Tuple<string, Color, Color>( "Pink", Color.DeepPink, Color.Black ),
            new Tuple<string, Color, Color>( "Violet", Color.Violet, Color.Black ),
            


        };
        //{
        //    get => _allColors;
        //    private set 
        //    {
        //        _allColors = value;
        //        Notify();
        //    }
        //}
            
            //

            //typeof(Color).GetRuntimeFields().Where(x => x.FieldType == typeof(Color) && x.IsInitOnly);


        public ICommand SwitchPlayerCommand { get; }
        public ICommand TimerOffCommand { get; }

        public Main()
        {
            Current = this;

            SwitchPlayerCommand = new Command<PlayerStats>(SwitchPlayer);
            TimerOffCommand = new Command(TimerOff);

            CountdownTimer = new Timer(1000);
            CountdownTimer.Elapsed += OnCountDownTimerTick;
            CountdownTimer.AutoReset = true;           
            PlayerStatsList.Add(new PlayerStats() { BackgroundTuple = AllColors.FirstOrDefault(x => x.Item1 == "MTGWhite"), Player = "Player 1" });
            PlayerStatsList.Add(new PlayerStats() { BackgroundTuple = AllColors.FirstOrDefault(x => x.Item1 == "MTGLightRed"), Player = "Player 2" });
            PlayerStatsList.Add(new PlayerStats() { BackgroundTuple = AllColors.FirstOrDefault(x => x.Item1 == "MTGLightBlue"), Player = "Player 3" });
            PlayerStatsList.Add(new PlayerStats() { BackgroundTuple = AllColors.FirstOrDefault(x => x.Item1 == "MTGGreen"), Player = "Player 4" });

            MenuItems = new List<View.UserControls.RingMenu.IMenuItem>();
            MenuItems.Add(new MenuItemVM(ResetAllLife) { Name = "ReloadLife.png" });
            MenuItems.Add(new MenuItemVM(ResetAllTime) { Name = "ReloadTime.png" });
            MenuItems.Add(new MenuItemVM(() => { ResetAllLife(); ResetAllTime(); }) { Name = "ReloadAll.png" });
            MenuItems.Add(new MenuItemVM(() => { NavigateSettings(); }) { Name = "SettingsAlternative.png" });
            MenuItems.Add(new MenuItemVM(() => {Shuffle<PlayerStats>(PlayerStatsList); Notify(nameof(PlayerStatsList)); }) { Name = "Shuffle.png" });

        }

       
        public void Shuffle<T>(IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
           
        }
        public void TimerOff()
        {
            CountdownTimer.Stop();
            CountdownTimer.Enabled = false;
        }

        public void SwitchPlayer(PlayerStats player)
        {
            CurrentPlayerStats.IsCurrent = false;
            CurrentPlayerStats = player;
            CurrentPlayerStats.IsCurrent = true;
            if (!CountdownTimer.Enabled)
            {
                CountdownTimer.Enabled = true;
                CountdownTimer.Start();
            }
        }

        private void OnCountDownTimerTick(object sender, ElapsedEventArgs e)
        {
            if (CurrentPlayerStats is null)
            {
                return;
            }
            CurrentPlayerStats.Countdown = CurrentPlayerStats.Countdown.Subtract(TimeSpan.FromSeconds(1));           
        }

        private void ResetAllLife()
        {
            foreach (var playerStats in PlayerStatsList)
            {
                playerStats.LifePoints = _maxLifeTotal;
            } 
        }
        private void ResetAllTime()
        {
            foreach (var playerStats in PlayerStatsList)
            {
                playerStats.Countdown = _maxTime;
            }
        }

        private void NavigateSettings()
        {
            NavigateSettinsEvent?.Invoke(null,null);
        }

    }
}
