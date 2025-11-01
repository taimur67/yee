using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005E4 RID: 1508
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class OrderRequestLureOfExcess : DiplomaticOrder
	{
		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06001C43 RID: 7235 RVA: 0x0006175E File Offset: 0x0005F95E
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.LureOfExcess;
			}
		}

		// Token: 0x06001C44 RID: 7236 RVA: 0x00061762 File Offset: 0x0005F962
		[JsonConstructor]
		public OrderRequestLureOfExcess()
		{
		}

		// Token: 0x06001C45 RID: 7237 RVA: 0x0006176A File Offset: 0x0005F96A
		public OrderRequestLureOfExcess(int targetID) : base(targetID, null)
		{
		}
	}
}
