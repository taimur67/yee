using System;

namespace LoG
{
	// Token: 0x020002BF RID: 703
	public static class DamageExtensions
	{
		// Token: 0x06000D77 RID: 3447 RVA: 0x00035260 File Offset: 0x00033460
		public static DamageType GetDamageType(this BattlePhase phase)
		{
			DamageType result;
			switch (phase)
			{
			case BattlePhase.Ranged:
				result = DamageType.Ranged;
				break;
			case BattlePhase.Melee:
				result = DamageType.Melee;
				break;
			case BattlePhase.Infernal:
				result = DamageType.Infernal;
				break;
			default:
				throw new ArgumentOutOfRangeException("phase", phase, null);
			}
			return result;
		}
	}
}
