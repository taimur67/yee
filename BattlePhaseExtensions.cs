using System;

namespace LoG
{
	// Token: 0x020001C9 RID: 457
	public static class BattlePhaseExtensions
	{
		// Token: 0x06000894 RID: 2196 RVA: 0x000297EA File Offset: 0x000279EA
		public static CombatStatType GetCombatStat(this BattlePhase phase)
		{
			if (phase == BattlePhase.Ranged)
			{
				return CombatStatType.Ranged;
			}
			if (phase == BattlePhase.Melee)
			{
				return CombatStatType.Melee;
			}
			if (phase == BattlePhase.Infernal)
			{
				return CombatStatType.Infernal;
			}
			return CombatStatType.Ranged;
		}
	}
}
