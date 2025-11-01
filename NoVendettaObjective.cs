using System;

namespace LoG
{
	// Token: 0x02000528 RID: 1320
	public class NoVendettaObjective : VendettaObjective
	{
		// Token: 0x060019B1 RID: 6577 RVA: 0x0005A295 File Offset: 0x00058495
		public override bool IsCompleted(TurnState turn)
		{
			return false;
		}

		// Token: 0x060019B2 RID: 6578 RVA: 0x0005A298 File Offset: 0x00058498
		public override void DeepClone(out VendettaObjective clone)
		{
			clone = new NoVendettaObjective();
			base.DeepCloneVendettaObjectiveParts(clone);
		}
	}
}
