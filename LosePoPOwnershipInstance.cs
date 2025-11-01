using System;

namespace LoG
{
	// Token: 0x0200072C RID: 1836
	public class LosePoPOwnershipInstance : EdictCandidateEffectModuleInstance
	{
		// Token: 0x060022C7 RID: 8903 RVA: 0x00078DA4 File Offset: 0x00076FA4
		public override void DeepClone(out TurnModuleInstance clone)
		{
			LosePoPOwnershipInstance losePoPOwnershipInstance = new LosePoPOwnershipInstance();
			base.DeepCloneEdictCandidateEffectModuleInstanceParts(losePoPOwnershipInstance);
			clone = losePoPOwnershipInstance;
		}
	}
}
