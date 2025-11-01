using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x020001BB RID: 443
	public static class RivalryBoost
	{
		// Token: 0x06000834 RID: 2100 RVA: 0x000265DC File Offset: 0x000247DC
		public static float FromState(DiplomaticState state)
		{
			if (state is NeutralState)
			{
				return -0.15f;
			}
			if (state is VendettaState)
			{
				return 0.15f;
			}
			if (state is ArmisticeState)
			{
				return -0.15f;
			}
			if (state is BloodFeudState)
			{
				return 0.2f;
			}
			if (state is ExcommunicatedState)
			{
				return 0.1f;
			}
			return 0f;
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x00026634 File Offset: 0x00024834
		public static void ApplyToBothSides(PlayerState us, PlayerState them, float boostAmount)
		{
			us.Animosity.BoostValue(them.Id, boostAmount);
			them.Animosity.BoostValue(us.Id, boostAmount);
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x0002665C File Offset: 0x0002485C
		public static void ApplyToOneSide_RoleBias(PlayerState playerToUpdate, PlayerState other)
		{
			if (playerToUpdate.AITags.Contains(AITag.BiasedAgainstHumans))
			{
				playerToUpdate.Animosity.BoostValue(other.Id, (other.Role == PlayerRole.Human) ? 0.1f : -0.1f);
			}
			if (playerToUpdate.AITags.Contains(AITag.BiasedAgainstOtherAIs))
			{
				playerToUpdate.Animosity.BoostValue(other.Id, (other.Role != PlayerRole.Human) ? 0.1f : -0.1f);
			}
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x000266D4 File Offset: 0x000248D4
		public static void ApplyToOneSide_VassalAndLiege(PlayerState playerToUpdate, PlayerState other, int lordId)
		{
			if (lordId == other.Id)
			{
				bool flag = playerToUpdate.IsPowerBehindTheThrone || (playerToUpdate.IsKingmaker && playerToUpdate.KingmakerPuppetId == other.Id);
				playerToUpdate.Animosity.BoostValue(other.Id, flag ? -0.05f : -0.25f);
				return;
			}
			playerToUpdate.Animosity.BoostValue(other.Id, -0.2f);
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x00026750 File Offset: 0x00024950
		public static void ApplyToOneSide_VassalAndThirdParty(PlayerState playerToUpdate, PlayerState other, int vassalId)
		{
			playerToUpdate.Animosity.BoostValue(other.Id, (vassalId == other.Id) ? -0.15f : -0.1f);
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x00026778 File Offset: 0x00024978
		public static void ApplyToOneSide_DraconicRazzia(PlayerState playerToUpdate, PlayerState other, int instigatorId)
		{
			playerToUpdate.Animosity.BoostValue(other.Id, (playerToUpdate.Id == instigatorId) ? 0.1f : 0.3f);
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x000267A0 File Offset: 0x000249A0
		public static void ApplyToOneSide_ChainsOfAvarice(PlayerState playerToUpdate, PlayerState other, int instigatorId)
		{
			playerToUpdate.Animosity.BoostValue(other.Id, (playerToUpdate.Id == instigatorId) ? -0.2f : -0.15f);
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x000267C8 File Offset: 0x000249C8
		public static void ApplyToOneSide_LureOfExcess(PlayerState playerToUpdate, PlayerState other, int instigatorId)
		{
			playerToUpdate.Animosity.BoostValue(other.Id, (playerToUpdate.Id == instigatorId) ? -0.2f : 0.1f);
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x000267F0 File Offset: 0x000249F0
		public static void ApplyToOneSide_MorePrestige(PlayerState playerToUpdate, PlayerState other, float gameProgress)
		{
			float num = 5f / MathF.Min(0.25f, gameProgress);
			float num2 = (float)playerToUpdate.SpendablePrestige + (float)playerToUpdate.PassivePrestige * num;
			float num3 = (float)other.SpendablePrestige + (float)other.PassivePrestige * num;
			if (num2 < num3)
			{
				playerToUpdate.Animosity.BoostValue(other.Id, 0.4f * gameProgress);
			}
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x00026857 File Offset: 0x00024A57
		public static void ApplyToOneSide_HigherRank(PlayerState playerToUpdate, PlayerState other)
		{
			if (playerToUpdate.Rank < other.Rank)
			{
				playerToUpdate.Animosity.BoostValue(other.Id, 0.2f);
			}
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x00026880 File Offset: 0x00024A80
		public static void ApplyTowardsSpecific_ConclaveFavourite(PlayerState playerToUpdate, PlayerState other, int conclaveFavouriteId, float gameProgress)
		{
			float num = gameProgress * gameProgress;
			if (playerToUpdate.Id != conclaveFavouriteId)
			{
				playerToUpdate.Animosity.BoostValue(conclaveFavouriteId, 0.5f * num);
			}
			if (other.Id != conclaveFavouriteId)
			{
				playerToUpdate.Animosity.BoostValue(other.Id, -0.5f * num);
			}
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x000268CE File Offset: 0x00024ACE
		public static void ApplyTowardsSpecific_Usurper(PlayerState playerToUpdate, PlayerState other, int usurperId)
		{
			if (playerToUpdate.Id != usurperId)
			{
				playerToUpdate.Animosity.BoostValue(usurperId, 0.45f);
			}
			if (other.Id != usurperId)
			{
				playerToUpdate.Animosity.BoostValue(other.Id, -0.8f);
			}
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x0002690C File Offset: 0x00024B0C
		public static void ApplyTowardsBelphegor(PlayerState nonBelphegorPlayer, PlayerState belphegor, TurnContext turnContext)
		{
			using (SimProfilerBlock.ProfilerBlock(""))
			{
				foreach (GamePiece gamePiece in turnContext.CurrentTurn.GetActiveGamePiecesForPlayer(nonBelphegorPlayer.Id))
				{
					float boostAmount = 0f;
					if (gamePiece.IsFixture())
					{
						boostAmount = ((gamePiece.Id == nonBelphegorPlayer.StrongholdId) ? 0.8f : 0.4f);
					}
					else if (gamePiece.IsLegionOrTitan())
					{
						boostAmount = 0.2f;
					}
					if (RivalryProcessor.IsChoking(turnContext, gamePiece))
					{
						nonBelphegorPlayer.Animosity.BoostValue(belphegor.Id, boostAmount);
					}
				}
			}
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x000269D4 File Offset: 0x00024BD4
		public static void ApplyAuraAnimosity(PlayerState belphegor, TurnContext turnContext)
		{
			using (SimProfilerBlock.ProfilerBlock(""))
			{
				List<PlayerState> list = IEnumerableExtensions.ToList<PlayerState>(turnContext.CurrentTurn.EnumeratePlayerStates(false, false));
				list.Remove(belphegor);
				using (IEnumerator<GamePiece> enumerator = turnContext.CurrentTurn.GetActiveGamePiecesForPlayer(belphegor.Id).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.StaticDataId == "The Dark Pylon")
						{
							foreach (PlayerState playerState in list)
							{
								playerState.Animosity.BoostValue(belphegor.Id, 0.1f);
							}
						}
					}
				}
				HashSet<HexCoord> hashSet = new HashSet<HexCoord>();
				foreach (ValueTuple<HexCoord, Aura> valueTuple in turnContext.CurrentTurn.EnumerateAllAuras())
				{
					HexCoord item = valueTuple.Item1;
					Aura item2 = valueTuple.Item2;
					if (item2.AbilitySourceId == "Noxious Fumes")
					{
						foreach (HexCoord item3 in turnContext.HexBoard.EnumerateRangeNormalized(item, item2.Radius))
						{
							hashSet.Add(item3);
						}
					}
				}
				foreach (PlayerState playerState2 in list)
				{
					foreach (HexCoord item4 in turnContext.HexBoard.GetHexCoordsControlledByPlayer(playerState2.Id))
					{
						if (hashSet.Contains(item4))
						{
							playerState2.Animosity.BoostValue(belphegor.Id, 0.2f);
							break;
						}
					}
				}
			}
		}

		// Token: 0x040003D1 RID: 977
		public const float MakeDemand = 0.4f;

		// Token: 0x040003D2 RID: 978
		public const float Extort = 0.6f;

		// Token: 0x040003D3 RID: 979
		public const float Insult = 0.4f;

		// Token: 0x040003D4 RID: 980
		public const float VileCalumny_ScapegoatToInstigator = 0.3f;

		// Token: 0x040003D5 RID: 981
		public const float VileCalumny_TargetToInstigator = 0.1f;

		// Token: 0x040003D6 RID: 982
		public const float VileCalumny_TargetToScapeGoat = 0.3f;

		// Token: 0x040003D7 RID: 983
		public const float Humiliate = 0.6f;

		// Token: 0x040003D8 RID: 984
		public const float LureOfExcess = 0.3f;

		// Token: 0x040003D9 RID: 985
		public const float LegionKilled = 0.5f;

		// Token: 0x040003DA RID: 986
		public const float RitualCast = 0.3f;

		// Token: 0x040003DB RID: 987
		public const float MachineInvoked = 0.9f;

		// Token: 0x040003DC RID: 988
		public const float NeutralOngoing = -0.15f;

		// Token: 0x040003DD RID: 989
		public const float VendettaOngoing = 0.15f;

		// Token: 0x040003DE RID: 990
		public const float ArmisticeOngoing = -0.15f;

		// Token: 0x040003DF RID: 991
		public const float BloodFeudOngoing = 0.2f;

		// Token: 0x040003E0 RID: 992
		public const float NegativeBiasOngoing = 0.1f;

		// Token: 0x040003E1 RID: 993
		public const float PositiveBiasOngoing = -0.1f;

		// Token: 0x040003E2 RID: 994
		public const float NeighboursOngoing = 0.1f;

		// Token: 0x040003E3 RID: 995
		public const float HigherRankOngoing = 0.2f;

		// Token: 0x040003E4 RID: 996
		public const float MaxHigherPrestigeOngoing = 0.4f;

		// Token: 0x040003E5 RID: 997
		public const float MaxConclaveFavouriteOngoing = 0.5f;

		// Token: 0x040003E6 RID: 998
		public const float MaxNotConclaveFavouriteOngoing = -0.5f;

		// Token: 0x040003E7 RID: 999
		public const float BloodLiegeToVassalOngoing = -0.2f;

		// Token: 0x040003E8 RID: 1000
		public const float BloodVassalToLiegeOngoing = -0.05f;

		// Token: 0x040003E9 RID: 1001
		public const float OtherToVassalOngoing = -0.15f;

		// Token: 0x040003EA RID: 1002
		public const float VassalToOtherOngoing = -0.1f;

		// Token: 0x040003EB RID: 1003
		public const float BloodVassalToLiegeOngoing_AsManipulator = -0.25f;

		// Token: 0x040003EC RID: 1004
		public const float ExcommunicatedOngoing = 0.1f;

		// Token: 0x040003ED RID: 1005
		public const float UsurperOngoing = 0.45f;

		// Token: 0x040003EE RID: 1006
		public const float NotUsurperDuringUsurpationOngoing = -0.8f;

		// Token: 0x040003EF RID: 1007
		public const float ChainsOfAvariceOngoing_AsInstigator = -0.2f;

		// Token: 0x040003F0 RID: 1008
		public const float ChainsOfAvariceOngoing_AsTarget = -0.15f;

		// Token: 0x040003F1 RID: 1009
		public const float LureOfExcessOngoing_AsInstigator = -0.2f;

		// Token: 0x040003F2 RID: 1010
		public const float LureOfExcessOngoing_AsTarget = 0.1f;

		// Token: 0x040003F3 RID: 1011
		public const float DraconicRazziaOngoing_AsInstigator = 0.1f;

		// Token: 0x040003F4 RID: 1012
		public const float DraconicRazziaOngoing_AsTarget = 0.3f;

		// Token: 0x040003F5 RID: 1013
		public const float ChokingStronghold = 0.8f;

		// Token: 0x040003F6 RID: 1014
		public const float ChokingFixture = 0.4f;

		// Token: 0x040003F7 RID: 1015
		public const float ChokingLegion = 0.2f;

		// Token: 0x040003F8 RID: 1016
		public const float PerPylonScore = 0.1f;

		// Token: 0x040003F9 RID: 1017
		public const float ChokingTerritory = 0.2f;
	}
}
