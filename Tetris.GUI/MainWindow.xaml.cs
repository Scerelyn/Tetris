using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Tetris.GameEngine;
using System.Threading;
using System.Timers;
using SharpDX.XInput;
using Tetris.GameEngine.Interfaces;
using System.Windows.Media.Imaging;

namespace Tetris.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ITetrisPage currentPage;
        private bool[] music = new bool[] { true, true, true, true, true };

        public MainWindow()
        {
            InitializeComponent();
            SetContent(new MainMenuPage());
                    }

        private void SetContent(ITetrisPage p)
        {
            currentPage = p;
            this.Content = p;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
         {
            currentPage.Page_KeyDown(sender, e);
        }

        public void StartNewGame()
        {
            SetContent(new GameBoardPage(false, music));
        }

        public void StartUltraGame()
        {
            SetContent(new GameBoardPage(true, music));
        }

        public void ReturnToMenu()
        {
            SetContent(new MainMenuPage());
        }

        public void MusicSelection()
        {
            SetContent(new MusicMenuPage(music));
        }
    }
}
