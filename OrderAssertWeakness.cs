using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005D3 RID: 1491
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class OrderAssertWeakness : DiplomaticOrder, IGrievanceAccessor, ISelectionAccessor
	{
		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06001C08 RID: 7176 RVA: 0x00060F35 File Offset: 0x0005F135
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.AssertWeakness;
			}
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06001C09 RID: 7177 RVA: 0x00060F39 File Offset: 0x0005F139
		// (set) Token: 0x06001C0A RID: 7178 RVA: 0x00060F41 File Offset: 0x0005F141
		[JsonProperty]
		public GrievanceContext GrievanceResponse { get; set; }

		// Token: 0x06001C0B RID: 7179 RVA: 0x00060F4A File Offset: 0x0005F14A
		[JsonConstructor]
		public OrderAssertWeakness()
		{
		}

		// Token: 0x06001C0C RID: 7180 RVA: 0x00060F52 File Offset: 0x0005F152
		public OrderAssertWeakness(int targetID) : base(targetID, null)
		{
		}

		// Token: 0x06001C0D RID: 7181 RVA: 0x00060F5C File Offset: 0x0005F15C
		public override IEnumerable<OrderTypes> GetRelatedOrderTypes()
		{
			yield return OrderTypes.DeclareBloodFeud;
			yield break;
		}
	}
}
