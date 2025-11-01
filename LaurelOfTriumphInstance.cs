using System;

namespace LoG
{
	// Token: 0x02000726 RID: 1830
	public class LaurelOfTriumphInstance : EdictCandidateEffectModuleInstance
	{
		// Token: 0x060022BB RID: 8891 RVA: 0x00078C00 File Offset: 0x00076E00
		public override void DeepClone(out TurnModuleInstance clone)
		{
			LaurelOfTriumphInstance laurelOfTriumphInstance = new LaurelOfTriumphInstance();
			base.DeepCloneEdictCandidateEffectModuleInstanceParts(laurelOfTriumphInstance);
			clone = laurelOfTriumphInstance;
		}
	}
}
