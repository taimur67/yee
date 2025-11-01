using System;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005AC RID: 1452
	[Serializable]
	public class ObjectiveCondition_SuccessfullyCastRitual : ObjectiveCondition_SuccessfullyCastRitual<RitualCastEvent>
	{
		// Token: 0x06001B41 RID: 6977 RVA: 0x0005EBAF File Offset: 0x0005CDAF
		public ObjectiveCondition_SuccessfullyCastRitual()
		{
		}

		// Token: 0x06001B42 RID: 6978 RVA: 0x0005EBBE File Offset: 0x0005CDBE
		public ObjectiveCondition_SuccessfullyCastRitual(int targetPlayerId)
		{
			base.TargetingPlayer = new int?(targetPlayerId);
		}

		// Token: 0x06001B43 RID: 6979 RVA: 0x0005EBDC File Offset: 0x0005CDDC
		protected override bool Filter(TurnContext context, RitualCastEvent @event, PlayerState owner, PlayerState target)
		{
			RitualStaticData ritualStaticData;
			return (this.Ritual.IsEmpty() || !(@event.RitualId != this.Ritual.Id)) && (this.RitualType == RitualType.All || !context.Database.TryFetch<RitualStaticData>(@event.RitualId, out ritualStaticData) || this.RitualType == ritualStaticData.RitualType) && base.Filter(context, @event, owner, target);
		}

		// Token: 0x04000C52 RID: 3154
		[JsonProperty]
		public ConfigRef<RitualStaticData> Ritual;

		// Token: 0x04000C53 RID: 3155
		[JsonProperty]
		public RitualType RitualType = RitualType.All;
	}
}
