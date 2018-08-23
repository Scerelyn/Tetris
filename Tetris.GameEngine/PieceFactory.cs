using System;
using System.Collections.Generic;

namespace Tetris.GameEngine
{
    public static class PieceFactory
    {
        #region Private Fields

        /// <summary>
        /// List of Tetris Pieces
        /// </summary>
        private static List<Piece> _pieces;
        /// <summary>
        /// The bag to shuffle into and draw from
        /// </summary>
        private static Stack<Piece> _bag = new Stack<Piece>();
        #endregion

        #region Constructors

        /// <summary>
        /// Adds some basic Tetris Pieces to the list
        /// </summary>
        static PieceFactory()
        {
            Initialize();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a specific Piece
        /// </summary>
        /// <param name="id">ID of Piece (0-6)</param>
        /// <returns>the Piece (or null if invalid Value)</returns>
        public static Piece GetPiecebyId(int id)
        {
            if (_pieces.Count > id && id >= 0)
            {
                return _pieces[id];
            }
            return null;
        }

        /// <summary>
        /// Returns a random Piece
        /// </summary>
        /// <returns>Random Piece</returns>
        public static Piece GetRandomPiece(Random r)
        {
            //to avoid duplicate pieces, and abide by the tetris standards, we use a bag randomizing algorithm
            if (_bag.Count > 0)
            {
                return _bag.Pop(); //normally we get from a bag
            }
            else //if its empty
            {
                List<Piece> cpyPieces = new List<Piece>(_pieces); //duplicate the _pieces list
                while (cpyPieces.Count > 0) //while the copied _pieces has pieces left in it
                {
                    int rng = r.Next(cpyPieces.Count);
                    _bag.Push(cpyPieces[rng]); //push a random piece
                    cpyPieces.RemoveAt(rng); //remove it to eventually end the loop
                } //by here, the bag has one of each piece which will pop off one when this method is called again, then refill it again if needed
                return _bag.Pop(); //now get a piece
            }
        }

        /// <summary>
        /// Resets the bag-based randomizer
        /// </summary>
        public static void ResetRandomizer()
        {
            _bag.Clear();
        }

        #endregion

        #region Public Properties
        public static int Count
        {
            get 
            { 
                return _pieces.Count; 
            }
        }

        #endregion

        #region Helpers
        public static void Initialize()
        {
            _pieces = new List<Piece>();

            //following SRS rotations
            //http://tetris.wikia.com/wiki/SRS
            //using the image on the site, left most is 0 deg, left center is 90 deg, right center is 180 deg, and right is 270 deg
            //due to the engine limitations, padding zeroes cannot be used

            //####, I piece
            int[,] IPiece0Deg = new int[,] { { 1, 1, 1, 1 } };
            int[,] IPiece90Deg = new int[,] { { 1 }, { 1 }, { 1 }, { 1 } };
            int[,] IPiece180Deg = new int[,] { { 1, 1, 1, 1 } };
            int[,] IPiece270Deg = new int[,] { { 1 }, { 1 }, { 1 }, { 1 } };
            List<int[,]> IRotationStates = new List<int[,]>() { IPiece0Deg, IPiece90Deg, IPiece180Deg, IPiece270Deg };

            _pieces.Add(new Piece(IPiece0Deg, PieceType.I, IRotationStates, 0));

            //##
            //##, O Piece
            int[,] OPiece0Deg =   new int[,] { { 2, 2 }, { 2, 2 } };
            int[,] OPiece90Deg =  new int[,] { { 2, 2 }, { 2, 2 } };
            int[,] OPiece180Deg = new int[,] { { 2, 2 }, { 2, 2 } };
            int[,] OPiece270Deg = new int[,] { { 2, 2 }, { 2, 2 } }; // seems redundant but if we want O spin for whatever reason, just change these
            List<int[,]> ORotationStates = new List<int[,]>() { OPiece0Deg, OPiece90Deg, OPiece180Deg, OPiece270Deg, };

            _pieces.Add(new Piece(OPiece0Deg, PieceType.O, ORotationStates, 0));

            //  #
            //###, L Piece
            int[,] LPiece0Deg   = new int[,] { { 0, 0, 3 }, { 3, 3, 3 } };
            int[,] LPiece90Deg  = new int[,] { { 3, 0 }, { 3, 0 }, { 3, 3 } };
            int[,] LPiece180Deg = new int[,] { { 3, 3, 3 }, { 3, 0, 0 } };
            int[,] LPiece270Deg = new int[,] { { 3, 3 }, { 0, 3 }, { 0, 3 } };
            List<int[,]> LRotationStates = new List<int[,]>() { LPiece0Deg, LPiece90Deg, LPiece180Deg, LPiece270Deg };

            _pieces.Add(new Piece(LPiece0Deg, PieceType.L, LRotationStates, 0));

            //#
            //###, J Piece
            int[,] JPiece0Deg   = new int[,] { { 4, 0, 0 }, { 4, 4, 4 } };
            int[,] JPiece90Deg  = new int[,] { { 4, 4 }, { 4, 0 }, { 4, 0 } };
            int[,] JPiece180Deg = new int[,] { { 4, 4, 4 }, { 0, 0, 4 } };
            int[,] JPiece270Deg = new int[,] { { 0, 4 }, { 0, 4 }, { 4, 4 } };
            List<int[,]> JRotationStates = new List<int[,]>() { JPiece0Deg, JPiece90Deg, JPiece180Deg, JPiece270Deg };

            _pieces.Add(new Piece(JPiece0Deg, PieceType.J, JRotationStates, 0));

            // ##, S Piece
            //##
            int[,] SPiece0Deg   = new int[,] { { 0, 5, 5 }, { 5, 5, 0 } };
            int[,] SPiece90Deg  = new int[,] { { 5, 0 }, { 5, 5 }, { 0, 5 } };
            int[,] SPiece180Deg = new int[,] { { 0, 5, 5 }, { 5, 5, 0 } };
            int[,] SPiece270Deg = new int[,] { { 5, 0 }, { 5, 5 }, { 0, 5 } };
            List<int[,]> SRotationStates = new List<int[,]>() { SPiece0Deg, SPiece90Deg, SPiece180Deg, SPiece270Deg };

            _pieces.Add(new Piece(SPiece0Deg, PieceType.S, SRotationStates, 0));

            //##
            // ##, Z Piece
            int[,] ZPiece0Deg   = new int[,]{ { 6, 6, 0 }, { 0, 6, 6 } };
            int[,] ZPiece90Deg  = new int[,] { { 0, 6 }, { 6, 6 }, { 6, 0 } };
            int[,] ZPiece180Deg = new int[,] { { 6, 6, 0 }, { 0, 6, 6 } };
            int[,] ZPiece270Deg = new int[,] { { 0, 6 }, { 6, 6 }, { 6, 0 } };
            List<int[,]> ZRotationStates = new List<int[,]>() { ZPiece0Deg, ZPiece90Deg, ZPiece180Deg, ZPiece270Deg };

            _pieces.Add(new Piece(ZPiece0Deg, PieceType.Z, ZRotationStates, 0));

            //###, T Piece
            // #
            int[,] TPiece0Deg   = new int[,] { { 0, 7, 0 }, { 7, 7, 7 } };
            int[,] TPiece90Deg  = new int[,] { { 7, 0 }, { 7, 7 }, { 7, 0 } };
            int[,] TPiece180Deg = new int[,] { { 7, 7, 7 }, { 0, 7, 0 } };
            int[,] TPiece270Deg = new int[,] { { 0, 7 }, { 7, 7 }, { 0, 7 } };
            List<int[,]> TRotationStates = new List<int[,]>() { TPiece0Deg, TPiece90Deg, TPiece180Deg, TPiece270Deg };

            _pieces.Add(new Piece(TPiece0Deg, PieceType.T, TRotationStates, 0));
        }

        #endregion
    }
}

