﻿using System;
using System.Timers;
using Tetris.GameEngine.Interfaces;

namespace Tetris.GameEngine
{
    public class Game : IMovable, IMode
    {
        private const int _default_board_width = 10;
        private const int _default_board_height = 22;

        public enum GameStatus
        {
            ReadyToStart,
            InProgress,
            Paused,
            Finished
        }

        #region Private Fields

        /// <summary>
        /// The Playfield
        /// </summary>
        private Board _gameBoard;
        private GameStatus _status;
        private Piece _currPiece;
        private Piece _nextPiece;
        private Random _rnd;
        private int _posX;
        private int _posY;
        private int _lines;
        private int _score;
        private Piece _heldPiece = null;

        #endregion

        #region Constructors

        public Game()
        {
            _gameBoard = new Board(_default_board_width, _default_board_height);

            _currPiece = null;
            _nextPiece = null;
            _status = GameStatus.ReadyToStart;
            ShadowPieceMode = true;
            NextPieceMode = true;
            _rnd = new Random();
            _posX = _posY = 0;
            _lines = 0;
            _score = 0;
        }

        #endregion

        #region Public Methods

        public void Start()
        {
            if (this._status != GameStatus.ReadyToStart)
            {
                throw new InvalidOperationException("Only game with status 'ReadyToStart' can be started");
            }
            this._status = GameStatus.InProgress;
            DropNewPiece();
        }


        public void Pause()
        {
            if (this._status == GameStatus.InProgress)
            {
                this._status = GameStatus.Paused;
            }
            else if (this._status == GameStatus.Paused)
            {
                StartCountDown();
            }
            else
            {
                return;
            }
        }

        private void StartCountDown()
        {
            TimerNum.Elapsed += CountdownEvent;
            TimerNum.Interval = 1000;
            TimerNum.Start();
            String s = TimerNum.ToString();
        }
        public void CountdownEvent(Object Sender, ElapsedEventArgs args)
        {
            CountDownNum--;
            if (CountDownNum == 0)
            {
                TimerNum.Stop();
                CountDownNum = 3;
                this._status = GameStatus.InProgress;
            }
        }

        public void GameOver()
        {
            {
                throw new InvalidOperationException("Only game with status 'InProgress' or 'Pause'  can be finished");
            }
            _status = GameStatus.Finished;
        }

        public void HoldPiece()
        {
            DropNewPiece(_currPiece.InitialRotationState);
        }

        #endregion

        #region Public Properties

        public int PosX
        {
            {
                return this._posX;
            }
        }

        public int PosY
        {
            get
            {
                return this._posY;
            }
        }

        public Board ActualBoard
        {
            get
            {
                if (this.Status == GameStatus.ReadyToStart)
                {
                    return this._gameBoard;
                }
                Board tmp_board = (Board)_gameBoard.Clone();
                Piece tmp_piece = (Piece)_currPiece.Clone();
                if (ShadowPieceMode == true)
                {
                    Piece shadow_piece = (Piece)_currPiece.Clone();
                    tmp_board.FixShadowPiece(shadow_piece, _posX, _posY);
                }
                tmp_board.FixPiece(tmp_piece, _posX, _posY);
                return tmp_board;
            }
        }

        public Piece NextPiece
        {
            }
        }

        public Piece CurrPiece
        {
            get
            {
                return _currPiece;
            }
        }

        public GameStatus Status
        {
            {
            }
        }

        public int Lines
        {
            }
        }

        public int Score
        {
            }
        }
        public Timer TimerNum { get; private set; } = new Timer();
        public int CountDownNum { get; private set; } = 3;

        #endregion

        #region Private Methods

        private void Step()
        {
            if (this.Status == GameStatus.InProgress)
            {
                if (_gameBoard.CanPosAt(_currPiece, _posX, _posY + 1))
                {
                    _posY++;
                }
                else
                {
                    _gameBoard.FixPiece(_currPiece, _posX, _posY);
                    int currLinesMade = _gameBoard.CheckLines();
                    _lines += currLinesMade;
                    switch (currLinesMade)
                    {
                        case 1:
                            _score += 40;
                            break;
                        case 2:
                            _score += 100;
                            break;
                        case 3:
                            _score += 300;
                            break;
                        case 4:
                            _score += 1200;
                            break;
                    }

                    if (_gameBoard.IsTopReached())
                    {
                        GameOver();
                    }
                    else
                    {
                        DropNewPiece();
                    }
                }
            }
        }

        {
            if (pieceToHold != null) //if dropnewpiece was called with a piece to hold,
            {
                if (_heldPiece != null) // if we have a held piece ready
                {
                    Piece p = _currPiece; //temp
                    _currPiece = _heldPiece; //swap the current piece with the held on
                    _heldPiece = p.InitialRotationState; //then swap the heldpiece
                    _posY = _currPiece.InitPosY; //reset positions
                    _posX = ((_gameBoard.Width - 1) / 2) + _currPiece.InitPosX;
                }
                else //if the _heldPiece is not null
                {
                    _heldPiece = pieceToHold.InitialRotationState; //store it
                    //then just call in the next piece
                    _rnd = new Random(DateTime.Now.Millisecond);
                    _currPiece = (_nextPiece != null) ? _nextPiece : PieceFactory.GetRandomPiece(_rnd);
                    _posY = _currPiece.InitPosY;
                    _posX = ((_gameBoard.Width - 1) / 2) + _currPiece.InitPosX;
                    _nextPiece = PieceFactory.GetRandomPiece(_rnd);
                }
            }
            else //else just act as if we just wanted a new piece
            {
                _rnd = new Random(DateTime.Now.Millisecond);
                _posY = _currPiece.InitPosY;
                _nextPiece = PieceFactory.GetRandomPiece(_rnd);
            }
        }

        #endregion

        #region IMovable Implementation

        public void MoveRight()
        {
            if (_gameBoard.CanPosAt(_currPiece, _posX + 1, _posY))
            {
                _posX++;
            }
        }

        public void MoveLeft()
        {
            if (_gameBoard.CanPosAt(_currPiece, _posX - 1, _posY))
            {
                _posX--;
            }
        }

        public void MoveDown()
        {
            Step();
        }

        public void SmashDown()
        {
            while (_gameBoard.CanPosAt(_currPiece, _posX, _posY + 1))
            {
                Step();
            }
            MoveDown();
        }

        {
            Piece tmp_piece = goRight ? _currPiece.RotateRight() : _currPiece.RotateLeft();
            if (_gameBoard.CanPosAt(tmp_piece, _posX, _posY))
            {
                _currPiece = tmp_piece;
                return;
            }
            else
            {
                //wall kicking logic here
                {
                    if (_gameBoard.CanPosAt(tmp_piece, _posX + 1, _posY)) // attempt rightward wall kick
                    {
                        _currPiece = tmp_piece;
                        _posX++;
                    }
                    else if (_gameBoard.CanPosAt(tmp_piece, _posX + 1, _posY + 1)) // attempt down right wall kick
                    {
                        _currPiece = tmp_piece;
                        _posX++;
                        _posY++;
                    }
                    else if (_gameBoard.CanPosAt(tmp_piece, _posX - 1, _posY)) // attempt leftward wall kick
                    {
                        _currPiece = tmp_piece;
                        _posX--;
                    }
                    else if (_gameBoard.CanPosAt(tmp_piece, _posX - 1, _posY + 1)) // attempt down left wall kick
                    {
                        _currPiece = tmp_piece;
                        _posX--;
                        _posY++;
                    }
                    else if (_gameBoard.CanPosAt(tmp_piece, _posX, _posY + 1)) // attempt down left wall kick
                    {
                        _currPiece = tmp_piece;
                        _posY++;
                    }
                    {
                            for (int yKick = 2; yKick < 4; yKick++)
                            {
                                if (_gameBoard.CanPosAt(tmp_piece, _posX + xKick, _posY)) // attempt rightward wall kick
                                {
                                    _currPiece = tmp_piece;
                                    _posX += xKick;
                                    goto ExitLoop; //a goto to a label just outside the two for loops since staying in the for loops will cause the piece to move too much
                                }
                                else if (_gameBoard.CanPosAt(tmp_piece, _posX + xKick, _posY + yKick)) // attempt down right wall kick
                                {
                                    _currPiece = tmp_piece;
                                    _posX += xKick;
                                    _posY += yKick;
                                    goto ExitLoop;
                                }
                                else if (_gameBoard.CanPosAt(tmp_piece, _posX - xKick, _posY)) // attempt leftward wall kick
                                {
                                    _currPiece = tmp_piece;
                                    _posX -= xKick;
                                    goto ExitLoop;
                                }
                                else if (_gameBoard.CanPosAt(tmp_piece, _posX - xKick, _posY + yKick)) // attempt down left wall kick
                                {
                                    _currPiece = tmp_piece;
                                    _posX -= xKick;
                                    _posY += yKick;
                                    goto ExitLoop;
                                }
                                else if (_gameBoard.CanPosAt(tmp_piece, _posX, _posY + yKick)) // attempt down left wall kick
                                {
                                    _currPiece = tmp_piece;
                                    _posY += yKick;
                                    goto ExitLoop;
                                }
                            }
                        }
                        //a label to exit the two for loops, or else we move way too much
                    }
                }
            }
        }

        #endregion

        #region IMode Implementation

        public bool NextPieceMode
        {
            get; set;
        }

        public bool ShadowPieceMode
        {
            get; set;
        }

        #endregion
    }
}
