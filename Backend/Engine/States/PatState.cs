using System.Collections.Generic;
using System.Linq;
using Backend.Engine.RuleManager;
using Backend.Model;
using Backend.Model.Pieces;

namespace Backend.Engine.States
{
    internal class PatState : IState
    {
        public bool IsInState(Board board, Color color)
        {
            RuleGroup ruleGroup = new PawnRuleGroup();
            ruleGroup.AddGroup(new BishopRuleGroup());
            ruleGroup.AddGroup(new KingRuleGroup());
            ruleGroup.AddGroup(new KnightRuleGroup());
            ruleGroup.AddGroup(new QueenRuleGroup());
            ruleGroup.AddGroup(new RookRuleGroup());

            var possibleSquares = new List<Square>();
            foreach (var square in board.Squares.OfType<Square>().Where(x => x?.Piece?.Color == color))
                if (square.Piece != null)
                    possibleSquares = possibleSquares.Concat(ruleGroup.PossibleMoves(square.Piece)).ToList();
            return possibleSquares.Count == 0;
        }

        public string Explain() => "On est tout pat";
    }
}