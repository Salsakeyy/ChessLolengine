using System.Collections.Generic;
using System.Linq;
using Backend.Engine.Rules;
using Backend.Model;
using Backend.Model.Pieces;

namespace Backend.Engine.States
{
    public class CheckState : IState
    {
        public bool IsInState(Board board, Color color)
        {
            /*
             * On construit des groupes de règles spéciales qui ne tienne pas compte
             * de celle de la mise en echec
             */
            var tempBoard = new Board(board);
            var queenMovementCheckRules = new List<IRule> {new QueenMovementRule(), new CanOnlyTakeEnnemyRule()};

            var pawnMovementCheckRules = new List<IRule> {new PawnMovementRule(), new CanOnlyTakeEnnemyRule()};

            var kingMovementCheckRules = new List<IRule> {new KingMovementRule(), new CanOnlyTakeEnnemyRule(), new CastlingRule()};

            var knightMovementCheckRules = new List<IRule>
            {
                new KnightMovementRule(),
                new CanOnlyTakeEnnemyRule()
            };

            var rookMovementCheckRules = new List<IRule> {new CanOnlyTakeEnnemyRule(), new RookMovementRule()};

            var bishopMovementCheckRules = new List<IRule>
            {
                new CanOnlyTakeEnnemyRule(),
                new BishopMovementRule()
            };

            var rulesGroup = new Dictionary<Type, List<IRule>>
            {
                {Type.Queen, queenMovementCheckRules},
                {Type.Pawn, pawnMovementCheckRules},
                {Type.Knight, knightMovementCheckRules},
                {Type.Rook, rookMovementCheckRules},
                {Type.Bishop, bishopMovementCheckRules},
                {Type.King, kingMovementCheckRules}
            };


            // On cherche le roi
            var concernedKing = tempBoard.Squares.OfType<Square>()
                .First(x => x?.Piece?.Type == Type.King && x?.Piece?.Color == color).Piece;

            var res = false;
            foreach (var rules in rulesGroup)
            {
                var possibleMoves = new List<Square>();
                concernedKing.Type = rules.Key;
                possibleMoves = possibleMoves.Concat(rules.Value.First().PossibleMoves(concernedKing)).ToList();
                rules.Value.ForEach(
                    x => possibleMoves = possibleMoves.Intersect(x.PossibleMoves(concernedKing)).ToList());

                if (possibleMoves.Any(x => x?.Piece?.Type == rules.Key))
                    // Vérifier si il ne faut pas être d'une couleur différente
                    res = true;
            }
            concernedKing.Type = Type.King;
            return res;
        }

        public string Explain() => "Le roi du joueur est en echec";
    }
}