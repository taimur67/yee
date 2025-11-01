using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005D5 RID: 1493
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class OrderDeclareBloodFeud : DiplomaticOrder
	{
		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06001C11 RID: 7185 RVA: 0x000610B2 File Offset: 0x0005F2B2
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.DeclareBloodFeud;
			}
		}

		// Token: 0x06001C12 RID: 7186 RVA: 0x000610B6 File Offset: 0x0005F2B6
		[JsonConstructor]
		public OrderDeclareBloodFeud()
		{
		}

		// Token: 0x06001C13 RID: 7187 RVA: 0x000610BE File Offset: 0x0005F2BE
		public OrderDeclareBloodFeud(int targetID) : base(targetID, null)
		{
		}

		// Token: 0x06001C14 RID: 7188 RVA: 0x000610C8 File Offset: 0x0005F2C8
		public override IEnumerable<OrderTypes> GetRelatedOrderTypes()
		{
			yield return OrderTypes.AssertWeakness;
			yield break;
		}
	}
}
