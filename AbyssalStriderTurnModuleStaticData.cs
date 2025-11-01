using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000406 RID: 1030
	public class AbyssalStriderTurnModuleStaticData : NeutralForceTurnModuleStaticData
	{
		// Token: 0x04000915 RID: 2325
		[JsonProperty]
		public float SpawnsPerNeutralCanton;

		// Token: 0x04000916 RID: 2326
		[JsonProperty]
		public int BaseSpawnPeriod;
	}
}
