using System;
using Core.StaticData;

namespace LoG
{
	// Token: 0x020003BD RID: 957
	public static class PraetorDuelTransactions
	{
		// Token: 0x060012C1 RID: 4801 RVA: 0x00047D71 File Offset: 0x00045F71
		public static bool TryGetItemAndData<GameItemT, ItemDataT>(this TurnContext context, Identifier item, out GameItemT instance, out ItemDataT data) where GameItemT : GameItem where ItemDataT : IdentifiableStaticData
		{
			data = default(ItemDataT);
			return context.CurrentTurn.TryFetchGameItem<GameItemT>(item, out instance) && context.Database.TryFetch<ItemDataT>(instance.StaticDataReference, out data);
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x00047DA8 File Offset: 0x00045FA8
		public static Result AddBribe(TurnProcessContext context, PraetorDuelParticipantData participant, Payment payment)
		{
			Problem problem = context.CurrentTurn.FindPlayerState(participant.PlayerId, null).AcceptPayment(payment) as Problem;
			if (problem != null)
			{
				return problem;
			}
			participant.Bribe = payment;
			return Result.Success;
		}

		// Token: 0x060012C3 RID: 4803 RVA: 0x00047DE4 File Offset: 0x00045FE4
		public static bool DuelIsStillValid(TurnProcessContext context, PraetorDuelData duel)
		{
			PlayerState playerState;
			PlayerState playerState2;
			return context.GetPlayers(duel, out playerState, out playerState2) && !playerState.Eliminated && !playerState2.Eliminated;
		}

		// Token: 0x060012C4 RID: 4804 RVA: 0x00047E14 File Offset: 0x00046014
		public static PraetorCombatMovePowerChangedEvent AdjustCombatMovePower(this TurnProcessContext context, PlayerState owner, Praetor praetor, PraetorCombatMoveInstance combatMove, int power)
		{
			int power2 = combatMove.Power;
			combatMove.Power += power;
			return new PraetorCombatMovePowerChangedEvent(owner.Id, praetor, combatMove.CombatMoveReference, power2, combatMove.Power);
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x00047E58 File Offset: 0x00046058
		public static PraetorCombatMoveTypeChangedEvent ChangeCombatMove(this TurnProcessContext context, PlayerState owner, Praetor praetor, PraetorCombatMoveInstance moveInstance, ConfigRef<PraetorCombatMoveStaticData> newMove)
		{
			PraetorCombatMoveInstance previousValue = moveInstance.DeepClone<PraetorCombatMoveInstance>();
			moveInstance.CombatMoveReference = newMove;
			return new PraetorCombatMoveTypeChangedEvent(owner.Id, praetor, previousValue, moveInstance.DeepClone<PraetorCombatMoveInstance>());
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x00047E8C File Offset: 0x0004608C
		public static PraetorDuelTransactions.TechniqueReplacementMode GetTechniqueReplacementMode(TurnContext context, PlayerState player, Praetor praetor)
		{
			if (player.CanReplaceAnyCombatCard)
			{
				return PraetorDuelTransactions.TechniqueReplacementMode.Any;
			}
			return PraetorDuelTransactions.TechniqueReplacementMode.SameType;
		}

		// Token: 0x0200094A RID: 2378
		public enum TechniqueReplacementMode
		{
			// Token: 0x0400159C RID: 5532
			None,
			// Token: 0x0400159D RID: 5533
			SameType,
			// Token: 0x0400159E RID: 5534
			Any
		}
	}
}
