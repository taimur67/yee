using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004A4 RID: 1188
	[BindableGameEvent]
	[Serializable]
	public class SelectLevelUpRewardRequest : DecisionRequest<SelectLevelUpRewardResponse>
	{
		// Token: 0x0600164C RID: 5708 RVA: 0x000528B5 File Offset: 0x00050AB5
		[JsonConstructor]
		public SelectLevelUpRewardRequest(DecisionId decisionId) : base(decisionId)
		{
		}

		// Token: 0x0600164D RID: 5709 RVA: 0x000528C9 File Offset: 0x00050AC9
		public SelectLevelUpRewardRequest(DecisionId decisionId, Identifier gamePieceId, List<GamePieceRewardStaticData> rewardOptions, int oldLevel, int newLevel) : base(decisionId)
		{
			this.OldLevel = oldLevel;
			this.NewLevel = newLevel;
			this.GamePieceId = gamePieceId;
			this.RewardOptions = rewardOptions;
		}

		// Token: 0x0600164E RID: 5710 RVA: 0x000528FB File Offset: 0x00050AFB
		public override TurnLogEntryType GetTurnLogEntryType()
		{
			return TurnLogEntryType.LegionLevelUp;
		}

		// Token: 0x04000B13 RID: 2835
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public int OldLevel;

		// Token: 0x04000B14 RID: 2836
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public int NewLevel;

		// Token: 0x04000B15 RID: 2837
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Identifier GamePieceId;

		// Token: 0x04000B16 RID: 2838
		[JsonProperty]
		public List<GamePieceRewardStaticData> RewardOptions = new List<GamePieceRewardStaticData>();
	}
}
