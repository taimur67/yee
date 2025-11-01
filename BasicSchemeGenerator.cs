using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200046F RID: 1135
	[Serializable]
	public abstract class BasicSchemeGenerator : SchemeGenerator
	{
		// Token: 0x06001529 RID: 5417 RVA: 0x00050081 File Offset: 0x0004E281
		protected sealed override IEnumerable<SchemeObjective> GenerateSchemesInternal(TurnContext context, PlayerState player)
		{
			yield return this.GenerateScheme(context, player);
			yield break;
		}

		// Token: 0x0600152A RID: 5418 RVA: 0x0005009F File Offset: 0x0004E29F
		protected virtual SchemeObjective GenerateScheme(TurnContext context, PlayerState player)
		{
			return new SchemeObjective(this.GenerateConditions(context, player))
			{
				PublicReward = this.PublicPrestigeReward,
				PrivateReward = this.PrivatePrestigeReward,
				IsGrandScheme = this.IsGrand
			};
		}

		// Token: 0x0600152B RID: 5419 RVA: 0x000500D2 File Offset: 0x0004E2D2
		protected virtual IEnumerable<ObjectiveCondition> GenerateConditions(TurnContext context, PlayerState player)
		{
			return Enumerable.Empty<ObjectiveCondition>();
		}

		// Token: 0x04000AA4 RID: 2724
		[JsonProperty]
		public int PublicPrestigeReward;

		// Token: 0x04000AA5 RID: 2725
		[JsonProperty]
		public int PrivatePrestigeReward;

		// Token: 0x04000AA6 RID: 2726
		[JsonProperty]
		public bool IsGrand;
	}
}
