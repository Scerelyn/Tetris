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
    /// Interaction logic for MainMenuPage.xaml
    /// </summary>
    public partial class MainMenuPage : Page, ITetrisPage
    {
        private Brush[] colors = new Brush[10];
        private Brush[] borderColors = new Brush[10];
        public MainMenuPage()
        {
            InitializeComponent();
            SetColors();
            GenerateBorders();
            GenerateLeftBoard();
        }

        private void GenerateLeftBoard()
        {
            int[,] leftBoard = new int[,]
                {
                    {4,4,5,5,6,6,0,7,1,3},
                    {4,7,0,5,6,6,6,3,1,1},
                    {4,7,0,6,2,2,0,5,5,1},
                    {3,3,3,0,2,2,5,5,0,1},
                    {4,4,4,6,6,3,0,2,2,3},
                    {0,7,4,5,6,6,0,2,2,3},
                    {3,7,7,5,5,1,1,1,1,0},
                    {3,7,1,0,5,0,0,5,5,0},
                    {3,3,1,0,7,0,5,5,2,2},
                    {0,6,1,7,7,7,6,6,2,2},
                    {6,6,1,0,4,4,0,6,6,4},
                    {6,7,7,7,4,5,0,1,0,4},
                    {0,0,7,0,4,5,5,1,5,5},
                    {2,2,1,3,5,5,0,1,2,2},
                    {0,6,1,5,5,3,3,4,4,4},
                    {6,6,1,0,0,7,3,5,0,4},
                    {6,0,6,6,7,7,3,5,5,0},
                    {4,4,4,6,6,7,0,3,5,0},
                    {0,7,4,2,2,3,3,3,0,0},
                    {7,7,7,2,2,0,1,1,1,1}
                };
            DrawBoard(LeftGrid, leftBoard);
            int[,] rightBoard = new int[,]
            {
                {2,2,6,0,5,5,3,7,4,1},
                {1,4,4,5,5,0,7,7,7,7},
                {1,4,2,2,0,6,3,3,7,7},
                {1,4,2,2,6,6,0,3,3,7},
                {1,5,5,4,6,6,6,3,3,0},
                {5,5,0,4,4,4,6,6,3,3},
                {0,1,0,6,2,2,4,5,7,0},
                {0,1,6,6,2,2,4,4,4,3},
                {1,1,6,0,5,5,4,3,3,3},
                {1,2,2,5,5,0,4,4,4,6},
                {1,2,2,0,3,7,7,7,6,6},
                {1,0,3,3,3,7,7,1,6,6},
                {5,5,1,4,3,3,0,1,6,0},
                {0,5,1,2,2,3,4,1,2,2},
                {0,6,1,2,2,3,4,1,2,2},
                {6,6,1,0,3,4,4,5,5,1},
                {6,7,3,3,3,6,6,0,5,1},
                {7,7,7,4,0,7,6,6,3,1},
                {0,5,5,4,7,7,2,2,3,1},
                {5,5,4,4,0,7,2,2,3,3},
            };
            DrawBoard(RightGrid, rightBoard);
        }

        private void DrawBoard(Grid g, int[,] board)
        {
            for(int i = 0; i < 20; i++)
            {
                g.RowDefinitions.Add(new RowDefinition());
                if(i % 2 == 0)
                {
                    g.ColumnDefinitions.Add(new ColumnDefinition());
                }
                for(int j = 0; j < 10; j++)
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
            int rowIndex = 0;
            int colIndex = 0;
            //set corners
            for(int i = 0; i < 4; i++)
            {
                Label piece = GenerateLabel(9);
                Grid.SetRow(piece, rowIndex);
                Grid.SetColumn(piece, colIndex);
                MidGrid.Children.Add(piece);

                if(rowIndex == 2)
                {
                    rowIndex = 0;
                    colIndex = 2;
                }
                else
                {
                    rowIndex = 2;
                }
            }

            //set left and right
            for(int i = 0; i < 2; i++)
            {
                Grid column = LeftBorder;
                if(i == 1 )
                {
                    column = RightBorder;
                }
                for(int j = 0; j < 18; j++)
                {
                    column.RowDefinitions.Add(new RowDefinition());
                    Label piece = GenerateLabel(9);
                    Grid.SetRow(piece, j);
                    column.Children.Add(piece);
                }
            }

            //set top and bottom
            for(int i = 0; i < 2;i++)
            {
                Grid row = TopBorder;
                if(i == 1)
                {
                    row = BottomBorder;
                }
                for(int j = 0; j < 8; j++)
                {
                    row.ColumnDefinitions.Add(new ColumnDefinition());
                    Label piece = GenerateLabel(9);
                    Grid.SetColumn(piece, j);
                    row.Children.Add(piece);
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

        public void Page_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = (MainWindow)this.Parent;
            mw.StartNewGame();
        }
    }
}
