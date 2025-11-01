using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200038F RID: 911
	[Serializable]
	public class RankContext : ModifierContext
	{
		// Token: 0x06001151 RID: 4433 RVA: 0x00042D67 File Offset: 0x00040F67
		[JsonConstructor]
		public RankContext()
		{
		}

		// Token: 0x06001152 RID: 4434 RVA: 0x00042D6F File Offset: 0x00040F6F
		public RankContext(int playerId, Rank rank)
		{
			this.PlayerId = playerId;
			this.Rank = rank;
		}

		// Token: 0x06001153 RID: 4435 RVA: 0x00042D85 File Offset: 0x00040F85
		public override string ToString()
		{
			return this.Rank.ToString();
		}

		// Token: 0x06001154 RID: 4436 RVA: 0x00042D98 File Offset: 0x00040F98
		public override void DeepClone(out ModifierContext modifierContext)
		{
			modifierContext = new RankContext
			{
				Rank = this.Rank,
				PlayerId = this.PlayerId
			};
		}

		// Token: 0x040007FD RID: 2045
		[JsonProperty]
		public Rank Rank;

		// Token: 0x040007FE RID: 2046
		[JsonProperty]
		public int PlayerId;
	}
}
