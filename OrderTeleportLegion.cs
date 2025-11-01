using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200064A RID: 1610
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class OrderTeleportLegion : OrderMoveLegion
	{
		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x06001DB3 RID: 7603 RVA: 0x000666EE File Offset: 0x000648EE
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.TeleportLegion;
			}
		}

		// Token: 0x06001DB4 RID: 7604 RVA: 0x000666F2 File Offset: 0x000648F2
		[JsonConstructor]
		public OrderTeleportLegion()
		{
		}

		// Token: 0x06001DB5 RID: 7605 RVA: 0x000666FA File Offset: 0x000648FA
		public OrderTeleportLegion(Identifier gamePiece, AttackOutcomeIntent returnPanda = AttackOutcomeIntent.Default, FlankIntent moveIntent = FlankIntent.Undefined) : base(gamePiece, returnPanda, moveIntent)
		{
		}

		// Token: 0x04000C97 RID: 3223
		[JsonProperty]
		public HexCoord DestinationHex;
	}
}
