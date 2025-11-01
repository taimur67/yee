using System;
using System.Linq;

namespace LoG
{
	// Token: 0x02000596 RID: 1430
	[Serializable]
	public class ObjectiveCondition_LoseLegionsToAnEvent : ObjectiveCondition_EventFilter<GrandEventPlayed>
	{
		// Token: 0x06001B15 RID: 6933 RVA: 0x0005E69C File Offset: 0x0005C89C
		protected override bool Filter(TurnContext context, GrandEventPlayed @event, PlayerState owner, PlayerState target, out int strength)
		{
			if (!base.Filter(context, @event, null, null, out strength))
			{
				return false;
			}
			if (owner.Id == target.Id)
			{
				return false;
			}
			strength = @event.Enumerate<LegionKilledEvent>().Count((LegionKilledEvent t) => t.OriginalOwner == target.Id);
			return strength > 0;
		}
	}
}
