using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005E0 RID: 1504
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class OrderMakeDemand : DiplomaticDemandOrder
	{
		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x06001C34 RID: 7220 RVA: 0x000614D5 File Offset: 0x0005F6D5
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.Demand;
			}
		}

		// Token: 0x06001C35 RID: 7221 RVA: 0x000614D8 File Offset: 0x0005F6D8
		public OrderMakeDemand(int targetID, Payment payment = null) : base(targetID, payment)
		{
		}

		// Token: 0x06001C36 RID: 7222 RVA: 0x000614E2 File Offset: 0x0005F6E2
		[JsonConstructor]
		public OrderMakeDemand()
		{
		}

		// Token: 0x06001C37 RID: 7223 RVA: 0x000614EC File Offset: 0x0005F6EC
		public static int GetMaximumTokenCount(PlayerState player)
		{
			int result;
			switch (player.Rank)
			{
			case Rank.Marquis:
				result = 4;
				break;
			case Rank.Duke:
				result = 5;
				break;
			case Rank.Prince:
				result = 6;
				break;
			default:
				result = 3;
				break;
			}
			return result;
		}

		// Token: 0x06001C38 RID: 7224 RVA: 0x00061528 File Offset: 0x0005F728
		public static DemandOptions GetMaximumTokenDemandOption(PlayerState player)
		{
			DemandOptions result;
			switch (OrderMakeDemand.GetMaximumTokenCount(player))
			{
			case 4:
				result = DemandOptions.FourTribute;
				break;
			case 5:
				result = DemandOptions.FiveTribute;
				break;
			case 6:
				result = DemandOptions.SixTribute;
				break;
			default:
				result = DemandOptions.ThreeTribute;
				break;
			}
			return result;
		}

		// Token: 0x06001C39 RID: 7225 RVA: 0x00061561 File Offset: 0x0005F761
		public override IEnumerable<OrderTypes> GetRelatedOrderTypes()
		{
			yield return OrderTypes.Extort;
			yield break;
		}

		// Token: 0x06001C3A RID: 7226 RVA: 0x0006156C File Offset: 0x0005F76C
		public override List<DemandOptions> GetValidOptions(PlayerState player, TurnState turnState)
		{
			List<DemandOptions> list = new List<DemandOptions>();
			switch (OrderMakeDemand.GetMaximumTokenCount(player))
			{
			case 3:
				list.Add(DemandOptions.ThreeTribute);
				return list;
			case 4:
				list.Add(DemandOptions.FourTribute);
				list.Add(DemandOptions.ThreeTribute);
				return list;
			case 5:
				list.Add(DemandOptions.FiveTribute);
				list.Add(DemandOptions.FourTribute);
				list.Add(DemandOptions.ThreeTribute);
				return list;
			case 6:
				list.Add(DemandOptions.SixTribute);
				list.Add(DemandOptions.FiveTribute);
				list.Add(DemandOptions.FourTribute);
				list.Add(DemandOptions.ThreeTribute);
				return list;
			}
			list.Add(DemandOptions.TwoTribute);
			return list;
		}
	}
}
