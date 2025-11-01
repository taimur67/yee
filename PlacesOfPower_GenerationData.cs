using System;
using System.Linq;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000453 RID: 1107
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PlacesOfPower_GenerationData : IdentifiableStaticData
	{
		// Token: 0x060014FC RID: 5372 RVA: 0x0004FA98 File Offset: 0x0004DC98
		public GenerationRange GetNumPoPs(BoardSize_GenerationData boardPreset)
		{
			return this.NumPoPs.FirstOrDefault((PlacesOfPower_GenerationData.PlacesOfPowerConfig x) => x.BoardSize.Id == boardPreset.Id).PlacesOfPower;
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x0004FACE File Offset: 0x0004DCCE
		public bool TryGetNumPops(int boardRows, int boardCols, out GenerationRange minMaxPops)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x0004FAD5 File Offset: 0x0004DCD5
		public bool TryGetClosestConfig(GenerationRange boardSize, out PlacesOfPower_GenerationData.PlacesOfPowerConfig config)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000A71 RID: 2673
		[JsonProperty]
		public PlacesOfPower_GenerationData.PlacesOfPowerConfig[] NumPoPs;

		// Token: 0x020009A1 RID: 2465
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public struct PlacesOfPowerConfig
		{
			// Token: 0x04001698 RID: 5784
			[JsonProperty]
			public ConfigRef<BoardSize_GenerationData> BoardSize;

			// Token: 0x04001699 RID: 5785
			[JsonProperty]
			public GenerationRange PlacesOfPower;
		}
	}
}
