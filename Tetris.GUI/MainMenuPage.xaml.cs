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
            rowIndex = 0;
            colIndex = 0;
            for(int i = 0; i < 2; i++)
            {
                Grid column = new Grid();
                for(int j = 0; j < 8; j++)
                {

                }

                colIndex = 2;
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
