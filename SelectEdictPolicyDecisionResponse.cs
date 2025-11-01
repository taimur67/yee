using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004BA RID: 1210
	[Serializable]
	public class SelectEdictPolicyDecisionResponse : DecisionResponse
	{
		// Token: 0x060016A2 RID: 5794 RVA: 0x000531C9 File Offset: 0x000513C9
		public void Select(string edictEffectId)
		{
			this.SelectedPolicyId = edictEffectId;
		}

		// Token: 0x060016A3 RID: 5795 RVA: 0x000531D2 File Offset: 0x000513D2
		public bool IsSelected(string edictEffectId)
		{
			return this.SelectedPolicyId == edictEffectId;
		}

		// Token: 0x060016A4 RID: 5796 RVA: 0x000531E0 File Offset: 0x000513E0
		public override void DeepClone(out DecisionResponse clone)
		{
			SelectEdictPolicyDecisionResponse selectEdictPolicyDecisionResponse = new SelectEdictPolicyDecisionResponse
			{
				SelectedPolicyId = this.SelectedPolicyId.DeepClone()
			};
			base.DeepCloneDecisionResponseParts(selectEdictPolicyDecisionResponse);
			clone = selectEdictPolicyDecisionResponse;
		}

		// Token: 0x04000B33 RID: 2867
		[JsonProperty]
		public string SelectedPolicyId;
	}
}
