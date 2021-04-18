using System;
using System.Collections.Generic;
using System.Linq;
using Backend.Engine;
using Backend.Model;
using Backend.Model.Pieces;

namespace Backend.Lolengine
{
    public static class LolengineLogic
    {
        public static List<Coordinate> GetBestMoves(Container container)
        {
            var pieces = new List<Piece>();

            for (var i = 0; i < container.Board.Squares.GetLength(0); i++)
            {
                for (var index2 = 0; index2 < container.Board.Squares.GetLength(0); index2++)
                {
                    if (container.Board.Squares[i, index2].Piece != null)
                    {
                        if (container.Board.Squares[i, index2].Piece.Color == Color.Black)
                        {
                            pieces.Add(container.Board.Squares[i, index2].Piece);
                        }
                    }
                }
            }

            var realEngine = new RealEngine(container);
            var moves = new List<Moves>();

            foreach (var piece in pieces)
            {
                var possibleMoves = realEngine.PossibleMoves(piece);
                if (possibleMoves.Any())
                {
                    moves.Add(new Moves(piece, possibleMoves));
                }
            }

            var random = new Random();
            var moveToDo = moves[random.Next(moves.Count)];
            var coordinates = new List<Coordinate>
            {
                moveToDo.Piece.Square.Coordinate,
                moveToDo.PossibleMoves.First().Coordinate
            };

            return coordinates;
        }
    }

    public class Moves
    {
        public Moves(Piece piece, List<Square> possibleMoves)
        {
            Piece = piece;
            PossibleMoves = new List<Square>(possibleMoves);

        }

        public List<Square> PossibleMoves { get; set; }
        public Piece Piece { get; set; }
    }
}
