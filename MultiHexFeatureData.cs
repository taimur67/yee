using System;
using System.Collections.Generic;
using Core.StaticData.Attributes;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000458 RID: 1112
	[StaticDataValueType]
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class MultiHexFeatureData
	{
		// Token: 0x04000A7B RID: 2683
		[JsonProperty]
		public List<CubeCoord> OccupiedHexes = new List<CubeCoord>();

		// Token: 0x04000A7C RID: 2684
		[JsonProperty]
		public HexUtils.HexOrientationFlags Orientations;
	}
}
