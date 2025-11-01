using System;

namespace LoG
{
	// Token: 0x02000526 RID: 1318
	public class CaptureHexesVendettaObjective : VendettaObjective
	{
		// Token: 0x170003AB RID: 939
		// (get) Token: 0x060019AB RID: 6571 RVA: 0x0005A23F File Offset: 0x0005843F
		public override string ObjectiveKey
		{
			get
			{
				return base.ObjectiveKey + ".CaptureHexes";
			}
		}

		// Token: 0x060019AC RID: 6572 RVA: 0x0005A251 File Offset: 0x00058451
		public override void DeepClone(out VendettaObjective clone)
		{
			clone = new CaptureHexesVendettaObjective();
			base.DeepCloneVendettaObjectiveParts(clone);
		}
	}
}
