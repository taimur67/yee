using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000650 RID: 1616
	[Serializable]
	public class OrderConflictProblem : Problem
	{
		// Token: 0x06001DEE RID: 7662 RVA: 0x000672EE File Offset: 0x000654EE
		public OrderConflictProblem()
		{
		}

		// Token: 0x06001DEF RID: 7663 RVA: 0x000672F6 File Offset: 0x000654F6
		public OrderConflictProblem(ActionConflict conflict, ActionableOrder order, ActionableOrder conflictedOrder)
		{
			this.Conflict = conflict;
			this.Order = order;
			this.ConflictedOrder = conflictedOrder;
		}

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x06001DF0 RID: 7664 RVA: 0x00067313 File Offset: 0x00065513
		public override string DebugString
		{
			get
			{
				return string.Format("Conflict ({0}) between ({1}) and ({2})", this.Conflict, this.Order, this.ConflictedOrder);
			}
		}

		// Token: 0x04000CC7 RID: 3271
		[JsonProperty]
		public ActionableOrder Order;

		// Token: 0x04000CC8 RID: 3272
		[JsonProperty]
		public ActionableOrder ConflictedOrder;

		// Token: 0x04000CC9 RID: 3273
		[JsonProperty]
		public ActionConflict Conflict;
	}
}
