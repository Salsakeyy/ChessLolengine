using Backend.Engine.Rules;
using Backend.Model.Pieces;

namespace Backend.Engine.RuleManager
{
    public class KnightRuleGroup : RuleGroup
    {
        public KnightRuleGroup()
        {
            Rules.Add(new CanOnlyTakeEnnemyRule());
            Rules.Add(new KnightMovementRule());
            Rules.Add(new WillNotMakeCheck());
        }

        protected override Type Type => Type.Knight;
    }
}