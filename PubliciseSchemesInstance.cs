using System;

namespace LoG
{
	// Token: 0x02000732 RID: 1842
	public class PubliciseSchemesInstance : EdictCandidateEffectModuleInstance
	{
		// Token: 0x060022D3 RID: 8915 RVA: 0x00078FB0 File Offset: 0x000771B0
		public override void DeepClone(out TurnModuleInstance clone)
		{
			PubliciseSchemesInstance publiciseSchemesInstance = new PubliciseSchemesInstance();
			base.DeepCloneEdictCandidateEffectModuleInstanceParts(publiciseSchemesInstance);
			clone = publiciseSchemesInstance;
		}
	}
}
