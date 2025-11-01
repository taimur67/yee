using System;

namespace LoG
{
	// Token: 0x02000720 RID: 1824
	public class GivePrestigeToLowestInstance : EdictEffectModuleInstance
	{
		// Token: 0x060022AF RID: 8879 RVA: 0x000789CC File Offset: 0x00076BCC
		public override void DeepClone(out TurnModuleInstance clone)
		{
			GivePrestigeToLowestInstance givePrestigeToLowestInstance = new GivePrestigeToLowestInstance();
			base.DeepCloneEdictEffectModuleInstanceParts(givePrestigeToLowestInstance);
			clone = givePrestigeToLowestInstance;
		}
	}
}
