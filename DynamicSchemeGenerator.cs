using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using LoG.Simulation.Extensions;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000469 RID: 1129
	[Serializable]
	public abstract class DynamicSchemeGenerator : SchemeGenerator
	{
		// Token: 0x06001519 RID: 5401 RVA: 0x0004FE4C File Offset: 0x0004E04C
		protected override void SetSchemeParams(SchemeObjective scheme, TurnContext context, PlayerState player)
		{
			ObjectiveDifficulty dynamicDifficulty = scheme.CalculateDynamicDifficulty(context, player);
			scheme.IsGrandScheme = (dynamicDifficulty >= this.GrandThreshold);
			scheme.PrivateReward = (from x in this.PrivateRewards
			where x.Difficulty <= dynamicDifficulty
			select x).SelectMaxOrDefault((SchemeReward x) => x.Difficulty, null).PrestigeReward;
			scheme.PublicReward = (from x in this.PublicRewards
			where x.Difficulty <= dynamicDifficulty
			select x).SelectMaxOrDefault((SchemeReward x) => x.Difficulty, null).PrestigeReward;
		}

		// Token: 0x04000A9E RID: 2718
		[JsonProperty]
		public List<SchemeReward> PublicRewards;

		// Token: 0x04000A9F RID: 2719
		[JsonProperty]
		public List<SchemeReward> PrivateRewards;

		// Token: 0x04000AA0 RID: 2720
		[JsonProperty]
		[DefaultValue(ObjectiveDifficulty.Moderate)]
		public ObjectiveDifficulty GrandThreshold = ObjectiveDifficulty.Moderate;
	}
}
