using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x020001BC RID: 444
	public static class RivalryProcessor
	{
		// Token: 0x06000842 RID: 2114 RVA: 0x00026C6C File Offset: 0x00024E6C
		public static bool IsChoking(TurnContext context, GameItem item)
		{
			using (IEnumerator<Ability> enumerator = context.GetAllAbilitiesFor(item).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.SourceId == "Choking")
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x00026CCC File Offset: 0x00024ECC
		public static bool IsStrongholdChoking(TurnContext context, PlayerState player)
		{
			GameItem item = context.CurrentTurn.FetchGameItem(player.StrongholdId);
			return RivalryProcessor.IsChoking(context, item);
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x00026CF4 File Offset: 0x00024EF4
		public static bool IsStrongholdChoking(TurnContext context, int playerId)
		{
			PlayerState playerState = context.CurrentTurn.FindPlayerState(playerId, null);
			return playerState != null && RivalryProcessor.IsStrongholdChoking(context, playerState);
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x00026D1B File Offset: 0x00024F1B
		public static bool IsSourceOfChoking(PlayerState player)
		{
			return player.ArchfiendId == "Belphegor";
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x00026D2D File Offset: 0x00024F2D
		public static bool IsSourceOfChoking(TurnContext context, int playerId)
		{
			PlayerState playerState = context.CurrentTurn.FindPlayerState(playerId, null);
			return ((playerState != null) ? playerState.ArchfiendId : null) == "Belphegor";
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x00026D54 File Offset: 0x00024F54
		public static bool CaresAboutPrestige(TurnState turn, PlayerState player)
		{
			int num;
			return !player.Excommunicated && !turn.CurrentDiplomaticTurn.IsVassalOfAny(player.Id, out num) && (!player.IsKingmaker || player.KingmakerPuppetId != turn.ConclaveFavouriteId);
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x00026DA0 File Offset: 0x00024FA0
		private static void ProcessUndermineVictor(TurnState turn, GameRules rules, PlayerState playerToUpdate, PlayerState otherPlayer)
		{
			PlayerState conclaveFavourite = turn.GetConclaveFavourite();
			int num = (conclaveFavourite != null) ? conclaveFavourite.Id : int.MinValue;
			GamePiece pandaemonium = turn.GetPandaemonium();
			int num2 = (pandaemonium != null && pandaemonium.ControllingPlayerId != -1) ? pandaemonium.ControllingPlayerId : int.MinValue;
			bool flag = num2 != int.MinValue;
			bool flag2 = !flag && RivalryProcessor.CaresAboutPrestige(turn, playerToUpdate);
			RivalryBoost.ApplyToOneSide_HigherRank(playerToUpdate, otherPlayer);
			if (flag2)
			{
				int gameDuration = rules.GameDuration;
				float gameProgress = MathF.Min(1f, (float)turn.TurnValue / (float)gameDuration);
				RivalryBoost.ApplyToOneSide_MorePrestige(playerToUpdate, otherPlayer, gameProgress);
				if (num != -2147483648)
				{
					RivalryBoost.ApplyTowardsSpecific_ConclaveFavourite(playerToUpdate, otherPlayer, num, gameProgress);
				}
			}
			if (flag)
			{
				RivalryBoost.ApplyTowardsSpecific_Usurper(playerToUpdate, otherPlayer, num2);
			}
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x00026E48 File Offset: 0x00025048
		public static void ProcessTurn(TurnContext turnContext)
		{
			GameRules rules = turnContext.Rules;
			TurnState currentTurn = turnContext.CurrentTurn;
			int gameDuration = rules.GameDuration;
			MathF.Min(1f, (float)currentTurn.TurnValue / (float)gameDuration);
			foreach (GameEvent gameEvent in currentTurn.GameEvents)
			{
				PlayerState playerState = currentTurn.FindPlayerState(gameEvent.TriggeringPlayerID, null);
				PlayerState playerState2 = currentTurn.FindPlayerState(gameEvent.AffectedPlayerID, null);
				if (playerState2 != null && playerState != null && playerState.Id != playerState2.Id)
				{
					if (gameEvent is MakeDemandEvent)
					{
						playerState2.Animosity.BoostValue(playerState.Id, 0.4f);
					}
					if (gameEvent is ExtortEvent)
					{
						playerState2.Animosity.BoostValue(playerState.Id, 0.6f);
					}
					if (gameEvent is InsultHurledEvent)
					{
						playerState2.Animosity.BoostValue(playerState.Id, 0.4f);
					}
					if (gameEvent is HumiliateEvent)
					{
						playerState2.Animosity.BoostValue(playerState.Id, 0.6f);
					}
					VileCalumnySentEvent vileCalumnySentEvent = gameEvent as VileCalumnySentEvent;
					if (vileCalumnySentEvent != null)
					{
						playerState2.Animosity.BoostValue(vileCalumnySentEvent.TriggeringPlayerID, 0.1f);
						playerState2.Animosity.BoostValue(vileCalumnySentEvent.ScapegoatId, 0.3f);
						PlayerState playerState3 = currentTurn.FindPlayerState(vileCalumnySentEvent.ScapegoatId, null);
						if (playerState3 != null)
						{
							playerState3.Animosity.BoostValue(playerState.Id, 0.3f);
						}
					}
					RequestLureOfExcessEvent requestLureOfExcessEvent = gameEvent as RequestLureOfExcessEvent;
					if (requestLureOfExcessEvent != null)
					{
						playerState2.Animosity.BoostValue(requestLureOfExcessEvent.TriggeringPlayerID, 0.3f);
					}
					if (gameEvent is LegionKilledEvent)
					{
						playerState2.Animosity.BoostValue(playerState.Id, 0.5f);
					}
					RitualCastEvent ritualCastEvent = gameEvent as RitualCastEvent;
					if (ritualCastEvent != null && playerState.Id != playerState2.Id)
					{
						switch (ritualCastEvent.MaskingContext.MaskingMode)
						{
						case RitualMaskingMode.NoMasking:
							playerState2.Animosity.BoostValue(playerState.Id, 0.3f);
							break;
						case RitualMaskingMode.Masked:
							if (!ritualCastEvent.MaskingContext.MaskingSuccessful)
							{
								playerState2.Animosity.BoostValue(playerState.Id, 0.3f);
							}
							break;
						case RitualMaskingMode.Framed:
							if (ritualCastEvent.MaskingContext.MaskingSuccessful)
							{
								playerState2.Animosity.BoostValue(ritualCastEvent.MaskingContext.FramedPlayerId, 0.3f);
							}
							else
							{
								playerState2.Animosity.BoostValue(playerState.Id, 0.3f);
							}
							break;
						}
					}
					ManuscriptEvent manuscriptEvent = gameEvent as ManuscriptEvent;
					if (manuscriptEvent != null && manuscriptEvent.Category == ManuscriptCategory.Schematic)
					{
						foreach (int num in gameEvent.AffectedPlayerIds)
						{
							if (num != -1 && num != -2147483648 && num != playerState.Id)
							{
								PlayerState playerState4 = currentTurn.FindPlayerState(num, null);
								if (playerState4 != null)
								{
									playerState4.Animosity.BoostValue(playerState.Id, 0.9f);
								}
							}
						}
					}
				}
			}
			PlayerState playerState5 = currentTurn.FindPlayerState("Belphegor");
			if (playerState5 != null)
			{
				RivalryBoost.ApplyAuraAnimosity(playerState5, turnContext);
			}
			foreach (Tuple<PlayerState, PlayerState> value in currentTurn.EnumeratePlayerStatePairs(false, true, true))
			{
				PlayerState playerState6;
				PlayerState playerState7;
				value.Deconstruct(out playerState6, out playerState7);
				PlayerState playerState8 = playerState6;
				PlayerState playerState9 = playerState7;
				if (playerState8.Id != playerState9.Id)
				{
					if (playerState8.Eliminated || playerState9.Eliminated)
					{
						RivalryBoost.ApplyToBothSides(playerState8, playerState9, -1f);
					}
					else
					{
						if (playerState8.ArchfiendId == "Belphegor")
						{
							RivalryBoost.ApplyTowardsBelphegor(playerState9, playerState8, turnContext);
						}
						else if (playerState9.ArchfiendId == "Belphegor")
						{
							RivalryBoost.ApplyTowardsBelphegor(playerState8, playerState9, turnContext);
						}
						RivalryBoost.ApplyToOneSide_RoleBias(playerState8, playerState9);
						RivalryBoost.ApplyToOneSide_RoleBias(playerState9, playerState8);
						if (currentTurn.HexBoard.ArePlayersNeighbours(playerState8.Id, playerState9.Id))
						{
							RivalryBoost.ApplyToBothSides(playerState8, playerState9, 0.1f);
						}
						DiplomaticState diplomaticState = currentTurn.CurrentDiplomaticTurn.GetDiplomaticStatus(playerState8, playerState9).DiplomaticState;
						BloodVassalageState bloodVassalageState = diplomaticState as BloodVassalageState;
						if (bloodVassalageState != null)
						{
							RivalryBoost.ApplyToOneSide_VassalAndLiege(playerState8, playerState9, bloodVassalageState.BloodLordId);
							RivalryBoost.ApplyToOneSide_VassalAndLiege(playerState9, playerState8, bloodVassalageState.BloodLordId);
						}
						VassalisedState vassalisedState = diplomaticState as VassalisedState;
						if (vassalisedState != null)
						{
							RivalryBoost.ApplyToOneSide_VassalAndThirdParty(playerState8, playerState9, vassalisedState.VassalId);
							RivalryBoost.ApplyToOneSide_VassalAndThirdParty(playerState9, playerState8, vassalisedState.VassalId);
						}
						RivalryProcessor.ProcessUndermineVictor(currentTurn, rules, playerState8, playerState9);
						RivalryProcessor.ProcessUndermineVictor(currentTurn, rules, playerState9, playerState8);
						DiplomaticState_DraconicRazzia diplomaticState_DraconicRazzia = diplomaticState as DiplomaticState_DraconicRazzia;
						if (diplomaticState_DraconicRazzia != null)
						{
							RivalryBoost.ApplyToOneSide_DraconicRazzia(playerState8, playerState9, diplomaticState_DraconicRazzia.ActorId);
							RivalryBoost.ApplyToOneSide_DraconicRazzia(playerState9, playerState8, diplomaticState_DraconicRazzia.ActorId);
						}
						DiplomaticState_ChainsOfAvarice diplomaticState_ChainsOfAvarice = diplomaticState as DiplomaticState_ChainsOfAvarice;
						if (diplomaticState_ChainsOfAvarice != null)
						{
							RivalryBoost.ApplyToOneSide_ChainsOfAvarice(playerState8, playerState9, diplomaticState_ChainsOfAvarice.ActorId);
							RivalryBoost.ApplyToOneSide_ChainsOfAvarice(playerState9, playerState8, diplomaticState_ChainsOfAvarice.ActorId);
						}
						DiplomaticState_LureOfExcess diplomaticState_LureOfExcess = diplomaticState as DiplomaticState_LureOfExcess;
						if (diplomaticState_LureOfExcess != null)
						{
							RivalryBoost.ApplyToOneSide_LureOfExcess(playerState8, playerState9, diplomaticState_LureOfExcess.ActorId);
							RivalryBoost.ApplyToOneSide_LureOfExcess(playerState9, playerState8, diplomaticState_LureOfExcess.ActorId);
						}
						RivalryBoost.ApplyToBothSides(playerState8, playerState9, RivalryBoost.FromState(diplomaticState));
					}
				}
			}
			foreach (PlayerState playerState10 in currentTurn.EnumeratePlayerStates(false, true))
			{
				ArchfiendAnimosity animosity = playerState10.Animosity;
				float num3;
				int num2 = playerState10.Animosity.GetPlayerIDWithMostAnimosity(out num3);
				if (num2 == playerState10.Id)
				{
					SimLogger logger = SimLogger.Logger;
					if (logger != null)
					{
						logger.Error(string.Format("Player {0} chose themselves are their rival", playerState10.Id));
					}
					num2 = int.MinValue;
				}
				PlayerState playerState11;
				float num4;
				if (!currentTurn.TryGetNemesis(playerState10, out playerState11, out num4) || playerState11 == null || playerState11.Eliminated)
				{
					animosity.SetNemesis(currentTurn, (num3 > 0.4f) ? num2 : int.MinValue);
				}
				else if (num2 != playerState11.Id && num3 > num4 + 0.2f)
				{
					animosity.SetNemesis(currentTurn, num2);
				}
			}
		}

		// Token: 0x040003FA RID: 1018
		public const string ChokingAbilityId = "Choking";

		// Token: 0x040003FB RID: 1019
		public const string BelphegorId = "Belphegor";
	}
}
