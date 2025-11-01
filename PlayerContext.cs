using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000386 RID: 902
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PlayerContext : ModifierContext
	{
		// Token: 0x0600112F RID: 4399 RVA: 0x00042AA9 File Offset: 0x00040CA9
		[JsonConstructor]
		public PlayerContext()
		{
		}

		// Token: 0x06001130 RID: 4400 RVA: 0x00042AB1 File Offset: 0x00040CB1
		public PlayerContext(int playerId, string archfiendId = "")
		{
			this.PlayerId = playerId;
			this.ArchfiendId = archfiendId;
		}

		// Token: 0x06001131 RID: 4401 RVA: 0x00042AC7 File Offset: 0x00040CC7
		public override string ToString()
		{
			return string.Format("{0}({1})", this.ArchfiendId, this.PlayerId);
		}

		// Token: 0x06001132 RID: 4402 RVA: 0x00042AE4 File Offset: 0x00040CE4
		protected void DeepClonePlayerContextParts(PlayerContext playerContext)
		{
			playerContext.PlayerId = this.PlayerId;
			playerContext.ArchfiendId = this.ArchfiendId.DeepClone();
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x00042B04 File Offset: 0x00040D04
		public override void DeepClone(out ModifierContext modifierContext)
		{
			PlayerContext playerContext = new PlayerContext();
			this.DeepClonePlayerContextParts(playerContext);
			modifierContext = playerContext;
		}

		// Token: 0x040007F3 RID: 2035
		[JsonProperty]
		public int PlayerId;

		// Token: 0x040007F4 RID: 2036
		[JsonProperty]
		public string ArchfiendId;
	}
}
