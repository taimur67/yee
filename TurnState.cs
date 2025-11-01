using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Core.StaticData;
using Game.StaticData;
using LoG.Simulation.Extensions;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020006DC RID: 1756
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class TurnState : IDeepClone<TurnState>
	{
		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06002081 RID: 8321 RVA: 0x00071953 File Offset: 0x0006FB53
		// (set) Token: 0x06002082 RID: 8322 RVA: 0x0007195B File Offset: 0x0006FB5B
		[JsonProperty]
		public PlayerState ForceMajeurePlayer { get; private set; } = new PlayerState(-1, 100);

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06002083 RID: 8323 RVA: 0x00071964 File Offset: 0x0006FB64
		[JsonIgnore]
		public IReadOnlyList<GameEvent> GameEvents
		{
			get
			{
				return this.TurnEvents;
			}
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06002084 RID: 8324 RVA: 0x0007196C File Offset: 0x0006FB6C
		[JsonIgnore]
		public IEnumerable<GameEntity> AllGameEntities
		{
			get
			{
				return this.EnumeratePlayerStates(true, true).Concat(this.AllGameItems);
			}
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06002085 RID: 8325 RVA: 0x00071981 File Offset: 0x0006FB81
		[JsonIgnore]
		public IReadOnlyCollection<GameItem> AllGameItems
		{
			get
			{
				return this.GameItems.Values;
			}
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06002086 RID: 8326 RVA: 0x0007198E File Offset: 0x0006FB8E
		[JsonIgnore]
		public IReadOnlyDictionary<Identifier, GameItem> GameItemLookup
		{
			get
			{
				return this.GameItems;
			}
		}

		// Token: 0x06002087 RID: 8327 RVA: 0x00071996 File Offset: 0x0006FB96
		public PlayerRole GetPlayerRole(int playerID)
		{
			return this.FindPlayerState(playerID, null).Role;
		}

		// Token: 0x06002088 RID: 8328 RVA: 0x000719A5 File Offset: 0x0006FBA5
		public static implicit operator SchemeId(TurnState turn)
		{
			return turn.GenerateNextSchemeId();
		}

		// Token: 0x06002089 RID: 8329 RVA: 0x000719AD File Offset: 0x0006FBAD
		public static implicit operator DecisionId(TurnState turn)
		{
			return turn.GenerateNextDecisionId();
		}

		// Token: 0x0600208A RID: 8330 RVA: 0x000719B5 File Offset: 0x0006FBB5
		private SchemeId GenerateNextSchemeId()
		{
			return (SchemeId)this.NextIdentifier();
		}

		// Token: 0x0600208B RID: 8331 RVA: 0x000719BD File Offset: 0x0006FBBD
		private DecisionId GenerateNextDecisionId()
		{
			return (DecisionId)this.NextIdentifier();
		}

		// Token: 0x0600208C RID: 8332 RVA: 0x000719C5 File Offset: 0x0006FBC5
		public Identifier GenerateIdentifier()
		{
			return (Identifier)this.NextIdentifier();
		}

		// Token: 0x0600208D RID: 8333 RVA: 0x000719D0 File Offset: 0x0006FBD0
		public int NextIdentifier()
		{
			int nextIdentifier = this._nextIdentifier;
			this._nextIdentifier = nextIdentifier + 1;
			return nextIdentifier;
		}

		// Token: 0x0600208E RID: 8334 RVA: 0x000719F0 File Offset: 0x0006FBF0
		public void DeepClone(out TurnState clone)
		{
			using (SimProfilerBlock.ProfilerBlock("TurnState.DeepClone"))
			{
				clone = new TurnState
				{
					Random = this.Random.DeepClone<SimulationRandom>(),
					ForceMajeurePlayer = this.ForceMajeurePlayer.DeepClone<PlayerState>(),
					_nextNFTId = this._nextNFTId,
					Victory = this.Victory.DeepClone<GameVictory>(),
					CompletedEdicts = this.CompletedEdicts.DeepClone(),
					NextEdictId = this.NextEdictId.DeepClone(),
					EmergencyVoteCount = this.EmergencyVoteCount,
					TurnValue = this.TurnValue,
					PreviousTurnPhase = this.PreviousTurnPhase,
					TurnPhase = this.TurnPhase,
					LastPhaseChangeTurn = this.LastPhaseChangeTurn,
					_nextIdentifier = this._nextIdentifier,
					TurnEvents = this.TurnEvents.DeepClone<GameEvent>(),
					GameItems = this.GameItems.DeepClone(),
					CurrentDiplomaticTurn = this.CurrentDiplomaticTurn.DeepClone<DiplomaticTurnState>(),
					BazaarState = this.BazaarState.DeepClone<BazaarState>(),
					HexBoard = this.HexBoard.DeepClone<HexBoard>(),
					RegentPlayerId = this.RegentPlayerId,
					ConclaveFavouriteId = this.ConclaveFavouriteId,
					RegencyOrder = this.RegencyOrder.DeepClone(),
					GlobalModifierStack = this.GlobalModifierStack.DeepClone<GlobalModifierStack>(),
					PlayerStates = this.PlayerStates.DeepClone<PlayerState>(),
					ActiveModules = this.ActiveModules.DeepClone<TurnModuleInstance>(),
					DeadItemReferences = this.DeadItemReferences.DeepClone(),
					PandaemoniumCapturedCount = this.PandaemoniumCapturedCount,
					StriderCarryCapacityMultiplier = this.StriderCarryCapacityMultiplier,
					VictoryRuleProcessors = this.VictoryRuleProcessors.DeepClone<VictoryRuleProcessor>(),
					IsPrivateSchemeValid = this.IsPrivateSchemeValid,
					Auras = this.Auras.DeepClone<Aura>()
				};
			}
		}

		// Token: 0x0600208F RID: 8335 RVA: 0x00071BE8 File Offset: 0x0006FDE8
		public TurnState PrepareForNewTurn(int turnValue)
		{
			this.TurnValue = turnValue;
			this.TurnEvents.Clear();
			return this;
		}

		// Token: 0x06002090 RID: 8336 RVA: 0x00071C00 File Offset: 0x0006FE00
		public ResourceNFT CreateNFT(params ResourceAccumulation[] accumulation)
		{
			int nextNFTId = this._nextNFTId;
			this._nextNFTId = nextNFTId + 1;
			return new ResourceNFT(nextNFTId, accumulation);
		}

		// Token: 0x06002091 RID: 8337 RVA: 0x00071C24 File Offset: 0x0006FE24
		public Guid PushGlobalModifier(ModifiableTargetGroup modifiableTargetGroup)
		{
			PlayerTargetGroup playerTargetGroup = modifiableTargetGroup as PlayerTargetGroup;
			if (playerTargetGroup == null)
			{
				GameItemTargetGroup gameItemTargetGroup = modifiableTargetGroup as GameItemTargetGroup;
				if (gameItemTargetGroup != null)
				{
					this.GlobalModifierStack.Push(gameItemTargetGroup);
				}
			}
			else
			{
				this.GlobalModifierStack.Push(playerTargetGroup);
			}
			return modifiableTargetGroup.Id;
		}

		// Token: 0x06002092 RID: 8338 RVA: 0x00071C67 File Offset: 0x0006FE67
		public void PopGlobalModifier(Guid id)
		{
			this.GlobalModifierStack.Pop(id);
		}

		// Token: 0x06002093 RID: 8339 RVA: 0x00071C78 File Offset: 0x0006FE78
		public int GetNextPlayerIndex(int currentPlayerIndex)
		{
			int num = currentPlayerIndex + 1;
			if (num >= this.PlayerStates.Count)
			{
				num = 0;
			}
			return num;
		}

		// Token: 0x06002094 RID: 8340 RVA: 0x00071C9A File Offset: 0x0006FE9A
		public IEnumerable<PlayerState> EnumeratePlayerStates(bool includeForceMajeure = false, bool includeEliminated = false)
		{
			if (includeForceMajeure)
			{
				yield return this.ForceMajeurePlayer;
			}
			foreach (PlayerState playerState in this.PlayerStates)
			{
				if (!playerState.Eliminated || (playerState.Eliminated && includeEliminated))
				{
					yield return playerState;
				}
			}
			List<PlayerState>.Enumerator enumerator = default(List<PlayerState>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06002095 RID: 8341 RVA: 0x00071CB8 File Offset: 0x0006FEB8
		public IEnumerable<PlayerState> EnumeratePlayerStatesExcept(int skippedId, bool includeForceMajeure = false, bool includeEliminated = false)
		{
			if (includeForceMajeure)
			{
				yield return this.ForceMajeurePlayer;
			}
			foreach (PlayerState playerState in this.PlayerStates)
			{
				if (playerState.Id != skippedId && (!playerState.Eliminated || includeEliminated))
				{
					yield return playerState;
				}
			}
			List<PlayerState>.Enumerator enumerator = default(List<PlayerState>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06002096 RID: 8342 RVA: 0x00071CDD File Offset: 0x0006FEDD
		public IEnumerable<Tuple<PlayerState, PlayerState>> EnumeratePlayerStatePairs(bool includeForceMajeure = false, bool isCommutative = false, bool includeEliminated = false)
		{
			List<PlayerState> players = IEnumerableExtensions.ToList<PlayerState>(this.EnumeratePlayerStates(includeForceMajeure, includeEliminated));
			int num;
			for (int i = 0; i < players.Count; i = num + 1)
			{
				for (int j = i + 1; j < players.Count; j = num + 1)
				{
					yield return new Tuple<PlayerState, PlayerState>(players[i], players[j]);
					if (!isCommutative)
					{
						yield return new Tuple<PlayerState, PlayerState>(players[j], players[i]);
					}
					num = j;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06002097 RID: 8343 RVA: 0x00071D04 File Offset: 0x0006FF04
		public PlayerState GetPlayerStateWithMostPrestige()
		{
			PlayerState result = null;
			int num = -1;
			foreach (PlayerState playerState in this.EnumeratePlayerStates(false, false))
			{
				int spendablePrestige = playerState.SpendablePrestige;
				if (spendablePrestige > num)
				{
					num = spendablePrestige;
					result = playerState;
				}
			}
			return result;
		}

		// Token: 0x06002098 RID: 8344 RVA: 0x00071D64 File Offset: 0x0006FF64
		public IEnumerable<PlayerState> GetConclaveMemberPlayers()
		{
			return from x in this.EnumeratePlayerStates(false, false)
			where !x.Excommunicated
			select x;
		}

		// Token: 0x06002099 RID: 8345 RVA: 0x00071D92 File Offset: 0x0006FF92
		public IEnumerable<PlayerState> GetNonEliminatedPlayers()
		{
			return this.EnumeratePlayerStates(false, false);
		}

		// Token: 0x0600209A RID: 8346 RVA: 0x00071D9C File Offset: 0x0006FF9C
		public int GetNumberOfPlayers(bool includeForceMajeure = false, bool includeEliminated = false)
		{
			return this.PlayerStates.Count((PlayerState x) => (!x.Eliminated | includeEliminated) && (x.Id != -1 | includeForceMajeure));
		}

		// Token: 0x0600209B RID: 8347 RVA: 0x00071DD4 File Offset: 0x0006FFD4
		public IEnumerable<PlayerState> EnumeratePlayerStatesInTurnOrder(bool includeForceMajeure = false, bool includeEliminated = false)
		{
			return this.EnumeratePlayerStatesStartingWithPlayer(this.RegentPlayerId, includeForceMajeure, includeEliminated);
		}

		// Token: 0x0600209C RID: 8348 RVA: 0x00071DE4 File Offset: 0x0006FFE4
		public IEnumerable<PlayerState> EnumeratePlayerStatesStartingWithPlayer(int startingRegentId, bool includeForceMajeure = false, bool includeEliminated = false)
		{
			int playerStatesCount = this.PlayerStates.Count;
			int num;
			for (int i = 0; i < playerStatesCount; i = num + 1)
			{
				int playerIdInRegencyOrder = this.GetPlayerIdInRegencyOrder(startingRegentId, i);
				PlayerState playerState = this.FindPlayerState(playerIdInRegencyOrder, null);
				if (!playerState.Eliminated || includeEliminated)
				{
					yield return playerState;
				}
				num = i;
			}
			if (includeForceMajeure)
			{
				yield return this.ForceMajeurePlayer;
			}
			yield break;
		}

		// Token: 0x0600209D RID: 8349 RVA: 0x00071E0C File Offset: 0x0007000C
		public PlayerState FindPlayerState(int playerId, string playfabId = null)
		{
			if (playerId == -1)
			{
				return this.ForceMajeurePlayer;
			}
			if (playfabId != null)
			{
				foreach (PlayerState playerState in this.PlayerStates)
				{
					if (playerState.PlayFabId == playfabId)
					{
						return playerState;
					}
				}
			}
			if (playerId >= 0 && playerId < this.PlayerStates.Count)
			{
				return this.PlayerStates[playerId];
			}
			return null;
		}

		// Token: 0x0600209E RID: 8350 RVA: 0x00071E9C File Offset: 0x0007009C
		public PlayerState FindPlayerState(PlayerIndex index)
		{
			return this.FindPlayerState((int)index, null);
		}

		// Token: 0x0600209F RID: 8351 RVA: 0x00071EA8 File Offset: 0x000700A8
		public PlayerState FindPlayerState(string archfiendId)
		{
			foreach (PlayerState playerState in this.PlayerStates)
			{
				if (playerState.ArchfiendId == archfiendId)
				{
					return playerState;
				}
			}
			return null;
		}

		// Token: 0x060020A0 RID: 8352 RVA: 0x00071F0C File Offset: 0x0007010C
		public Result ApplyAnimosityPreset(AnimosityPreset preset)
		{
			foreach (PlayerState playerState in this.EnumeratePlayerStates(false, false))
			{
				List<AnimosityData> animosity;
				if (!preset.TryGetAnimosity(playerState.Id, out animosity))
				{
					return new SimulationError("$No Animosity preset for archfiend ID {player.Id}. Does it need adding to the file?");
				}
				playerState.SetAnimosity(animosity);
			}
			return Result.Success;
		}

		// Token: 0x060020A1 RID: 8353 RVA: 0x00071F84 File Offset: 0x00070184
		public DiplomaticPairStatus GetDiplomaticStatus(PlayerState firstPlayer, PlayerState secondPlayer)
		{
			return this.GetDiplomaticStatus(firstPlayer.Id, secondPlayer.Id);
		}

		// Token: 0x060020A2 RID: 8354 RVA: 0x00071F98 File Offset: 0x00070198
		public DiplomaticPairStatus GetDiplomaticStatus(int firstPlayerId, int secondPlayerId)
		{
			PlayerPair playerPair = new PlayerPair(firstPlayerId, secondPlayerId);
			return this.GetDiplomaticStatus(playerPair);
		}

		// Token: 0x060020A3 RID: 8355 RVA: 0x00071FB5 File Offset: 0x000701B5
		public DiplomaticPairStatus GetDiplomaticStatus(PlayerPair playerPair)
		{
			return this.CurrentDiplomaticTurn[playerPair];
		}

		// Token: 0x060020A4 RID: 8356 RVA: 0x00071FC4 File Offset: 0x000701C4
		public void ClearDecisionRequests()
		{
			foreach (PlayerState playerState in this.EnumeratePlayerStates(true, false))
			{
				playerState.DecisionRequests.Clear();
			}
		}

		// Token: 0x060020A5 RID: 8357 RVA: 0x00072018 File Offset: 0x00070218
		public void ClearPlayerTurns()
		{
			foreach (PlayerState playerState in this.EnumeratePlayerStates(true, false))
			{
				playerState.PlayerTurn = new PlayerTurn();
			}
		}

		// Token: 0x060020A6 RID: 8358 RVA: 0x0007206C File Offset: 0x0007026C
		public DecisionAddedEvent AddDecisionToAskPlayer(int playerId, DecisionRequest decisionRequest)
		{
			PlayerState playerState = this.FindPlayerState(playerId, null);
			if (playerState == null)
			{
				return null;
			}
			playerState.AddDecisionRequest(decisionRequest);
			return new DecisionAddedEvent(playerId, decisionRequest.DecisionId);
		}

		// Token: 0x060020A7 RID: 8359 RVA: 0x0007209A File Offset: 0x0007029A
		public void RemoveDecisionToAskPlayer(int playerId, DecisionResponse decisionResponse)
		{
			PlayerState playerState = this.FindPlayerState(playerId, null);
			if (playerState == null)
			{
				return;
			}
			playerState.PlayerTurn.Decisions.Remove(decisionResponse);
		}

		// Token: 0x060020A8 RID: 8360 RVA: 0x000720BA File Offset: 0x000702BA
		public void AddKnowledgeModifier(PlayerIndex playerId, KnowledgeModifier knowledgeModifier, bool isUniqueKnowledge = false)
		{
			this.AddKnowledgeModifierInternal(this.FindPlayerState(playerId), knowledgeModifier, isUniqueKnowledge);
		}

		// Token: 0x060020A9 RID: 8361 RVA: 0x000720CC File Offset: 0x000702CC
		public void AddKnowledgeModifier(BitMask targets, KnowledgeModifier knowledgeModifier, bool isUniqueKnowledge = false)
		{
			foreach (PlayerState playerState in this.EnumeratePlayerStates(false, false))
			{
				if (targets.IsSet(playerState.Id))
				{
					this.AddKnowledgeModifierInternal(playerState, knowledgeModifier, isUniqueKnowledge);
				}
			}
		}

		// Token: 0x060020AA RID: 8362 RVA: 0x0007212C File Offset: 0x0007032C
		private void AddKnowledgeModifierInternal(PlayerState player, KnowledgeModifier modifier, bool isUnique)
		{
			player.AddKnowledgeModifier(modifier, isUnique);
		}

		// Token: 0x060020AB RID: 8363 RVA: 0x00072138 File Offset: 0x00070338
		public T AddGameItem<T>() where T : GameItem, new()
		{
			T t = Activator.CreateInstance<T>();
			t.Id = this.GenerateIdentifier();
			T item = t;
			return this.AddGameItem<T>(item);
		}

		// Token: 0x060020AC RID: 8364 RVA: 0x00072163 File Offset: 0x00070363
		public bool RemoveGameItem(Identifier id)
		{
			return this.RemoveGameItem<GameItem>(this.FetchGameItem(id));
		}

		// Token: 0x060020AD RID: 8365 RVA: 0x00072172 File Offset: 0x00070372
		public bool RemoveGameItem<T>(T item) where T : GameItem
		{
			return this.GameItems.Remove(item);
		}

		// Token: 0x060020AE RID: 8366 RVA: 0x0007218A File Offset: 0x0007038A
		public void ReplaceGameItem(GameItem item)
		{
			this.GameItems[item.Id] = item;
		}

		// Token: 0x060020AF RID: 8367 RVA: 0x0007219E File Offset: 0x0007039E
		public T AddGameItem<T>(T item) where T : GameItem
		{
			this.GameItems.Add(item.Id, item);
			return item;
		}

		// Token: 0x060020B0 RID: 8368 RVA: 0x000721C0 File Offset: 0x000703C0
		public GameItem FetchGameItem(Identifier id)
		{
			GameItem result;
			this.GameItems.TryGetValue(id, out result);
			return result;
		}

		// Token: 0x060020B1 RID: 8369 RVA: 0x000721E0 File Offset: 0x000703E0
		public GameItem FetchGameItem(string staticDataReference)
		{
			GameItem result;
			this.TryFetchGameItem<GameItem>(staticDataReference, out result);
			return result;
		}

		// Token: 0x060020B2 RID: 8370 RVA: 0x000721F8 File Offset: 0x000703F8
		public bool FetchGameItem(Identifier id, out GameItem item)
		{
			return this.GameItems.TryGetValue(id, out item);
		}

		// Token: 0x060020B3 RID: 8371 RVA: 0x00072207 File Offset: 0x00070407
		public T FetchGameItem<T>(Identifier id) where T : GameItem
		{
			return (T)((object)this.FetchGameItem(id));
		}

		// Token: 0x060020B4 RID: 8372 RVA: 0x00072215 File Offset: 0x00070415
		public T TryFetchGameItem<T>(Identifier id) where T : GameItem
		{
			return this.FetchGameItem(id) as T;
		}

		// Token: 0x060020B5 RID: 8373 RVA: 0x00072228 File Offset: 0x00070428
		public bool TryFetchGameItem<T>(Identifier id, out T value) where T : GameItem
		{
			value = this.TryFetchGameItem<T>(id);
			return value != null;
		}

		// Token: 0x060020B6 RID: 8374 RVA: 0x00072245 File Offset: 0x00070445
		public bool TryFetchGameItem(Identifier id, out GameItem value)
		{
			value = this.FetchGameItem(id);
			return value != null;
		}

		// Token: 0x060020B7 RID: 8375 RVA: 0x00072255 File Offset: 0x00070455
		public bool TryFetchGameItem<T>(ConfigRef staticDataId, out T value) where T : GameItem
		{
			if (staticDataId.IsEmpty())
			{
				value = default(T);
				return false;
			}
			return this.TryFetchGameItem<T>(staticDataId.Id, out value);
		}

		// Token: 0x060020B8 RID: 8376 RVA: 0x00072278 File Offset: 0x00070478
		public bool TryFetchGameItem<T>(string staticId, out T value) where T : GameItem
		{
			value = IEnumerableExtensions.FirstOrDefault<T>((from t in this.GameItems.Values
			where t.StaticDataId == staticId
			select t).OfType<T>());
			return value != null;
		}

		// Token: 0x060020B9 RID: 8377 RVA: 0x000722CC File Offset: 0x000704CC
		public bool TryFetchGameItemForPlayer<T>(int player, out T value) where T : GameItem
		{
			value = IEnumerableExtensions.FirstOrDefault<T>(this.GetGameItemsControlledBy<T>(player));
			return value != null;
		}

		// Token: 0x060020BA RID: 8378 RVA: 0x000722F0 File Offset: 0x000704F0
		public bool TryFetchGameItemForPlayer<T>(int player, Identifier id, out T value) where T : GameItem
		{
			value = this.GetGameItemsControlledBy<T>(player).FirstOrDefault((T t) => t.Id == id);
			return value != null;
		}

		// Token: 0x060020BB RID: 8379 RVA: 0x00072338 File Offset: 0x00070538
		public bool TryFetchGameItemForPlayer<T>(int player, string staticId, out T value) where T : GameItem
		{
			value = this.GetGameItemsControlledBy<T>(player).FirstOrDefault((T t) => t.StaticDataId == staticId);
			return value != null;
		}

		// Token: 0x060020BC RID: 8380 RVA: 0x00072380 File Offset: 0x00070580
		public GamePiece GetAssociatedGamePiece(GameItem item)
		{
			GamePiece gamePiece = item as GamePiece;
			if (gamePiece != null)
			{
				return gamePiece;
			}
			return this.GetControllingPiece(item.Id);
		}

		// Token: 0x060020BD RID: 8381 RVA: 0x000723A5 File Offset: 0x000705A5
		public bool DoesGamePieceHavePraetor(GamePiece piece)
		{
			return piece.Slots.Any((Identifier id) => this.FetchGameItem(id).Category == GameItemCategory.Praetor);
		}

		// Token: 0x060020BE RID: 8382 RVA: 0x000723C0 File Offset: 0x000705C0
		public Praetor GetPraetorOrDefaultOnGamePiece(GamePiece piece)
		{
			Praetor result = null;
			for (int i = 0; i < piece.Slots.Count; i++)
			{
				Identifier id = piece.Slots[i];
				if (this.TryFetchGameItem<Praetor>(id, out result))
				{
					break;
				}
			}
			return result;
		}

		// Token: 0x060020BF RID: 8383 RVA: 0x000723FE File Offset: 0x000705FE
		public bool HasControllingGamePiece(Identifier gameItemId)
		{
			return this.GetControllingPiece(gameItemId) != null;
		}

		// Token: 0x060020C0 RID: 8384 RVA: 0x0007240C File Offset: 0x0007060C
		public GamePiece GetControllingPiece(Identifier gameItemId)
		{
			return this.EnumerateAllGamePieces().FirstOrDefault((GamePiece x) => x.Slots.Contains(gameItemId));
		}

		// Token: 0x060020C1 RID: 8385 RVA: 0x0007243D File Offset: 0x0007063D
		public bool TryFindControllingPiece(Identifier gameItemId, out GamePiece piece)
		{
			piece = this.GetControllingPiece(gameItemId);
			return piece != null;
		}

		// Token: 0x060020C2 RID: 8386 RVA: 0x00072450 File Offset: 0x00070650
		public IEnumerable<GamePiece> GetPiecesControlledBy(int playerId)
		{
			return from x in this.EnumerateAllGamePieces()
			where x.ControllingPlayerId == playerId
			select x;
		}

		// Token: 0x060020C3 RID: 8387 RVA: 0x00072481 File Offset: 0x00070681
		public GamePiece GetHonourGuard(int playerID)
		{
			return this.GetPiecesControlledBy(playerID).FirstOrDefault((GamePiece t) => t.IsLegionOrTitan() && !t.CanBeConverted);
		}

		// Token: 0x060020C4 RID: 8388 RVA: 0x000724AE File Offset: 0x000706AE
		public IEnumerable<GamePiece> GetAllConvertableLegionsAndTitans(int playerID)
		{
			return from t in this.GetPiecesControlledBy(playerID)
			where t.IsLegionOrTitan() && t.CanBeConverted
			select t;
		}

		// Token: 0x060020C5 RID: 8389 RVA: 0x000724DC File Offset: 0x000706DC
		public bool HasItem(int playerID, ConfigRef staticItemID)
		{
			GameItem gameItem;
			if (!this.TryFetchGameItem<GameItem>(staticItemID.Id, out gameItem))
			{
				return false;
			}
			if (!gameItem.IsActive)
			{
				return false;
			}
			PlayerState playerState = this.FindControllingPlayer(gameItem);
			return playerState != null && playerState.Id == playerID;
		}

		// Token: 0x060020C6 RID: 8390 RVA: 0x0007251B File Offset: 0x0007071B
		public IEnumerable<GameItem> GetGameItemsControlledBy(int playerId)
		{
			return this.EnumerateGameItems<GameItem>(this.GetGameIdentifiersControlledBy(playerId, false).ToHashSet<Identifier>());
		}

		// Token: 0x060020C7 RID: 8391 RVA: 0x00072530 File Offset: 0x00070730
		public IEnumerable<GameItem> GetVaultItemsControlledBy(int playerId)
		{
			PlayerState playerState = this.FindPlayerState(playerId, null);
			return this.EnumerateGameItems<GameItem>(playerState.VaultedItems);
		}

		// Token: 0x060020C8 RID: 8392 RVA: 0x00072552 File Offset: 0x00070752
		public IEnumerable<T> GetGameItemsControlledBy<T>(int playerId) where T : GameItem
		{
			return this.GetGameItemsControlledBy(playerId).OfType<T>();
		}

		// Token: 0x060020C9 RID: 8393 RVA: 0x00072560 File Offset: 0x00070760
		public IEnumerable<GameItem> GetFieldedGameItemsControlledBy(int playerId)
		{
			return this.GetFieldedGameItemsControlledBy<GameItem>(playerId);
		}

		// Token: 0x060020CA RID: 8394 RVA: 0x00072569 File Offset: 0x00070769
		public IEnumerable<GameItem> GetFieldedGameItemsControlledBy<T>(int playerId) where T : GameItem
		{
			foreach (Identifier id in this.GetFieldedIdentifiersControlledBy(playerId))
			{
				T t;
				if (this.TryFetchGameItem<T>(id, out t) && !t.Category.IsPlaceholder())
				{
					yield return t;
				}
			}
			IEnumerator<Identifier> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060020CB RID: 8395 RVA: 0x00072580 File Offset: 0x00070780
		public IEnumerable<Identifier> GetFieldedIdentifiersControlledBy(int playerId)
		{
			List<Identifier>.Enumerator enumerator2;
			foreach (GamePiece gamePiece in this.GetActiveGamePiecesForPlayer(playerId))
			{
				yield return gamePiece.Id;
				foreach (Identifier identifier in gamePiece.Slots)
				{
					yield return identifier;
				}
				enumerator2 = default(List<Identifier>.Enumerator);
				gamePiece = null;
			}
			IEnumerator<GamePiece> enumerator = null;
			PlayerState playerState = this.FindPlayerState(playerId, null);
			foreach (Identifier identifier2 in playerState.RitualState.SlottedItems)
			{
				yield return identifier2;
			}
			enumerator2 = default(List<Identifier>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060020CC RID: 8396 RVA: 0x00072597 File Offset: 0x00070797
		public IEnumerable<Identifier> GetAllItemsSlottedIntoGamePieces(int playerId)
		{
			foreach (GamePiece gamePiece in this.GetActiveGamePiecesForPlayer(playerId))
			{
				foreach (Identifier identifier in gamePiece.Slots)
				{
					yield return identifier;
				}
				List<Identifier>.Enumerator enumerator2 = default(List<Identifier>.Enumerator);
			}
			IEnumerator<GamePiece> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060020CD RID: 8397 RVA: 0x000725AE File Offset: 0x000707AE
		public IEnumerable<GameItem> GetAllEventCardsHeldByPlayer(int playerID)
		{
			return from t in this.GetVaultItemsControlledBy(playerID)
			where t.Category == GameItemCategory.EventCard
			select t;
		}

		// Token: 0x060020CE RID: 8398 RVA: 0x000725DB File Offset: 0x000707DB
		public IEnumerable<Identifier> GetGameIdentifiersControlledBy(int playerId, bool includeRelics = false)
		{
			foreach (Identifier identifier in this.GetFieldedIdentifiersControlledBy(playerId))
			{
				yield return identifier;
			}
			IEnumerator<Identifier> enumerator = null;
			PlayerState playerState = this.FindPlayerState(playerId, null);
			foreach (Identifier identifier2 in playerState.VaultedItems)
			{
				yield return identifier2;
			}
			enumerator = null;
			foreach (Identifier identifier3 in playerState.ActiveSchemeCards)
			{
				yield return identifier3;
			}
			enumerator = null;
			if (includeRelics)
			{
				foreach (Identifier identifier4 in playerState.ActiveRelics)
				{
					yield return identifier4;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x060020CF RID: 8399 RVA: 0x000725F9 File Offset: 0x000707F9
		public IEnumerable<ConfigRef> EnumerateConsumedConfigRefs()
		{
			return this.DeadItemReferences.Concat(from t in this.AllGameEntities
			select t.StaticDataReference);
		}

		// Token: 0x060020D0 RID: 8400 RVA: 0x00072630 File Offset: 0x00070830
		public IEnumerable<GameItem> EnumerateAllGameItems()
		{
			return this.AllGameItems;
		}

		// Token: 0x060020D1 RID: 8401 RVA: 0x00072638 File Offset: 0x00070838
		public IEnumerable<T> EnumerateGameItems<T>(IEnumerable<Identifier> ids) where T : GameItem
		{
			foreach (Identifier id in ids)
			{
				T t;
				if (this.TryFetchGameItem<T>(id, out t))
				{
					yield return t;
				}
			}
			IEnumerator<Identifier> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060020D2 RID: 8402 RVA: 0x0007264F File Offset: 0x0007084F
		public IEnumerable<T> EnumerateGameItems<T>() where T : GameItem
		{
			foreach (GameItem gameItem in this.EnumerateAllGameItems())
			{
				T t = gameItem as T;
				if (t != null)
				{
					yield return t;
				}
			}
			IEnumerator<GameItem> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060020D3 RID: 8403 RVA: 0x00072660 File Offset: 0x00070860
		public IEnumerable<GameItem> EnumerateGameItems(ConfigRef staticDataRef)
		{
			return from t in this.AllGameItems
			where t.StaticDataReference == staticDataRef
			select t;
		}

		// Token: 0x060020D4 RID: 8404 RVA: 0x00072691 File Offset: 0x00070891
		public IEnumerable<T> EnumerateGameItems<T>(ConfigRef staticDataRef) where T : GameItem
		{
			return this.EnumerateGameItems(staticDataRef).OfType<T>();
		}

		// Token: 0x060020D5 RID: 8405 RVA: 0x000726A0 File Offset: 0x000708A0
		public IEnumerable<GameItem> EnumerateGameItems(string staticDataId)
		{
			return from t in this.AllGameItems
			where t.StaticDataId == staticDataId
			select t;
		}

		// Token: 0x060020D6 RID: 8406 RVA: 0x000726D1 File Offset: 0x000708D1
		public IEnumerable<T> EnumerateGameItems<T>(string staticDataId) where T : GameItem
		{
			return this.EnumerateGameItems(staticDataId).OfType<T>();
		}

		// Token: 0x060020D7 RID: 8407 RVA: 0x000726DF File Offset: 0x000708DF
		public IEnumerable<T> EnumerateActiveGameItems<T>() where T : GameItem
		{
			return from t in this.EnumerateAllGameItems().OfType<T>()
			where t.IsActive
			select t;
		}

		// Token: 0x060020D8 RID: 8408 RVA: 0x00072710 File Offset: 0x00070910
		public IEnumerable<GamePiece> EnumerateAllGamePieces()
		{
			foreach (GameItem gameItem in this.EnumerateAllGameItems())
			{
				GamePiece gamePiece = gameItem as GamePiece;
				if (gamePiece != null)
				{
					yield return gamePiece;
				}
			}
			IEnumerator<GameItem> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060020D9 RID: 8409 RVA: 0x00072720 File Offset: 0x00070920
		public IEnumerable<GamePiece> GetActiveGamePieces()
		{
			foreach (GamePiece gamePiece in this.EnumerateAllGamePieces())
			{
				if (gamePiece.IsActive)
				{
					yield return gamePiece;
				}
			}
			IEnumerator<GamePiece> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060020DA RID: 8410 RVA: 0x00072730 File Offset: 0x00070930
		public IEnumerable<GamePiece> GetPiecesByCategory(GamePieceCategory category)
		{
			return from t in this.GetActiveGamePieces()
			where t.SubCategory == category
			select t;
		}

		// Token: 0x060020DB RID: 8411 RVA: 0x00072764 File Offset: 0x00070964
		public IEnumerable<GamePiece> GetGamePiecesAtLocation(HexCoord coord)
		{
			return from t in this.EnumerateAllGamePieces()
			where t.Location == coord
			select t;
		}

		// Token: 0x060020DC RID: 8412 RVA: 0x00072798 File Offset: 0x00070998
		public IEnumerable<GamePiece> EnumeratePiecesInRadius(HexCoord coord, int radius)
		{
			return from t in this.EnumerateAllGamePieces()
			where coord.HexDistance(t.Location) <= radius
			select t;
		}

		// Token: 0x060020DD RID: 8413 RVA: 0x000727D0 File Offset: 0x000709D0
		public IEnumerable<GamePiece> GetActiveGamePiecesForPlayer(int playerId)
		{
			foreach (GamePiece gamePiece in this.GetActiveGamePieces())
			{
				if (gamePiece.ControllingPlayerId == playerId)
				{
					yield return gamePiece;
				}
			}
			IEnumerator<GamePiece> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060020DE RID: 8414 RVA: 0x000727E8 File Offset: 0x000709E8
		public IEnumerable<GamePiece> GetAllGamePiecesForPlayer(int playerId)
		{
			return from t in this.EnumerateAllGamePieces()
			where t.ControllingPlayerId == playerId
			select t;
		}

		// Token: 0x060020DF RID: 8415 RVA: 0x00072819 File Offset: 0x00070A19
		public IEnumerable<GamePiece> GetAllActiveLegionsForPlayer(int playerId)
		{
			return from t in this.GetActiveGamePiecesForPlayer(playerId)
			where t.IsLegionOrTitan()
			select t;
		}

		// Token: 0x060020E0 RID: 8416 RVA: 0x00072846 File Offset: 0x00070A46
		public IEnumerable<GamePiece> GetAllActiveLegions()
		{
			return from t in this.GetActiveGamePieces()
			where t.IsLegionOrTitan()
			select t;
		}

		// Token: 0x060020E1 RID: 8417 RVA: 0x00072872 File Offset: 0x00070A72
		public IEnumerable<GamePiece> GetAllActiveFixtures()
		{
			return from t in this.GetActiveGamePieces()
			where t.IsFixture()
			select t;
		}

		// Token: 0x060020E2 RID: 8418 RVA: 0x000728A0 File Offset: 0x00070AA0
		public IEnumerable<GamePiece> GetAllActivePoPsForPlayer(int playerId, bool includeStronghold = false)
		{
			return from t in this.GetActiveGamePiecesForPlayer(playerId)
			where t.SubCategory == GamePieceCategory.PoP || (includeStronghold && t.SubCategory == GamePieceCategory.Stronghold)
			select t;
		}

		// Token: 0x060020E3 RID: 8419 RVA: 0x000728D2 File Offset: 0x00070AD2
		public IEnumerable<GamePiece> GetInactiveGamePieces()
		{
			return from t in this.EnumerateAllGamePieces()
			where !t.IsActive
			select t;
		}

		// Token: 0x060020E4 RID: 8420 RVA: 0x00072900 File Offset: 0x00070B00
		public IEnumerable<GamePiece> GetPlacesOfPower(int playerId, bool includeStrongholds = false)
		{
			return this.GetActiveGamePiecesForPlayer(playerId).Where(delegate(GamePiece t)
			{
				if (includeStrongholds)
				{
					GamePieceCategory subCategory = t.SubCategory;
					return subCategory == GamePieceCategory.PoP || subCategory == GamePieceCategory.Stronghold;
				}
				return t.SubCategory == GamePieceCategory.PoP;
			});
		}

		// Token: 0x060020E5 RID: 8421 RVA: 0x00072932 File Offset: 0x00070B32
		public IEnumerable<GamePiece> GetAllStrongholds()
		{
			foreach (PlayerState playerState in this.EnumeratePlayerStates(false, false))
			{
				GamePiece stronghold = this.GetStronghold(playerState.Id);
				if (stronghold != null)
				{
					yield return stronghold;
				}
			}
			IEnumerator<PlayerState> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060020E6 RID: 8422 RVA: 0x00072944 File Offset: 0x00070B44
		public GamePiece GetStronghold(int playerId)
		{
			PlayerState playerState = this.FindPlayerState(playerId, null);
			if (playerState.StrongholdId == Identifier.Invalid)
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Warn(string.Format("Stronghold ID for player {0} is invalid. This is an illegal configuration.", playerId));
				}
				return null;
			}
			GamePiece gamePiece = this.FetchGameItem<GamePiece>(playerState.StrongholdId);
			if (gamePiece == null)
			{
				SimLogger logger2 = SimLogger.Logger;
				if (logger2 == null)
				{
					return gamePiece;
				}
				logger2.Warn(string.Format("Failed to find a stronghold for player {0}. This is likely due to an invalid PlayerState configuration.", playerId));
			}
			return gamePiece;
		}

		// Token: 0x060020E7 RID: 8423 RVA: 0x000729B3 File Offset: 0x00070BB3
		public GamePiece GetPandaemonium()
		{
			return this.EnumerateAllGamePieces().FirstOrDefault((GamePiece t) => t.SubCategory == GamePieceCategory.Pandaemonium);
		}

		// Token: 0x060020E8 RID: 8424 RVA: 0x000729E0 File Offset: 0x00070BE0
		public bool GetPandaemoniumOwnedByConclave(bool resultNoPanda)
		{
			GamePiece pandaemonium = this.GetPandaemonium();
			if (pandaemonium != null)
			{
				return pandaemonium.ControllingPlayerId == this.ForceMajeurePlayer.Id;
			}
			return resultNoPanda;
		}

		// Token: 0x060020E9 RID: 8425 RVA: 0x00072A0C File Offset: 0x00070C0C
		public GamePiece GetActiveGamePieceAt(HexCoord coord)
		{
			coord = this.HexBoard.ToRelativeHex(coord);
			return (from x in this.EnumerateAllGamePieces()
			where x.IsActive
			select x).FirstOrDefault((GamePiece t) => t.Location == coord);
		}

		// Token: 0x060020EA RID: 8426 RVA: 0x00072A78 File Offset: 0x00070C78
		public bool TryGetActiveGamePieceAt(HexCoord coord, out GamePiece actor)
		{
			actor = this.GetActiveGamePieceAt(coord);
			return actor != null;
		}

		// Token: 0x060020EB RID: 8427 RVA: 0x00072A88 File Offset: 0x00070C88
		public GamePiece GetGamePieceAt(HexCoord coord)
		{
			coord = this.HexBoard.ToRelativeHex(coord);
			return this.GetActiveGamePieces().FirstOrDefault((GamePiece t) => t.Location == coord);
		}

		// Token: 0x060020EC RID: 8428 RVA: 0x00072AD0 File Offset: 0x00070CD0
		public bool AnyAdjacentFixture(HexCoord coord, int playerId = -2147483648)
		{
			foreach (HexCoord coord2 in this.HexBoard.GetNeighbours(coord, false))
			{
				GamePiece gamePieceAt = this.GetGamePieceAt(coord2);
				if (gamePieceAt != null && gamePieceAt.IsFixture() && (playerId == -2147483648 || gamePieceAt.ControllingPlayerId == playerId))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060020ED RID: 8429 RVA: 0x00072B48 File Offset: 0x00070D48
		public bool AllPlayerTurnsSubmitted()
		{
			return this.EnumeratePlayerStates(false, false).All((PlayerState t) => t.PlayerTurn != null);
		}

		// Token: 0x060020EE RID: 8430 RVA: 0x00072B78 File Offset: 0x00070D78
		public bool SubmitPlayerTurn(int playerId, PlayerTurn playerTurn)
		{
			PlayerState playerState = this.FindPlayerState(playerId, null);
			if (playerState == null)
			{
				return false;
			}
			playerState.PlayerTurn = playerTurn;
			return true;
		}

		// Token: 0x060020EF RID: 8431 RVA: 0x00072B9C File Offset: 0x00070D9C
		public void AssignGameEventIds()
		{
			int num = 0;
			foreach (GameEvent gameEvent in this.GetGameEvents())
			{
				num = (gameEvent.EventID = num + 1);
			}
		}

		// Token: 0x060020F0 RID: 8432 RVA: 0x00072BF0 File Offset: 0x00070DF0
		public T AddGameEvent<T>(T gameEvent) where T : GameEvent
		{
			this.TurnEvents.Add(gameEvent);
			return gameEvent;
		}

		// Token: 0x060020F1 RID: 8433 RVA: 0x00072C04 File Offset: 0x00070E04
		public IEnumerable<GameEvent> GetGameEvents()
		{
			return this.TurnEvents.SelectMany((GameEvent t) => t.EnumerateSelfAndAllChildEvents());
		}

		// Token: 0x060020F2 RID: 8434 RVA: 0x00072C30 File Offset: 0x00070E30
		public IEnumerable<T> GetGameEvents<T>() where T : GameEvent
		{
			return this.GetGameEvents().OfType<T>();
		}

		// Token: 0x060020F3 RID: 8435 RVA: 0x00072C3D File Offset: 0x00070E3D
		public bool TryGetGameEvent<T>(out T evt) where T : GameEvent
		{
			evt = IEnumerableExtensions.FirstOrDefault<T>(this.GetGameEvents<T>());
			return evt != null;
		}

		// Token: 0x060020F4 RID: 8436 RVA: 0x00072C5E File Offset: 0x00070E5E
		public bool HasGameEventOfType<T>() where T : GameEvent
		{
			return this.GetGameEvents().Any((GameEvent e) => e.GetType() == typeof(T));
		}

		// Token: 0x060020F5 RID: 8437 RVA: 0x00072C8A File Offset: 0x00070E8A
		public void RemoveHiddenEvents(int playerId)
		{
			this.TurnEvents.RemoveHiddenGameEvents(playerId);
		}

		// Token: 0x060020F6 RID: 8438 RVA: 0x00072C98 File Offset: 0x00070E98
		public void SetNeutral(GamePiece pop)
		{
			if (pop.HasTag<EntityTag_CannotBeNeutral>())
			{
				return;
			}
			this.HexBoard.SetOwnership(pop.Location, -1);
			pop.ControllingPlayerId = -1;
		}

		// Token: 0x060020F7 RID: 8439 RVA: 0x00072CBC File Offset: 0x00070EBC
		public HexCoord FirstVacantTraversableHexCoord(List<HexCoord> coord)
		{
			coord.RemoveAll(delegate(HexCoord c)
			{
				Hex hex = this.HexBoard[c];
				if (hex == null)
				{
					return true;
				}
				switch (hex.Type)
				{
				case TerrainType.Mountain:
					return true;
				case TerrainType.Impassable:
					return true;
				case TerrainType.River:
					return true;
				case TerrainType.Volcano:
					return true;
				}
				return false;
			});
			List<HexCoord> list = IEnumerableExtensions.ToList<HexCoord>(coord);
			foreach (GamePiece gamePiece in this.EnumerateAllGamePieces())
			{
				HexCoord item = this.HexBoard.ToRelativeHex(gamePiece.Location);
				if (coord.Contains(item))
				{
					list.Remove(item);
				}
			}
			if (IEnumerableExtensions.Any<HexCoord>(list))
			{
				return IEnumerableExtensions.First<HexCoord>(list);
			}
			return HexCoord.Invalid;
		}

		// Token: 0x060020F8 RID: 8440 RVA: 0x00072D54 File Offset: 0x00070F54
		public bool DoesPlayerControlItem(int playerId, Identifier itemId)
		{
			return this.GetGameIdentifiersControlledBy(playerId, false).Any((Identifier t) => t == itemId);
		}

		// Token: 0x060020F9 RID: 8441 RVA: 0x00072D88 File Offset: 0x00070F88
		public PlayerState FindControllingPlayer(GameItem item)
		{
			if (item == null)
			{
				return null;
			}
			GamePiece gamePiece = item as GamePiece;
			if (gamePiece != null)
			{
				return this.FindPlayerState(gamePiece.ControllingPlayerId, null);
			}
			foreach (PlayerState playerState in this.EnumeratePlayerStates(true, true))
			{
				if (this.DoesPlayerControlItem(playerState.Id, item.Id))
				{
					return playerState;
				}
			}
			return null;
		}

		// Token: 0x060020FA RID: 8442 RVA: 0x00072E08 File Offset: 0x00071008
		public PlayerState FindControllingPlayer(Identifier itemId)
		{
			if (itemId == Identifier.Invalid)
			{
				return null;
			}
			GameItem item;
			if (!this.TryFetchGameItem(itemId, out item))
			{
				return null;
			}
			return this.FindControllingPlayer(item);
		}

		// Token: 0x060020FB RID: 8443 RVA: 0x00072E2F File Offset: 0x0007102F
		public bool TryFindControllingPlayer(GameItem item, out PlayerState player)
		{
			player = this.FindControllingPlayer(item);
			return player != null;
		}

		// Token: 0x060020FC RID: 8444 RVA: 0x00072E3F File Offset: 0x0007103F
		public bool TryFindControllingPlayer(Identifier itemId, out PlayerState player)
		{
			player = this.FindControllingPlayer(itemId);
			return player != null;
		}

		// Token: 0x060020FD RID: 8445 RVA: 0x00072E4F File Offset: 0x0007104F
		private int GetRegentIndex(int regentId)
		{
			int num = this.RegencyOrder.IndexOf(regentId);
			if (num == -1)
			{
				SimLogger logger = SimLogger.Logger;
				if (logger == null)
				{
					return num;
				}
				logger.Error(string.Format("Could not find regentId: {0} in RegencyOrder", regentId));
			}
			return num;
		}

		// Token: 0x060020FE RID: 8446 RVA: 0x00072E80 File Offset: 0x00071080
		public int GetPlayerIdInRegencyOrder(int baseRegentId, int offset)
		{
			if (offset < 0)
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Error("Negative offset is not supported for GetPlayerIdInRegencyOrder");
				}
			}
			if (this.RegencyOrder.Count == 0)
			{
				SimLogger logger2 = SimLogger.Logger;
				if (logger2 != null)
				{
					logger2.Error("RegencyOrder has not been initialised");
				}
				return offset;
			}
			int index = (this.GetRegentIndex(baseRegentId) + offset) % this.RegencyOrder.Count;
			return this.RegencyOrder[index];
		}

		// Token: 0x060020FF RID: 8447 RVA: 0x00072EEC File Offset: 0x000710EC
		public bool TryGetNemesis(PlayerState player, out PlayerState nemesis)
		{
			float num;
			return this.TryGetNemesis(player, out nemesis, out num);
		}

		// Token: 0x06002100 RID: 8448 RVA: 0x00072F04 File Offset: 0x00071104
		public bool TryGetNemesis(PlayerState player, out PlayerState nemesis, out float animosityTowardsNemesis)
		{
			nemesis = null;
			animosityTowardsNemesis = -1f;
			int currentNemesisId = player.Animosity.CurrentNemesisId;
			if (currentNemesisId == player.Id)
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Error(string.Format("Player {0} is their own nemesis", player.Id));
				}
				return false;
			}
			if (currentNemesisId == -2147483648 || currentNemesisId == -1)
			{
				return false;
			}
			nemesis = this.FindPlayerState(currentNemesisId, null);
			if (nemesis == null)
			{
				return false;
			}
			if (nemesis.Eliminated)
			{
				nemesis = null;
				return false;
			}
			animosityTowardsNemesis = player.Animosity.GetValue(currentNemesisId);
			return true;
		}

		// Token: 0x06002101 RID: 8449 RVA: 0x00072F94 File Offset: 0x00071194
		public bool IsMyNemesisShared(int actorId)
		{
			PlayerState playerState = this.FindPlayerState(actorId, null);
			if (playerState == null)
			{
				return false;
			}
			int currentNemesisId = playerState.Animosity.CurrentNemesisId;
			if (currentNemesisId == -2147483648)
			{
				return false;
			}
			foreach (PlayerState playerState2 in this.EnumeratePlayerStates(false, false))
			{
				int num;
				if (playerState2.Id != actorId && playerState2.Id != currentNemesisId && playerState2.Role != PlayerRole.Human && (!this.CurrentDiplomaticTurn.IsVassalOfAny(playerState2.Id, out num) || num != currentNemesisId) && playerState2.Animosity.CurrentNemesisId != currentNemesisId)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002102 RID: 8450 RVA: 0x0007304C File Offset: 0x0007124C
		public bool TryGetConclaveFavouriteId(out int conclaveFavouriteId)
		{
			PlayerState conclaveFavourite = this.GetConclaveFavourite();
			if (conclaveFavourite.Eliminated || conclaveFavourite.Excommunicated)
			{
				conclaveFavouriteId = int.MinValue;
				return false;
			}
			conclaveFavouriteId = conclaveFavourite.Id;
			return true;
		}

		// Token: 0x06002103 RID: 8451 RVA: 0x00073082 File Offset: 0x00071282
		public PlayerState GetConclaveFavourite()
		{
			return this.FindPlayerState(this.ConclaveFavouriteId, null);
		}

		// Token: 0x06002104 RID: 8452 RVA: 0x00073094 File Offset: 0x00071294
		public bool IsForSale(GameItemStaticData item)
		{
			return this.GetAllBazaarItems().Any((GameItem y) => y.StaticDataId == item.Id);
		}

		// Token: 0x06002105 RID: 8453 RVA: 0x000730C5 File Offset: 0x000712C5
		public bool IsForSale(GameItem item)
		{
			return this.BazaarState.IsForSale(item);
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x000730D3 File Offset: 0x000712D3
		public IEnumerable<GameItem> GetAllBazaarItems()
		{
			return this.BazaarState.GetAllBazaarItems().Select(new Func<Identifier, GameItem>(this.FetchGameItem)).ExcludeNull<GameItem>();
		}

		// Token: 0x06002107 RID: 8455 RVA: 0x000730F6 File Offset: 0x000712F6
		public IEnumerable<GameItem> GetAllBazaarItemsForCategory(GameItemCategory category, bool includeEarlyAccess = false)
		{
			return this.BazaarState.GetAllItemsForCategory(category, includeEarlyAccess).Select(new Func<Identifier, GameItem>(this.FetchGameItem)).ExcludeNull<GameItem>();
		}

		// Token: 0x06002108 RID: 8456 RVA: 0x0007311B File Offset: 0x0007131B
		public IEnumerable<GameItem> GetEarlyAccessBazaarItemsForCategory(GameItemCategory category)
		{
			return this.BazaarState.GetEarlyAccessBazaarItemsForCategory(category).Select(new Func<Identifier, GameItem>(this.FetchGameItem)).ExcludeNull<GameItem>();
		}

		// Token: 0x06002109 RID: 8457 RVA: 0x00073140 File Offset: 0x00071340
		public bool IsInEndGame()
		{
			using (List<VictoryRuleProcessor>.Enumerator enumerator = this.VictoryRuleProcessors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsInEndGame())
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600210A RID: 8458 RVA: 0x0007319C File Offset: 0x0007139C
		public void SetTurnPhase(TurnPhase phase)
		{
			if (this.TurnPhase == phase)
			{
				return;
			}
			this.PreviousTurnPhase = this.TurnPhase;
			this.TurnPhase = phase;
			this.LastPhaseChangeTurn = this.TurnValue;
		}

		// Token: 0x0600210B RID: 8459 RVA: 0x000731C8 File Offset: 0x000713C8
		public int GetRandomRoll(int min, int max, bool rollWithAdvantage = false)
		{
			int num = this.Random.Next(min, max);
			if (rollWithAdvantage)
			{
				num = Math.Max(num, this.Random.Next(min, max));
			}
			return num;
		}

		// Token: 0x04000E4B RID: 3659
		public const int InvalidTurnValue = -1;

		// Token: 0x04000E4C RID: 3660
		[JsonProperty]
		[PublicKnowledge]
		public SimulationRandom Random = new SimulationRandom();

		// Token: 0x04000E4E RID: 3662
		[JsonProperty]
		private int _nextNFTId;

		// Token: 0x04000E4F RID: 3663
		[JsonProperty]
		[PublicKnowledge]
		public GameVictory Victory;

		// Token: 0x04000E50 RID: 3664
		[JsonProperty]
		[PublicKnowledge]
		public List<string> CompletedEdicts = new List<string>();

		// Token: 0x04000E51 RID: 3665
		[JsonProperty]
		[PublicKnowledge]
		public string NextEdictId;

		// Token: 0x04000E52 RID: 3666
		[JsonProperty]
		[PublicKnowledge]
		public int EmergencyVoteCount;

		// Token: 0x04000E53 RID: 3667
		[JsonProperty]
		[PublicKnowledge]
		public int TurnValue;

		// Token: 0x04000E54 RID: 3668
		[JsonProperty]
		[PublicKnowledge]
		public TurnPhase PreviousTurnPhase;

		// Token: 0x04000E55 RID: 3669
		[JsonProperty]
		[PublicKnowledge]
		public TurnPhase TurnPhase;

		// Token: 0x04000E56 RID: 3670
		[JsonProperty]
		[PublicKnowledge]
		public int LastPhaseChangeTurn;

		// Token: 0x04000E57 RID: 3671
		[JsonProperty]
		[PublicKnowledge]
		public int _nextIdentifier = 1;

		// Token: 0x04000E58 RID: 3672
		[JsonProperty]
		[PublicKnowledge]
		private List<GameEvent> TurnEvents = new List<GameEvent>();

		// Token: 0x04000E59 RID: 3673
		[JsonProperty]
		[PublicKnowledge]
		private SortedDictionary<Identifier, GameItem> GameItems = new SortedDictionary<Identifier, GameItem>();

		// Token: 0x04000E5A RID: 3674
		[JsonProperty]
		[PublicKnowledge]
		public DiplomaticTurnState CurrentDiplomaticTurn = new DiplomaticTurnState();

		// Token: 0x04000E5B RID: 3675
		[JsonProperty]
		[PublicKnowledge]
		public BazaarState BazaarState = new BazaarState();

		// Token: 0x04000E5C RID: 3676
		[JsonProperty]
		[PublicKnowledge]
		public HexBoard HexBoard;

		// Token: 0x04000E5D RID: 3677
		[JsonProperty]
		[PublicKnowledge]
		public int RegentPlayerId;

		// Token: 0x04000E5E RID: 3678
		[JsonProperty]
		[PublicKnowledge]
		public int ConclaveFavouriteId;

		// Token: 0x04000E5F RID: 3679
		[JsonProperty]
		[PublicKnowledge]
		public List<int> RegencyOrder = new List<int>();

		// Token: 0x04000E60 RID: 3680
		[JsonProperty]
		[PublicKnowledge]
		public GlobalModifierStack GlobalModifierStack = new GlobalModifierStack();

		// Token: 0x04000E61 RID: 3681
		[JsonProperty]
		public List<PlayerState> PlayerStates = new List<PlayerState>();

		// Token: 0x04000E62 RID: 3682
		[JsonProperty]
		[PublicKnowledge]
		public HashSet<ConfigRef> DeadItemReferences = new HashSet<ConfigRef>();

		// Token: 0x04000E63 RID: 3683
		[JsonProperty]
		public List<TurnModuleInstance> ActiveModules = new List<TurnModuleInstance>();

		// Token: 0x04000E64 RID: 3684
		[JsonProperty]
		[DefaultValue(0)]
		[PublicKnowledge]
		public int PandaemoniumCapturedCount;

		// Token: 0x04000E65 RID: 3685
		[JsonProperty]
		[DefaultValue(true)]
		[PublicKnowledge]
		public bool IsPrivateSchemeValid = true;

		// Token: 0x04000E66 RID: 3686
		[JsonProperty]
		[DefaultValue(1)]
		public int StriderCarryCapacityMultiplier = 1;

		// Token: 0x04000E67 RID: 3687
		[JsonProperty]
		[PublicKnowledge]
		public List<VictoryRuleProcessor> VictoryRuleProcessors = new List<VictoryRuleProcessor>();

		// Token: 0x04000E68 RID: 3688
		[JsonProperty]
		[PublicKnowledge]
		public List<Aura> Auras = new List<Aura>();
	}
}
