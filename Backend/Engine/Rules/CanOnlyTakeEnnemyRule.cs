using System.Collections.Generic;
using System.Linq;
using Backend.Model;
using Backend.Model.Pieces;

namespace Backend.Engine.Rules
{
    public class CanOnlyTakeEnnemyRule : IRule
    {
        public bool IsMoveValid(Move move, Board board) =>
            move.PieceColor != board.PieceAt(move.TargetCoordinate)?.Color;

        public List<Square> PossibleMoves(Piece piece)
        {
            return piece.Square.Board.Squares.OfType<Square>().ToList().FindAll(x => piece.Color != x?.Piece?.Color);
        }
    }
}