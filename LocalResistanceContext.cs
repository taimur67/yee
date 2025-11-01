using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200037C RID: 892
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class LocalResistanceContext : ModifierContext
	{
		// Token: 0x06001111 RID: 4369 RVA: 0x000428A8 File Offset: 0x00040AA8
		[JsonConstructor]
		public LocalResistanceContext()
		{
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x000428B0 File Offset: 0x00040AB0
		public LocalResistanceContext(Identifier gamePieceId, int localResistanceBonus)
		{
			this.GamePieceId = gamePieceId;
			this.LocalResistanceBonus = localResistanceBonus;
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x000428C6 File Offset: 0x00040AC6
		public override void DeepClone(out ModifierContext modifierContext)
		{
			modifierContext = new LocalResistanceContext
			{
				LocalResistanceBonus = this.LocalResistanceBonus,
				GamePieceId = this.GamePieceId
			};
		}

		// Token: 0x040007E8 RID: 2024
		[JsonProperty]
		public Identifier GamePieceId;

		// Token: 0x040007E9 RID: 2025
		[JsonProperty]
		public int LocalResistanceBonus;
	}
}
