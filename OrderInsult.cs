using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005DE RID: 1502
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class OrderInsult : DiplomaticOrder
	{
		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x06001C2E RID: 7214 RVA: 0x00061451 File Offset: 0x0005F651
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.Insult;
			}
		}

		// Token: 0x06001C2F RID: 7215 RVA: 0x00061454 File Offset: 0x0005F654
		[JsonConstructor]
		public OrderInsult()
		{
		}

		// Token: 0x06001C30 RID: 7216 RVA: 0x0006145C File Offset: 0x0005F65C
		public OrderInsult(int targetID) : base(targetID, null)
		{
		}

		// Token: 0x06001C31 RID: 7217 RVA: 0x00061466 File Offset: 0x0005F666
		public override IEnumerable<OrderTypes> GetRelatedOrderTypes()
		{
			yield return OrderTypes.Humiliate;
			yield break;
		}
	}
}
