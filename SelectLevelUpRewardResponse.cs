using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004A5 RID: 1189
	[Serializable]
	public class SelectLevelUpRewardResponse : DecisionResponse
	{
		// Token: 0x0600164F RID: 5711 RVA: 0x000528FF File Offset: 0x00050AFF
		[JsonConstructor]
		public SelectLevelUpRewardResponse()
		{
		}

		// Token: 0x06001650 RID: 5712 RVA: 0x0005290E File Offset: 0x00050B0E
		public bool IsSelected(int index)
		{
			return this.SelectedRewardIndex == index;
		}

		// Token: 0x06001651 RID: 5713 RVA: 0x00052919 File Offset: 0x00050B19
		public void Select(int index)
		{
			this.SelectedRewardIndex = index;
		}

		// Token: 0x06001652 RID: 5714 RVA: 0x00052924 File Offset: 0x00050B24
		public override void DeepClone(out DecisionResponse clone)
		{
			SelectLevelUpRewardResponse selectLevelUpRewardResponse = new SelectLevelUpRewardResponse
			{
				SelectedRewardIndex = this.SelectedRewardIndex
			};
			base.DeepCloneDecisionResponseParts(selectLevelUpRewardResponse);
			clone = selectLevelUpRewardResponse;
		}

		// Token: 0x04000B17 RID: 2839
		public const int InvalidRewardIndex = -1;

		// Token: 0x04000B18 RID: 2840
		[JsonProperty]
		[DefaultValue(-1)]
		public int SelectedRewardIndex = -1;
	}
}
