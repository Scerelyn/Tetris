using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tetris.GameEngine;
using System.Threading;

namespace Tetris.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Game tetris = new Game();
        WMPLib.WindowsMediaPlayer Player;
        public MainWindow()
        {
            InitializeComponent();
            SetColors();
            SetBoard();
            tetris.Start();
            timer = new System.Timers.Timer(800);
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Enabled = true;
            timer.Start();
            Console.WriteLine(board[1, 1]);
        }

        private void SetColors()
        {
            colors[0] = new SolidColorBrush(Colors.Black);
            colors[1] = new SolidColorBrush(Colors.Cyan);
            colors[2] = new SolidColorBrush(Colors.Yellow);
            colors[3] = new SolidColorBrush(Colors.Blue);
            colors[4] = new SolidColorBrush(Colors.DarkOrange);
            colors[5] = new SolidColorBrush(Colors.LawnGreen);
            colors[6] = new SolidColorBrush(Colors.Red);
            colors[7] = new SolidColorBrush(Colors.DarkMagenta);
            colors[8] = new SolidColorBrush(Colors.Gray);
        }

        private void SetBoard()
        {
            
            for (int i = 0; i < 10; i++)
            {
                Tetrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < 20; i++)
            {
                Tetrid.RowDefinitions.Add(new RowDefinition());
            }
            Thread t = new Thread(NewThread);
            t.Start();


        }

        private void PlayFile(String url)
        {
            Player = new WMPLib.WindowsMediaPlayer();
            Player.PlayStateChange += Player_PlayStateChange;
            Player.URL = url;
            Player.controls.play();
        }

        private void Player_PlayStateChange(int NewState)
        {
            if ((WMPLib.WMPPlayState)NewState == WMPLib.WMPPlayState.wmppsStopped)
            {
                PlayFile("C:/Users/Jt/Desktop/Tetris/SoundResources/TypeA.mp3");
            }
        }

        private void NewThread()
        {
            PlayFile("C:/Users/Jt/Desktop/Tetris/SoundResources/TypeA.mp3");
        }
    }
}
