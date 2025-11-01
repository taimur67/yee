using System;

namespace LoG
{
	// Token: 0x02000741 RID: 1857
	public class ShuffleRegencyOrderInstance : EdictEffectModuleInstance
	{
		// Token: 0x060022F1 RID: 8945 RVA: 0x00079480 File Offset: 0x00077680
		public override void DeepClone(out TurnModuleInstance clone)
		{
			ShuffleRegencyOrderInstance shuffleRegencyOrderInstance = new ShuffleRegencyOrderInstance();
			base.DeepCloneEdictEffectModuleInstanceParts(shuffleRegencyOrderInstance);
			clone = shuffleRegencyOrderInstance;
		}
	}
}
