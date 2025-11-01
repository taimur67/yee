using System;
using System.Collections.Generic;
using System.Linq;
using Core.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000570 RID: 1392
	[Serializable]
	public class ObjectiveCondition_DestroyLegionAfterUsingRitual : ObjectiveCondition_EventFilter<LegionKilledEvent>
	{
		// Token: 0x06001AA8 RID: 6824 RVA: 0x0005CF88 File Offset: 0x0005B188
		protected override bool Filter(TurnContext context, LegionKilledEvent @event, PlayerState owner, PlayerState target)
		{
			if (!base.Filter(context, @event, owner, target))
			{
				return false;
			}
			return IEnumerableExtensions.Any<RitualCastEvent>(from castRitual in context.CurrentTurn.GetGameEvents().OfType<RitualCastEvent>()
			where castRitual.Succeeded
			where castRitual.TriggeringPlayerID == owner.Id
			where castRitual.TargetContext.ItemId == @event.GamePieceId
			where this.AcceptedRituals.Any((ConfigRef<RitualStaticData> accepted) => accepted.Id == castRitual.RitualId)
			select castRitual);
		}

		// Token: 0x04000C11 RID: 3089
		public List<ConfigRef<RitualStaticData>> AcceptedRituals = new List<ConfigRef<RitualStaticData>>();
	}
}
