using System;
using Backend.Model;
using Backend.Model.Pieces;
using Type = Backend.Model.Pieces.Type;

namespace Backend.Command
{
    [Serializable]
    public class EnPassantCommand : ICompensableCommand
    {
        private ICompensableCommand _firstMove;
        private ICompensableCommand _secondMove;


        public EnPassantCommand(Move move, Board board)
        {
            Move = move;

            var isWhite = move.PieceColor == Color.White;
            var isLeft = move.StartCoordinate.X > move.TargetCoordinate.X;

            var x = move.StartCoordinate.X + (isLeft ? -1 : 1);
            var y = move.StartCoordinate.Y;

            var startSquare = board.SquareAt(move.StartCoordinate);
            var secondSquare = board.Squares[x, y];
            var thirdSquare = board.Squares[x, y + (isWhite ? -1 : 1)];

            _firstMove = new MoveCommand(new Move(startSquare, secondSquare, Move.PieceType, Move.PieceColor), board);
            _secondMove = new MoveCommand(new Move(secondSquare, thirdSquare, Move.PieceType, Move.PieceColor), board);
        }

        private EnPassantCommand(EnPassantCommand command, Board board)
        {
            Move = command.Move;
            _firstMove = command._firstMove.Copy(board);
            _secondMove = command._secondMove.Copy(board);
        }

        public void Execute()
        {
            _firstMove.Execute();
            _secondMove.Execute();
        }

        public void Compensate()
        {
            _secondMove.Compensate();
            _firstMove.Compensate();
        }

        public bool TakePiece => true;

        public Move Move { get; }

        public Type PieceType => Move.PieceType;

        public Color PieceColor => Move.PieceColor;

        public ICompensableCommand Copy(Board board) => new EnPassantCommand(this, board);

        public override string ToString() => "En passant de " + Move.StartCoordinate + " vers " + Move.TargetCoordinate;
    }
}