using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004C3 RID: 1219
	[Serializable]
	public class SelectPowerDecisionResponse : DecisionResponse
	{
		// Token: 0x060016CC RID: 5836 RVA: 0x00053989 File Offset: 0x00051B89
		public void Select(int index)
		{
			this.SelectedPower = index;
		}

		// Token: 0x060016CD RID: 5837 RVA: 0x00053994 File Offset: 0x00051B94
		public override void DeepClone(out DecisionResponse clone)
		{
			SelectPowerDecisionResponse selectPowerDecisionResponse = new SelectPowerDecisionResponse
			{
				SelectedPower = this.SelectedPower
			};
			base.DeepCloneDecisionResponseParts(selectPowerDecisionResponse);
			clone = selectPowerDecisionResponse;
		}

		// Token: 0x04000B3B RID: 2875
		[JsonProperty]
		[DefaultValue(-1)]
		public int SelectedPower = -1;
	}
}
