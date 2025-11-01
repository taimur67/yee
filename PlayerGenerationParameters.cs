using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200028C RID: 652
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PlayerGenerationParameters
	{
		// Token: 0x06000CA5 RID: 3237 RVA: 0x00031F8C File Offset: 0x0003018C
		[JsonConstructor]
		public PlayerGenerationParameters()
		{
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x00031F9F File Offset: 0x0003019F
		public PlayerGenerationParameters(PlayerRole role, string archfiendId, int baseOrderSlots)
		{
			this.Role = role;
			this.ArchfiendId = archfiendId;
			this.BaseOrderSlots = baseOrderSlots;
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x00031FC7 File Offset: 0x000301C7
		public void SetArchfiend(string archfiendId)
		{
			this.ArchfiendId = archfiendId;
		}

		// Token: 0x040005A0 RID: 1440
		[JsonProperty]
		public string PlayFabId;

		// Token: 0x040005A1 RID: 1441
		[JsonProperty]
		public string PlatformDisplayName;

		// Token: 0x040005A2 RID: 1442
		[JsonProperty]
		public string ArchfiendId;

		// Token: 0x040005A3 RID: 1443
		[JsonProperty]
		public List<string> Relics = new List<string>();

		// Token: 0x040005A4 RID: 1444
		[JsonProperty]
		public int BaseOrderSlots;

		// Token: 0x040005A5 RID: 1445
		[JsonProperty]
		public PlayerRole Role;

		// Token: 0x040005A6 RID: 1446
		[JsonProperty]
		public AIDifficulty Difficulty;
	}
}
