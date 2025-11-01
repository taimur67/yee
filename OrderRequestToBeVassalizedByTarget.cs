using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005E8 RID: 1512
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class OrderRequestToBeVassalizedByTarget : DiplomaticOrder
	{
		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x06001C55 RID: 7253 RVA: 0x00061BC2 File Offset: 0x0005FDC2
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.RequestToBeVassalizedByTarget;
			}
		}

		// Token: 0x06001C56 RID: 7254 RVA: 0x00061BC6 File Offset: 0x0005FDC6
		[JsonConstructor]
		public OrderRequestToBeVassalizedByTarget()
		{
		}

		// Token: 0x06001C57 RID: 7255 RVA: 0x00061BCE File Offset: 0x0005FDCE
		public OrderRequestToBeVassalizedByTarget(int targetID, Payment payment) : base(targetID, payment)
		{
		}

		// Token: 0x06001C58 RID: 7256 RVA: 0x00061BD8 File Offset: 0x0005FDD8
		public override IEnumerable<OrderTypes> GetRelatedOrderTypes()
		{
			yield return OrderTypes.RequestToBeBloodLordOfTarget;
			yield return OrderTypes.SendEmissary;
			yield break;
		}
	}
}
