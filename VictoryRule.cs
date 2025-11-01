using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020006F7 RID: 1783
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public abstract class VictoryRule : IDeepClone<VictoryRule>
	{
		// Token: 0x06002228 RID: 8744
		public abstract VictoryRuleProcessor CreateAndInitProcessor(TurnState turnState);

		// Token: 0x06002229 RID: 8745
		public abstract void DeepClone(out VictoryRule clone);
	}
}
