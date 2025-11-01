using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020006FA RID: 1786
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public abstract class VictoryRuleProcessor<TVictoryRule> : VictoryRuleProcessor where TVictoryRule : VictoryRule
	{
		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06002235 RID: 8757 RVA: 0x000770F2 File Offset: 0x000752F2
		public override Type AssociatedVictoryRuleType
		{
			get
			{
				return typeof(TVictoryRule);
			}
		}

		// Token: 0x06002236 RID: 8758 RVA: 0x000770FE File Offset: 0x000752FE
		public virtual void Init(TVictoryRule victoryRule, TurnState turnState)
		{
		}

		// Token: 0x06002237 RID: 8759 RVA: 0x00077100 File Offset: 0x00075300
		protected virtual int GetPowerBehindTheThrone(TurnState turn, int winnerId)
		{
			foreach (PlayerState playerState in turn.EnumeratePlayerStates(false, false))
			{
				if (playerState.IsPowerBehindTheThrone)
				{
					BloodVassalageState bloodVassalageState = turn.GetDiplomaticStatus(winnerId, playerState.Id).DiplomaticState as BloodVassalageState;
					if (bloodVassalageState != null && bloodVassalageState.BloodLordId == winnerId)
					{
						return bloodVassalageState.VassalId;
					}
				}
			}
			return int.MinValue;
		}

		// Token: 0x06002238 RID: 8760 RVA: 0x0007718C File Offset: 0x0007538C
		protected virtual List<PlayerState> GetKingmakers(TurnState turn, int winnerId)
		{
			IEnumerable<PlayerState> enumerable = turn.EnumeratePlayerStates(false, false);
			List<PlayerState> list = new List<PlayerState>();
			foreach (PlayerState playerState in enumerable)
			{
				if (playerState.IsKingmaker && playerState.KingmakerPuppetId == winnerId)
				{
					list.Add(playerState);
				}
			}
			return list;
		}

		// Token: 0x06002239 RID: 8761 RVA: 0x000771F8 File Offset: 0x000753F8
		public int CompareTrialWorthiness(TurnState turn, PlayerState lhs, PlayerState rhs)
		{
			int conclaveFavouriteId = turn.ConclaveFavouriteId;
			int result = 0;
			if (!VictoryRuleProcessor<TVictoryRule>.CompareInEqual<bool, bool>(!lhs.Eliminated, !rhs.Eliminated, ref result) && !VictoryRuleProcessor<TVictoryRule>.CompareInEqual<bool, bool>(!lhs.Excommunicated, !rhs.Excommunicated, ref result) && !VictoryRuleProcessor<TVictoryRule>.CompareInEqual<bool, bool>(lhs.Id == conclaveFavouriteId, rhs.Id == conclaveFavouriteId, ref result) && !VictoryRuleProcessor<TVictoryRule>.CompareInEqual<int, int>(lhs.SpendablePrestige, rhs.SpendablePrestige, ref result))
			{
				VictoryRuleProcessor<TVictoryRule>.CompareInEqual<int, int>((int)lhs.Rank, (int)rhs.Rank, ref result);
			}
			return result;
		}

		// Token: 0x0600223A RID: 8762 RVA: 0x0007728C File Offset: 0x0007548C
		public void SortByWorthiness(TurnState turn, List<PlayerState> players)
		{
			players.Sort((PlayerState lhs, PlayerState rhs) => this.CompareTrialWorthiness(turn, lhs, rhs));
		}

		// Token: 0x0600223B RID: 8763 RVA: 0x000772BF File Offset: 0x000754BF
		public static bool CompareInEqual<T, Q>(T lhs, Q rhs, ref int comp) where T : IComparable<Q>
		{
			comp = lhs.CompareTo(rhs);
			return comp != 0;
		}
	}
}
