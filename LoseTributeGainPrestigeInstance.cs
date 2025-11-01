using System;

namespace LoG
{
	// Token: 0x0200072F RID: 1839
	public class LoseTributeGainPrestigeInstance : EdictCandidateEffectModuleInstance
	{
		// Token: 0x060022CD RID: 8909 RVA: 0x00078EE0 File Offset: 0x000770E0
		public override void DeepClone(out TurnModuleInstance clone)
		{
			LoseTributeGainPrestigeInstance loseTributeGainPrestigeInstance = new LoseTributeGainPrestigeInstance();
			base.DeepCloneEdictCandidateEffectModuleInstanceParts(loseTributeGainPrestigeInstance);
			clone = loseTributeGainPrestigeInstance;
		}
	}
}
