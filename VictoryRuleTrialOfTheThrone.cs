using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020006FE RID: 1790
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class VictoryRuleTrialOfTheThrone : VictoryRule
	{
		// Token: 0x06002249 RID: 8777 RVA: 0x000775C5 File Offset: 0x000757C5
		public override VictoryRuleProcessor CreateAndInitProcessor(TurnState turnState)
		{
			VictoryRuleProcessorTrialOfTheThrone victoryRuleProcessorTrialOfTheThrone = new VictoryRuleProcessorTrialOfTheThrone();
			victoryRuleProcessorTrialOfTheThrone.Init(this, turnState);
			return victoryRuleProcessorTrialOfTheThrone;
		}

		// Token: 0x0600224A RID: 8778 RVA: 0x000775D4 File Offset: 0x000757D4
		public override void DeepClone(out VictoryRule clone)
		{
			clone = new VictoryRuleTrialOfTheThrone
			{
				TrialLengthMin = this.TrialLengthMin,
				TrialLengthMax = this.TrialLengthMax
			};
		}

		// Token: 0x04000F24 RID: 3876
		[JsonProperty]
		[DefaultValue(5)]
		public int TrialLengthMin = 5;

		// Token: 0x04000F25 RID: 3877
		[JsonProperty]
		[DefaultValue(13)]
		public int TrialLengthMax = 13;
	}
}
