using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000718 RID: 1816
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EdictEffectModuleInstance : TurnModuleInstance
	{
		// Token: 0x0600229E RID: 8862 RVA: 0x00078775 File Offset: 0x00076975
		protected void DeepCloneEdictEffectModuleInstanceParts(EdictEffectModuleInstance edictEffectModuleInstance)
		{
			edictEffectModuleInstance.EdictId = this.EdictId.DeepClone();
			edictEffectModuleInstance.EffectId = this.EffectId.DeepClone();
			base.DeepCloneTurnModuleInstanceParts(edictEffectModuleInstance);
		}

		// Token: 0x0600229F RID: 8863 RVA: 0x000787A0 File Offset: 0x000769A0
		public override void DeepClone(out TurnModuleInstance clone)
		{
			EdictEffectModuleInstance edictEffectModuleInstance = new EdictEffectModuleInstance();
			this.DeepCloneEdictEffectModuleInstanceParts(edictEffectModuleInstance);
			clone = edictEffectModuleInstance;
		}

		// Token: 0x04000F37 RID: 3895
		[JsonProperty]
		public string EdictId;

		// Token: 0x04000F38 RID: 3896
		[JsonProperty]
		public string EffectId;
	}
}
