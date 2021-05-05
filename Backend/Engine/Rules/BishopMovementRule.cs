﻿using System.Collections.Generic;
using System.Linq;
using Backend.Model;
using Backend.Model.Pieces;

namespace Backend.Engine.Rules
{
    internal class BishopMovementRule : IRule
    {
        //TODO Refactor (copy/paste)
        /// <inheritdoc />
        public bool IsMoveValid(Move move, Board board)
            => PossibleMoves(board.PieceAt(move.StartCoordinate)).Contains(board.SquareAt(move.TargetCoordinate));

        /// <inheritdoc />
        public List<Square> PossibleMoves(Piece piece)
        {
            var diagonalUpLeft = new List<Square>();
            var diagonalUpRight = new List<Square>();
            var diagonalDownLeft = new List<Square>();
            var diagonalDownRight = new List<Square>();

            var diagonalUpLeftEnd = false;
            var diagonalUpRightEnd = false;
            var diagonalDownLeftEnd = false;
            var diagonalDownRightEnd = false;


            var board = piece.Square.Board;

            for (var i = 1; i < 8; i++)
            {
                if (piece.Square.X - i < 8 && piece.Square.X - i >= 0 && piece.Square.Y - i < 8 &&
                    piece.Square.Y - i >= 0 && !diagonalUpLeftEnd)
                {
                    var square = board.Squares[piece.Square.X - i, piece.Square.Y - i];
                    diagonalUpLeft.Add(square);
                    if (square.Piece != null)
                    {
                        if (piece.Color == square.Piece.Color) diagonalUpLeft.Remove(square);
                        diagonalUpLeftEnd = true;
                    }
                }
                if (piece.Square.X + i < 8 && piece.Square.X + i >= 0 && piece.Square.Y - i < 8 &&
                    piece.Square.Y - i >= 0 && !diagonalUpRightEnd)
                {
                    var square = board.Squares[piece.Square.X + i, piece.Square.Y - i];
                    diagonalUpRight.Add(square);
                    if (square.Piece != null)
                    {
                        if (piece.Color == square.Piece.Color) diagonalUpRight.Remove(square);
                        diagonalUpRightEnd = true;
                    }
                }
                if (piece.Square.X - i < 8 && piece.Square.X - i >= 0 && piece.Square.Y + i < 8 &&
                    piece.Square.Y + i >= 0 && !diagonalDownLeftEnd)
                {
                    var square = board.Squares[piece.Square.X - i, piece.Square.Y + i];
                    diagonalDownLeft.Add(square);
                    if (square.Piece != null)
                    {
                        if (piece.Color == square.Piece.Color) diagonalDownLeft.Remove(square);
                        diagonalDownLeftEnd = true;
                    }
                }
                if (piece.Square.X + i < 8 && piece.Square.X + i >= 0 && piece.Square.Y + i < 8 &&
                    piece.Square.Y + i >= 0 && !diagonalDownRightEnd)
                {
                    var square = board.Squares[piece.Square.X + i, piece.Square.Y + i];
                    diagonalDownRight.Add(square);
                    if (square.Piece != null)
                    {
                        if (piece.Color == square.Piece.Color) diagonalDownRight.Remove(square);
                        diagonalDownRightEnd = true;
                    }
                }
            }

            var possibleSquares = diagonalUpLeft.Concat(diagonalUpRight)
                .Concat(diagonalDownLeft)
                .Concat(diagonalDownRight)
                .ToList();
            return possibleSquares;
        }
    }
}