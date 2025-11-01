using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000647 RID: 1607
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class OrderSeekManuscripts : ActionableOrder
	{
		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x06001DA8 RID: 7592 RVA: 0x00066475 File Offset: 0x00064675
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.SeekManuscripts;
			}
		}

		// Token: 0x06001DA9 RID: 7593 RVA: 0x00066479 File Offset: 0x00064679
		[JsonConstructor]
		public OrderSeekManuscripts()
		{
		}
	}
}
