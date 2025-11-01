using System;

namespace LoG
{
	// Token: 0x02000711 RID: 1809
	public class DistributeBorderCantonsInstance : EdictCandidateEffectModuleInstance
	{
		// Token: 0x0600228F RID: 8847 RVA: 0x000784E4 File Offset: 0x000766E4
		public override void DeepClone(out TurnModuleInstance clone)
		{
			DistributeBorderCantonsInstance distributeBorderCantonsInstance = new DistributeBorderCantonsInstance();
			base.DeepCloneEdictCandidateEffectModuleInstanceParts(distributeBorderCantonsInstance);
			clone = distributeBorderCantonsInstance;
		}
	}
}
