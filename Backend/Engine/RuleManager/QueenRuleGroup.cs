using Backend.Engine.Rules;
using Backend.Model.Pieces;

namespace Backend.Engine.RuleManager
{
    public class QueenRuleGroup : RuleGroup
    {
        public QueenRuleGroup()
        {
            Rules.Add(new QueenMovementRule());
            Rules.Add(new CanOnlyTakeEnnemyRule());
            Rules.Add(new WillNotMakeCheck());
        }

        protected override Type Type => Type.Queen;
    }
}