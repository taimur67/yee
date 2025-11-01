using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000383 RID: 899
	public class TerrainContext : ModifierContext
	{
		// Token: 0x06001126 RID: 4390 RVA: 0x00042A1D File Offset: 0x00040C1D
		public override void DeepClone(out ModifierContext modifierContext)
		{
			modifierContext = new TerrainContext
			{
				TerrainType = this.TerrainType
			};
		}

		// Token: 0x040007F0 RID: 2032
		[JsonProperty]
		public TerrainType TerrainType;
	}
}
