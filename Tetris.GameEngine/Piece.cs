using System;
using System.Collections.Generic;

namespace Tetris.GameEngine
{
    public class Piece: ICloneable
    {
        #region Private Fields

        private int[,] _piece;
        private int _initPosX;
        private int _initPosY;
        private List<int[,]> _rotationStates = null; // possible states to rotate to
        private int _rotationIndex; // which state to rotate on

        #endregion

        #region Constructors

        public Piece(int[,] p, List<int[,]> rotationStates=null, int rotateIndex=0)
        {
            if (p == null)
            {
                throw new ArgumentNullException();
            }
            _piece = (int[,])p.Clone();
            _initPosY = (p.GetUpperBound(0) + 1) * -1;
            _initPosX = 0;
            _rotationStates = rotationStates;
            _rotationIndex = rotateIndex;
        }

        #endregion

        #region Public Properties

        public int Height
        {
            get
            {
                return _piece.GetUpperBound(0) + 1;
            }
        }

        public int Width
        {
            get
            {
                return _piece.GetUpperBound(1) + 1;
            }
        }

        public int InitPosX
        {
            get 
            { 
                return _initPosX; 
            }
        }

        public int InitPosY
        {
            get 
            { 
                return _initPosY; 
            }
        }

        public Piece InitialRotationState
        {
            get
            {
                if (_rotationStates != null && _rotationStates.Count > 0 )
                {
                    return new Piece(_rotationStates[0], _rotationStates, 0);
                }
                else
                {
                    return this;
                }
            }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Rotates a Piece clockwise
        /// </summary>
        /// <returns>rotated Piece</returns>
        public Piece RotateRight()
        {
            int[,] rotated = new int[this.Width, this.Height];
            if (_rotationStates == null || _rotationStates.Count <= 0) // default roation behavior
            {
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        rotated[j, Height - i - 1] = _piece[i, j];
                    }
                }
                return new Piece( rotated );
            }
            else // if we have rotated states, use them
            {
                _rotationIndex = _rotationIndex >= _rotationStates.Count-1 ? 0 : _rotationIndex + 1; // move the rotation index up 1, if over the states size, move to 0
                return new Piece(_rotationStates[_rotationIndex], _rotationStates, _rotationIndex); // return the new piece with the new int array, copy over the states and index
            }
        }

        /// <summary>
        /// Rotates a Piece counter clockwise
        /// </summary>
        /// <returns>rotated Piece</returns>
        public Piece RotateLeft()
        {
            int[,] rotated = new int[this.Width, this.Height];
            if (_rotationStates.Count <= 0) // default roation behavior
            {
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        rotated[j, Height - i - 1] = _piece[i, j];
                    }
                }
                return new Piece(rotated);
            }
            else // if we have rotated states, use them
            {
                _rotationIndex = _rotationIndex < 0 ? _rotationStates.Count-1 : _rotationIndex - 1; // move the rotation index down 1, if under 0, move to states list size minus 1
                return new Piece(_rotationStates[_rotationIndex], _rotationStates, _rotationIndex); // return the new piece with the new int array, copy over the states and index
            }
        }

        public void MakeItShadow()
        {
            for (int i = 0; i < this.Height; i++)
            {
                for (int j = 0; j < this.Width; j++)
                {
                    if (this[i, j] != 0)
                    {
                        _piece[i, j] = 8;
                    }
                }
            }
        }

        public int[,] ToArray()
        {
            return _piece;
        }

        #endregion

        #region Public Indexers

        public int this[int h, int w]
        {
            get 
            {
                if ( ( h < 0 ) || ( h >= this.Height ) || ( w < 0 ) || ( w >= this.Width ) )
                {
                    throw new IndexOutOfRangeException("Index is out of range!");
                }
                return _piece[h, w];
            }
        }

        #endregion

        #region ICloneable Implementation

        public object Clone()
        {
            return new Piece(this._piece);
        }

        #endregion
    }
}