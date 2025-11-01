using System;

namespace LoG
{
	// Token: 0x02000738 RID: 1848
	public class RefreshBazaarInstance : EdictEffectModuleInstance
	{
		// Token: 0x060022DF RID: 8927 RVA: 0x00079278 File Offset: 0x00077478
		public override void DeepClone(out TurnModuleInstance clone)
		{
			RefreshBazaarInstance refreshBazaarInstance = new RefreshBazaarInstance();
			base.DeepCloneEdictEffectModuleInstanceParts(refreshBazaarInstance);
			clone = refreshBazaarInstance;
		}
	}
}
