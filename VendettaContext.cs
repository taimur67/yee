using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004F0 RID: 1264
	[Serializable]
	public class VendettaContext : GrievanceContext
	{
		// Token: 0x060017EB RID: 6123 RVA: 0x00056609 File Offset: 0x00054809
		public override int GetTotalTarget()
		{
			return this.Objective.GetTotalConditionTargets();
		}

		// Token: 0x060017EC RID: 6124 RVA: 0x00056616 File Offset: 0x00054816
		public override int GetDuration()
		{
			return this.TurnTotal;
		}

		// Token: 0x060017ED RID: 6125 RVA: 0x0005661E File Offset: 0x0005481E
		public override void DeepClone(out GrievanceContext clone)
		{
			clone = new VendettaContext
			{
				Objective = this.Objective.DeepClone<VendettaObjective>(),
				TurnTotal = this.TurnTotal
			};
			base.DeepCloneGrievanceContextParts(clone);
		}

		// Token: 0x04000B6D RID: 2925
		[JsonProperty]
		public VendettaObjective Objective;

		// Token: 0x04000B6E RID: 2926
		[JsonProperty]
		public int TurnTotal;
	}
}
