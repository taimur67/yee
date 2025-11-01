using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x02000651 RID: 1617
	public static class PlayerTurnExtensions
	{
		// Token: 0x06001DF1 RID: 7665 RVA: 0x00067334 File Offset: 0x00065534
		public static Result CheckConflicts(this PlayerTurn turn, ActionableOrder order)
		{
			List<ActionConflict> list = order.GeneratePotentialConflicts().ToList<ActionConflict>();
			if (list.Count > 0)
			{
				foreach (ActionableOrder actionableOrder in turn.Orders)
				{
					if (!(actionableOrder.ActionInstanceId == order.ActionInstanceId))
					{
						List<ActionConflict> list2 = IEnumerableExtensions.ToList<ActionConflict>(actionableOrder.GeneratePotentialConflicts());
						foreach (ActionConflict actionConflict in list)
						{
							foreach (ActionConflict other in list2)
							{
								if (actionConflict.ConflictsWith(other))
								{
									return order.ConflictProblem(actionConflict, actionableOrder);
								}
							}
						}
					}
				}
			}
			return Result.Success;
		}
	}
}
