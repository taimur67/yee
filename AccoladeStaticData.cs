using System;
using System.ComponentModel;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000422 RID: 1058
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public abstract class AccoladeStaticData : IdentifiableStaticData
	{
		// Token: 0x170002FD RID: 765
		// (get) Token: 0x060014B7 RID: 5303 RVA: 0x0004F350 File Offset: 0x0004D550
		public int AwardLimit
		{
			get
			{
				if (this.TieBreakBehaviour != TieBreakBehaviour.AcceptAll)
				{
					return this._awardLimit;
				}
				return int.MaxValue;
			}
		}

		// Token: 0x060014B8 RID: 5304
		public abstract int DetermineMetric(TurnContext context, PlayerState player);

		// Token: 0x040009DD RID: 2525
		[JsonProperty]
		[DefaultValue(1)]
		public int _awardLimit = 1;

		// Token: 0x040009DE RID: 2526
		[JsonProperty]
		public TieBreakBehaviour TieBreakBehaviour;

		// Token: 0x040009DF RID: 2527
		[JsonProperty]
		public int Rank;

		// Token: 0x040009E0 RID: 2528
		[JsonProperty]
		public Evaluation Eval;

		// Token: 0x040009E1 RID: 2529
		[JsonProperty]
		public StatisticMetricAccumulation Accumulation;
	}
}
