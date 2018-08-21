﻿using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
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
using Tetris.GameEngine.Interfaces;

namespace Tetris.GUI
{
    /// <summary>
    /// Interaction logic for GameBoardPage.xaml
    /// </summary>
    public partial class GameBoardPage : Page, IGameView, ITetrisPage
    {
        WMPLib.WindowsMediaPlayer Player;
        private static Game tetris;
        private static Board board;
        private static System.Timers.Timer timer;
        private static System.Timers.Timer CountDowntimer;
        private static int timerCount = 0;
        private static readonly int timerStep = 10;
        private Brush[] colors = new Brush[10];
        private Brush[] borderColors = new Brush[10];
        private Label[,] locations;
        private Controller p1Controller = null;
        private System.Timers.Timer controllerPollTimer;
        private State prevControllerState;

        public GameBoardPage()
        {
            InitializeComponent();
            Focus();
            tetris = new Game(this);
            SetColors();
            GenerateBorders();
            SetBoard();
            tetris.Start();
            timer = new System.Timers.Timer(800);
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Enabled = true;
            timer.Start();

            p1Controller = new Controller(0);
            if (p1Controller.IsConnected)
            {
                controllerPollTimer = new System.Timers.Timer(10);
                controllerPollTimer.Elapsed += (source, args) =>
                {
                    if (p1Controller.IsConnected)
                    {
                        HandlControllerInput();
                    }
                };
                controllerPollTimer.Start();
                prevControllerState = p1Controller.GetState();
            }

            Console.WriteLine(board[1, 1]);
            Thread t = new Thread(NewThread);
            t.Start();
            CountDownLabel.DataContext = tetris;
            Binding b = new Binding("CountDownNum");
            CountDownLabel.SetBinding(Label.ContentProperty, b);
        }

        private void GenerateBorders()
        {
            InitializeComponent();
            LeftBorder.Children.Clear();
            LeftBorder.RowDefinitions.Clear();
            RightBorder.Children.Clear();
            RightBorder.RowDefinitions.Clear();
            for (int i = 0; i < 20; i++)
            {
                if (i != 0)
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

        private void SetColors(int themeID = 0)
        {
            switch (themeID)
            {
                case 1: //water
                    colors[0] = new SolidColorBrush(Colors.Black);
                    colors[1] = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Tetris.GUI;component/AltTextures/water-cyan.jpg", UriKind.RelativeOrAbsolute)));
                    colors[2] = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Tetris.GUI;component/AltTextures/water-yellow.jpg", UriKind.RelativeOrAbsolute)));
                    colors[3] = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Tetris.GUI;component/AltTextures/water-blue.jpg", UriKind.RelativeOrAbsolute)));
                    colors[4] = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Tetris.GUI;component/AltTextures/water-orange.jpg", UriKind.RelativeOrAbsolute)));
                    colors[5] = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Tetris.GUI;component/AltTextures/water-green.jpg", UriKind.RelativeOrAbsolute)));
                    colors[6] = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Tetris.GUI;component/AltTextures/water-red.jpg", UriKind.RelativeOrAbsolute)));
                    colors[7] = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Tetris.GUI;component/AltTextures/water-purple.jpg", UriKind.RelativeOrAbsolute)));
                    colors[8] = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Tetris.GUI;component/AltTextures/water-grey.jpg", UriKind.RelativeOrAbsolute)));
                    colors[9] = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Tetris.GUI;component/AltTextures/water-dgrey.jpg", UriKind.RelativeOrAbsolute)));

                    borderColors[0] = new SolidColorBrush(Colors.Black);
                    borderColors[1] = new SolidColorBrush(Color.FromRgb(16, 200, 200));
                    borderColors[2] = new SolidColorBrush(Color.FromRgb(150, 150, 6));
                    borderColors[3] = new SolidColorBrush(Color.FromRgb(26, 48, 202));
                    borderColors[4] = new SolidColorBrush(Color.FromRgb(230, 93, 46));
                    borderColors[5] = new SolidColorBrush(Color.FromRgb(28, 167, 5));
                    borderColors[6] = new SolidColorBrush(Color.FromRgb(200, 16, 16));
                    borderColors[7] = new SolidColorBrush(Color.FromRgb(92, 37, 92));
                    borderColors[8] = new SolidColorBrush(Colors.Gray);
                    borderColors[9] = new SolidColorBrush(Color.FromRgb(67, 52, 52));
                    break;
                case 2: //wireframe
                    colors[0] = new SolidColorBrush(Colors.Black);
                    colors[1] = new SolidColorBrush(Colors.Black);
                    colors[2] = new SolidColorBrush(Colors.Black);
                    colors[3] = new SolidColorBrush(Colors.Black);
                    colors[4] = new SolidColorBrush(Colors.Black);
                    colors[5] = new SolidColorBrush(Colors.Black);
                    colors[6] = new SolidColorBrush(Colors.Black);
                    colors[7] = new SolidColorBrush(Colors.Black);
                    colors[8] = new SolidColorBrush(Colors.Black);
                    colors[9] = new SolidColorBrush(Colors.Black);

                    borderColors[0] = new SolidColorBrush(Colors.Black);
                    borderColors[1] = new SolidColorBrush(Color.FromRgb(16, 200, 200));
                    borderColors[2] = new SolidColorBrush(Color.FromRgb(150, 150, 6));
                    borderColors[3] = new SolidColorBrush(Color.FromRgb(26, 48, 202));
                    borderColors[4] = new SolidColorBrush(Color.FromRgb(230, 93, 46));
                    borderColors[5] = new SolidColorBrush(Color.FromRgb(28, 167, 5));
                    borderColors[6] = new SolidColorBrush(Color.FromRgb(200, 16, 16));
                    borderColors[7] = new SolidColorBrush(Color.FromRgb(92, 37, 92));
                    borderColors[8] = new SolidColorBrush(Colors.Gray);
                    borderColors[9] = new SolidColorBrush(Color.FromRgb(67, 52, 52));
                    break;

                case 3: //~gradiants~
                    LinearGradientBrush[] brushes = new LinearGradientBrush[10];
                    colors[0] = new SolidColorBrush(Colors.Black);

                    brushes[1] = new LinearGradientBrush();
                    brushes[1].GradientStops.Add(new GradientStop(Colors.Cyan, 0.0));
                    brushes[1].GradientStops.Add(new GradientStop(Colors.DarkCyan, 1.0));
                    colors[1] = brushes[1];
                    brushes[2] = new LinearGradientBrush();
                    brushes[2].GradientStops.Add(new GradientStop(Colors.Yellow, 0.0));
                    brushes[2].GradientStops.Add(new GradientStop(Colors.DarkGoldenrod, 1.0));
                    colors[2] = brushes[2];
                    brushes[3] = new LinearGradientBrush();
                    brushes[3].GradientStops.Add(new GradientStop(Colors.DodgerBlue, 0.0));
                    brushes[3].GradientStops.Add(new GradientStop(Colors.Blue, 1.0));
                    colors[3] = brushes[3];
                    brushes[4] = new LinearGradientBrush();
                    brushes[4].GradientStops.Add(new GradientStop(Colors.Orange, 0.0));
                    brushes[4].GradientStops.Add(new GradientStop(Colors.OrangeRed, 1.0));
                    colors[4] = brushes[4];
                    brushes[5] = new LinearGradientBrush();
                    brushes[5].GradientStops.Add(new GradientStop(Colors.LawnGreen, 0.0));
                    brushes[5].GradientStops.Add(new GradientStop(Colors.Olive, 1.0));
                    colors[5] = brushes[5];
                    brushes[6] = new LinearGradientBrush();
                    brushes[6].GradientStops.Add(new GradientStop(Colors.Red, 0.0));
                    brushes[6].GradientStops.Add(new GradientStop(Colors.Maroon, 1.0));
                    colors[6] = brushes[6];
                    brushes[7] = new LinearGradientBrush();
                    brushes[7].GradientStops.Add(new GradientStop(Colors.MediumPurple, 0.0));
                    brushes[7].GradientStops.Add(new GradientStop(Colors.Purple, 1.0));
                    colors[7] = brushes[7];
                    brushes[8] = new LinearGradientBrush();
                    brushes[8].GradientStops.Add(new GradientStop(Colors.LightGray, 0.0));
                    brushes[8].GradientStops.Add(new GradientStop(Colors.GhostWhite, 1.0));
                    colors[8] = brushes[8];
                    brushes[9] = new LinearGradientBrush();
                    brushes[9].GradientStops.Add(new GradientStop(Colors.Black, 0.0));
                    brushes[9].GradientStops.Add(new GradientStop(Colors.DarkGray, 1.0));
                    colors[9] = brushes[9];

                    borderColors[0] = new SolidColorBrush(Colors.Black);
                    borderColors[1] = new SolidColorBrush(Color.FromRgb(16, 200, 200));
                    borderColors[2] = new SolidColorBrush(Color.FromRgb(150, 150, 6));
                    borderColors[3] = new SolidColorBrush(Color.FromRgb(26, 48, 202));
                    borderColors[4] = new SolidColorBrush(Color.FromRgb(230, 93, 46));
                    borderColors[5] = new SolidColorBrush(Color.FromRgb(28, 167, 5));
                    borderColors[6] = new SolidColorBrush(Color.FromRgb(200, 16, 16));
                    borderColors[7] = new SolidColorBrush(Color.FromRgb(92, 37, 92));
                    borderColors[8] = new SolidColorBrush(Colors.Gray);
                    borderColors[9] = new SolidColorBrush(Color.FromRgb(67, 52, 52));
                    break;
                case 4: //Vadim Gerasimov's colors
                    colors[0] = Brushes.Black;
                    colors[1] = Brushes.Maroon;
                    colors[2] = Brushes.Navy;
                    colors[3] = Brushes.Purple;
                    colors[4] = Brushes.Silver;
                    colors[5] = Brushes.DarkGreen;
                    colors[6] = Brushes.Teal;
                    colors[7] = Brushes.Brown;
                    colors[8] = Brushes.GhostWhite;
                    colors[9] = Brushes.BlueViolet;

                    borderColors[0] = Brushes.Black;
                    borderColors[1] = Brushes.Maroon;
                    borderColors[2] = Brushes.Navy;
                    borderColors[3] = Brushes.Purple;
                    borderColors[4] = Brushes.Silver;
                    borderColors[5] = Brushes.DarkGreen;
                    borderColors[6] = Brushes.Teal;
                    borderColors[7] = Brushes.Brown;
                    borderColors[8] = Brushes.GhostWhite;
                    borderColors[9] = Brushes.BlueViolet;

                    break;
                //default colors
                case 0:
                default:
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
                    break;
            }
            GenerateBorders();
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
            tetris = new Game(this);
            board = tetris.ActualBoard;
            locations = new Label[10, 20];
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
            RedrawHeld();
            DrawNexts();
        }

        private void RedrawHeld()
        {
            if (tetris.HeldPieceType() != null)
            {
                FillBlock(HeldPieceGrid, (PieceType)tetris.HeldPieceType());
            }
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
            if (grid == NextPiece1Grid)
            {
                thickness = 2;
            }
            switch (pieceType)
            {

                case PieceType.I:
                    for (int i = 0; i < 4; i++)
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
                    for (int i = 0; i < 4; i++)
                    {
                        Label cell = new Label();
                        cell.Background = colors[5];
                        cell.BorderBrush = borderColors[5];
                        cell.BorderThickness = new Thickness(thickness);
                        Grid.SetColumn(cell, sX);
                        Grid.SetRow(cell, sY);
                        if (sX != 1 || (sY == 1 && sX == 1))
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
                    int tX = 0;
                    int tY = 2;
                    for (int i = 0; i < 4; i++)
                    {
                        Label cell = new Label();
                        cell.Background = colors[7];
                        cell.BorderBrush = borderColors[7];
                        cell.BorderThickness = new Thickness(thickness);
                        Grid.SetColumn(cell, tX);
                        Grid.SetRow(cell, tY);
                        if (tX != 1)
                        {
                            tX++;
                        }
                        else if (tX == 1 && tY == 1)
                        {
                            tY++;
                            tX++;
                        }
                        else
                        {
                            tY--;
                        }
                        grid.Children.Add(cell);
                    }
                    break;
                case PieceType.O:
                    int oX = 1;
                    int oY = 1;
                    for (int i = 0; i < 4; i++)
                    {
                        Label cell = new Label();
                        cell.Background = colors[2];
                        cell.BorderBrush = borderColors[2];
                        cell.BorderThickness = new Thickness(thickness);
                        Grid.SetColumn(cell, oX);
                        Grid.SetRow(cell, oY);

                        if (oX != 2)
                        {
                            oX++;
                        }
                        else if (oY == 1 && oX == 2)
                        {
                            oX--;
                            oY++;
                        }

                        grid.Children.Add(cell);
                    }
                    break;
                case PieceType.L:
                    int lX = 0;
                    int lY = 2;
                    for (int i = 0; i < 4; i++)
                    {
                        Label cell = new Label();
                        cell.Background = colors[3];
                        cell.BorderBrush = borderColors[3];
                        cell.BorderThickness = new Thickness(thickness);
                        Grid.SetColumn(cell, lX);
                        Grid.SetRow(cell, lY);

                        if (lX != 2)
                        {
                            lX++;
                        }
                        else
                        {
                            lY--;
                        }

                        grid.Children.Add(cell);
                    }
                    break;
                case PieceType.J:
                    int jX = 0;
                    int jY = 1;
                    for (int i = 0; i < 4; i++)
                    {
                        Label cell = new Label();
                        cell.Background = colors[4];
                        cell.BorderBrush = borderColors[4];
                        cell.BorderThickness = new Thickness(thickness);
                        Grid.SetColumn(cell, jX);
                        Grid.SetRow(cell, jY);

                        if (jY != 1)
                        {
                            jX++;
                        }
                        else
                        {
                            jY++;
                        }

                        grid.Children.Add(cell);
                    }
                    break;
            }

        }

        public void Page_KeyDown(object sender, KeyEventArgs e)
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
                    if (tetris.Status != Game.GameStatus.Paused)
                    {
                        tetris.MoveRight();
                    }
                    break;
                case Key.X:
                case Key.Up:
                    if (tetris.Status != Game.GameStatus.Paused)
                    {
                        tetris.Rotate();
                    }
                    break;
                case Key.Down:
                    if (tetris.Status != Game.GameStatus.Paused)
                    {
                        tetris.MoveDown();
                    }
                    break;
                case Key.Space:
                    if (tetris.Status != Game.GameStatus.Paused)
                    {
                        tetris.SmashDown();
                    }
                    break;
                case Key.LeftCtrl:
                case Key.RightCtrl:
                case Key.Z:
                    if (tetris.Status != Game.GameStatus.Paused)
                    {
                        tetris.Rotate(false);
                    }
                    break;
                case Key.F1:
                case Key.Escape:
                    tetris.Pause();
                    if (!tetris.InCountdownState)
                    {
                        PauseLabel.Visibility = Visibility.Visible;
                    }
                    else if (tetris.InCountdownState)
                    {
                        PauseLabel.Visibility = Visibility.Hidden;
                        CountDownLabel.Visibility = Visibility.Visible;
                    }
                    break;
                case Key.LeftShift:
                case Key.RightShift:
                case Key.C:
                    if (tetris.Status != Game.GameStatus.Paused)
                    {
                        FillBlock(HeldPieceGrid, tetris.GetCurrentPieceType());
                        tetris.HoldPiece();
                    }
                    break;
                case Key.D1:
                    SetColors(0);
                    break;
                case Key.D2:
                    SetColors(1);
                    break;
                case Key.D3:
                    SetColors(2);
                    break;
                case Key.D4:
                    SetColors(3);
                    break;
                case Key.D5:
                    SetColors(4);
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

        public void Update(IGameView G)
        {
            GameBoardPage M = (GameBoardPage)G;
            this.Dispatcher.Invoke(new Action(() =>
                M.CountDownLabel.Visibility = Visibility.Hidden));

        }

        private void HandlControllerInput()
        {
            State curState = p1Controller.GetState();
            if (curState.Gamepad.Buttons != prevControllerState.Gamepad.Buttons)
            {
                switch (curState.Gamepad.Buttons)
                {
                    case GamepadButtonFlags.A:
                        if (tetris.Status != Game.GameStatus.Paused)
                        {
                            tetris.Rotate(false);
                        }
                        break;
                    case GamepadButtonFlags.B:
                        if (tetris.Status != Game.GameStatus.Paused)
                        {
                            tetris.Rotate(true);
                        }
                        break;
                    case GamepadButtonFlags.DPadUp:
                        if (tetris.Status != Game.GameStatus.Paused)
                        {
                            tetris.SmashDown();
                        }
                        break;
                    case GamepadButtonFlags.DPadDown:
                        if (tetris.Status != Game.GameStatus.Paused)
                        {
                            tetris.MoveDown();
                        }
                        break;
                    case GamepadButtonFlags.DPadLeft:
                        if (tetris.Status != Game.GameStatus.Paused)
                        {
                            tetris.MoveLeft();
                        }
                        break;
                    case GamepadButtonFlags.DPadRight:
                        if (tetris.Status != Game.GameStatus.Paused)
                        {
                            tetris.MoveRight();
                        }
                        break;
                    case GamepadButtonFlags.Start:
                        tetris.Pause();
                        Dispatcher.Invoke(() =>
                        {
                            if (!tetris.InCountdownState)
                            {
                                PauseLabel.Visibility = Visibility.Visible;
                            }
                            else if (tetris.InCountdownState)
                            {
                                PauseLabel.Visibility = Visibility.Hidden;
                                CountDownLabel.Visibility = Visibility.Visible;
                            }
                        });
                        break;
                    case GamepadButtonFlags.X:
                        if (tetris.Status != Game.GameStatus.Paused)
                        {
                            FillBlock(HeldPieceGrid, tetris.GetCurrentPieceType());
                            tetris.HoldPiece();
                        }
                        break;
                }
                Dispatcher.Invoke(() =>
                {
                    DrawPiece();
                });
                prevControllerState = curState;
            }
        }
    }
}
