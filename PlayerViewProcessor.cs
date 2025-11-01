using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000301 RID: 769
	public static class PlayerViewProcessor
	{
		// Token: 0x06000EF2 RID: 3826 RVA: 0x0003B2D8 File Offset: 0x000394D8
		public static TurnState GeneratePlayerView(GameDatabase database, GameState state, int playerId)
		{
			TurnState currentTurn = state.CurrentTurn;
			return PlayerViewProcessor.GeneratePlayerView(database, currentTurn, playerId);
		}

		// Token: 0x06000EF3 RID: 3827 RVA: 0x0003B2F8 File Offset: 0x000394F8
		public static TurnState GeneratePlayerView(GameDatabase database, GameState state, int turnValue, int playerId)
		{
			TurnState turnState = state.FetchTurnStateByTurnId(turnValue);
			return PlayerViewProcessor.GeneratePlayerView(database, turnState, playerId);
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x0003B318 File Offset: 0x00039518
		public static TurnState GeneratePlayerView(GameDatabase database, in TurnState truth, int playerId)
		{
			TurnState result;
			using (SimProfilerBlock.ProfilerBlock(""))
			{
				TurnState turnState = truth.DeepClone<TurnState>();
				if (playerId == -1)
				{
					result = truth;
				}
				else
				{
					turnState = PlayerViewProcessor.GeneratePublicKnowledge(turnState);
					PlayerViewProcessor.RemoveHiddenEvents(turnState, playerId);
					PlayerViewProcessor.RemoveHiddenKnowledge(turnState, truth, playerId);
					PlayerViewProcessor.AddPrivateKnowledge(turnState, truth, playerId);
					PlayerViewProcessor.ProcessKnowledgeContexts(truth, turnState, playerId);
					PlayerViewProcessor.AddActiveRituals(truth, turnState, playerId);
					PlayerViewProcessor.ProcessKnowledgeModifiers(database, turnState, truth, playerId);
					PlayerViewProcessor.FinalKnowledgePass(turnState, truth, playerId);
					result = turnState;
				}
			}
			return result;
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x0003B3A4 File Offset: 0x000395A4
		private static void AddActiveRituals(in TurnState truth, TurnState playerView, int playerId)
		{
			foreach (PlayerState playerState in truth.EnumeratePlayerStates(false, false))
			{
				foreach (Identifier id in playerState.RitualState.SlottedItems)
				{
					ActiveRitual activeRitual;
					if (truth.TryFetchGameItem<ActiveRitual>(id, out activeRitual) && activeRitual.AffectsPlayer(truth, playerId))
					{
						PlayerState apparentSource = activeRitual.GetApparentSource(truth);
						PlayerState playerState2;
						if (apparentSource == null)
						{
							playerState2 = playerView.FindPlayerState(PlayerIndex.ForceMajeure);
						}
						else
						{
							playerState2 = playerView.FindPlayerState(apparentSource);
						}
						if (!playerState2.RitualState.SlottedItems.Contains(activeRitual))
						{
							playerState2.RitualState.SlottedItems.Add(activeRitual);
						}
					}
				}
			}
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x0003B4A0 File Offset: 0x000396A0
		private static void FinalKnowledgePass(TurnState playerView, in TurnState truth, int playerId)
		{
			PlayerViewProcessor.RemoveUnknownPendingVendettaConditions(playerView, truth, playerView.FindPlayerState(playerId, null));
			PlayerViewProcessor.RemovePrestigeIfHidden(playerView, playerId);
			PlayerViewProcessor.RemoveEnemySchemeProgress(playerView, playerId);
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x0003B4C0 File Offset: 0x000396C0
		private static void RemovePrestigeIfHidden(TurnState playerView, int currentPlayerId)
		{
			foreach (PlayerState playerState in playerView.EnumeratePlayerStates(false, false))
			{
				if (playerState.Id != currentPlayerId && playerState.HiddenPrestige)
				{
					playerState.SpendablePrestige = 0;
				}
			}
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x0003B528 File Offset: 0x00039728
		private static void RemoveEnemySchemeProgress(TurnState playerView, int currentPlayerId)
		{
			foreach (PlayerState playerState in playerView.EnumeratePlayerStates(false, false))
			{
				if (playerState.Id != currentPlayerId)
				{
					foreach (Identifier id in playerState.ActiveSchemeCards)
					{
						foreach (ObjectiveCondition objectiveCondition in playerView.FetchGameItem<SchemeCard>(id).Scheme.Conditions)
						{
							objectiveCondition.Count = 0;
						}
					}
				}
			}
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x0003B5F8 File Offset: 0x000397F8
		private static void ProcessKnowledgeContexts(in TurnState truth, TurnState view, int playerId)
		{
			foreach (KeyValuePair<int, PlayerKnowledgeContext> keyValuePair in truth.FindPlayerState(playerId, null).PlayerKnowledgeContexts)
			{
				int num;
				PlayerKnowledgeContext playerKnowledgeContext;
				keyValuePair.Deconstruct(out num, out playerKnowledgeContext);
				int playerId2 = num;
				PlayerKnowledgeContext context = playerKnowledgeContext;
				PlayerState opponentView = view.FindPlayerState(playerId2, null);
				PlayerViewProcessor.ApplyKnowledgeContext(truth, view, opponentView, context);
			}
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x0003B674 File Offset: 0x00039874
		private static void ApplyKnowledgeContext(in TurnState truth, TurnState view, PlayerState opponentView, PlayerKnowledgeContext context)
		{
			foreach (Identifier identifier in context.RelicContents)
			{
				if (!IEnumerableExtensions.Contains<Identifier>(opponentView.ActiveRelics, identifier))
				{
					opponentView.GiveRelic(identifier);
				}
			}
			foreach (KeyValuePair<PowerType, PlayerPowerLevel> keyValuePair in context.KnownPowers)
			{
				PowerType powerType;
				PlayerPowerLevel playerPowerLevel;
				keyValuePair.Deconstruct(out powerType, out playerPowerLevel);
				PowerType type = powerType;
				PlayerPowerLevel value = playerPowerLevel;
				opponentView.PowersLevels[type] = value;
			}
			foreach (Identifier item in context.EventContents)
			{
				opponentView.AddToVault(item);
			}
			foreach (Identifier item2 in context.VaultContents)
			{
				opponentView.AddToVault(item2);
			}
			opponentView.RitualState.SlottedItems.AddRange(context.RitualContents);
			foreach (Identifier schemeCardId in context.SchemeContents)
			{
				opponentView.AddSchemeCard(schemeCardId);
			}
			opponentView.Resources.Clear();
			opponentView.Resources.Add(view.CreateNFT(new ResourceAccumulation[]
			{
				context.ResourceContents
			}));
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x0003B840 File Offset: 0x00039A40
		private static void RemoveHiddenEvents(TurnState playerView, int playerId)
		{
			using (SimProfilerBlock.ProfilerBlock(""))
			{
				playerView.RemoveHiddenEvents(playerId);
			}
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x0003B87C File Offset: 0x00039A7C
		private static void RemoveHiddenKnowledge(TurnState playerView, TurnState truth, int playerId)
		{
			using (SimProfilerBlock.ProfilerBlock(""))
			{
				foreach (GamePiece gamePiece in truth.EnumerateAllGamePieces())
				{
					if (gamePiece.ControllingPlayerId != playerId)
					{
						foreach (Identifier id in gamePiece.Slots)
						{
							Stratagem stratagem = playerView.FetchGameItem(id) as Stratagem;
							if (stratagem != null)
							{
								stratagem.Tactics = null;
								stratagem.ClearAbilities();
							}
						}
					}
				}
				foreach (PlayerState playerState in truth.EnumeratePlayerStates(true, true))
				{
					PlayerState playerState2 = playerView.FindPlayerState(playerId, null);
					if (playerState.Id != playerId && playerState.HiddenPrestige)
					{
						playerState2.SpendablePrestige = 0;
					}
				}
				foreach (DiplomaticPairStatus diplomaticPairStatus in playerView.CurrentDiplomaticTurn.Standings)
				{
					VendettaState vendettaState = diplomaticPairStatus.DiplomaticState as VendettaState;
					if (vendettaState != null && vendettaState.Vendetta.ActorId != playerId && !truth.FindPlayerState(playerId, null).HasVendettaKnowledge)
					{
						vendettaState.Vendetta.Objective.ClearConditions();
					}
				}
			}
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x0003BA70 File Offset: 0x00039C70
		private static void RemoveUnknownPendingVendettaConditions(TurnState playerView, TurnState truth, PlayerState player)
		{
			if (player.HasVendettaKnowledge)
			{
				return;
			}
			foreach (DecisionRequest decisionRequest in player.DecisionRequests)
			{
				IGrievanceAccessor grievanceAccessor = decisionRequest as IGrievanceAccessor;
				if (grievanceAccessor != null && grievanceAccessor.PrivateGrievance)
				{
					VendettaContext vendettaContext = grievanceAccessor.GrievanceResponse as VendettaContext;
					if (vendettaContext != null)
					{
						vendettaContext.Objective = null;
						vendettaContext.PrestigeReward = null;
					}
				}
			}
			foreach (GameEvent gameEvent in playerView.GameEvents)
			{
				if (gameEvent.TriggeringPlayerID != player.Id)
				{
					IGrievanceAccessor grievanceAccessor = gameEvent as IGrievanceAccessor;
					if (grievanceAccessor != null)
					{
						VendettaContext vendettaContext2 = grievanceAccessor.GrievanceResponse as VendettaContext;
						if (vendettaContext2 != null)
						{
							vendettaContext2.Objective = null;
							vendettaContext2.PrestigeReward = null;
						}
					}
				}
			}
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x0003BB68 File Offset: 0x00039D68
		public static TurnState GeneratePublicKnowledge(TurnState unmodifiedPlayerView)
		{
			TurnState result;
			using (SimProfilerBlock.ProfilerBlock(""))
			{
				TurnState turnState = new TurnState();
				turnState.CopyFieldsWithAttributeFrom(unmodifiedPlayerView, typeof(PublicKnowledgeAttribute));
				foreach (PlayerState playerState in unmodifiedPlayerView.PlayerStates)
				{
					PlayerState playerState2 = new PlayerState(playerState.Id, -1);
					playerState2.CopyFieldsWithAttributeFrom(playerState, typeof(PublicKnowledgeAttribute));
					playerState2.PowersLevels = new PlayerPowersLevels(int.MinValue, 6);
					foreach (Identifier id in playerState.ActiveSchemeCards)
					{
						SchemeCard schemeCard;
						if (unmodifiedPlayerView.TryFetchGameItem<SchemeCard>(id, out schemeCard) && schemeCard.Scheme.IsVisible)
						{
							playerState2.AddSchemeCard(schemeCard);
						}
					}
					foreach (Identifier id2 in playerState.VaultedItems)
					{
						GameItem gameItem;
						if (unmodifiedPlayerView.TryFetchGameItem(id2, out gameItem) && gameItem.PublicKnowledge)
						{
							playerState2.AddToVault(gameItem);
						}
					}
					foreach (Identifier id3 in playerState.ActiveRelics)
					{
						GameItem gameItem2;
						if (unmodifiedPlayerView.TryFetchGameItem(id3, out gameItem2) && gameItem2.PublicKnowledge)
						{
							playerState2.GiveRelic(gameItem2);
						}
					}
					turnState.PlayerStates.Add(playerState2);
				}
				result = turnState;
			}
			return result;
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x0003BD90 File Offset: 0x00039F90
		private static void ProcessKnowledgeModifiers(GameDatabase database, TurnState view, TurnState truth, int playerId)
		{
			using (SimProfilerBlock.ProfilerBlock(""))
			{
				List<KnowledgeModifier> list = IEnumerableExtensions.ToList<KnowledgeModifier>(PlayerViewProcessor.GenerateAllKnowledgeModifiersForPlayer(new TurnContext(null, view, database), playerId));
				PlayerState player = view.FindPlayerState(playerId, null);
				foreach (PlayerState playerState in truth.EnumeratePlayerStates(false, false))
				{
					foreach (IModifier modifier in truth.GenerateAllModifiersFor(database, playerState))
					{
						IRevealsKnowledge revealsKnowledge = modifier as IRevealsKnowledge;
						if (revealsKnowledge != null)
						{
							list.Add(revealsKnowledge.GenerateKnowledgeModifier(truth, player, playerState));
						}
					}
				}
				foreach (KnowledgeModifier knowledgeModifier in from x in list
				orderby x.Priority descending
				select x)
				{
					knowledgeModifier.Process(view, truth, playerId);
				}
			}
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x0003BED0 File Offset: 0x0003A0D0
		public static IEnumerable<KnowledgeModifier> GenerateAllKnowledgeModifiersForPlayer(TurnContext context, int playerId)
		{
			foreach (KnowledgeModifier knowledgeModifier in PlayerViewProcessor.GetKnowledgeModifiersForPlayer(context, playerId))
			{
				yield return knowledgeModifier;
			}
			IEnumerator<KnowledgeModifier> enumerator = null;
			foreach (KnowledgeModifier knowledgeModifier2 in PlayerViewProcessor.GenerateGlobalTransientKnowledgeModifiers(context, playerId))
			{
				yield return knowledgeModifier2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x0003BEE7 File Offset: 0x0003A0E7
		public static IEnumerable<KnowledgeModifier> GetKnowledgeModifiersForPlayer(TurnContext context, int playerId)
		{
			return context.CurrentTurn.FindPlayerState(playerId, null).AllKnowledgeModifiers;
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x0003BEFB File Offset: 0x0003A0FB
		public static IEnumerable<KnowledgeModifier> GenerateGlobalTransientKnowledgeModifiers(TurnContext context, int playerId)
		{
			TurnState turn = context.CurrentTurn;
			PlayerState player = turn.FindPlayerState(playerId, null);
			foreach (PlayerState targetPlayer in turn.EnumeratePlayerStates(false, false))
			{
				IEnumerable<IModifier> enumerable = turn.GenerateAllModifiersFor(context.Database, targetPlayer);
				foreach (IModifier modifier in enumerable)
				{
					IRevealsKnowledge revealsKnowledge = modifier as IRevealsKnowledge;
					if (revealsKnowledge != null)
					{
						yield return revealsKnowledge.GenerateKnowledgeModifier(turn, player, targetPlayer);
					}
				}
				IEnumerator<IModifier> enumerator2 = null;
				targetPlayer = null;
			}
			IEnumerator<PlayerState> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x0003BF14 File Offset: 0x0003A114
		private static void AddPrivateKnowledge(TurnState view, TurnState truth, int playerId)
		{
			using (SimProfilerBlock.ProfilerBlock(""))
			{
				PlayerState playerState = truth.FindPlayerState(playerId, null);
				if (playerState != null)
				{
					view.PlayerStates[playerId] = playerState.DeepClone<PlayerState>();
				}
			}
		}
	}
}
