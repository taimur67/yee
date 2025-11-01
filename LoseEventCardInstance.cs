using System;

namespace LoG
{
	// Token: 0x02000729 RID: 1833
	public class LoseEventCardInstance : EdictEffectModuleInstance
	{
		// Token: 0x060022C1 RID: 8897 RVA: 0x00078CCC File Offset: 0x00076ECC
		public override void DeepClone(out TurnModuleInstance clone)
		{
			LoseEventCardInstance loseEventCardInstance = new LoseEventCardInstance();
			base.DeepCloneEdictEffectModuleInstanceParts(loseEventCardInstance);
			clone = loseEventCardInstance;
		}
	}
}
