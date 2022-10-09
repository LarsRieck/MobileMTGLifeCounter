using MobileMTGLifeCounter.Control.MVVMBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace MobileMTGLifeCounter.Control
{
    public class PlayerStats:ViewModelBase
    {
        private BusinessLogic.PlayerStats _model = new BusinessLogic.PlayerStats();
        private bool _isCurrent = false;  
        private Color _background = Color.LightCoral;
        private Color _fontColor = Color.Black;

        public string Player
        {
            get => _model.Player;
            set
            {
                _model.Player = value;
                Notify();
            }
        }
        public int LifePoints
        { 
            get => _model.LifePoints;
            set
            {
                _model.LifePoints = value;
                Notify();
            }
        }

        public TimeSpan Countdown
        {
            get => _model.Countdown;
            set
            {
                _model.Countdown = value;
                Notify();
            }
        }
        public bool IsCurrent
        {
            get => _isCurrent;
            set
            {
                _isCurrent = value;
                Notify();
                Notify("Border");
            }
        }
        public Color Background
        {
            get => _background;
            set
            {
                _background = value;

                Notify();
                Notify(nameof(Border));
            }
        }

        public Color FontColor
        {
            get => _fontColor;
            set
            {
                _fontColor = value;

                Notify();
            }
        }

       public Tuple<string,Color,Color> BackgroundTuple
        {
            get => Main.Current.AllColors.FirstOrDefault(x => x.Item2 == Background);
            set
            {
                Background = value.Item2;
                FontColor = value.Item3;
                Notify();
            }
        }

        public Color Border => IsCurrent ? Color.White : Background;

       


        public ICommand IncreaseLifeCommand { get; }
        public ICommand DecreaseLifeCommand { get; }
        

        public PlayerStats()
        {
            IncreaseLifeCommand = new Command(execute: () =>
            {
                LifePoints++;
            });

            DecreaseLifeCommand = new Command(execute: () =>
            {
                LifePoints--;
            });           
            
        }

       





    }
}
