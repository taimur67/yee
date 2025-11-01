using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020001C1 RID: 449
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class LegionPositionHistory : IDeepClone<LegionPositionHistory>
	{
		// Token: 0x06000851 RID: 2129 RVA: 0x000275B1 File Offset: 0x000257B1
		[JsonConstructor]
		private LegionPositionHistory()
		{
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x000275B9 File Offset: 0x000257B9
		public LegionPositionHistory(HexCoord previous, HexCoord current)
		{
			this.Previous = previous;
			this.Current = current;
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x000275CF File Offset: 0x000257CF
		public void DeepClone(out LegionPositionHistory clone)
		{
			clone = new LegionPositionHistory(this.Previous, this.Current);
		}

		// Token: 0x04000408 RID: 1032
		[JsonProperty]
		public HexCoord Previous;

		// Token: 0x04000409 RID: 1033
		[JsonProperty]
		public HexCoord Current;
	}
}
