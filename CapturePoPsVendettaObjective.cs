using System;

namespace LoG
{
	// Token: 0x02000527 RID: 1319
	public class CapturePoPsVendettaObjective : VendettaObjective
	{
		// Token: 0x170003AC RID: 940
		// (get) Token: 0x060019AE RID: 6574 RVA: 0x0005A26A File Offset: 0x0005846A
		public override string ObjectiveKey
		{
			get
			{
				return base.ObjectiveKey + ".CapturePoPs";
			}
		}

		// Token: 0x060019AF RID: 6575 RVA: 0x0005A27C File Offset: 0x0005847C
		public override void DeepClone(out VendettaObjective clone)
		{
			clone = new CapturePoPsVendettaObjective();
			base.DeepCloneVendettaObjectiveParts(clone);
		}
	}
}
