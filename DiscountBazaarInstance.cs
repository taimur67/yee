using System;

namespace LoG
{
	// Token: 0x0200070E RID: 1806
	public class DiscountBazaarInstance : EdictEffectModuleInstance
	{
		// Token: 0x0600228A RID: 8842 RVA: 0x0007844C File Offset: 0x0007664C
		public override void DeepClone(out TurnModuleInstance clone)
		{
			DiscountBazaarInstance discountBazaarInstance = new DiscountBazaarInstance();
			base.DeepCloneEdictEffectModuleInstanceParts(discountBazaarInstance);
			clone = discountBazaarInstance;
		}
	}
}
