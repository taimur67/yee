using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005A5 RID: 1445
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ObjectiveCondition_Steal : ObjectiveCondition_SuccessfullyCastRitual<StealGameItemRitualEvent>
	{
		// Token: 0x06001B2F RID: 6959 RVA: 0x0005E9F5 File Offset: 0x0005CBF5
		public ObjectiveCondition_Steal()
		{
		}

		// Token: 0x06001B30 RID: 6960 RVA: 0x0005E9FD File Offset: 0x0005CBFD
		public ObjectiveCondition_Steal(GameItemCategory category)
		{
			this.Category = new GameItemCategory?(category);
		}

		// Token: 0x06001B31 RID: 6961 RVA: 0x0005EA11 File Offset: 0x0005CC11
		protected override bool Filter(TurnContext context, StealGameItemRitualEvent @event, PlayerState owner, PlayerState target)
		{
			return (this.Category == null || @event.ItemCategory == this.Category.Value) && base.Filter(context, @event, owner, target);
		}

		// Token: 0x04000C4E RID: 3150
		[JsonProperty]
		public GameItemCategory? Category;
	}
}
