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
