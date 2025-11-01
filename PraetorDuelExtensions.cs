using System;
using Core.StaticData;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x020003B6 RID: 950
	public static class PraetorDuelExtensions
	{
		// Token: 0x06001296 RID: 4758 RVA: 0x00046D0B File Offset: 0x00044F0B
		public static bool GetPlayers(this TurnProcessContext context, PraetorDuelData duel, out PlayerState challenger, out PlayerState defender)
		{
			challenger = context.CurrentTurn.FindPlayerState(duel.Challenger.PlayerId, null);
			defender = context.CurrentTurn.FindPlayerState(duel.Defender.PlayerId, null);
			return challenger != null && defender != null;
		}

		// Token: 0x06001297 RID: 4759 RVA: 0x00046D4A File Offset: 0x00044F4A
		public static void StartDuel(this TurnProcessContext context, int challenger, int defender, Identifier challengerPraetor)
		{
			context.CurrentTurn.GetDiplomaticStatus(challenger, defender).SetPraetorDuel(context, new PraetorDuelData(challenger, defender, challengerPraetor));
		}

		// Token: 0x06001298 RID: 4760 RVA: 0x00046D68 File Offset: 0x00044F68
		public static bool TryGetPraetorDuel(this TurnState turn, PlayerPair contestants, out PraetorDuelData duel)
		{
			return turn.CurrentDiplomaticTurn.TryGetPraetorDuel(contestants, out duel);
		}

		// Token: 0x06001299 RID: 4761 RVA: 0x00046D77 File Offset: 0x00044F77
		public static bool TryGetPraetorDuel(this TurnProcessContext context, PlayerPair contestants, out PraetorDuelData duel)
		{
			return context.Diplomacy.TryGetPraetorDuel(contestants, out duel);
		}

		// Token: 0x0600129A RID: 4762 RVA: 0x00046D88 File Offset: 0x00044F88
		private static bool TryGetPraetorDuel(this DiplomaticTurnState diplomaticTurnState, PlayerPair contestants, out PraetorDuelData duel)
		{
			duel = null;
			PraetorDuelState praetorDuelState = diplomaticTurnState.GetDiplomaticStatus(contestants).DiplomaticState as PraetorDuelState;
			if (praetorDuelState != null)
			{
				duel = praetorDuelState.DuelData;
			}
			return duel != null;
		}

		// Token: 0x0600129B RID: 4763 RVA: 0x00046DBA File Offset: 0x00044FBA
		public static bool TryGetPraetorCombatMove(this TurnContext context, Praetor praetor, ConfigRef moveReference, out PraetorCombatMoveInstance instance, out PraetorCombatMoveStaticData data)
		{
			return true & context.Database.TryFetch<PraetorCombatMoveStaticData>(moveReference, out data) & praetor.TryGetTechniqueInstance(moveReference, out instance);
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x00046DD8 File Offset: 0x00044FD8
		public static int GetXPRequiredForNextLevel(this GameDatabase database, Praetor praetor)
		{
			PraetorLevelEconomy praetorLevelEconomy;
			if (database.TryFetchSingle(out praetorLevelEconomy))
			{
				return praetorLevelEconomy.GetNextLevelOrLast(praetor.Level).XPRequired;
			}
			return 1;
		}

		// Token: 0x0600129D RID: 4765 RVA: 0x00046E04 File Offset: 0x00045004
		public static GameEvent ApplyDuelExperience(this TurnProcessContext context, int owningPlayer, Praetor praetor, int experience)
		{
			ExperienceGainedEvent experienceGainedEvent = new ExperienceGainedEvent(owningPlayer, praetor, praetor.Experience, experience);
			praetor.Experience += experience;
			PraetorLevelEconomy praetorLevelEconomy;
			if (context.Database.TryFetchSingle(out praetorLevelEconomy))
			{
				PraetorLevelEconomy.LevelPair nextLevelOrLast = praetorLevelEconomy.GetNextLevelOrLast(praetor.Level);
				while (nextLevelOrLast.XPRequired > 0 && praetor.Experience >= nextLevelOrLast.XPRequired)
				{
					experienceGainedEvent.AddChildEvent(context.ApplyLevel(owningPlayer, praetor, praetor.Level + 1));
					praetor.Experience -= nextLevelOrLast.XPRequired;
					nextLevelOrLast = praetorLevelEconomy.GetNextLevelOrLast(praetor.Level);
				}
			}
			return experienceGainedEvent;
		}

		// Token: 0x0600129E RID: 4766 RVA: 0x00046EA0 File Offset: 0x000450A0
		public static GameEvent ApplyLevel(this TurnProcessContext context, int owningPlayer, GameItem item, int level)
		{
			GameEvent result = new LevelChangedEvent(owningPlayer, item, item.Level, level);
			item.SetLevel(level);
			return result;
		}
	}
}
