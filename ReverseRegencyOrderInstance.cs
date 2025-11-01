using System;

namespace LoG
{
	// Token: 0x0200073E RID: 1854
	public class ReverseRegencyOrderInstance : EdictEffectModuleInstance
	{
		// Token: 0x060022EB RID: 8939 RVA: 0x00079418 File Offset: 0x00077618
		public override void DeepClone(out TurnModuleInstance clone)
		{
			ReverseRegencyOrderInstance reverseRegencyOrderInstance = new ReverseRegencyOrderInstance();
			base.DeepCloneEdictEffectModuleInstanceParts(reverseRegencyOrderInstance);
			clone = reverseRegencyOrderInstance;
		}
	}
}
