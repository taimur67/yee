using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020001BF RID: 447
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ExpectedBattle : IDeepClone<ExpectedBattle>
	{
		// Token: 0x0600084C RID: 2124 RVA: 0x00027519 File Offset: 0x00025719
		[JsonConstructor]
		public ExpectedBattle()
		{
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x00027533 File Offset: 0x00025733
		public ExpectedBattle(HexCoord location, Identifier ourLegion)
		{
			this.Location = location;
			this.OurLegion = ourLegion;
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x0002755B File Offset: 0x0002575B
		public void DeepClone(out ExpectedBattle clone)
		{
			clone = new ExpectedBattle
			{
				Location = this.Location,
				OurLegion = this.OurLegion,
				IsSupportAlreadyPlanned = this.IsSupportAlreadyPlanned
			};
		}

		// Token: 0x04000403 RID: 1027
		[JsonProperty]
		[DefaultValue(2147483647)]
		public HexCoord Location = HexCoord.Invalid;

		// Token: 0x04000404 RID: 1028
		[JsonProperty]
		[DefaultValue(Identifier.Invalid)]
		public Identifier OurLegion = Identifier.Invalid;

		// Token: 0x04000405 RID: 1029
		[JsonProperty]
		public bool IsSupportAlreadyPlanned;
	}
}
