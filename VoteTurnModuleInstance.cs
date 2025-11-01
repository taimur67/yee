using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200074E RID: 1870
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class VoteTurnModuleInstance : TurnModuleInstance
	{
		// Token: 0x06002310 RID: 8976 RVA: 0x00079C54 File Offset: 0x00077E54
		protected void DeepCloneVoteTurnModuleInstanceParts(VoteTurnModuleInstance voteTurnModuleInstance)
		{
			voteTurnModuleInstance.Unannounced = this.Unannounced;
			voteTurnModuleInstance.EdictId = this.EdictId.DeepClone();
			voteTurnModuleInstance.VoteTurn = this.VoteTurn;
			base.DeepCloneTurnModuleInstanceParts(voteTurnModuleInstance);
		}

		// Token: 0x06002311 RID: 8977 RVA: 0x00079C88 File Offset: 0x00077E88
		public override void DeepClone(out TurnModuleInstance clone)
		{
			VoteTurnModuleInstance voteTurnModuleInstance = new VoteTurnModuleInstance();
			this.DeepCloneVoteTurnModuleInstanceParts(voteTurnModuleInstance);
			clone = voteTurnModuleInstance;
		}

		// Token: 0x04000F4A RID: 3914
		[JsonProperty]
		public bool Unannounced;

		// Token: 0x04000F4B RID: 3915
		[JsonProperty]
		public string EdictId;

		// Token: 0x04000F4C RID: 3916
		[JsonProperty]
		public int VoteTurn;
	}
}
