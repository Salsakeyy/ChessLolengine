using Backend.Engine.Rules;
using Backend.Model.Pieces;

namespace Backend.Engine.RuleManager
{
    public class KingRuleGroup : RuleGroup
    {
        public KingRuleGroup()
        {
            Rules.Add(new KingMovementRule());
            Rules.Add(new CanOnlyTakeEnnemyRuleKing());
            Rules.Add(new CastlingRule());
            Rules.Add(new WillNotMakeCheck());
        }

        protected override Type Type => Type.King;
    }
}