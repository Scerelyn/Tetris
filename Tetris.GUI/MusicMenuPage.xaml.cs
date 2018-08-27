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
        private Brush[] colors = new Brush[10];
        private Brush[] borderColors = new Brush[10];
        public MusicMenuPage(bool[] music)
        {
            InitializeComponent();
            Music = music;
            SetColors();
            GenerateBorders();
            GenerateSides();
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

        private void GenerateSides()
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
            DrawBoard(LeftGrid, board);

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

            DrawBoard(RightGrid, board);

        }

        //TODO The following methods are copied form main menu page. It would be best to update them to use the same methods elsewhere
        private void DrawBoard(Grid g, int[,] board)
        {
            for (int i = 0; i < 20; i++)
            {
                g.RowDefinitions.Add(new RowDefinition());
                if (i % 2 == 0)
                {
                    g.ColumnDefinitions.Add(new ColumnDefinition());
                }
                for (int j = 0; j < 10; j++)
                {
                    Label piece = GenerateLabel(board[i, j]);
                    Grid.SetRow(piece, i);
                    Grid.SetColumn(piece, j);
                    g.Children.Add(piece);
                }
            }
        }

        private void GenerateBorders()
        {


            //set left and right
            for (int i = 0; i < 2; i++)
            {
                Grid column = LeftBorder;
                if (i == 1)
                {
                    column = RightBorder;
                }
                for (int j = 0; j < 20; j++)
                {
                    column.RowDefinitions.Add(new RowDefinition());
                    Label piece = GenerateLabel(9);
                    Grid.SetRow(piece, j);
                    column.Children.Add(piece);
                }
            }
        }

        private void SetColors()
        {
            colors[0] = new SolidColorBrush(Colors.Black);
            colors[1] = new SolidColorBrush(Colors.Cyan);
            colors[2] = new SolidColorBrush(Colors.Yellow);
            colors[3] = new SolidColorBrush(Colors.DodgerBlue);
            colors[4] = new SolidColorBrush(Colors.DarkOrange);
            colors[5] = new SolidColorBrush(Colors.LawnGreen);
            colors[6] = new SolidColorBrush(Colors.Red);
            colors[7] = new SolidColorBrush(Colors.MediumPurple);
            colors[8] = new SolidColorBrush(Colors.LightGray);
            colors[9] = new SolidColorBrush(Colors.Gray);

            borderColors[0] = new SolidColorBrush(Colors.Black);
            borderColors[1] = new SolidColorBrush(Color.FromRgb(16, 200, 200));
            borderColors[2] = new SolidColorBrush(Color.FromRgb(200, 200, 16));
            borderColors[3] = new SolidColorBrush(Color.FromRgb(26, 48, 202));
            borderColors[4] = new SolidColorBrush(Color.FromRgb(230, 93, 46));
            borderColors[5] = new SolidColorBrush(Color.FromRgb(58, 197, 15));
            borderColors[6] = new SolidColorBrush(Color.FromRgb(200, 16, 16));
            borderColors[7] = new SolidColorBrush(Color.FromRgb(92, 37, 92));
            borderColors[8] = new SolidColorBrush(Colors.Gray);
            borderColors[9] = new SolidColorBrush(Color.FromRgb(67, 52, 52));
        }

        private Label GenerateLabel(int color)
        {
            Label piece = new Label();
            piece.Background = colors[color];
            piece.BorderBrush = borderColors[color];
            piece.BorderThickness = new Thickness(2);
            return piece;
        }
    }
}