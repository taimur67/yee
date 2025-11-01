using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200046C RID: 1132
	[Serializable]
	public class IncreaseRankSchemeGenerator : DynamicSchemeGenerator
	{
		// Token: 0x06001520 RID: 5408 RVA: 0x0004FFE3 File Offset: 0x0004E1E3
		protected override IEnumerable<SchemeObjective> GenerateSchemesInternal(TurnContext context, PlayerState player)
		{
			yield return this.GenerateScheme(context, player);
			yield break;
		}

		// Token: 0x06001521 RID: 5409 RVA: 0x00050004 File Offset: 0x0004E204
		protected virtual SchemeObjective GenerateScheme(TurnContext context, PlayerState player)
		{
			return new SchemeObjective(new ObjectiveCondition[]
			{
				new ObjectiveCondition_ArchfiendRank
				{
					Target = (int)this.TargetRank
				}
			});
		}

		// Token: 0x04000AA3 RID: 2723
		[JsonProperty]
		public Rank TargetRank;
	}
}
