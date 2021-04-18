using Backend.Engine.Rules;
using Backend.Model.Pieces;

namespace Backend.Engine.RuleManager
{
    public class BishopRuleGroup : RuleGroup
    {
        public BishopRuleGroup()
        {
            Rules.Add(new CanOnlyTakeEnnemyRule());
            Rules.Add(new BishopMovementRule());
            Rules.Add(new WillNotMakeCheck());
        }

        protected override Type Type => Type.Bishop;
    }
}