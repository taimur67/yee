using System;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200056E RID: 1390
	[Serializable]
	public class ObjectiveCondition_DestroyGameItem : ObjectiveCondition_EventFilter<ItemBanishedEvent>
	{
		// Token: 0x06001AA3 RID: 6819 RVA: 0x0005CEA8 File Offset: 0x0005B0A8
		protected override bool Filter(TurnContext context, ItemBanishedEvent @event, PlayerState owner, PlayerState target)
		{
			if (!this.TargetGameItem.IsEmpty())
			{
				GameItem gameItem;
				if (!context.CurrentTurn.TryFetchGameItem(@event.ItemId, out gameItem))
				{
					return false;
				}
				if (!gameItem.StaticDataReference.Equals(this.TargetGameItem))
				{
					return false;
				}
			}
			return base.Filter(context, @event, owner, target);
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06001AA4 RID: 6820 RVA: 0x0005CEF9 File Offset: 0x0005B0F9
		public override string Name
		{
			get
			{
				return string.Format("Destroy GameItem: {0}", this.TargetGameItem);
			}
		}

		// Token: 0x04000C0F RID: 3087
		[JsonProperty]
		public ConfigRef<GameItemStaticData> TargetGameItem;
	}
}
