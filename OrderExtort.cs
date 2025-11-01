using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005DA RID: 1498
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class OrderExtort : DiplomaticDemandOrder
	{
		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x06001C1E RID: 7198 RVA: 0x000612A0 File Offset: 0x0005F4A0
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.Extort;
			}
		}

		// Token: 0x06001C1F RID: 7199 RVA: 0x000612A3 File Offset: 0x0005F4A3
		public OrderExtort(int targetID, Payment payment) : base(targetID, payment)
		{
		}

		// Token: 0x06001C20 RID: 7200 RVA: 0x000612AD File Offset: 0x0005F4AD
		[JsonConstructor]
		public OrderExtort()
		{
		}

		// Token: 0x06001C21 RID: 7201 RVA: 0x000612B5 File Offset: 0x0005F4B5
		public override List<DemandOptions> GetValidOptions(PlayerState player, TurnState turnState)
		{
			return new List<DemandOptions>
			{
				DemandOptions.SelectedArtifact,
				DemandOptions.SelectedPraetor
			};
		}

		// Token: 0x06001C22 RID: 7202 RVA: 0x000612CA File Offset: 0x0005F4CA
		public override IEnumerable<OrderTypes> GetRelatedOrderTypes()
		{
			yield return OrderTypes.Demand;
			yield break;
		}
	}
}
