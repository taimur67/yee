using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000717 RID: 1815
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EdictCandidateEffectModuleInstance : EdictEffectModuleInstance
	{
		// Token: 0x0600229B RID: 8859 RVA: 0x00078738 File Offset: 0x00076938
		protected void DeepCloneEdictCandidateEffectModuleInstanceParts(EdictCandidateEffectModuleInstance edictCandidateEffectModuleInstance)
		{
			edictCandidateEffectModuleInstance.TargetPlayerId = this.TargetPlayerId;
			base.DeepCloneEdictEffectModuleInstanceParts(edictCandidateEffectModuleInstance);
		}

		// Token: 0x0600229C RID: 8860 RVA: 0x00078750 File Offset: 0x00076950
		public override void DeepClone(out TurnModuleInstance clone)
		{
			EdictCandidateEffectModuleInstance edictCandidateEffectModuleInstance = new EdictCandidateEffectModuleInstance();
			this.DeepCloneEdictCandidateEffectModuleInstanceParts(edictCandidateEffectModuleInstance);
			clone = edictCandidateEffectModuleInstance;
		}

		// Token: 0x04000F36 RID: 3894
		[JsonProperty]
		public int TargetPlayerId;
	}
}
