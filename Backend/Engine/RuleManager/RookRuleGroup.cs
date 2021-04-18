using Backend.Engine.Rules;
using Backend.Model.Pieces;

namespace Backend.Engine.RuleManager
{
    public class RookRuleGroup : RuleGroup
    {
        public RookRuleGroup()
        {
            Rules.Add(new RookMovementRule());
            Rules.Add(new CanOnlyTakeEnnemyRule());
            Rules.Add(new WillNotMakeCheck());
        }

        protected override Type Type => Type.Rook;
    }
}