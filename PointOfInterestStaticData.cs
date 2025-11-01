using System;
using System.Collections.Generic;
using Core.StaticData.Attributes;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000457 RID: 1111
	[StaticDataValueType]
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PointOfInterestStaticData : TerrainStaticData
	{
		// Token: 0x04000A7A RID: 2682
		[JsonProperty]
		public List<MultiHexFeatureData> HexFeatures = new List<MultiHexFeatureData>();
	}
}
