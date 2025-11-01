using System;
using System.Collections.Generic;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000295 RID: 661
	[Serializable]
	public class LoadoutSettings
	{
		// Token: 0x040005C0 RID: 1472
		public Dictionary<string, RelicSetStaticData> RelicSettings = new Dictionary<string, RelicSetStaticData>();
	}
}
