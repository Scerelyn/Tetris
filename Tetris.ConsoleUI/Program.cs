using System;
using System.Threading;
using Tetris.GameEngine;
using System.Timers;
using SharpDX.XInput;

namespace TetrisConsoleUI
{
    class TetrisConsoleUI
    {
        private static Game _game;
        private static ConsoleDrawing _drawer;
        private static System.Timers.Timer _gameTimer;
        private static int _timerCounter = 0;
        private static readonly int _timerStep = 10;
        private static Controller controller;
        private static System.Timers.Timer controllerPollTimer;
        static int Main(string[] args)
        {
            //preparing Console
            Console.Clear();
            Console.CursorVisible = false;

            _drawer = new ConsoleDrawing();

            ConsoleDrawing.ShowControls();

            Console.ReadKey(true);
            Console.Clear();

            _game = new Game();
            _game.Start();
            _gameTimer = new System.Timers.Timer(800);
            _gameTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            _gameTimer.Start();

            
            _drawer.DrawScene(_game);

            controller = new Controller(0);
            controllerPollTimer = new System.Timers.Timer(80);
            controllerPollTimer.Elapsed += ControllerPoll;
            controllerPollTimer.Start();


            while (_game.Status != Game.GameStatus.Finished)
            {
                if (Console.KeyAvailable)
                {
                    KeyPressedHandler(Console.ReadKey(true));
                    _drawer.DrawScene(_game);
                    _gameTimer.Enabled = true;
                }
            }
            _gameTimer.Stop();
            _drawer.ShowGameOver(_game);

            Console.ResetColor();
            Console.CursorVisible = true;
            return 0;
        }

        private static void ControllerPoll(object sender, ElapsedEventArgs e)
        {
            if (_game.Status == Game.GameStatus.Finished)
            {
                controllerPollTimer.Stop();
            }
            if (controller.IsConnected)
            {
                ControllerButtonHandler();
                _drawer.DrawScene(_game);
            }
        }

        private static void ControllerButtonHandler()
        {
            State prevState = controller.GetState();
            switch (prevState.Gamepad.Buttons)
            {
                case GamepadButtonFlags.A:
                    _game.Rotate(false);
                    break;
                case GamepadButtonFlags.B:
                    _game.Rotate(true);
                    break;
                case GamepadButtonFlags.DPadUp:
                    _game.SmashDown();
                    break;
                case GamepadButtonFlags.DPadDown:
                    _game.MoveDown();
                    break;
                case GamepadButtonFlags.DPadLeft:
                    _game.MoveLeft();
                    break;
                case GamepadButtonFlags.DPadRight:
                    _game.MoveRight();
                    break;
                case GamepadButtonFlags.Start:
                    _game.Pause();
                    break;
                case GamepadButtonFlags.X:
                    _game.HoldPiece();
                    break;
            }
        }

        private static void KeyPressedHandler(ConsoleKeyInfo input_key)
        {
            switch (input_key.Key)
            {
                case ConsoleKey.LeftArrow:
                    if (_game.Status != Game.GameStatus.Paused)
                        _game.MoveLeft();
                    break;
                case ConsoleKey.Z:
                    if (_game.Status != Game.GameStatus.Paused)
                        _game.MoveLeft();
                    break;
                case ConsoleKey.RightArrow:
                    if (_game.Status != Game.GameStatus.Paused)
                        _game.MoveRight();
                    break;
                case ConsoleKey.UpArrow:
                    if (_game.Status != Game.GameStatus.Paused)
                        _game.Rotate();
                    break;
                case ConsoleKey.DownArrow:
                    if (_game.Status != Game.GameStatus.Paused)
                    {
                        _game.MoveDown();
                        _gameTimer.Enabled = false;
                    }
                    break;
                case ConsoleKey.Spacebar:
                    if (_game.Status != Game.GameStatus.Paused)
                        _game.SmashDown();
                    break;
                case ConsoleKey.N:
                    _game.NextPieceMode = !_game.NextPieceMode;
                    break;
                case ConsoleKey.G:
                    _game.ShadowPieceMode = !_game.ShadowPieceMode;
                    break;
                case ConsoleKey.P:
                    _game.Pause();
                    break;
                case ConsoleKey.Escape:
                    _game.GameOver();
                    break;
                case ConsoleKey.C:
                    _game.HoldPiece();
                    break;
                default:
                    break;
            }
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
#if DEBUG
            (source as System.Timers.Timer).Stop();
#endif
            if (_game.Status != Game.GameStatus.Finished)
            {
                if (_game.Status != Game.GameStatus.Paused)
                {
                    _timerCounter += _timerStep;
                    _game.MoveDown();
                    if (_game.Status == Game.GameStatus.Finished)
                    {
                        _gameTimer.Stop();
                    }
                    else
                    {
                        _drawer.DrawScene(_game);
                        if (_timerCounter >= (1000 - (_game.Lines * 10)))
                        {
                            _gameTimer.Interval -= 50;
                            _timerCounter = 0;
                        }
                    }
                }
                else if (_game.CountDownNum > 0)
                {
                    _drawer.DrawScene(_game);
                }
            }
#if DEBUG
            (source as System.Timers.Timer).Start();
#endif
        }
    }
}