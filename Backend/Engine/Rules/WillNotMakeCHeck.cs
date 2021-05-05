using System.Collections.Generic;
using System.Linq;
using Backend.Command;
using Backend.Engine.States;
using Backend.Model;
using Backend.Model.Pieces;

namespace Backend.Engine.Rules
{
    public class WillNotMakeCheck : IRule
    {
        public bool IsMoveValid(Move move, Board board)
        {
            IState checkState = new CheckState();
            var tempBoard = new Board(board);

            var castling = new CastlingRule().IsMoveValid(move, board) && move.PieceType == Type.King &&
                           tempBoard.PieceAt(move.TargetCoordinate)?.Type == Type.Rook &&
                           move.PieceColor == tempBoard.PieceAt(move.TargetCoordinate)?.Color;

            if (!castling)
                if (move.PieceColor == tempBoard.PieceAt(move.TargetCoordinate)?.Color)
                    return true;
            var command = castling
                ? new CastlingCommand(move, tempBoard)
                : new MoveCommand(move, tempBoard) as ICompensableCommand;

            command.Execute();

            return !checkState.IsInState(tempBoard, move.PieceColor);
        }

        public List<Square> PossibleMoves(Piece piece)
        {
            return
                piece.Square.Board.Squares.OfType<Square>()
                    .ToList()
                    .FindAll(x => IsMoveValid(new Move(piece, x), piece.Square.Board));
        }
    }
}