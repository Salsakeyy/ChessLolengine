using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Backend.Command;
using Backend.Engine.RuleManager;
using Backend.Engine.States;
using Backend.Model;
using Backend.Model.Pieces;
using Type = Backend.Model.Pieces.Type;

namespace Backend.Engine
{
    public class RealEngine : IEngine
    {
        private Container _container;
        private CompensableConversation _conversation;
        private Pawn _enPassantPawnBlack;
        private Pawn _enPassantPawnWhite;
        private ObservableCollection<ICompensableCommand> _moves;
        private RuleGroup _ruleGroups;

        /// <summary>
        ///     RealEngine constructor
        /// </summary>
        /// <param name="container">The model the engine will work with</param>
        public RealEngine(Container container)
        {
            Board = container.Board;
            _container = container;
            _moves = container.Moves;

            _conversation = new CompensableConversation(container.Moves);

            _ruleGroups = new PawnRuleGroup();
            _ruleGroups.AddGroup(new BishopRuleGroup());
            _ruleGroups.AddGroup(new KingRuleGroup());
            _ruleGroups.AddGroup(new KnightRuleGroup());
            _ruleGroups.AddGroup(new QueenRuleGroup());
            _ruleGroups.AddGroup(new RookRuleGroup());
        }

        /// <summary>
        ///     The board the engine works with
        /// </summary>
        public Board Board { get; }

        /// <summary>
        ///     Ask the engine to do a move
        /// </summary>
        /// <param name="move">The move to do</param>
        /// <returns>True if the move was valid and therefore has been done</returns>
        public bool DoMove(Move move)
        {
            //No reason to move if it's the same square
            if (move.StartCoordinate == move.TargetCoordinate) 
                return false;

            var piece = Board.PieceAt(move.StartCoordinate);
            var targetPiece = Board.PieceAt(move.TargetCoordinate);

            //TODO generate exception
            if (!_ruleGroups.Handle(move, Board)) 
                return false;

            ICompensableCommand command;
            switch (move.PieceType)
            {
                case Type.King when targetPiece?.Type == Type.Rook && move.PieceColor == targetPiece.Color
                                    || Math.Abs(move.TargetCoordinate.X - move.StartCoordinate.X) == 2:
                    command = new CastlingCommand(move, Board);
                    break;
                case Type.Pawn when targetPiece == null && move.StartCoordinate.X != move.TargetCoordinate.X:
                    command = new EnPassantCommand(move, Board);
                    break;
                case Type.Pawn when move.TargetCoordinate.Y == (move.PieceColor == Color.White ? 0 : 7):
                    command = new PromoteCommand(move, Board);
                    break;
                default:
                    command = new MoveCommand(move, Board);
                    break;
            }

            //En passant
            if (move.PieceColor == Color.White)
            {
                if (_enPassantPawnWhite != null)
                {
                    _enPassantPawnWhite.EnPassant = false;
                    _enPassantPawnWhite = null;
                }
            }
            else
            {
                if (_enPassantPawnBlack != null)
                {
                    _enPassantPawnBlack.EnPassant = false;
                    _enPassantPawnBlack = null;
                }
            }

            if (move.PieceType == Type.Pawn && Math.Abs(move.StartCoordinate.Y - move.TargetCoordinate.Y) == 2)
            {
                if (move.PieceColor == Color.White)
                {
                    _enPassantPawnWhite = (Pawn) piece;
                    _enPassantPawnWhite.EnPassant = true;
                }
                else
                {
                    _enPassantPawnBlack = (Pawn) piece;
                    _enPassantPawnBlack.EnPassant = true;
                }
            }

            //Number of moves since last capture
            if (Board.PieceAt(move.TargetCoordinate) == null)
                _container.HalfMoveSinceLastCapture++;
            else
                _container.HalfMoveSinceLastCapture = 0;

            _conversation.Execute(command);
            _moves.Add(command);

            return true;
        }


        public BoardState CurrentState()
        {
            IState checkState = new CheckState();
            IState patState = new PatState();


            var color = _moves.Count == 0 ? Color.White : _moves[_moves.Count - 1].PieceColor;

            var check = checkState.IsInState(Board, color == Color.White ? Color.Black : Color.White);

            var pat = patState.IsInState(Board, color == Color.White ? Color.Black : Color.White);

            if (pat && check)
                return color == Color.Black ? BoardState.WhiteCheckMate : BoardState.BlackCheckMate;
            if (pat)
                return color == Color.Black ? BoardState.WhitePat : BoardState.BlackPat;
            if (check)
                return color == Color.Black ? BoardState.WhiteCheck : BoardState.BlackCheck;

            return BoardState.Normal;
        }

        public List<Square> PossibleMoves(Piece piece)
        {
            return _ruleGroups.PossibleMoves(piece);
        }

        /// <summary>
        ///     Undo the last command that has been done
        /// </summary>
        /// <returns>True if anything has been done</returns>
        public Move Undo()
        {
            var command = _conversation.Undo();
            if (command == null) return null;

            if (_container.HalfMoveSinceLastCapture != 0)
                _container.HalfMoveSinceLastCapture--;
            else
            {
                var count = 0;
                for (var i = _moves.Count - 1; i > 0; i--)
                    if (!_moves[i].TakePiece)
                        count++;
                    else
                        break;
                _container.HalfMoveSinceLastCapture = count;
            }

            _moves.Remove(command);
            return command.Move;
        }

        /// <summary>
        ///     Redo the last command that has been undone
        /// </summary>
        /// <returns>True if anything has been done</returns>
        public Move Redo()
        {
            var command = _conversation.Redo();
            if (command == null) return null;

            //Number of moves since last capture
            if (!command.TakePiece)
                _container.HalfMoveSinceLastCapture++;
            else
                _container.HalfMoveSinceLastCapture = 0;

            _moves.Add(command);
            return command.Move;
        }
    }
}