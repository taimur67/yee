using System;
using System.Collections.Generic;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000579 RID: 1401
	[Serializable]
	public class ObjectiveCondition_DiplomaticActionWithActiveRitual : ObjectiveCondition_EventFilter<DiplomaticEvent>
	{
		// Token: 0x06001ABF RID: 6847 RVA: 0x0005D70C File Offset: 0x0005B90C
		protected override bool Filter(TurnContext context, DiplomaticEvent @event, PlayerState owner, PlayerState target)
		{
			return base.Filter(context, @event, owner, target) && (this.ValidDiplomaticActions.Count <= 0 || this.ValidDiplomaticActions.Contains(@event.OrderType)) && ObjectiveCondition.AnyActiveRitualsMatch(context.CurrentTurn.GetActiveRituals(owner), this.ValidActiveRituals);
		}

		// Token: 0x04000C23 RID: 3107
		[JsonProperty]
		public List<ConfigRef<RitualStaticData>> ValidActiveRituals = new List<ConfigRef<RitualStaticData>>();

		// Token: 0x04000C24 RID: 3108
		[JsonProperty]
		public List<OrderTypes> ValidDiplomaticActions = new List<OrderTypes>();
	}
}
