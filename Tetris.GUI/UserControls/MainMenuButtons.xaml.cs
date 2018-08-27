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

namespace Tetris.GUI.UserControls
{
    /// <summary>
    /// Interaction logic for MainMenuButtons.xaml
    /// </summary>
    public partial class MainMenuButtons : UserControl
    {
        private MainMenuPage mmp;
        public MainMenuButtons(RoutedEventHandler classicListener, RoutedEventHandler ultraListener, RoutedEventHandler MusicListener, Func<Object, RoutedEventArgs> SettingListener)
        {
            InitializeComponent();
            NewGameButton.Click += classicListener;
        }
    }
}
