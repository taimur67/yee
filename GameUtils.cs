using System;
using System.Collections.Generic;
using System.Linq;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000361 RID: 865
	public static class GameUtils
	{
		// Token: 0x0600106D RID: 4205 RVA: 0x00040875 File Offset: 0x0003EA75
		public static bool LevelRoll(TurnState turn, GamePiece lhs, GamePiece rhs)
		{
			return turn.Random.NextDouble() > 1.0 / (double)rhs.Level;
		}

		// Token: 0x0600106E RID: 4206 RVA: 0x00040898 File Offset: 0x0003EA98
		public static ModifiableValue GetStat(this GamePiece piece, CombatStatType stat)
		{
			ModifiableValue result;
			switch (stat)
			{
			case CombatStatType.Ranged:
				result = piece.CombatStats.Ranged;
				break;
			case CombatStatType.Melee:
				result = piece.CombatStats.Melee;
				break;
			case CombatStatType.Infernal:
				result = piece.CombatStats.Infernal;
				break;
			case CombatStatType.RangedResist:
				result = piece.CombatStats.RangedResist;
				break;
			case CombatStatType.MeleeResist:
				result = piece.CombatStats.MeleeResist;
				break;
			case CombatStatType.InfernalResist:
				result = piece.CombatStats.InfernalResist;
				break;
			default:
				result = null;
				break;
			}
			return result;
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x0004091C File Offset: 0x0003EB1C
		public static int GetTotalStat(this GamePiece piece, CombatStatType stat)
		{
			int result;
			switch (stat)
			{
			case CombatStatType.Ranged:
				result = piece.TotalRanged();
				break;
			case CombatStatType.Melee:
				result = piece.TotalMelee();
				break;
			case CombatStatType.Infernal:
				result = piece.TotalInfernal();
				break;
			default:
				result = piece.GetStat(stat);
				break;
			}
			return result;
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x00040968 File Offset: 0x0003EB68
		public static void InitializeRegencyOrder(this TurnState turn)
		{
			if (turn == null)
			{
				return;
			}
			int numberOfPlayers = turn.GetNumberOfPlayers(false, true);
			if (turn.RegencyOrder.Count == numberOfPlayers)
			{
				return;
			}
			int num = turn.PlayerStates.FindIndex((PlayerState x) => x.Id == turn.RegentPlayerId);
			List<int> list = IEnumerableExtensions.ToList<int>((from player in turn.PlayerStates
			select player.Id).Shuffle(turn.Random));
			turn.RegencyOrder = new List<int>(numberOfPlayers);
			for (int i = 0; i < numberOfPlayers; i++)
			{
				int index = (num + i) % numberOfPlayers;
				turn.RegencyOrder.Add(list[index]);
			}
		}

		// Token: 0x06001071 RID: 4209 RVA: 0x00040A50 File Offset: 0x0003EC50
		public static void InitializeVictoryRuleProcessors(this TurnState turnState, GameRules gameRules)
		{
			foreach (VictoryRule victoryRule in gameRules.VictoryRules)
			{
				Type type = victoryRule.GetType();
				bool flag = false;
				using (List<VictoryRuleProcessor>.Enumerator enumerator2 = turnState.VictoryRuleProcessors.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.AssociatedVictoryRuleType == type)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					VictoryRuleProcessor item = victoryRule.CreateAndInitProcessor(turnState);
					turnState.VictoryRuleProcessors.Add(item);
				}
			}
		}

		// Token: 0x06001072 RID: 4210 RVA: 0x00040B0C File Offset: 0x0003ED0C
		public static int TurnsUntilRegency(this TurnState turn, int playerId)
		{
			int regentPlayerId = turn.RegentPlayerId;
			int count = turn.PlayerStates.Count;
			for (int i = 0; i < count; i++)
			{
				if (turn.GetPlayerIdInRegencyOrder(regentPlayerId, i) == playerId)
				{
					return i;
				}
			}
			return int.MaxValue;
		}

		// Token: 0x06001073 RID: 4211 RVA: 0x00040B4A File Offset: 0x0003ED4A
		public static int TurnsUntilRegency(this TurnState turn, PlayerState player)
		{
			return turn.TurnsUntilRegency(player.Id);
		}

		// Token: 0x06001074 RID: 4212 RVA: 0x00040B58 File Offset: 0x0003ED58
		public static ReceiveEventCardEvent GiveNewEventCard(this TurnProcessContext context, PlayerState player)
		{
			EventCard nextEventCard = context.GetNextEventCard(player);
			if (nextEventCard == null)
			{
				return null;
			}
			player.GiveEventCard(context.CurrentTurn, nextEventCard.Id);
			return new ReceiveEventCardEvent(player.Id, nextEventCard);
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x00040B96 File Offset: 0x0003ED96
		public static string GetNextEdictId(this TurnProcessContext context)
		{
			EdictStaticData nextEdict = context.GetNextEdict();
			if (nextEdict == null)
			{
				return null;
			}
			return nextEdict.Id;
		}

		// Token: 0x06001076 RID: 4214 RVA: 0x00040BAC File Offset: 0x0003EDAC
		public static EdictStaticData GetNextEdict(this TurnProcessContext context)
		{
			GameDatabase database = context.Database;
			TurnState turn = context.CurrentTurn;
			List<EdictStaticData> list = IEnumerableExtensions.ToList<EdictStaticData>(from t in database.Enumerate<EdictStaticData>()
			where !context.Rules.BlacklistedEntities.Contains(t.ConfigRef)
			select t into edict
			where turn.EdictCanBeTriggered(edict, database)
			select edict);
			if (list.Count <= 0)
			{
				return null;
			}
			return list.Random(turn.Random);
		}

		// Token: 0x06001077 RID: 4215 RVA: 0x00040C38 File Offset: 0x0003EE38
		public static bool EdictCanBeTriggered(this TurnState turn, EdictStaticData edict, GameDatabase database)
		{
			if (turn.TurnValue < edict.CannotTriggerUntilTurn)
			{
				return false;
			}
			if (turn.CompletedEdicts.Contains(edict.Id))
			{
				return false;
			}
			EdictCandidateStaticData edictCandidateStaticData = edict as EdictCandidateStaticData;
			IEnumerable<PlayerState> enumerable;
			return edictCandidateStaticData == null || database.Fetch(edictCandidateStaticData.Effect).SufficientValidCandidates(turn, out enumerable);
		}

		// Token: 0x06001078 RID: 4216 RVA: 0x00040C90 File Offset: 0x0003EE90
		public static EventCard GetNextEventCard(this TurnProcessContext context, PlayerState owner = null)
		{
			GameDatabase database = context.Database;
			TurnState currentTurn = context.CurrentTurn;
			List<EventCardStaticData> list = IEnumerableExtensions.ToList<EventCardStaticData>(context.GetAvailableEventCards());
			if (list.Count == 0)
			{
				CollectionExtensions.Remove<ConfigRef>(currentTurn.DeadItemReferences, from t in database.Enumerate<EventCardStaticData>()
				select t.ConfigRef);
				list = IEnumerableExtensions.ToList<EventCardStaticData>(context.GetAvailableEventCards());
			}
			EventCardStaticData staticData;
			if (list.TryGetRandom(currentTurn.Random, out staticData))
			{
				return currentTurn.SpawnEventCard(staticData, owner);
			}
			return null;
		}

		// Token: 0x06001079 RID: 4217 RVA: 0x00040D1C File Offset: 0x0003EF1C
		private static IEnumerable<EventCardStaticData> GetAvailableEventCards(this TurnContext context)
		{
			HashSet<ConfigRef> consumedCards = context.CurrentTurn.EnumerateConsumedConfigRefs().ToHashSet<ConfigRef>();
			CollectionExtensions.AddRange<ConfigRef>(consumedCards, context.Rules.BlacklistedEntities);
			return from x in context.Database.Enumerate<EventCardStaticData>()
			where !consumedCards.Contains(x.ConfigRef)
			select x;
		}

		// Token: 0x0600107A RID: 4218 RVA: 0x00040D77 File Offset: 0x0003EF77
		public static int CalculateRemainingEventSlotsForPlayer(this TurnState turn, PlayerState player)
		{
			return player.MaxEventCards - player.CountEventCards(turn);
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x00040D8C File Offset: 0x0003EF8C
		public static void QueueEventCardDraw(this TurnProcessContext context, int playerId, int drawCount = 1)
		{
			context.EventDrawContext.AddDraws(playerId, drawCount);
		}

		// Token: 0x0600107C RID: 4220 RVA: 0x00040D9B File Offset: 0x0003EF9B
		public static GameEvent DrawEventCard(this TurnProcessContext context, PlayerState player)
		{
			return context.DrawEventCard(player, player.EventCardDraw.Value);
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x00040DB0 File Offset: 0x0003EFB0
		public static GameEvent DrawEventCard(this TurnProcessContext context, PlayerState player, int draw)
		{
			if (player == null)
			{
				return null;
			}
			TurnState currentTurn = context.CurrentTurn;
			int num = currentTurn.CalculateRemainingEventSlotsForPlayer(player);
			if (draw <= num)
			{
				return context.GiveNewEventCard(player);
			}
			List<Identifier> list = new List<Identifier>();
			for (int i = 0; i < draw; i++)
			{
				EventCard nextEventCard = context.GetNextEventCard(player);
				if (nextEventCard != null)
				{
					list.Add(nextEventCard);
				}
			}
			if (list.Count >= 1)
			{
				List<Identifier> existingCards = IEnumerableExtensions.ToList<Identifier>(from x in player.EnumerateEventCards(currentTurn)
				select x.Id);
				SelectEventCardRequest selectEventCardRequest = new SelectEventCardRequest(currentTurn)
				{
					ExistingCards = existingCards,
					CandidateCards = list
				};
				currentTurn.AddDecisionToAskPlayer(player.Id, selectEventCardRequest);
				return new DecisionAddedEvent(player.Id, selectEventCardRequest.DecisionId);
			}
			return null;
		}
	}
}
