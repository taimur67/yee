using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020006F8 RID: 1784
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class VictoryRuleAbyss : VictoryRule
	{
		// Token: 0x0600222B RID: 8747 RVA: 0x000770BD File Offset: 0x000752BD
		public override VictoryRuleProcessor CreateAndInitProcessor(TurnState turnState)
		{
			VictoryRuleProcessorAbyss victoryRuleProcessorAbyss = new VictoryRuleProcessorAbyss();
			victoryRuleProcessorAbyss.Init(this, turnState);
			return victoryRuleProcessorAbyss;
		}

		// Token: 0x0600222C RID: 8748 RVA: 0x000770CC File Offset: 0x000752CC
		public override void DeepClone(out VictoryRule clone)
		{
			clone = new VictoryRuleAbyss();
		}
	}
}
