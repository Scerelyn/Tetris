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
        WMPLib.WindowsMediaPlayer Player = new WMPLib.WindowsMediaPlayer();
        private static Game tetris;
        private static Board board;
        private static System.Timers.Timer timer;
        private static int timerCount = 0;
        private static int scoreToNext = 1000;
        private static readonly int timerStep = 10;
        private static int level = 1;
        private Brush[] colors = new Brush[10];
        private Brush[] borderColors = new Brush[10];
        private Label[,] locations;
        private Controller p1Controller = null;
        private System.Timers.Timer controllerPollTimer;
        private System.Timers.Timer controllerConnectionTimer;
        private State prevControllerState;
        private bool controllerWasIn = false;
        private WMPLib.WindowsMediaPlayer Player2 = new WMPLib.WindowsMediaPlayer();
        private Random rand = new Random();
        private Thread t;
        private System.Timers.Timer gameTime;
        private int minutes = 0;
        private int seconds = 0;
        private bool IsUltra = false;
        private bool[] Music;

        public GameBoardPage(bool isUltra, bool[] music)
        {
            InitializeComponent();
            Music = music;
            IsUltra = isUltra;
            Focus();
            PieceFactory.ResetRandomizer();
            tetris = new Game(this);
            SetColors();
            GenerateBorders();
            SetBoard();
            tetris.Start();
            timer = new System.Timers.Timer(800);
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Enabled = true;
            timer.Start();
            gameTime = new System.Timers.Timer(1000);
            gameTime.Elapsed += new ElapsedEventHandler(GameTimeEvent);
            gameTime.Enabled = true;
            gameTime.Start();

            controllerConnectionTimer = new System.Timers.Timer(500);
            controllerConnectionTimer.Elapsed += ControllerConnectionTimedEvent;
            controllerConnectionTimer.Start();
            p1Controller = new Controller(0);
            controllerPollTimer = new System.Timers.Timer(20);
            controllerPollTimer.Elapsed += HandlControllerInput;
            if (p1Controller.IsConnected)
            {
                controllerPollTimer.Start();
                prevControllerState = p1Controller.GetState();
                controllerWasIn = true;
                HelpLabel_MovePieces.Content = "DPad left/right - Move piece";
                HelpLabel_RotateCCW.Content = "B - Rotate counterclockwise";
                HelpLabel_RotateCW.Content = "A - Rotate clockwise";
                HelpLabel_HoldPiece.Content = "X - Hold piece";
                HelpLabel_HardDropPiece.Content = "DPad up - Drop piece to bottom";
                HelpLabel_SoftDropPiece.Content = "DPad down - Drop piece one row";
                HelpLabel_Pause.Content = "Start - Pause";
            }

            Console.WriteLine(board[1, 1]);
            t = new Thread(NewThread);
            t.Start();
            CountDownLabel.DataContext = tetris;
            Binding b = new Binding("CountDownNum");
            CountDownLabel.SetBinding(Label.ContentProperty, b);
            PieceFactory.ResetRandomizer();
            tetris.Pause();
            CountDownLabel.Visibility = Visibility.Visible;
            tetris.Pause();
        }

        private void GenerateBorders()
        {
            InitializeComponent();
            LeftBorder.Children.Clear();
            LeftBorder.RowDefinitions.Clear();
            RightBorder.Children.Clear();
            RightBorder.RowDefinitions.Clear();
            for (int i = 0; i < 21; i++)
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
            for (int i = 0; i < 21; i++)
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
                    colors = Themes.Water.FillBrushes;
                    borderColors = Themes.Water.BorderBrushes;
                    break;
                case 2: //wireframe
                    colors = Themes.XRay.FillBrushes;
                    borderColors = Themes.XRay.BorderBrushes;
                    break;
                case 3: //~gradiants~
                    colors = Themes.ClassicGradiant.FillBrushes;
                    borderColors = Themes.ClassicGradiant.BorderBrushes;
                    break;
                case 4: //Vadim Gerasimov's colors
                    colors = Themes.VladimClassic.FillBrushes;
                    borderColors = Themes.VladimClassic.BorderBrushes;
                    break;
                case 5: //Terminal ish theme
                    colors = Themes.PlainText.FillBrushes;
                    borderColors = Themes.PlainText.BorderBrushes;
                    break;
                case 6: //haha funny ee moh gees
                    colors = Themes.Emoji.FillBrushes;
                    borderColors = Themes.Emoji.BorderBrushes;
                    break;
                //default colors
                case 0:
                default:
                    colors = Themes.Classic.FillBrushes;
                    borderColors = Themes.Classic.BorderBrushes;
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

        private void GameTimeEvent(object source, ElapsedEventArgs e)
        {
            if (tetris.Status != Game.GameStatus.Finished && tetris.Status != Game.GameStatus.Paused)
            {
                if (seconds == 59)
                {
                    seconds = 0;
                    minutes++;
                    if (IsUltra && minutes == 3)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            tetris.Status = Game.GameStatus.Paused;
                            GameOverLabel.Visibility = Visibility.Visible;
                            Player.PlayStateChange -= Player_PlayStateChange;
                            Player.controls.stop();
                            Player.PlayStateChange -= Player_PlayStateChange;
                            Player2.URL = "./Sounds/GameOver.mp3";
                            Player2.controls.play();
                            timer.Stop();
                        });
                    }
                }
                else
                {
                    seconds++;
                }
                Dispatcher.Invoke(() =>
                {
                    TimeBoard.Content = "Time: " + minutes + ":" + seconds.ToString("00");
                });
            }

        }

        /// <summary>
        /// Handles controller connected behavior when it is unplugged or plugged in during gameplay
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ControllerConnectionTimedEvent(object sender, ElapsedEventArgs args)
        {
            if (p1Controller.IsConnected && !controllerWasIn) //controller just connected in, and was not on before
            {
                controllerPollTimer.Start();
                prevControllerState = p1Controller.GetState();
                controllerWasIn = true;
                Dispatcher.Invoke(()=> 
                {
                    HelpLabel_MovePieces.Content = "DPad left/right - Move piece";
                    HelpLabel_RotateCCW.Content = "B - Rotate counterclockwise";
                    HelpLabel_RotateCW.Content = "A - Rotate clockwise";
                    HelpLabel_HoldPiece.Content = "X - Hold piece";
                    HelpLabel_HardDropPiece.Content = "DPad up - Drop piece to bottom";
                    HelpLabel_SoftDropPiece.Content = "DPad down - Drop piece one row";
                    HelpLabel_Pause.Content = "Start - Pause";
                });
            }
            else if (!p1Controller.IsConnected && controllerWasIn) //controller was pluggin in before, but then was disconnected
            {
                controllerPollTimer.Stop();
                controllerWasIn = false;
                Dispatcher.Invoke(() => 
                {
                    HelpLabel_MovePieces.Content = "LEFT/RIGHT - Move piece";
                    HelpLabel_RotateCCW.Content = "CTRL/Z - Rotate counterclockwise";
                    HelpLabel_RotateCW.Content = "UP/X - Rotate clockwise";
                    HelpLabel_HoldPiece.Content = "X - Hold piece";
                    HelpLabel_HardDropPiece.Content = "Space - Drop piece to bottom";
                    HelpLabel_SoftDropPiece.Content = "DOWN - Drop piece one row";
                    HelpLabel_Pause.Content = "ESC - Pause";
                });
            }
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Console.WriteLine(board[1, 1]);

            if (tetris.Status != Game.GameStatus.Finished)
            {
                if (tetris.Status != Game.GameStatus.Paused)
                {
                    timerCount += timerStep;
                    int linesbefore = tetris.Lines;
                    tetris.MoveDown();
                    if (linesbefore < tetris.Lines)
                    {
                        Player2.URL = "./Sounds/ClearLine.mp3";
                        Player2.controls.play();
                    }
                    if (tetris.Status == Game.GameStatus.Finished)
                    {
                        GameOverLabel.Visibility = Visibility.Visible;
                        Player.PlayStateChange -= Player_PlayStateChange;
                        Player.controls.stop();
                        Player.PlayStateChange -= Player_PlayStateChange;
                        Player2.URL = "./Sounds/GameOver.mp3";
                        Player2.controls.play();
                        timer.Stop();
                    }
                    else
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            DrawPiece();

                        });
                        if (scoreToNext <= tetris.Score)
                        {
                            timer.Interval -= 50;
                            timerCount = 0;
                            level++;
                            Dispatcher.Invoke(() =>
                            {
                                LevelLabel.Content = "Level: " + level;
                            });
                            scoreToNext += 1000;
                        }
                    }
                }
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                    {
                        GameOverLabel.Visibility = Visibility.Visible;
                        Player.PlayStateChange -= Player_PlayStateChange;
                        Player.controls.stop();
                        Player.PlayStateChange -= Player_PlayStateChange;
                        Player2.URL = "./Sounds/GameOver.mp3";
                        Player2.controls.play();
                        timer.Stop();
                    });

            }
        }
        private void DrawPiece()
        {
            ScoreBoard.Content = "Score: " + tetris.Score;
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
                        Player2.URL = "./Sounds/MovePiece.mp3";
                        Player2.controls.play();
                    }
                    break;
                case Key.Right:
                    if (tetris.Status != Game.GameStatus.Paused)
                    {
                        tetris.MoveRight();
                        Player2.URL = "./Sounds/MovePiece.mp3";
                        Player2.controls.play();
                    }
                    break;
                case Key.X:
                case Key.Up:
                    if (tetris.Status != Game.GameStatus.Paused)
                    {
                        tetris.Rotate();
                        Player2.URL = "./Sounds/RotatePiece.mp3";
                        Player2.controls.play();
                    }
                    break;
                case Key.Down:
                    if (tetris.Status != Game.GameStatus.Paused)
                    {
                        if (tetris.Status == Game.GameStatus.Finished)
                        {
                            GameOverLabel.Visibility = Visibility.Visible;
                            Player.PlayStateChange -= Player_PlayStateChange;
                            Player.controls.stop();
                            Player.PlayStateChange -= Player_PlayStateChange;
                            Player2.URL = "./Sounds/GameOver.mp3";
                            Player2.controls.play();
                            timer.Stop();
                        }
                        int linesbefore = tetris.Lines;
                        tetris.MoveDown();
                        if (linesbefore < tetris.Lines)
                        {
                            Player2.URL = "./Sounds/ClearLine.mp3";
                            Player2.controls.play();
                        }

                    }
                    break;
                case Key.Space:
                    if (tetris.Status != Game.GameStatus.Paused)
                    {
                        if (tetris.Status == Game.GameStatus.Finished)
                        {
                            GameOverLabel.Visibility = Visibility.Visible;
                            Player.PlayStateChange -= Player_PlayStateChange;
                            Player.controls.stop();
                            Player.PlayStateChange -= Player_PlayStateChange;
                            Player2.URL = "./Sounds/GameOver.mp3";
                            Player2.controls.play();
                            timer.Stop();
                        }
                        int linesbefore = tetris.Lines;
                        tetris.SmashDown();
                        if (linesbefore < tetris.Lines)
                        {
                            Player2.URL = "./Sounds/ClearLine.mp3";
                            Player2.controls.play();
                        }
                        else
                        {
                            Player2.URL = "./Sounds/LockPiece.mp3";
                            Player2.controls.play();
                        }

                    }
                    break;
                case Key.LeftCtrl:
                case Key.RightCtrl:
                case Key.Z:
                    if (tetris.Status != Game.GameStatus.Paused)
                    {
                        tetris.Rotate(false);
                        Player2.URL = "./Sounds/RotatePiece.mp3";
                        Player2.controls.play();
                    }
                    break;
                case Key.F1:
                case Key.Escape:
                    if (!tetris.InCountdownState)
                    {
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
                case Key.D6:
                    SetColors(5);
                    break;
                case Key.D7:
                    SetColors(6);
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
                List<int> list = new List<int>();
                for (int i = 0; i < Music.Length; i++)
                {
                    if (Music[i])
                    {
                        list.Add(i);
                    }
                }
                int a = 5;
                if (list.Count > 0)
                {
                    a = list[rand.Next(0, list.Count)];

                }
                switch (a)
                {
                    case 0:
                        PlayFile("./Sounds/TypeA.mp3");
                        break;
                    case 1:
                        PlayFile("./Sounds/TypeB.mp3");
                        break;
                    case 2:
                        PlayFile("./Sounds/Katyusha.mp3");
                        break;
                    case 3:
                        PlayFile("./Sounds/HeartOfFire.mp3");
                        break;
                    case 4:
                        PlayFile("./Sounds/IevanPolkka.mp3");
                        break;
                    default:
                        break;
                }
            }
        }


        private void NewThread()
        {
            List<int> list = new List<int>();
            for (int i = 0; i < Music.Length; i++)
            {
                if (Music[i])
                {
                    list.Add(i);
                }
            }
            int a = 5;
            if (list.Count > 0)
            {
                a = list[rand.Next(0, list.Count)];

            }
            switch (a)
            {
                case 0:
                    PlayFile("./Sounds/TypeA.mp3");
                    break;
                case 1:
                    PlayFile("./Sounds/TypeB.mp3");
                    break;
                case 2:
                    PlayFile("./Sounds/Katyusha.mp3");
                    break;
                case 3:
                    PlayFile("./Sounds/HeartOfFire.mp3");
                    break;
                case 4:
                    PlayFile("./Sounds/IevanPolkka.mp3");
                    break;
                default:
                    break;
            }
        }

        public void Update(IGameView G)
        {
            GameBoardPage M = (GameBoardPage)G;
            this.Dispatcher.Invoke(new Action(() =>
                M.CountDownLabel.Visibility = Visibility.Hidden));

        }

        private void HandlControllerInput(object sender, EventArgs args)
        {
            controllerPollTimer.Stop();
            if (p1Controller.IsConnected)
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
                                if (tetris.Status == Game.GameStatus.Finished)
                                {
                                    GameOverLabel.Visibility = Visibility.Visible;
                                    Player.PlayStateChange -= Player_PlayStateChange;
                                    Player.controls.stop();
                                    Player.PlayStateChange -= Player_PlayStateChange;
                                    Player2.URL = "./Sounds/GameOver.mp3";
                                    Player2.controls.play();
                                    timer.Stop();
                                }
                                int linesbefore = tetris.Lines;
                                tetris.SmashDown();
                                if (linesbefore > tetris.Lines)
                                {
                                    Player2.URL = "./Sounds/ClearLine.mp3";
                                    Player2.controls.play();
                                }
                                else
                                {
                                    Player2.URL = "./Sounds/LockPiece.mp3";
                                    Player2.controls.play();
                                }
                            }
                            break;
                        case GamepadButtonFlags.DPadDown:
                            if (tetris.Status != Game.GameStatus.Paused)
                            {
                                if (tetris.Status == Game.GameStatus.Finished)
                                {
                                    GameOverLabel.Visibility = Visibility.Visible;
                                    Player.PlayStateChange -= Player_PlayStateChange;
                                    Player.controls.stop();
                                    Player.PlayStateChange -= Player_PlayStateChange;
                                    Player2.URL = "./Sounds/GameOver.mp3";
                                    Player2.controls.play();
                                    timer.Stop();
                                }
                                int linesbefore = tetris.Lines;
                                tetris.MoveDown();
                                if (linesbefore > tetris.Lines)
                                {
                                    Player2.URL = "./Sounds/ClearLine.mp3";
                                    Player2.controls.play();
                                }
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
                                Dispatcher.Invoke(() => 
                                {
                                    FillBlock(HeldPieceGrid, tetris.GetCurrentPieceType());
                                });
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
            controllerPollTimer.Start();
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            Player.PlayStateChange -= Player_PlayStateChange;
            Player.controls.stop();
            Player.PlayStateChange += Player_PlayStateChange;
            t.Abort();
            controllerPollTimer.Stop();
            controllerPollTimer.Elapsed -= HandlControllerInput;
            controllerConnectionTimer.Stop();
            controllerConnectionTimer.Elapsed -= ControllerConnectionTimedEvent;
            MainWindow mw = (MainWindow)Parent;
            mw.ReturnToMenu();
        }
    }
}
