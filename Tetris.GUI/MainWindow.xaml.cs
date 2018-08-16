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
using System.Timers;
using Tetris.GUI.Converters;

namespace Tetris.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WMPLib.WindowsMediaPlayer Player;
        private static Game tetris = new Game();
        private static Board board;
        private static System.Timers.Timer timer;
        private static int timerCount = 0;
        private static readonly int timerStep = 10;
        private SolidColorBrush[] colors = new SolidColorBrush[10];
        private SolidColorBrush[] borderColors = new SolidColorBrush[10];
        private Label[,] locations;

        public MainWindow()
        {
            InitializeComponent();
            SetColors();
            GenerateBorders();
            SetBoard();
            tetris.Start();
            timer = new System.Timers.Timer(800);
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Enabled = true;
            timer.Start();
            Console.WriteLine(board[1, 1]);
            Thread t = new Thread(NewThread);
            t.Start();
        }

        private void GenerateBorders()
        {
            for(int i = 0; i <20; i++)
            {
                if(i != 0)
                {
                    RightBorder.RowDefinitions.Add(new RowDefinition());
                }
                Label piece = new Label();
                piece.Background = colors[9];
                piece.BorderThickness = new Thickness(2);
                piece.BorderBrush = borderColors[9];
                Grid.SetRow(piece, i);
                RightBorder.Children.Add(piece);
            }
            for (int i = 0; i < 20; i++)
            {
                if (i != 0)
                {
                    LeftBorder.RowDefinitions.Add(new RowDefinition());
                }
                Label piece = new Label();
                piece.Background = colors[9];
                piece.BorderThickness = new Thickness(2);
                piece.BorderBrush = borderColors[9];
                Grid.SetRow(piece, i);
                LeftBorder.Children.Add(piece);
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
            borderColors[1] = new SolidColorBrush(Color.FromRgb(16,200,200));
            borderColors[2] = new SolidColorBrush(Color.FromRgb(200, 200, 16));
            borderColors[3] = new SolidColorBrush(Color.FromRgb(26, 48, 202));
            borderColors[4] = new SolidColorBrush(Color.FromRgb(230, 93, 46));
            borderColors[5] = new SolidColorBrush(Color.FromRgb(58, 197, 15));
            borderColors[6] = new SolidColorBrush(Color.FromRgb(200, 16, 16));
            borderColors[7] = new SolidColorBrush(Color.FromRgb(92, 37, 92));
            borderColors[8] = new SolidColorBrush(Colors.Gray);
            borderColors[9] = new SolidColorBrush(Color.FromRgb(67, 52, 52));

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
                    piece.Background = new SolidColorBrush(Colors.White);
                    piece.BorderThickness = new Thickness(2);
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
            Console.WriteLine(board[1, 1]);
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
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    locations[i, j].Background = colors[arr[j + 2, i]];
                    locations[i, j].BorderBrush = borderColors[arr[j + 2, i]];
                }
            }

            DrawNexts();
        }

        private void DrawNexts()
        {
            FillBlock(NextPiece1Grid, tetris.NextPieces[0].TypePiece);
            FillBlock(NextPiece2Grid, tetris.NextPieces[1].TypePiece);
            FillBlock(NextPiece3Grid, tetris.NextPieces[2].TypePiece);
            FillBlock(NextPiece4Grid, tetris.NextPieces[3].TypePiece);
            FillBlock(NextPiece5Grid, tetris.NextPieces[4].TypePiece);
            FillBlock(NextPiece6Grid, tetris.NextPieces[5].TypePiece);
        }

        private void FillBlock(Grid grid, PieceType pieceType)
        {
            grid.Children.Clear();
            int thickness = 1;
            if(grid == NextPiece1Grid)
            {
                thickness = 2;
            }
            switch (pieceType)
            {

                case PieceType.I:
                    for(int i = 0; i < 4; i++)
                    {
                        Label cell = new Label();
                        cell.Background = colors[1];
                        cell.BorderBrush = borderColors[1];
                        cell.BorderThickness = new Thickness(thickness);
                        Grid.SetColumn(cell, i);
                        Grid.SetRow(cell, 1);
                        grid.Children.Add(cell);
                    }
                    break;
                case PieceType.S:
                    int sX = 0;
                    int sY = 2;
                    for(int i = 0; i < 4; i++)
                    {
                        Label cell = new Label();
                        cell.Background = colors[5];
                        cell.BorderBrush = borderColors[5];
                        cell.BorderThickness = new Thickness(thickness);
                        Grid.SetColumn(cell, sX);
                        Grid.SetRow(cell, sY);
                        if(sX != 1 || (sY == 1 && sX == 1))
                        {
                            sX++;
                        }
                        else
                        {
                            sY--;
                        }
                        grid.Children.Add(cell);
                    }
                    break;
                case PieceType.Z:
                    int zX = 0;
                    int zY = 1;
                    for (int i = 0; i < 4; i++)
                    {
                        Label cell = new Label();
                        cell.Background = colors[6];
                        cell.BorderBrush = borderColors[6];
                        cell.BorderThickness = new Thickness(thickness);
                        Grid.SetColumn(cell, zX);
                        Grid.SetRow(cell, zY);
                        if (zX != 1 || (zY == 2 && zX == 1))
                        {
                            zX++;
                        }
                        else
                        {
                            zY++;
                        }
                        grid.Children.Add(cell);
                    }
                    break;
                case PieceType.T:
                    break;
                case PieceType.O:
                    break;
                case PieceType.L:
                    break;
                case PieceType.J:
                    break;
            }

        }

        private void Tetrid_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:

                    if (tetris.Status != Game.GameStatus.Paused)
                    {
                        tetris.MoveLeft();
                    }
                    break;
                case Key.Right:
                    if(tetris.Status != Game.GameStatus.Paused)
                    {
                        tetris.MoveRight();
                    }
                    break;
                case Key.X:
                case Key.Up:
                    if(tetris.Status != Game.GameStatus.Paused)
                    {
                        tetris.Rotate();
                    }
                    break;
                case Key.Down:
                    if(tetris.Status != Game.GameStatus.Paused)
                    {
                        tetris.MoveDown();
                    }
                    break;
                case Key.Space:
                    if(tetris.Status != Game.GameStatus.Paused)
                    {
                        tetris.SmashDown();
                    }
                    break;
                case Key.LeftCtrl:
                case Key.RightCtrl:
                case Key.Z:
                    if(tetris.Status != Game.GameStatus.Paused)
                    {
                        tetris.Rotate(false);
                    }
                    break;
                case Key.F1:
                case Key.Escape:
                    tetris.Pause();
                    if(tetris.Status == Game.GameStatus.Paused)
                    {
                        PauseLabel.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        PauseLabel.Visibility = Visibility.Hidden;
                    }
                    break;
            }
            DrawPiece();
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
                PlayFile("TypeA.mp3");
            }
        }

        private void NewThread()
        {
            PlayFile("TypeA.mp3");
        }
    }
}
