using System;
using System.Collections.Generic;
using Backend.Core;
using Backend.Model;
using Backend.Model.Pieces;

namespace Backend.Lolengine
{
    public class LolengineController : PlayerControler
    {
        private readonly Container _container;

        public LolengineController(Container container)
         {
            _container = container;
        }

        public override void Play(Move move)
        {
            var coordinates = LolengineLogic.GetBestMoves(_container);

            if (coordinates.Count != 2) 
                return;

            var startCoordinate = coordinates[0];
            var targetCoordinate = coordinates[1];
            var test = new Move(_container.Board.PieceAt(startCoordinate), _container.Board.SquareAt(targetCoordinate));
            Move(test);
   
            //Move(new Move(_container.Board.SquareAt(startCoordinate), _container.Board.SquareAt(targCoordinate), Type.Pawn, Player.Color, Type.Knight));           
        }

        public override void Move(Move move)
        {
            Player.Move(move);
        }

        public override void InvalidMove(List<string> reasonsList)
        {
            //throw new System.NotImplementedException();
        }

        public override List<Square> PossibleMoves(Piece piece)
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
              //throw new NotImplementedException();
        }
    }
}