﻿using System;
using System.Collections.Generic;
using System.Linq;
using Backend.Model;
using Backend.Model.Pieces;

namespace Backend.Engine.Rules
{
    public class KnightMovementRule : IRule
    {
        public bool IsMoveValid(Move move, Board board) =>
            Math.Abs(move.TargetCoordinate.X - move.StartCoordinate.X) == 2 &&
            Math.Abs(move.TargetCoordinate.Y - move.StartCoordinate.Y) == 1 ||
            Math.Abs(move.TargetCoordinate.Y - move.StartCoordinate.Y) == 2 &&
            Math.Abs(move.TargetCoordinate.X - move.StartCoordinate.X) == 1;

        public List<Square> PossibleMoves(Piece piece)
        {
            return
                piece.Square.Board.Squares.OfType<Square>()
                    .ToList()
                    .FindAll(x => IsMoveValid(new Move(piece, x), piece.Square.Board));
        }
    }
}