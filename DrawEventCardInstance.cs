using System;

namespace LoG
{
	// Token: 0x02000714 RID: 1812
	public class DrawEventCardInstance : EdictEffectModuleInstance
	{
		// Token: 0x06002295 RID: 8853 RVA: 0x00078670 File Offset: 0x00076870
		public override void DeepClone(out TurnModuleInstance clone)
		{
			DrawEventCardInstance drawEventCardInstance = new DrawEventCardInstance();
			base.DeepCloneEdictEffectModuleInstanceParts(drawEventCardInstance);
			clone = drawEventCardInstance;
		}
	}
}
