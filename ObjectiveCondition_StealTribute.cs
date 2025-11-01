using System;
using System.Linq;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005A9 RID: 1449
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ObjectiveCondition_StealTribute : ObjectiveCondition_SuccessfullyCastRitual<RitualCastEvent>
	{
		// Token: 0x06001B3A RID: 6970 RVA: 0x0005EB2C File Offset: 0x0005CD2C
		public ObjectiveCondition_StealTribute()
		{
		}

		// Token: 0x06001B3B RID: 6971 RVA: 0x0005EB34 File Offset: 0x0005CD34
		public ObjectiveCondition_StealTribute(int minimumTokenCount)
		{
			this.MinimumTokenCount = minimumTokenCount;
		}

		// Token: 0x06001B3C RID: 6972 RVA: 0x0005EB43 File Offset: 0x0005CD43
		protected override bool Filter(TurnContext context, RitualCastEvent @event, PlayerState owner, PlayerState target)
		{
			return @event.LocalChildEvents.OfType<StealTributeEvent>().Any((StealTributeEvent stealTribute) => stealTribute.NumberOfTokensStolen >= this.MinimumTokenCount) && base.Filter(context, @event, owner, target);
		}

		// Token: 0x04000C51 RID: 3153
		[JsonProperty]
		public int MinimumTokenCount;
	}
}
