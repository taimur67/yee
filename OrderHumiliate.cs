using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005DC RID: 1500
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class OrderHumiliate : DiplomaticOrder, IGrievanceAccessor, ISelectionAccessor
	{
		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x06001C26 RID: 7206 RVA: 0x000613A8 File Offset: 0x0005F5A8
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.Humiliate;
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x06001C27 RID: 7207 RVA: 0x000613AB File Offset: 0x0005F5AB
		// (set) Token: 0x06001C28 RID: 7208 RVA: 0x000613B3 File Offset: 0x0005F5B3
		[JsonProperty]
		public GrievanceContext GrievanceResponse { get; set; }

		// Token: 0x06001C29 RID: 7209 RVA: 0x000613BC File Offset: 0x0005F5BC
		[JsonConstructor]
		public OrderHumiliate()
		{
		}

		// Token: 0x06001C2A RID: 7210 RVA: 0x000613C4 File Offset: 0x0005F5C4
		public OrderHumiliate(int targetID, Payment payment, GrievanceContext vendetta) : base(targetID, payment)
		{
			this.GrievanceResponse = vendetta;
		}

		// Token: 0x06001C2B RID: 7211 RVA: 0x000613D5 File Offset: 0x0005F5D5
		public override IEnumerable<OrderTypes> GetRelatedOrderTypes()
		{
			yield return OrderTypes.Insult;
			yield break;
		}
	}
}
