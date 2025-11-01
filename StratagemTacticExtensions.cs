using System;
using System.Collections.Generic;
using Core.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000475 RID: 1141
	public static class StratagemTacticExtensions
	{
		// Token: 0x0600153D RID: 5437 RVA: 0x0005028A File Offset: 0x0004E48A
		private static IEnumerable<GamePieceStat> EnumerateKeys()
		{
			yield return GamePieceStat.Ranged;
			yield return GamePieceStat.Melee;
			yield return GamePieceStat.Infernal;
			yield return GamePieceStat.RangedResist;
			yield return GamePieceStat.MeleeResist;
			yield return GamePieceStat.InfernalResist;
			yield return GamePieceStat.MaxHealth;
			yield return GamePieceStat.Movement;
			yield return GamePieceStat.Prestige;
			yield break;
		}

		// Token: 0x0600153E RID: 5438 RVA: 0x00050294 File Offset: 0x0004E494
		public static int GetTacticPowerValue(this StratagemTacticLevelStaticData tacticLevel)
		{
			foreach (IStaticData staticData in tacticLevel.Components)
			{
				GamePieceModifierStaticData gamePieceModifierStaticData = staticData as GamePieceModifierStaticData;
				if (gamePieceModifierStaticData != null)
				{
					foreach (GamePieceStat key in StratagemTacticExtensions.EnumerateKeys())
					{
						StatModificationBinding<GamePieceStat> statModificationBinding;
						if (gamePieceModifierStaticData.TryGetBinding(key, ModifierTarget.ValueOffset, out statModificationBinding) && statModificationBinding.Value != 0f)
						{
							return (int)statModificationBinding.Value;
						}
					}
				}
			}
			return 0;
		}

		// Token: 0x0600153F RID: 5439 RVA: 0x00050348 File Offset: 0x0004E548
		public static int GetTacticMinRoll(this StratagemTacticLevelStaticData tacticLevel)
		{
			foreach (IStaticData staticData in tacticLevel.Components)
			{
				DecreaseAttributeTacticStaticData decreaseAttributeTacticStaticData = staticData as DecreaseAttributeTacticStaticData;
				if (decreaseAttributeTacticStaticData != null)
				{
					return decreaseAttributeTacticStaticData.MinRoll;
				}
			}
			return 0;
		}

		// Token: 0x06001540 RID: 5440 RVA: 0x000503A8 File Offset: 0x0004E5A8
		public static int GetTacticMaxRoll(this StratagemTacticLevelStaticData tacticLevel)
		{
			foreach (IStaticData staticData in tacticLevel.Components)
			{
				DecreaseAttributeTacticStaticData decreaseAttributeTacticStaticData = staticData as DecreaseAttributeTacticStaticData;
				if (decreaseAttributeTacticStaticData != null)
				{
					return decreaseAttributeTacticStaticData.MaxRoll;
				}
			}
			return 0;
		}
	}
}
