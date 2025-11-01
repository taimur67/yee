using System;

namespace LoG
{
	// Token: 0x02000747 RID: 1863
	public class TaxTributeInstance : EdictEffectModuleInstance
	{
		// Token: 0x060022FE RID: 8958 RVA: 0x000796A4 File Offset: 0x000778A4
		public override void DeepClone(out TurnModuleInstance clone)
		{
			TaxTributeInstance taxTributeInstance = new TaxTributeInstance();
			base.DeepCloneEdictEffectModuleInstanceParts(taxTributeInstance);
			clone = taxTributeInstance;
		}
	}
}
