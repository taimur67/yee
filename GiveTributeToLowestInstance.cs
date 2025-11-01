using System;

namespace LoG
{
	// Token: 0x02000723 RID: 1827
	public class GiveTributeToLowestInstance : EdictEffectModuleInstance
	{
		// Token: 0x060022B5 RID: 8885 RVA: 0x00078AF0 File Offset: 0x00076CF0
		public override void DeepClone(out TurnModuleInstance clone)
		{
			GiveTributeToLowestInstance giveTributeToLowestInstance = new GiveTributeToLowestInstance();
			base.DeepCloneEdictEffectModuleInstanceParts(giveTributeToLowestInstance);
			clone = giveTributeToLowestInstance;
		}
	}
}
