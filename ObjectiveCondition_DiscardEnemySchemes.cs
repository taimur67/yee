using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200057C RID: 1404
	[Serializable]
	public class ObjectiveCondition_DiscardEnemySchemes : ObjectiveCondition_EventFilter<SchemeDiscardedEvent>
	{
		// Token: 0x06001AC9 RID: 6857 RVA: 0x0005D86C File Offset: 0x0005BA6C
		protected override bool Filter(TurnContext context, SchemeDiscardedEvent @event, PlayerState owner, PlayerState target)
		{
			return base.Filter(context, @event, owner, target) && @event.TriggeringPlayerID == owner.Id && @event.AffectedPlayerIds.Contains(target.Id) && (!this.MustBePrivate || @event.WasPrivate);
		}

		// Token: 0x04000C2A RID: 3114
		[JsonProperty]
		public bool MustBePrivate;
	}
}
