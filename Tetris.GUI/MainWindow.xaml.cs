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

        public MainWindow()
        {
            InitializeComponent();
            GameBoardPage gbp = new GameBoardPage();
            currentPage = gbp;
            this.Content = gbp;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            currentPage.Page_KeyDown(sender, e);
        }
    }
}
