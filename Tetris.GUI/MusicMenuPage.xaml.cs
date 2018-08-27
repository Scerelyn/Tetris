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

namespace Tetris.GUI
{
    /// <summary>
    /// Interaction logic for MusicMenuPage.xaml
    /// </summary>
    public partial class MusicMenuPage : Page, ITetrisPage
    {
        private bool[] Music;
        public MusicMenuPage(bool[] music)
        {
            InitializeComponent();
            Music = music;
            if(Music[0])
            {
                KorobeinikiBox.IsChecked = true;
            }
            if(Music[1])
            {
                TechmoBox.IsChecked = true;
            }
            if(Music[2])
            {
                KatyushaBox.IsChecked = true;
            }
            if(Music[3])
            {
                HeartOfFireBox.IsChecked = true;
            }
            if(Music[4])
            {
                IevanPolkkaBox.IsChecked = true;
            }
        }

        public void Page_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void MenuReturnButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = (MainWindow)Parent;
            mw.ReturnToMenu();
        }

        private void MusicBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            switch (cb.Name)
            {
                case ("KorobeinikiBox"):
                    Music[0] = !Music[0];
                    break;
                case ("TechmoBox"):
                    Music[1] = !Music[1];
                    break;
                case ("KatyushaBox"):
                    Music[2] = !Music[2];
                    break;
                case ("HeartOfFireBox"):
                    Music[3] = !Music[3];
                    break;
                case ("IevanPolkkaBox"):
                    Music[4] = !Music[4];
                    break;
            }

        }

        private void GenerateBorders()
        {
            int[,] board = new int[,]
            {
                {0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,5,5,0,0,0,0},
                {0,0,0,0,5,5,5,5,0,0},
                {0,0,0,0,5,0,0,5,5,0},
                {0,0,0,0,5,0,0,0,5,5},
                {0,0,0,0,5,0,0,0,0,5},
                {0,0,0,0,5,0,0,0,5,0},
                {0,0,0,0,5,0,0,0,0,0},
                {0,0,0,0,5,0,0,0,0,0},
                {0,0,0,0,5,0,0,0,0,0},
                {0,0,0,0,5,0,0,0,0,0},
                {0,5,5,5,5,0,0,0,0,0},
                {5,5,5,5,5,0,0,0,0,0},
                {5,5,5,5,5,0,0,0,0,0},
                {0,5,5,5,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0}
            };

            for(int i = 0; i < board.GetLength(0); i++)
            {
                for(int j = 0; j < board.GetLength(1); j++)
                {
                    if(board[i,j] == 5)
                    {
                        board[i, j] = 7;
                    }
                }
            }
        }
    }
}