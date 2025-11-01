using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020001BE RID: 446
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ScenarioFlags : IDeepClone<ScenarioFlags>
	{
		// Token: 0x0600084A RID: 2122 RVA: 0x000274EC File Offset: 0x000256EC
		public void DeepClone(out ScenarioFlags clone)
		{
			clone = new ScenarioFlags
			{
				Flags = this.Flags.DeepClone()
			};
		}

		// Token: 0x04000402 RID: 1026
		[JsonProperty]
		public Dictionary<string, bool> Flags = new Dictionary<string, bool>();
	}
}
