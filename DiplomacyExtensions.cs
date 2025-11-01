using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x02000502 RID: 1282
	public static class DiplomacyExtensions
	{
		// Token: 0x06001846 RID: 6214 RVA: 0x00056F6C File Offset: 0x0005516C
		public static void SetupDiplomacyPairs(this TurnState turn)
		{
			for (int i = 0; i < turn.PlayerStates.Count; i++)
			{
				for (int j = i + 1; j < turn.PlayerStates.Count; j++)
				{
					PlayerPair playerPair = new PlayerPair(i, j);
					turn.GetDiplomaticStatus(playerPair);
				}
			}
		}

		// Token: 0x06001847 RID: 6215 RVA: 0x00056FB8 File Offset: 0x000551B8
		public static bool StrongholdCaptureAuthorizedBetween(this TurnState turn, int playerId, int otherId)
		{
			if (playerId == otherId)
			{
				return false;
			}
			if (playerId == -1)
			{
				return false;
			}
			if (otherId == -1)
			{
				return false;
			}
			DiplomaticPairStatus diplomaticStatus = turn.CurrentDiplomaticTurn.GetDiplomaticStatus(playerId, otherId);
			DiplomaticState diplomaticState = (diplomaticStatus != null) ? diplomaticStatus.DiplomaticState : null;
			return diplomaticState != null && diplomaticState.AllowStrongholdCapture(turn.CurrentDiplomaticTurn, playerId, otherId);
		}

		// Token: 0x06001848 RID: 6216 RVA: 0x00056FF8 File Offset: 0x000551F8
		public static bool StrongholdCaptureAuthorizedWithAny(this TurnState turn, int playerId)
		{
			foreach (PlayerState playerState in turn.EnumeratePlayerStates(false, false))
			{
				if (playerState.Id != playerId && turn.StrongholdCaptureAuthorizedBetween(playerId, playerState.Id))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001849 RID: 6217 RVA: 0x00057060 File Offset: 0x00055260
		public static bool CombatAuthorizedBetween(this TurnState turn, int playerId, int otherId)
		{
			if (playerId == otherId)
			{
				return false;
			}
			if (playerId == -1)
			{
				return true;
			}
			if (otherId == -1)
			{
				return true;
			}
			DiplomaticPairStatus diplomaticStatus = turn.CurrentDiplomaticTurn.GetDiplomaticStatus(playerId, otherId);
			DiplomaticState diplomaticState = (diplomaticStatus != null) ? diplomaticStatus.DiplomaticState : null;
			return diplomaticState != null && diplomaticState.AllowCombat(turn.CurrentDiplomaticTurn, playerId, otherId);
		}

		// Token: 0x0600184A RID: 6218 RVA: 0x000570AC File Offset: 0x000552AC
		public static bool IsDuelInProgress(this TurnState turn, int playerId, int otherId)
		{
			if (playerId == otherId)
			{
				return false;
			}
			if (playerId == -1)
			{
				return false;
			}
			if (otherId == -1)
			{
				return false;
			}
			DiplomaticPairStatus diplomaticStatus = turn.CurrentDiplomaticTurn.GetDiplomaticStatus(playerId, otherId);
			DiplomaticState diplomaticState = (diplomaticStatus != null) ? diplomaticStatus.DiplomaticState : null;
			return diplomaticState != null && diplomaticState.AllowDuelling(turn.CurrentDiplomaticTurn, playerId, otherId);
		}

		// Token: 0x0600184B RID: 6219 RVA: 0x000570F8 File Offset: 0x000552F8
		public static bool CheckDiplomaticStates(this TurnState turn, int playerA, int playerB, BitMask validDiplomaticStates)
		{
			DiplomaticState diplomaticState = turn.CurrentDiplomaticTurn.GetDiplomaticStatus(playerA, playerB).DiplomaticState;
			return validDiplomaticStates.IsSet((int)diplomaticState.Type);
		}

		// Token: 0x0600184C RID: 6220 RVA: 0x00057128 File Offset: 0x00055328
		public static bool CheckDiplomaticStatusWithAny(this TurnState turn, int instigatorId, Func<DiplomaticPairStatus, bool> predicate)
		{
			foreach (PlayerState playerState in turn.EnumeratePlayerStates(false, false))
			{
				if (instigatorId != playerState.Id)
				{
					DiplomaticPairStatus diplomaticStatus = turn.CurrentDiplomaticTurn.GetDiplomaticStatus(instigatorId, playerState.Id);
					if (predicate(diplomaticStatus))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600184D RID: 6221 RVA: 0x0005719C File Offset: 0x0005539C
		public static bool CheckAnyVendettas(this TurnState turn, bool includeLiegeVendettas = false)
		{
			foreach (PlayerState playerState in turn.EnumeratePlayerStates(false, false))
			{
				foreach (PlayerState playerState2 in turn.EnumeratePlayerStates(false, false))
				{
					if (playerState.Id != playerState2.Id && turn.CheckVendetta(playerState.Id, playerState2.Id, includeLiegeVendettas))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600184E RID: 6222 RVA: 0x00057248 File Offset: 0x00055448
		public static bool CheckVendetta(this TurnState turn, int playerA, int playerB, bool includeLiegeVendettas = false)
		{
			if (turn.CurrentDiplomaticTurn.GetDiplomaticStatus(playerA, playerB).DiplomaticState is VendettaState)
			{
				return true;
			}
			if (!includeLiegeVendettas)
			{
				return false;
			}
			int playerA2;
			bool flag = turn.CurrentDiplomaticTurn.IsVassalOfAny(playerA, out playerA2);
			int playerB2;
			bool flag2 = turn.CurrentDiplomaticTurn.IsVassalOfAny(playerB, out playerB2);
			bool result;
			if (flag)
			{
				if (!flag2)
				{
					result = turn.CheckVendetta(playerA2, playerB, false);
				}
				else
				{
					result = turn.CheckVendetta(playerA2, playerB2, false);
				}
			}
			else
			{
				result = (flag2 && turn.CheckVendetta(playerA, playerB2, false));
			}
			return result;
		}

		// Token: 0x0600184F RID: 6223 RVA: 0x000572C8 File Offset: 0x000554C8
		public static bool CheckVendettaWithAnyPlayer(this TurnState turn, int player, bool includeLiegeVendettas = false)
		{
			foreach (PlayerState playerState in turn.EnumeratePlayerStates(false, false))
			{
				if (playerState.Id != player && turn.CheckVendetta(player, playerState.Id, includeLiegeVendettas))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001850 RID: 6224 RVA: 0x00057330 File Offset: 0x00055530
		public static bool CheckInstigatedVendettaWithPlayer(this TurnState turn, PlayerState attacker, PlayerState defender)
		{
			VendettaState vendettaState;
			return turn.CheckInstigatedVendettaWithPlayer(attacker, defender, out vendettaState, false);
		}

		// Token: 0x06001851 RID: 6225 RVA: 0x00057348 File Offset: 0x00055548
		public static bool CheckInstigatedVendettaWithPlayer(this TurnState turn, PlayerState attacker, PlayerState defender, bool includeVendettas)
		{
			VendettaState vendettaState;
			return turn.CheckInstigatedVendettaWithPlayer(attacker, defender, out vendettaState, includeVendettas);
		}

		// Token: 0x06001852 RID: 6226 RVA: 0x00057360 File Offset: 0x00055560
		public static bool CheckInstigatedVendettaWithPlayer(this TurnState turn, PlayerState attacker, PlayerState defender, out VendettaState result, bool includeLiegeVendettas = false)
		{
			result = null;
			DiplomaticPairStatus diplomaticStatus = turn.CurrentDiplomaticTurn.GetDiplomaticStatus(attacker, defender);
			VendettaState vendettaState = diplomaticStatus.DiplomaticState as VendettaState;
			if (vendettaState != null && diplomaticStatus.ActorId == attacker.Id)
			{
				result = vendettaState;
				return true;
			}
			if (!includeLiegeVendettas)
			{
				return false;
			}
			int playerId;
			bool flag = turn.CurrentDiplomaticTurn.IsVassalOfAny(attacker.Id, out playerId);
			int playerId2;
			bool flag2 = turn.CurrentDiplomaticTurn.IsVassalOfAny(defender.Id, out playerId2);
			bool result2;
			if (flag)
			{
				if (!flag2)
				{
					result2 = turn.CheckInstigatedVendettaWithPlayer(turn.FindPlayerState(playerId, null), defender, out result, false);
				}
				else
				{
					result2 = turn.CheckInstigatedVendettaWithPlayer(turn.FindPlayerState(playerId, null), turn.FindPlayerState(playerId2, null), out result, false);
				}
			}
			else
			{
				result2 = (flag2 && turn.CheckInstigatedVendettaWithPlayer(attacker, turn.FindPlayerState(playerId2, null), out result, false));
			}
			return result2;
		}

		// Token: 0x06001853 RID: 6227 RVA: 0x00057428 File Offset: 0x00055628
		public static bool CheckInstigatedVendettaWithAnyPlayer(this TurnState turn, PlayerState player, bool includeLiegeVendettas = false)
		{
			int playerId;
			includeLiegeVendettas &= turn.CurrentDiplomaticTurn.IsVassalOfAny(player.Id, out playerId);
			foreach (PlayerState playerState in turn.EnumeratePlayerStates(false, false))
			{
				if (playerState.Id != player.Id)
				{
					if (turn.CheckInstigatedVendettaWithPlayer(player, playerState))
					{
						return true;
					}
					if (includeLiegeVendettas)
					{
						return turn.CheckInstigatedVendettaWithPlayer(turn.FindPlayerState(playerId, null), playerState);
					}
				}
			}
			return false;
		}

		// Token: 0x06001854 RID: 6228 RVA: 0x000574BC File Offset: 0x000556BC
		public static bool IsPlayerDisgraced(this TurnState turn, PlayerState player)
		{
			List<Hex> list = IEnumerableExtensions.ToList<Hex>(turn.HexBoard.GetHexesControlledByPlayer(player.Id));
			if (list.Count > 1)
			{
				return false;
			}
			if (list.Count <= 0)
			{
				return false;
			}
			GamePiece gamePiece = turn.FetchGameItem<GamePiece>(player.StrongholdId);
			return list[0].HexCoord == gamePiece.Location;
		}

		// Token: 0x06001855 RID: 6229 RVA: 0x0005751A File Offset: 0x0005571A
		public static bool IsPlayerDisgraced(this TurnState turn, int playerId)
		{
			return turn.IsPlayerDisgraced(turn.FindPlayerState(playerId, null));
		}

		// Token: 0x06001856 RID: 6230 RVA: 0x0005752C File Offset: 0x0005572C
		public static bool HasEnoughVendettasForBloodFeud(this TurnContext context, PlayerState declarer, int targetId)
		{
			int num;
			int num2;
			return context.HasEnoughVendettasForBloodFeud(declarer, targetId, out num, out num2);
		}

		// Token: 0x06001857 RID: 6231 RVA: 0x00057548 File Offset: 0x00055748
		public static bool HasEnoughVendettasForBloodFeud(this TurnContext context, PlayerState declarer, int targetId, out int currentWins, out int requiredWins)
		{
			DiplomaticPairStatus diplomaticStatus = context.CurrentTurn.GetDiplomaticStatus(declarer.Id, targetId);
			currentWins = diplomaticStatus.GetVendettaWinCount(declarer.Id);
			requiredWins = context.Rules.GetRequiredVendettasForBloodFeud(declarer);
			return currentWins >= requiredWins;
		}

		// Token: 0x06001858 RID: 6232 RVA: 0x00057590 File Offset: 0x00055790
		public static bool IsBlockedByDiplomaticImmunity(this OrderTypes orderType)
		{
			switch (orderType)
			{
			case OrderTypes.Insult:
				return true;
			case OrderTypes.Demand:
				return true;
			case OrderTypes.Humiliate:
				return true;
			case OrderTypes.Extort:
				return true;
			}
			return false;
		}
	}
}
