using System;

namespace LoG
{
	// Token: 0x02000735 RID: 1845
	public class RedistributeTributeInstance : EdictEffectModuleInstance
	{
		// Token: 0x060022D9 RID: 8921 RVA: 0x00079078 File Offset: 0x00077278
		public override void DeepClone(out TurnModuleInstance clone)
		{
			RedistributeTributeInstance redistributeTributeInstance = new RedistributeTributeInstance();
			base.DeepCloneEdictEffectModuleInstanceParts(redistributeTributeInstance);
			clone = redistributeTributeInstance;
		}
	}
}
