using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000640 RID: 1600
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class OrderPurchaseRank : ActionableOrder
	{
		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06001D8C RID: 7564 RVA: 0x00066036 File Offset: 0x00064236
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.IncreaseRank;
			}
		}

		// Token: 0x06001D8D RID: 7565 RVA: 0x0006603A File Offset: 0x0006423A
		[JsonConstructor]
		public OrderPurchaseRank()
		{
		}

		// Token: 0x06001D8E RID: 7566 RVA: 0x00066042 File Offset: 0x00064242
		public OrderPurchaseRank(Rank targetRank)
		{
			this.TargetRank = targetRank;
		}

		// Token: 0x06001D8F RID: 7567 RVA: 0x00066051 File Offset: 0x00064251
		public override IEnumerable<ActionConflict> GeneratePotentialConflicts()
		{
			yield return new RankUpConflict();
			yield break;
		}

		// Token: 0x04000C95 RID: 3221
		[JsonProperty]
		public Rank TargetRank;
	}
}
