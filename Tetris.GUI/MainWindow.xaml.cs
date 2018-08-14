using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
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
using Tetris.GUI.Converters;

namespace Tetris.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private static Game tetris = new Game();
        private static Board board;
        private static Timer timer;
        private static int timerCount = 0;
        private static readonly int timerStep = 10;
        private SolidColorBrush[] colors = new SolidColorBrush[9];
        private Label[,] locations;


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
            tetris = new Game();
            board = tetris.ActualBoard;
           locations = new Label[10, 20];
            StandardTetrisNumToColorConverter convert = new StandardTetrisNumToColorConverter();
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 20; j++)
                {

                    Label piece = new Label();
                    piece.Background = new SolidColorBrush(Colors.LawnGreen);
                    //var binding = new Binding();
                    //piece.DataContext = board[j, i];
                    //binding.Source = board[j, i];
                    //binding.Converter = convert;
                    //binding.Mode = BindingMode.Default;
                    //binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                    //piece.SetBinding(BackgroundProperty, binding);
                    locations[i, j] = piece;
                    Grid.SetColumn(piece, i);
                    Grid.SetRow(piece, j);
                    Tetrid.Children.Add(piece);
                }
            }

            DrawPiece();
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Console.WriteLine(board[1,1]);
            if (tetris.Status != Game.GameStatus.Finished)
            {
                if (tetris.Status != Game.GameStatus.Paused)
                {
                    timerCount += timerStep;
                    tetris.MoveDown();
                    if (tetris.Status == Game.GameStatus.Finished)
                    {
                        timer.Stop();
                    }
                    else
                    {
                        this.Dispatcher.Invoke(() =>

                            {
                                DrawPiece();
                            });
                        
                        if (timerCount >= (1000 - (tetris.Lines * 10)))
                        {
                            timer.Interval -= 50;
                            timerCount = 0;
                        }
                    }
                }
            }
        }

        private void DrawPiece()
        {
            board = tetris.ActualBoard;
            int[,] arr = board.ToArray();
            for(int i = 0; i < 10; i++)
            {
                for(int j = 0; j < 20; j++)
                {
                    locations[i, j].Background = colors[arr[j, i]];
                }
            }
        }
    }
}
