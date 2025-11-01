using System;

namespace LoG
{
	// Token: 0x02000708 RID: 1800
	public class DestroyRandomTributeInstance : EdictEffectModuleInstance
	{
		// Token: 0x0600227E RID: 8830 RVA: 0x00078274 File Offset: 0x00076474
		public override void DeepClone(out TurnModuleInstance clone)
		{
			DestroyRandomTributeInstance destroyRandomTributeInstance = new DestroyRandomTributeInstance();
			base.DeepCloneEdictEffectModuleInstanceParts(destroyRandomTributeInstance);
			clone = destroyRandomTributeInstance;
		}
	}
}
