using System;
using Backend.Model.Pieces;
using Type = Backend.Model.Pieces.Type;

namespace Backend.Model
{
    /// <summary>
    ///     Represent a chess move independantly of the board
    /// </summary>
    [Serializable]
    public class Move
    {
        public Coordinate StartCoordinate { get; set; }

        public Coordinate TargetCoordinate { get; set; }

        public Color PieceColor { get; set; }

        public Type PieceType { get; set; }

        public Type PromotePieceType { get; set; }

        #region Constructors

        /// <summary>
        ///     Move constructor
        /// </summary>
        /// <param name="piece">The piece that move</param>
        /// <param name="targetSquare">The square the piece goes to</param>
        public Move(Piece piece, Square targetSquare)
        {
            PieceColor = piece.Color;
            PieceType = piece.Type;
            StartCoordinate = piece.Square.Coordinate;
            TargetCoordinate = targetSquare.Coordinate;
        }

        /// <summary>
        ///     Move constructor
        /// </summary>
        /// <param name="startSquare">The square the piece is on</param>
        /// <param name="targetSquare">The square the piece goes to</param>
        /// <param name="pieceType">The type of the piece</param>
        /// <param name="pieceColor">The pieceColor of the piece</param>
        public Move(Square startSquare, Square targetSquare, Type pieceType, Color pieceColor)
        {
            PieceColor = pieceColor;
            PieceType = pieceType;
            StartCoordinate = startSquare.Coordinate;
            TargetCoordinate = targetSquare.Coordinate;
        }

        /// <summary>
        ///     Promoting move constructor
        /// </summary>
        /// <param name="startSquare">The square the piece is on</param>
        /// <param name="targetSquare">The square the piece goes to</param>
        /// <param name="pieceType">The type of the piece</param>
        /// <param name="pieceColor">The color of the piece</param>
        /// <param name="promotePieceType">The type the piece promotes to</param>
        public Move(Square startSquare, Square targetSquare, Type pieceType, Color pieceColor, Type promotePieceType)
        {
            PieceColor = pieceColor;
            PieceType = pieceType;
            StartCoordinate = startSquare.Coordinate;
            TargetCoordinate = targetSquare.Coordinate;
            PromotePieceType = promotePieceType;
        }

        #endregion
    }
}