using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;
using LoG.Simulation.Extensions;
using ModestTree;

namespace LoG
{
	// Token: 0x020001C5 RID: 453
	public static class Accolades
	{
		// Token: 0x06000877 RID: 2167 RVA: 0x00027EB1 File Offset: 0x000260B1
		public static AccoladeAwardContext DetermineFinalAccolades(TurnContext context)
		{
			return Accolades.DetermineFinalAccolades(context, context.Database.FetchSingle<AccoladeEconomyData>());
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x00027EC4 File Offset: 0x000260C4
		public static AccoladeAwardContext DetermineFinalAccolades(TurnContext context, AccoladeEconomyData economy)
		{
			return Accolades.DetermineFinalAccolades(context, economy, Accolades.DetermineAllAccolades(context));
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x00027ED3 File Offset: 0x000260D3
		public static AccoladeAwardContext DetermineFinalAccolades(TurnContext context, AccoladeAwardContext fullAwards)
		{
			return Accolades.DetermineFinalAccolades(context, context.Database.FetchSingle<AccoladeEconomyData>(), fullAwards);
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x00027EE8 File Offset: 0x000260E8
		public static AccoladeAwardContext DetermineFinalAccolades(TurnContext context, AccoladeEconomyData economy, AccoladeAwardContext fullAwards)
		{
			AccoladeAwardContext accoladeAwardContext = new AccoladeAwardContext();
			if (economy == null)
			{
				Log.Warn("No accolade economy data.", Array.Empty<object>());
				return accoladeAwardContext;
			}
			foreach (KeyValuePair<PlayerState, List<AccoladeStaticData>> keyValuePair in fullAwards.Awards)
			{
				PlayerState playerState;
				List<AccoladeStaticData> source;
				keyValuePair.Deconstruct(out playerState, out source);
				PlayerState player = playerState;
				foreach (IGrouping<int, AccoladeStaticData> grouping in from t in source
				group t by t.Rank)
				{
					int key = grouping.Key;
					List<AccoladeStaticData> list = IEnumerableExtensions.ToList<AccoladeStaticData>(grouping);
					int num = Math.Min(list.Count, economy.GetNumAllowedOfRank(key));
					int num2 = 0;
					AccoladeStaticData accolade;
					while (num2 < num && list.TryPopRandom(context.Random, out accolade))
					{
						accoladeAwardContext.Award(player, accolade);
						num2++;
					}
				}
			}
			return accoladeAwardContext;
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x00028008 File Offset: 0x00026208
		public static AccoladeAwardContext DetermineAllAccolades(TurnContext context)
		{
			AccoladeAwardContext accoladeAwardContext = new AccoladeAwardContext();
			List<AccoladeStaticData> list = context.Database.Enumerate<AccoladeStaticData>().ToList<AccoladeStaticData>();
			list.SortOnValueAscending((AccoladeStaticData t) => t.Rank);
			foreach (AccoladeStaticData accoladeStaticData in list)
			{
				List<PlayerState> list2;
				if (Accolades.DetermineAccolades(context, accoladeStaticData, out list2))
				{
					foreach (PlayerState player in list2)
					{
						accoladeAwardContext.Award(player, accoladeStaticData);
					}
				}
			}
			return accoladeAwardContext;
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x000280D8 File Offset: 0x000262D8
		public static bool DetermineAccolades(TurnContext context, AccoladeStaticData data, out List<PlayerState> winners)
		{
			winners = null;
			List<PlayerState> list = IEnumerableExtensions.ToList<PlayerState>(Accolades.DetermineEligiblePlayers(context, data));
			if (list.Count > data.AwardLimit)
			{
				List<PlayerState> list2;
				if (!Accolades.AttemptTieBreak(context, data, list, out list2))
				{
					return false;
				}
				list = list2;
			}
			winners = IEnumerableExtensions.ToList<PlayerState>(list.Take(data.AwardLimit));
			return winners.Count > 0;
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x00028130 File Offset: 0x00026330
		public static bool AttemptTieBreak(TurnContext context, AccoladeStaticData data, List<PlayerState> players, out List<PlayerState> awards)
		{
			awards = new List<PlayerState>();
			if (data.TieBreakBehaviour == TieBreakBehaviour.DisqualifyEvent)
			{
				return false;
			}
			if (data.TieBreakBehaviour == TieBreakBehaviour.AcceptAll)
			{
				awards = players;
				return true;
			}
			if (data.TieBreakBehaviour == TieBreakBehaviour.LeastImpressive || data.TieBreakBehaviour == TieBreakBehaviour.MostImpressive)
			{
				players.Sort(new Comparison<PlayerState>(Accolades.SortPlayersByImpressiveness));
				if (data.TieBreakBehaviour == TieBreakBehaviour.LeastImpressive)
				{
					players.Reverse();
				}
				awards = players.Take(data.AwardLimit).ToList<PlayerState>();
				return true;
			}
			int num = Math.Min(data.AwardLimit, players.Count);
			int num2 = 0;
			PlayerState item;
			while (num2 < num && players.TryPopRandom(context.Random, out item))
			{
				awards.Add(item);
				num2++;
			}
			return true;
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x000281DC File Offset: 0x000263DC
		private static int SortPlayersByImpressiveness(PlayerState rhs, PlayerState lhs)
		{
			if (lhs.Eliminated && !rhs.Eliminated)
			{
				return -1;
			}
			if (!lhs.Eliminated && rhs.Eliminated)
			{
				return 1;
			}
			int result = 0;
			if (lhs.Rank.TryCompareTo(rhs.Rank, out result))
			{
				return result;
			}
			if (lhs.SpendablePrestige.TryCompareTo(rhs.SpendablePrestige, out result))
			{
				return result;
			}
			return 0;
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x0002824D File Offset: 0x0002644D
		public static IEnumerable<PlayerState> DetermineEligiblePlayers(TurnContext context, AccoladeStaticData data)
		{
			return Accolades.DetermineEligiblePlayers(context, context.CurrentTurn.EnumeratePlayerStates(false, true), data);
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x00028264 File Offset: 0x00026464
		public static IEnumerable<PlayerState> DetermineEligiblePlayers(TurnContext context, IEnumerable<PlayerState> eligiblePool, AccoladeStaticData data)
		{
			List<PlayerState> list = new List<PlayerState>();
			int num = int.MinValue;
			foreach (PlayerState playerState in eligiblePool)
			{
				int statValue = playerState.GameStatistics.GetStatValue(data.Id);
				if (statValue != 0)
				{
					if (data.Eval.IsBetter(statValue, num))
					{
						num = statValue;
						list.Clear();
					}
					if (statValue == num)
					{
						list.Add(playerState);
					}
				}
			}
			return list;
		}
	}
}
