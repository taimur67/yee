using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200037D RID: 893
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class LevelContext : ModifierContext
	{
		// Token: 0x06001114 RID: 4372 RVA: 0x000428E7 File Offset: 0x00040AE7
		[JsonConstructor]
		public LevelContext()
		{
		}

		// Token: 0x06001115 RID: 4373 RVA: 0x000428EF File Offset: 0x00040AEF
		public LevelContext(Identifier gamePieceId, int level)
		{
			this.GamePieceId = gamePieceId;
			this.Level = level;
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x00042905 File Offset: 0x00040B05
		public override void DeepClone(out ModifierContext modifierContext)
		{
			modifierContext = new LevelContext
			{
				Level = this.Level,
				GamePieceId = this.GamePieceId
			};
		}

		// Token: 0x040007EA RID: 2026
		[JsonProperty]
		public Identifier GamePieceId;

		// Token: 0x040007EB RID: 2027
		[JsonProperty]
		public int Level;
	}
}
