using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005E6 RID: 1510
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class OrderRequestToBeBloodLordOfTarget : DiplomaticOrder
	{
		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x06001C48 RID: 7240 RVA: 0x000617FB File Offset: 0x0005F9FB
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.RequestToBeBloodLordOfTarget;
			}
		}

		// Token: 0x06001C49 RID: 7241 RVA: 0x000617FF File Offset: 0x0005F9FF
		[JsonConstructor]
		public OrderRequestToBeBloodLordOfTarget()
		{
		}

		// Token: 0x06001C4A RID: 7242 RVA: 0x00061807 File Offset: 0x0005FA07
		public OrderRequestToBeBloodLordOfTarget(int targetID, Payment payment) : base(targetID, payment)
		{
		}

		// Token: 0x06001C4B RID: 7243 RVA: 0x00061811 File Offset: 0x0005FA11
		public override IEnumerable<ActionConflict> GeneratePotentialConflicts()
		{
			foreach (ActionConflict actionConflict in base.GeneratePotentialConflicts())
			{
				yield return actionConflict;
			}
			IEnumerator<ActionConflict> enumerator = null;
			yield return new ActionTypeConflict(this);
			yield break;
			yield break;
		}

		// Token: 0x06001C4C RID: 7244 RVA: 0x00061821 File Offset: 0x0005FA21
		public override IEnumerable<OrderTypes> GetRelatedOrderTypes()
		{
			yield return OrderTypes.RequestToBeVassalizedByTarget;
			yield return OrderTypes.SendEmissary;
			yield break;
		}

		// Token: 0x06001C4D RID: 7245 RVA: 0x0006182C File Offset: 0x0005FA2C
		public override Result ConflictProblem(ActionConflict conflict, ActionableOrder conflictingOrder)
		{
			DiplomaticOrder diplomaticOrder = conflictingOrder as DiplomaticOrder;
			if (diplomaticOrder != null)
			{
				return new Result.VassalageQueuedProblem(diplomaticOrder.TargetID, this.OrderType, conflictingOrder.OrderType);
			}
			return new Result.QueuedDiplomacyActionAgainstTargetProblem(this.TargetID, this.OrderType, conflictingOrder.OrderType);
		}
	}
}
