using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004AE RID: 1198
	[Serializable]
	public class DecisionResponse : ISteppedFlowControl, IDeepClone<DecisionResponse>
	{
		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06001672 RID: 5746 RVA: 0x00052D36 File Offset: 0x00050F36
		// (set) Token: 0x06001673 RID: 5747 RVA: 0x00052D3E File Offset: 0x00050F3E
		[JsonProperty]
		public DecisionId DecisionId { get; set; }

		// Token: 0x06001674 RID: 5748 RVA: 0x00052D47 File Offset: 0x00050F47
		public virtual string GetDebugString()
		{
			return this.ToString();
		}

		// Token: 0x06001675 RID: 5749 RVA: 0x00052D4F File Offset: 0x00050F4F
		public virtual IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			return Enumerable.Empty<ActionPhase>();
		}

		// Token: 0x06001676 RID: 5750 RVA: 0x00052D56 File Offset: 0x00050F56
		protected void DeepCloneDecisionResponseParts(DecisionResponse decisionResponse)
		{
			decisionResponse.DecisionId = this.DecisionId;
		}

		// Token: 0x06001677 RID: 5751 RVA: 0x00052D64 File Offset: 0x00050F64
		public virtual void DeepClone(out DecisionResponse clone)
		{
			clone = new DecisionResponse();
			this.DeepCloneDecisionResponseParts(clone);
		}

		// Token: 0x06001678 RID: 5752 RVA: 0x00052D75 File Offset: 0x00050F75
		public virtual IEnumerable<ResourceNFT> GetReservedResources()
		{
			yield break;
		}
	}
}
