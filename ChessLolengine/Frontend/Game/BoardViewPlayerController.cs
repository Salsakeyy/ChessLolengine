using System.Collections.Generic;
using Backend.Core;
using Backend.Model;
using Backend.Model.Pieces;
using Frontend.View.ModelView;

namespace Frontend.Game
{
    public class BoardViewPlayerController : PlayerControler
    {
        private BoardView _boardView;

        public BoardViewPlayerController(BoardView boardView)
        {
            _boardView = boardView;
        }

        public bool IsPlayable { get; set; }

        public override void Play(Move move)
        {
            IsPlayable = true;
        }

        public override void Move(Move move)
        {
            IsPlayable = false;
            Player.Move(move);
        }

        public override void InvalidMove(List<string> reasonsList)
        {
            IsPlayable = true;
        }

        public override List<Square> PossibleMoves(Piece piece)
        {
            return Player.PossibleMoves(piece);
        }

        public override void Stop()
        {
            IsPlayable = false;
        }
    }
}