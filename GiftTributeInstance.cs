using System;

namespace LoG
{
	// Token: 0x0200071D RID: 1821
	public class GiftTributeInstance : EdictEffectModuleInstance
	{
		// Token: 0x060022A9 RID: 8873 RVA: 0x00078870 File Offset: 0x00076A70
		public override void DeepClone(out TurnModuleInstance clone)
		{
			GiftTributeInstance giftTributeInstance = new GiftTributeInstance();
			base.DeepCloneEdictEffectModuleInstanceParts(giftTributeInstance);
			clone = giftTributeInstance;
		}
	}
}
