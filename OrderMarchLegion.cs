using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200063C RID: 1596
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class OrderMarchLegion : OrderMoveLegion
	{
		// Token: 0x06001D82 RID: 7554 RVA: 0x00065EEB File Offset: 0x000640EB
		[JsonConstructor]
		public OrderMarchLegion() : this(Identifier.Invalid, AttackOutcomeIntent.Default, FlankIntent.Undefined, Array.Empty<HexCoord>())
		{
		}

		// Token: 0x06001D83 RID: 7555 RVA: 0x00065EFB File Offset: 0x000640FB
		public OrderMarchLegion(Identifier gamePiece, AttackOutcomeIntent attackOutcomeIntent = AttackOutcomeIntent.Default, FlankIntent flankIntent = FlankIntent.Undefined, params HexCoord[] coords) : base(gamePiece, attackOutcomeIntent, FlankIntent.Undefined)
		{
			this.MovePath = IEnumerableExtensions.ToList<HexCoord>(coords);
		}

		// Token: 0x04000C8E RID: 3214
		[JsonProperty]
		public List<HexCoord> MovePath = new List<HexCoord>();
	}
}
