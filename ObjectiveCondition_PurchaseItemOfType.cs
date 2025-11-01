using System;

namespace LoG
{
	// Token: 0x0200059D RID: 1437
	[Serializable]
	public class ObjectiveCondition_PurchaseItemOfType : ObjectiveCondition_EventFilter<BazaarBidEvent>
	{
		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06001B21 RID: 6945 RVA: 0x0005E820 File Offset: 0x0005CA20
		public override string LocalizationKey
		{
			get
			{
				if (!this.MustBeBackroomDeal)
				{
					return string.Format("{0}.{1}", base.LocalizationKey, this.GameItemCategory);
				}
				return base.LocalizationKey + ".Backroom";
			}
		}

		// Token: 0x06001B22 RID: 6946 RVA: 0x0005E856 File Offset: 0x0005CA56
		protected override bool Filter(TurnContext context, BazaarBidEvent @event, PlayerState owner, PlayerState target)
		{
			return this.GameItemCategory == @event.GameItemCategory && (!this.MustBeBackroomDeal || @event.WasBackroomItem) && base.Filter(context, @event, owner, target);
		}

		// Token: 0x04000C4B RID: 3147
		[BindableValue(null, BindingOption.None)]
		public GameItemCategory GameItemCategory;

		// Token: 0x04000C4C RID: 3148
		public bool MustBeBackroomDeal;
	}
}
