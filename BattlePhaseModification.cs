using System;

namespace LoG
{
	// Token: 0x020001CB RID: 459
	public readonly struct BattlePhaseModification
	{
		// Token: 0x06000895 RID: 2197 RVA: 0x000297FE File Offset: 0x000279FE
		public BattlePhaseModification(Ability ability, BattlePhase battlePhase, BattlePhaseModificationType battlePhaseModificationType, CombatAbilityContext abilityContext)
		{
			this = new BattlePhaseModification(ability, battlePhase, battlePhaseModificationType, abilityContext.Actor, abilityContext.Opponent, abilityContext.BattleRole);
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x00029820 File Offset: 0x00027A20
		public BattlePhaseModification(Ability ability, BattlePhase battlePhase, BattlePhaseModificationType battlePhaseModificationType, GamePiece actor, GamePiece opponent, BattleRole battleRole)
		{
			this = new BattlePhaseModification(ability, battlePhase, battlePhaseModificationType, actor.Id, actor.ControllingPlayerId, actor.Level, opponent.Id, battleRole);
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x00029854 File Offset: 0x00027A54
		public BattlePhaseModification(Ability ability, BattlePhase battlePhase, BattlePhaseModificationType battlePhaseModificationType, Identifier actorId, int controllingPlayerId, int gamePieceLevel, Identifier opponentId, BattleRole battleRole)
		{
			this.Ability = ability;
			this.BattlePhase = battlePhase;
			this.BattlePhaseModificationType = battlePhaseModificationType;
			this.ActorId = actorId;
			this.ControllingPlayerId = controllingPlayerId;
			this.OpponentId = opponentId;
			this.GamePieceLevel = gamePieceLevel;
			this.BattleRole = battleRole;
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x00029894 File Offset: 0x00027A94
		private static BattlePhaseModification.TypeSort TypeSortValue(BattlePhaseModificationType type)
		{
			BattlePhaseModification.TypeSort result;
			switch (type)
			{
			case BattlePhaseModificationType.Twice:
				result = BattlePhaseModification.TypeSort.Twice;
				break;
			case BattlePhaseModificationType.First:
				result = BattlePhaseModification.TypeSort.FirstLast;
				break;
			case BattlePhaseModificationType.Last:
				result = BattlePhaseModification.TypeSort.FirstLast;
				break;
			case BattlePhaseModificationType.Skip:
				result = BattlePhaseModification.TypeSort.Skip;
				break;
			default:
				result = BattlePhaseModification.TypeSort.FirstLast;
				break;
			}
			return result;
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x000298CC File Offset: 0x00027ACC
		public static int Compare(BattlePhaseModification left, BattlePhaseModification right)
		{
			BattlePhaseModification.TypeSort typeSort = BattlePhaseModification.TypeSortValue(left.BattlePhaseModificationType);
			BattlePhaseModification.TypeSort typeSort2 = BattlePhaseModification.TypeSortValue(right.BattlePhaseModificationType);
			if (typeSort != typeSort2)
			{
				return typeSort - typeSort2;
			}
			if (typeSort != BattlePhaseModification.TypeSort.FirstLast)
			{
				return 0;
			}
			int num = right.GamePieceLevel - left.GamePieceLevel;
			if (num != 0)
			{
				return num;
			}
			if (left.BattleRole != BattleRole.Defender)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x04000429 RID: 1065
		public readonly BattlePhase BattlePhase;

		// Token: 0x0400042A RID: 1066
		public readonly BattlePhaseModificationType BattlePhaseModificationType;

		// Token: 0x0400042B RID: 1067
		public readonly Identifier ActorId;

		// Token: 0x0400042C RID: 1068
		public readonly int ControllingPlayerId;

		// Token: 0x0400042D RID: 1069
		public readonly Identifier OpponentId;

		// Token: 0x0400042E RID: 1070
		public readonly int GamePieceLevel;

		// Token: 0x0400042F RID: 1071
		public readonly BattleRole BattleRole;

		// Token: 0x04000430 RID: 1072
		public readonly Ability Ability;

		// Token: 0x02000888 RID: 2184
		private enum TypeSort
		{
			// Token: 0x0400124C RID: 4684
			Skip,
			// Token: 0x0400124D RID: 4685
			Twice,
			// Token: 0x0400124E RID: 4686
			FirstLast
		}
	}
}
