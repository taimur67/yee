using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x0200035E RID: 862
	public static class CombatStatTypeExtensions
	{
		// Token: 0x06001066 RID: 4198 RVA: 0x000407B8 File Offset: 0x0003E9B8
		public static GamePieceStat ToGamePieceStat(this CombatStatType stat)
		{
			GamePieceStat result;
			switch (stat)
			{
			case CombatStatType.Ranged:
				result = GamePieceStat.Ranged;
				break;
			case CombatStatType.Melee:
				result = GamePieceStat.Melee;
				break;
			case CombatStatType.Infernal:
				result = GamePieceStat.Infernal;
				break;
			case CombatStatType.RangedResist:
				result = GamePieceStat.RangedResist;
				break;
			case CombatStatType.MeleeResist:
				result = GamePieceStat.MeleeResist;
				break;
			case CombatStatType.InfernalResist:
				result = GamePieceStat.InfernalResist;
				break;
			default:
				result = GamePieceStat.None;
				break;
			}
			return result;
		}
	}
}
