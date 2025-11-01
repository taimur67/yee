using System;
using System.Collections.Generic;
using System.Linq;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000291 RID: 657
	public static class GameGeneratorUtil
	{
		// Token: 0x06000CC2 RID: 3266 RVA: 0x00033BDB File Offset: 0x00031DDB
		public static T SpawnGameItem<T>(this TurnState turn) where T : GameItem, new()
		{
			T t = turn.AddGameItem<T>();
			t.Status = GameItemStatus.InPlay;
			return t;
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x00033BF0 File Offset: 0x00031DF0
		public static T SpawnGameItem<T>(this TurnState turn, IdentifiableStaticData data, PlayerState owner = null, bool prepayUpkeep = false) where T : GameItem, new()
		{
			T t = turn.SpawnGameItem<T>();
			t.ConfigureFrom(data);
			if (prepayUpkeep)
			{
				t.CheatPayUpkeep();
			}
			return t;
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x00033C20 File Offset: 0x00031E20
		public static GamePiece SpawnGamePiece(this TurnContext context, GamePieceStaticData data, PlayerState owner, HexCoord location)
		{
			GamePiece gamePiece = context.CurrentTurn.SpawnGameItem(data, owner, false);
			gamePiece.Location = location;
			gamePiece.SpawnTurn = context.CurrentTurn.TurnValue;
			gamePiece.ControllingPlayerId = ((owner != null) ? owner.Id : -1);
			context.RecalculateModifiersFor(gamePiece.Id);
			context.RecalculateSupportModifiers(gamePiece.ControllingPlayerId);
			context.RecalculateAurasFromGamePiece(gamePiece);
			return gamePiece;
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x00033C88 File Offset: 0x00031E88
		public static GamePiece CloneGamePiece(this TurnState turn, GamePiece source, HexCoord location)
		{
			GamePiece gamePiece = source.DeepClone<GamePiece>();
			gamePiece.Location = location;
			gamePiece.Id = turn.GenerateIdentifier();
			turn.AddGameItem<GamePiece>(gamePiece);
			return gamePiece;
		}

		// Token: 0x06000CC6 RID: 3270 RVA: 0x00033CB8 File Offset: 0x00031EB8
		public static EventCard SpawnEventCard(this TurnState turn, EventCardStaticData staticData, PlayerState owner)
		{
			return turn.SpawnGameItem(staticData, owner, false);
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x00033CC3 File Offset: 0x00031EC3
		public static Praetor SpawnPraetor(this TurnState turn, PraetorStaticData staticData, PlayerState owner)
		{
			return turn.SpawnGameItem(staticData, owner, false);
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x00033CCE File Offset: 0x00031ECE
		public static Relic SpawnRelic(this TurnState turn, RelicStaticData staticData, PlayerState owner)
		{
			return turn.SpawnGameItem(staticData, owner, false);
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x00033CD9 File Offset: 0x00031ED9
		public static Artifact SpawnArtifact(this TurnState turn, ArtifactStaticData staticData, PlayerState owner)
		{
			return turn.SpawnGameItem(staticData, owner, false);
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x00033CE4 File Offset: 0x00031EE4
		public static GameItem SpawnGameItem(this TurnProcessContext context, GameItemStaticData data, PlayerState owner = null)
		{
			TurnState currentTurn = context.CurrentTurn;
			ManuscriptStaticData manuscriptStaticData = data as ManuscriptStaticData;
			GameItem result;
			if (manuscriptStaticData == null)
			{
				ArtifactStaticData artifactStaticData = data as ArtifactStaticData;
				if (artifactStaticData == null)
				{
					EventCardStaticData eventCardStaticData = data as EventCardStaticData;
					if (eventCardStaticData == null)
					{
						GamePieceStaticData gamePieceStaticData = data as GamePieceStaticData;
						if (gamePieceStaticData == null)
						{
							PraetorStaticData praetorStaticData = data as PraetorStaticData;
							if (praetorStaticData == null)
							{
								RelicStaticData relicStaticData = data as RelicStaticData;
								if (relicStaticData == null)
								{
									result = null;
								}
								else
								{
									result = currentTurn.SpawnRelic(relicStaticData, owner);
								}
							}
							else
							{
								result = currentTurn.SpawnPraetor(praetorStaticData, owner);
							}
						}
						else
						{
							result = context.SpawnGamePiece(gamePieceStaticData, owner, HexCoord.Invalid);
						}
					}
					else
					{
						result = currentTurn.SpawnEventCard(eventCardStaticData, owner);
					}
				}
				else
				{
					result = currentTurn.SpawnArtifact(artifactStaticData, owner);
				}
			}
			else
			{
				result = currentTurn.SpawnManuscript(manuscriptStaticData, owner);
			}
			return result;
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x00033D91 File Offset: 0x00031F91
		public static SchemeCard SpawnSchemeCard(this TurnState turn, SchemeObjective scheme)
		{
			SchemeCard schemeCard = turn.SpawnGameItem<SchemeCard>();
			schemeCard.Scheme = scheme;
			return schemeCard;
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x00033DA0 File Offset: 0x00031FA0
		public static Manuscript SpawnManuscript(this TurnState turn, ManuscriptStaticData staticData, PlayerState owner)
		{
			return turn.SpawnGameItem(staticData, owner, false);
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x00033DAC File Offset: 0x00031FAC
		public static GamePiece SpawnHostLegion(this TurnProcessContext context, PlayerState owner, HexCoord location)
		{
			ArchFiendStaticData archFiendStaticData = context.Database.Fetch<ArchFiendStaticData>(owner.ArchfiendId);
			if (archFiendStaticData == null)
			{
				return null;
			}
			GamePieceStaticData staticData = context.Database.Fetch(archFiendStaticData.Legion);
			return context.SpawnHostLegion(staticData, owner, location);
		}

		// Token: 0x06000CCE RID: 3278 RVA: 0x00033DF4 File Offset: 0x00031FF4
		public static GamePiece SpawnHostLegion(this TurnProcessContext context, GamePieceStaticData staticData, PlayerState owner, HexCoord location)
		{
			GamePiece gamePiece = context.SpawnLegion(staticData, owner, location);
			owner.PersonalGuardId = gamePiece.Id;
			return gamePiece;
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x00033E18 File Offset: 0x00032018
		public static GamePiece SpawnStronghold(this TurnProcessContext context, PlayerState owner, HexCoord location)
		{
			ArchFiendStaticData archFiendStaticData = context.Database.Fetch<ArchFiendStaticData>(owner.ArchfiendId);
			if (archFiendStaticData == null)
			{
				return null;
			}
			GamePieceStaticData staticData = context.Database.Fetch(archFiendStaticData.Garrison);
			GamePiece gamePiece = context.SpawnFixture(staticData, owner, location);
			owner.StrongholdId = gamePiece.Id;
			return gamePiece;
		}

		// Token: 0x06000CD0 RID: 3280 RVA: 0x00033E6B File Offset: 0x0003206B
		public static GamePiece SpawnPandemonium(this TurnProcessContext context, HexCoord location)
		{
			return context.SpawnFixture(context.Database.Fetch<GamePieceStaticData>("Pandemonium"), null, location);
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x00033E85 File Offset: 0x00032085
		public static GamePiece SpawnFixture(this TurnProcessContext context, GamePieceStaticData staticData, PlayerState owner, HexCoord location)
		{
			return context.SpawnGamePiece(staticData, owner, location);
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x00033E90 File Offset: 0x00032090
		public static GamePiece SpawnLegion(this TurnProcessContext context, GamePieceStaticData staticData, PlayerState owner, HexCoord location)
		{
			return context.SpawnGamePiece(staticData, owner, location);
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x00033E9C File Offset: 0x0003209C
		public static GamePiece SpawnLegionWithBehaviour<TInstance, TStaticData>(this TurnProcessContext turnProcessContext, GamePieceStaticData staticData, PlayerState owner, HexCoord location, TStaticData behaviourData) where TInstance : NeutralForceTurnModuleInstance where TStaticData : NeutralForceTurnModuleStaticData
		{
			TurnState currentTurn = turnProcessContext.CurrentTurn;
			GamePiece gamePiece = turnProcessContext.SpawnGamePiece(staticData, owner, location);
			TInstance tinstance = (TInstance)((object)TurnModuleInstanceFactory.CreateInstance(currentTurn, behaviourData));
			currentTurn.AddActiveTurnModule(turnProcessContext, tinstance);
			tinstance.GamePieceId = gamePiece.Id;
			return gamePiece;
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x00033EEC File Offset: 0x000320EC
		public static PlayerState AddPlayerState(this TurnState turn, ArchFiendStaticData data, GameRules rules = null, PlayerRole role = PlayerRole.Human, int orderSlots = 2, string playfabId = null, string platformDisplayName = null)
		{
			PlayerState playerState = new PlayerState(turn.PlayerStates.Count, orderSlots);
			playerState.ConfigureFrom(data);
			playerState.InitializePersistentData(turn.Random.Next());
			playerState.Role = role;
			playerState.PlayFabId = playfabId;
			playerState.PlatformDisplayName = platformDisplayName;
			if (rules != null)
			{
				playerState.CombatRewardMultiplier = rules.BaseCombatRewardMultiplier;
			}
			turn.PlayerStates.Add(playerState);
			return playerState;
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x00033F5C File Offset: 0x0003215C
		public static PlayerState AddPlayerState(this TurnProcessContext context, ArchFiendStaticData data, PlayerRole role = PlayerRole.Human, int orderSlots = 2, string playfabId = null, string platformDisplayName = null)
		{
			return context.CurrentTurn.AddPlayerState(data, context.Rules, role, orderSlots, playfabId, platformDisplayName);
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x00033F78 File Offset: 0x00032178
		public static GamePiece SpawnRandomPoP(this TurnProcessContext context, SimulationRandom rand, PlayerState player, HexCoord location)
		{
			List<GamePieceStaticData> list = IEnumerableExtensions.ToList<GamePieceStaticData>(context.EnumerateSpawnablePoPs());
			if (list.Count == 0)
			{
				return null;
			}
			GamePieceStaticData staticData = list.Random(rand);
			return context.SpawnFixture(staticData, player, location);
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x00033FAC File Offset: 0x000321AC
		public static IEnumerable<GamePieceStaticData> EnumerateSpawnablePoPs(this TurnProcessContext context)
		{
			GameDatabase database = context.Database;
			TurnState turn = context.CurrentTurn;
			return from x in database.Enumerate<GamePieceStaticData>()
			where x.SubCategory == GamePieceCategory.PoP && x.CanSpawnInGameGeneration
			where !turn.EnumerateConsumedConfigRefs().Any((ConfigRef y) => x.ConfigRef == y)
			select x;
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x0003400C File Offset: 0x0003220C
		public static void PopulateNeutralForces(this TurnProcessContext context)
		{
			NeutralForcePopulationData neutralForcePopulationData;
			if (!context.Database.TryFetchSingle(out neutralForcePopulationData))
			{
				return;
			}
			GamePieceStaticData gamePieceStaticData = context.Database.Fetch(neutralForcePopulationData.StartingNeutralForce);
			NeutralForceTurnModuleStaticData neutralForceTurnModuleStaticData = context.Database.Fetch(neutralForcePopulationData.StartingNeutralForceBehaviour);
			if (gamePieceStaticData == null)
			{
				return;
			}
			if (neutralForceTurnModuleStaticData == null)
			{
				return;
			}
			Hex[] array = IEnumerableExtensions.ToArray<Hex>(from x in context.HexBoard.GetNeutralHexes()
			where x.ControllingPlayerID == -1
			where context.IsValidSpawnPoint(context.CurrentTurn.ForceMajeurePlayer, x.HexCoord, null)
			select x);
			int count = (int)(neutralForcePopulationData.InitialSpawnsPerNeutralCanton * (float)array.Length);
			foreach (Hex hex in array.GetRandom(context.Random, count))
			{
				HexCoord location = context.HexBoard.ToRelativeHex(hex.HexCoord);
				context.SpawnLegionWithBehaviour(gamePieceStaticData, context.CurrentTurn.ForceMajeurePlayer, location, neutralForceTurnModuleStaticData);
			}
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x00034158 File Offset: 0x00032358
		public static void AssignInitialHexOwnership(this TurnState turn, GameDatabase database)
		{
			foreach (Hex hex in turn.HexBoard.Hexes)
			{
				hex.SetControllingPlayerID(-1);
			}
			foreach (int num in from t in turn.EnumeratePlayerStates(false, false)
			select t.Id)
			{
				GamePiece stronghold = turn.GetStronghold(num);
				if (stronghold != null)
				{
					HexCoord location = stronghold.Location;
					foreach (HexCoord coord in turn.HexBoard.GetNeighbours(location, true))
					{
						Hex hex2 = turn.HexBoard[coord];
						TerrainStaticData terrainStaticData;
						if (database.TryFindTerrainData(hex2, out terrainStaticData) && terrainStaticData.Capturable)
						{
							if (hex2.ControllingPlayerID == -1)
							{
								turn.HexBoard[coord].SetControllingPlayerID(num);
							}
							else
							{
								int controllingPlayerID = hex2.ControllingPlayerID;
							}
						}
					}
				}
			}
		}
	}
}
