using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000700 RID: 1792
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class VictoryRuleUsurp : VictoryRule
	{
		// Token: 0x06002261 RID: 8801 RVA: 0x00077D3F File Offset: 0x00075F3F
		public override VictoryRuleProcessor CreateAndInitProcessor(TurnState turnState)
		{
			VictoryRuleProcessorUsurp victoryRuleProcessorUsurp = new VictoryRuleProcessorUsurp();
			victoryRuleProcessorUsurp.Init(this, turnState);
			return victoryRuleProcessorUsurp;
		}

		// Token: 0x06002262 RID: 8802 RVA: 0x00077D4E File Offset: 0x00075F4E
		public override void DeepClone(out VictoryRule clone)
		{
			clone = new VictoryRuleUsurp();
		}
	}
}
