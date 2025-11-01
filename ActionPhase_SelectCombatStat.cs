using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x020006AD RID: 1709
	public class ActionPhase_SelectCombatStat : ActionPhase_Target<List<CombatStatType>>
	{
		// Token: 0x06001F5C RID: 8028 RVA: 0x0006C395 File Offset: 0x0006A595
		public ActionPhase_SelectCombatStat(Action<List<CombatStatType>> setTarget, ActionPhase_Target<List<CombatStatType>>.IsValidFunc validateTarget, int targetCount = 1) : base(setTarget, validateTarget, targetCount)
		{
		}
	}
}
