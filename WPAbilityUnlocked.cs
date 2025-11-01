using System;
using Core.StaticData;

namespace LoG
{
	// Token: 0x0200014F RID: 335
	public class WPAbilityUnlocked : WorldProperty
	{
		// Token: 0x060006A7 RID: 1703 RVA: 0x000214AE File Offset: 0x0001F6AE
		public WPAbilityUnlocked(ConfigRef order)
		{
			this.AbilityReference = order;
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x000214BD File Offset: 0x0001F6BD
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return planner.IsUnlocked(this.AbilityReference);
		}

		// Token: 0x04000304 RID: 772
		private ConfigRef AbilityReference;
	}
}
