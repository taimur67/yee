using System;
using System.Linq;

namespace LoG
{
	// Token: 0x02000571 RID: 1393
	[Serializable]
	public class ObjectiveCondition_DestroyLegionWithRitual : ObjectiveCondition_EventFilter<RitualCastEvent>
	{
		// Token: 0x06001AAA RID: 6826 RVA: 0x0005D04D File Offset: 0x0005B24D
		protected override bool Filter(TurnContext context, RitualCastEvent @event, PlayerState owner, PlayerState target, out int strength)
		{
			if (!base.Filter(context, @event, owner, target, out strength))
			{
				return false;
			}
			strength = @event.Enumerate<LegionKilledEvent>().Count<LegionKilledEvent>();
			return strength > 0;
		}
	}
}
