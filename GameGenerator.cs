using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;
using LoG.Simulation.Extensions;
using Zenject;

namespace LoG
{
	// Token: 0x0200028F RID: 655
	public class GameGenerator
	{
		// Token: 0x06000CAD RID: 3245 RVA: 0x000320EC File Offset: 0x000302EC
		public GameState CreateNewGameState(GameGenerationParameters parameters)
		{
			ValueTuple<GameState, TurnProcessContext, GameGenerationContext> valueTuple = this.CreateGameState(parameters, this._database);
			this._container.InjectInstance(new TurnProcessor()).ProcessTurnZero(valueTuple.Item1);
			return valueTuple.Item1;
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x0003212C File Offset: 0x0003032C
		[return: TupleElementNames(new string[]
		{
			"gameState",
			"processContext",
			"generationContext"
		})]
		public ValueTuple<GameState, TurnProcessContext, GameGenerationContext> CreateGameState(GameGenerationParameters parameters, GameDatabase database)
		{
			BoardGenerationParameters boardGeneration = parameters.BoardGeneration;
			GameParam_Data mapPreset;
			if (parameters.BoardGeneration.MapPreset.IsEmpty())
			{
				mapPreset = database.Fetch<GameParam_Data>("GamePreset_InfernalPlains");
			}
			else
			{
				mapPreset = database.Fetch(parameters.BoardGeneration.MapPreset);
			}
			GameState gameState = new GameState();
			gameState.Guid = (parameters.GameId ?? Guid.NewGuid());
			gameState.Seed = parameters.Seed;
			gameState.GameType = parameters.GameType;
			gameState.Rules = new GameRules();
			gameState.Rules.GameDuration = parameters.GameDuration;
			gameState.Rules.TriggerGenericCannedMessages = (gameState.GameType != GameType.SingleplayerCampaign);
			TurnState turnState = gameState.AddTurn(new TurnState());
			turnState.RegentPlayerId = ((parameters.StartingRegentId < 0) ? turnState.Random.Next() : parameters.StartingRegentId) % parameters.Players.Count<PlayerGenerationParameters>();
			HexBoard board = turnState.HexBoard = new HexBoard(boardGeneration.BoardRows, boardGeneration.BoardColumns);
			TurnProcessContext turnProcessContext = new TurnProcessContext(gameState.Rules, turnState, database);
			GameGenerationContext context = new GameGenerationContext(board, parameters.Seed);
			turnState.Random = context.Rand;
			List<PlayerState> list = new List<PlayerState>();
			foreach (PlayerGenerationParameters playerGenerationParameters in parameters.Players)
			{
				ArchFiendStaticData archFiendStaticData = database.Fetch<ArchFiendStaticData>(playerGenerationParameters.ArchfiendId);
				PlayerState playerState = turnProcessContext.AddPlayerState(archFiendStaticData, playerGenerationParameters.Role, playerGenerationParameters.BaseOrderSlots, playerGenerationParameters.PlayFabId, playerGenerationParameters.PlatformDisplayName);
				if (playerState.Role != PlayerRole.Human)
				{
					playerState.AIDifficulty = playerGenerationParameters.Difficulty;
				}
				else
				{
					playerState.AIDifficulty = AIDifficulty.Hardest;
				}
				this.AddRelics(turnProcessContext, parameters, playerGenerationParameters, playerState, archFiendStaticData);
				list.Add(playerState);
			}
			turnState.InitializeRegencyOrder();
			this.SpawnPointsOfInterest(context, database, parameters, mapPreset);
			this.SpawnRivers(context, database, parameters, mapPreset);
			this.SpawnBridges(context, parameters, mapPreset);
			HexCoord hexCoord = IEnumerableExtensions.ToList<HexCoord>(from x in board.Hexes
			where x.Type == TerrainType.Plain
			select x into y
			select y.HexCoord).Random(context.Rand);
			turnProcessContext.SpawnPandemonium(hexCoord);
			context.AvailableHexes.Remove(hexCoord);
			GameGenerator.InvalidateNeighbours(context, hexCoord, 3);
			context.Structures.Add(hexCoord);
			Func<HexCoord, bool> <>9__2;
			Func<HexCoord, bool> <>9__3;
			foreach (PlayerState playerState2 in list)
			{
				playerState2.SpendablePrestige = parameters.StartingPrestige;
				IEnumerable<HexCoord> availableHexes = context.AvailableHexes;
				Func<HexCoord, bool> predicate;
				if ((predicate = <>9__2) == null)
				{
					predicate = (<>9__2 = ((HexCoord x) => context.Structures.All((HexCoord y) => board.ShortestDistance(x, y) >= 4)));
				}
				List<HexCoord> list2 = IEnumerableExtensions.ToList<HexCoord>(availableHexes.Where(predicate));
				if (list2.Count <= 0)
				{
					IEnumerable<HexCoord> availableHexes2 = context.AvailableHexes;
					Func<HexCoord, bool> predicate2;
					if ((predicate2 = <>9__3) == null)
					{
						predicate2 = (<>9__3 = ((HexCoord x) => context.Structures.All((HexCoord y) => board.ShortestDistance(x, y) >= 3)));
					}
					list2 = IEnumerableExtensions.ToList<HexCoord>(availableHexes2.Where(predicate2));
				}
				HexCoord hexCoord2 = list2.Random(context.Rand);
				turnProcessContext.SpawnStronghold(playerState2, hexCoord2);
				context.AvailableHexes.Remove(hexCoord2);
				GameGenerator.InvalidateNeighbours(context, hexCoord2, 4);
				context.Structures.Add(hexCoord2);
				GamePiece gamePiece = turnProcessContext.SpawnHostLegion(playerState2, HexCoord.Invalid);
				HexCoord hexCoord3;
				if (!LegionMovementProcessor.TryFindSpawnPointFor(turnProcessContext, playerState2, gamePiece, out hexCoord3))
				{
					hexCoord3 = IEnumerableExtensions.ToList<HexCoord>(board.GetNeighbours(hexCoord2, false)).Random(context.Rand);
				}
				gamePiece.Location = hexCoord3;
				turnProcessContext.RecalculateSupportModifiers(playerState2.Id);
				turnProcessContext.RecalculateAurasFromGamePiece(gamePiece);
				context.AvailableHexes.Remove(hexCoord3);
				playerState2.AIPersistentData.NewRandomDiplomaticCooldown(4);
			}
			int numPops = context.Rand.Next(boardGeneration.MinPlacesOfPower, boardGeneration.MaxPlacesOfPower);
			this.SpawnPlacesOfPower(turnProcessContext, context, numPops);
			foreach (ValueTuple<TerrainType, FeatureData> valueTuple in from terrainFrequency in this.GetFeatureFrequencies(database, mapPreset)
			orderby terrainFrequency.Item2.Frequency descending
			select terrainFrequency)
			{
				TerrainType item = valueTuple.Item1;
				FeatureData item2 = valueTuple.Item2;
				TerrainStaticData terrainData = database.GetTerrainData(item);
				if (terrainData != null && !this.TrySpawnFeatures(turnProcessContext, context, database, item2, terrainData))
				{
					ManuscriptStaticData manuscriptThatOverridesTerrainType = this.GetManuscriptThatOverridesTerrainType(turnProcessContext, terrainData);
					if (manuscriptThatOverridesTerrainType != null)
					{
						gameState.Rules.BlacklistedEntities.Add(new ConfigRef(manuscriptThatOverridesTerrainType));
					}
				}
			}
			GameGenerator.SetupVictoryConditions(gameState, turnState);
			return new ValueTuple<GameState, TurnProcessContext, GameGenerationContext>(gameState, turnProcessContext, context);
		}

		// Token: 0x06000CAF RID: 3247 RVA: 0x000326DC File Offset: 0x000308DC
		private ManuscriptStaticData GetManuscriptThatOverridesTerrainType(TurnProcessContext processContext, TerrainStaticData terrainStaticData)
		{
			TerrainType legacyType = terrainStaticData.LegacyType;
			string text;
			if (legacyType <= TerrainType.Swamp)
			{
				if (legacyType == TerrainType.Ravine)
				{
					text = "Manuscript_RavineTraining";
					goto IL_42;
				}
				if (legacyType == TerrainType.Swamp)
				{
					text = "Manuscript_SwampTraining";
					goto IL_42;
				}
			}
			else
			{
				if (legacyType == TerrainType.Lava)
				{
					text = "Manuscript_LavaTraining";
					goto IL_42;
				}
				if (legacyType == TerrainType.Vent)
				{
					text = "Manuscript_VentsTraining";
					goto IL_42;
				}
			}
			text = null;
			IL_42:
			string text2 = text;
			if (text2 != null)
			{
				return processContext.Database.Fetch<ManuscriptStaticData>(text2);
			}
			return null;
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x0003273E File Offset: 0x0003093E
		private IEnumerable<ValueTuple<TerrainType, FeatureData>> GetFeatureFrequencies(GameDatabase database, GameParam_Data mapPreset)
		{
			foreach (TerrainType terrainType in TerrainTypeExtensions.GetFeatures())
			{
				ConfigRef<FeatureData> key;
				FeatureData item;
				if (mapPreset.TryGetFeatureFrequencyData(terrainType, out key) && database.TryFetch<FeatureData>(key, out item))
				{
					yield return new ValueTuple<TerrainType, FeatureData>(terrainType, item);
				}
			}
			IEnumerator<TerrainType> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x00032758 File Offset: 0x00030958
		private void SpawnPointsOfInterest(GameGenerationContext context, GameDatabase database, GameGenerationParameters parameters, GameParam_Data mapPreset)
		{
			if (mapPreset.PointOfInterest.IsEmpty())
			{
				return;
			}
			PointOfInterestStaticData pointOfInterestStaticData = database.Fetch(mapPreset.PointOfInterest);
			MultiHexFeatureData hexFeature = (pointOfInterestStaticData != null) ? pointOfInterestStaticData.HexFeatures.Random(context.Rand) : null;
			if (hexFeature == null)
			{
				return;
			}
			HexCoord hexCoord = context.Board.Hexes.Random(context.Rand).HexCoord;
			HexUtils.HexOrientationFlags orientation = IEnumerableExtensions.ToArray<HexUtils.HexOrientationFlags>(from HexUtils.HexOrientationFlags c in Enum.GetValues(typeof(HexUtils.HexOrientationFlags))
			where hexFeature.Orientations.HasFlag(c)
			select c).Random(context.Rand);
			context.Board[hexCoord].SetTerrainType(pointOfInterestStaticData.LegacyType);
			List<HexCoord> list = new List<HexCoord>();
			list.Add(hexCoord);
			foreach (CubeCoord pos in hexFeature.OccupiedHexes)
			{
				CubeCoord cubeCoord = pos.RotateToOrientation2(orientation);
				CubeCoord cubeCoord2 = (CubeCoord)hexCoord;
				CubeCoord cubeCoord3 = cubeCoord + cubeCoord2;
				HexCoord hexCoord2 = (HexCoord)cubeCoord3;
				context.Board[hexCoord2].SetTerrainType(pointOfInterestStaticData.LegacyType);
				list.Add(hexCoord2);
			}
			GameGenerator.UpdateValidGenerationHexes(context, list);
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x000328B8 File Offset: 0x00030AB8
		private void AddRelics(TurnContext context, GameGenerationParameters gameParameters, PlayerGenerationParameters playerParameters, PlayerState playerState, ArchFiendStaticData archfiend)
		{
			List<string> relics = playerParameters.Relics;
			if (relics != null && relics.Count > 0)
			{
				foreach (string key in this._database.ReturnMaxValueValidatedRelicIds(playerParameters.Relics))
				{
					RelicStaticData relic;
					if (context.Database.TryFetch<RelicStaticData>(key, out relic))
					{
						context.AddRelic(playerState, relic);
					}
				}
				return;
			}
			if (gameParameters.FillEmptyRelicsWithDefaults)
			{
				IReadOnlyList<WeightedValue<ConfigRef<RelicSetStaticData>>> recommendedRelics = archfiend.RecommendedRelics;
				if (recommendedRelics != null && recommendedRelics.Count == 0)
				{
					return;
				}
				ConfigRef<RelicSetStaticData> key2 = archfiend.RecommendedRelics.SelectRandom(context.Random);
				RelicSetStaticData set;
				if (context.Database.TryFetch<RelicSetStaticData>(key2, out set))
				{
					context.AddRelicSet(playerState, set);
				}
			}
		}

		// Token: 0x06000CB3 RID: 3251 RVA: 0x00032990 File Offset: 0x00030B90
		public static void SetupVictoryConditions(GameState gameState, TurnState firstTurn)
		{
			GameRules rules = gameState.Rules;
			rules.VictoryRules = new List<VictoryRule>();
			rules.VictoryRules.Add(new VictoryRuleTrialOfTheThrone());
			rules.VictoryRules.Add(new VictoryRuleUsurp());
			rules.VictoryRules.Add(new VictoryRuleAbyss());
			firstTurn.InitializeVictoryRuleProcessors(rules);
		}

		// Token: 0x06000CB4 RID: 3252 RVA: 0x000329E8 File Offset: 0x00030BE8
		private void SpawnBridges(GameGenerationContext context, GameGenerationParameters parameters, GameParam_Data mapPreset)
		{
			GameGenerator.<>c__DisplayClass9_0 CS$<>8__locals1 = new GameGenerator.<>c__DisplayClass9_0();
			CS$<>8__locals1.context = context;
			foreach (HexIsland hexIsland in CS$<>8__locals1.context.Features)
			{
				if (hexIsland.TerrainTypeData.LegacyType == TerrainType.River)
				{
					int num = hexIsland.Hexes.Count / 7 + CS$<>8__locals1.context.Rand.Next(1, 3);
					GameGenerator.<>c__DisplayClass9_1 CS$<>8__locals2;
					CS$<>8__locals2.bridges = new List<HexCoord>();
					for (int i = 0; i < num; i++)
					{
						int num2 = CS$<>8__locals1.context.Rand.Next(0, hexIsland.Hexes.Count);
						for (int j = 0; j < hexIsland.Hexes.Count; j++)
						{
							HexCoord hexCoord = hexIsland.Hexes[num2];
							if (CS$<>8__locals1.<SpawnBridges>g__IsValidBridge|0(hexCoord, ref CS$<>8__locals2))
							{
								CS$<>8__locals1.context.Board[hexCoord].SetTerrainType(TerrainType.LandBridge);
								CS$<>8__locals2.bridges.Add(hexCoord);
								break;
							}
							num2++;
							if (num2 >= hexIsland.Hexes.Count)
							{
								num2 = 0;
							}
						}
					}
					GameGenerator.UpdateValidGenerationHexes(CS$<>8__locals1.context, new TerrainType[]
					{
						TerrainType.River,
						TerrainType.LandBridge
					});
				}
			}
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x00032B5C File Offset: 0x00030D5C
		private void SpawnRivers(GameGenerationContext context, GameDatabase database, GameGenerationParameters gameParameters, GameParam_Data mapPreset)
		{
			RiverPathfinder riverPathfinder = new RiverPathfinder(context);
			BoardGenerationParameters boardGeneration = gameParameters.BoardGeneration;
			RiverData riverData = database.Fetch(mapPreset.RiversPreset);
			int num = 0;
			while ((float)num < riverData.Frequency)
			{
				HexCoord hexCoord = IEnumerableExtensions.ToList<HexCoord>(from x in context.AvailableHexes
				where x.column == 0 || x.row == 0
				select x).Random(context.Rand);
				HexRiver hexRiver = new HexRiver();
				bool flag = hexCoord.row == 0;
				int windiness = riverData.Windiness;
				List<HexCoord> list = new List<HexCoord>();
				for (int i = 1; i <= riverData.NumberOfPoints; i++)
				{
					float num2 = (float)i / (float)(riverData.NumberOfPoints + 1);
					HexCoord hexCoord2 = flag ? new HexCoord((int)((float)context.Board.Rows * num2), hexCoord.column) : new HexCoord(hexCoord.row, (int)((float)context.Board.Columns * num2));
					if (windiness > 0)
					{
						int num3 = context.Rand.Next(1, windiness);
						int num4 = (context.Rand.Next(0, 1) == 0) ? -1 : 1;
						num3 *= num4;
						if (flag)
						{
							hexCoord2.column += num3;
							int num5 = 0;
							while (!context.Contains(hexCoord2))
							{
								if (num5 >= 10)
								{
									break;
								}
								num5++;
								hexCoord2.column += num4;
							}
						}
						else
						{
							hexCoord2.row += num3;
							int num6 = 0;
							while (!context.Contains(hexCoord2) && num6 < 10)
							{
								num6++;
								hexCoord2.row += num4;
							}
						}
					}
					list.Add(hexCoord2);
				}
				hexRiver.Add(hexCoord);
				list.Add(hexCoord);
				foreach (HexCoord hexCoord3 in list)
				{
					if (!riverPathfinder.TryExtendRiver(hexRiver, hexCoord3))
					{
						SimLogger logger = SimLogger.Logger;
						if (logger == null)
						{
							return;
						}
						logger.Warn(string.Format("River started from {0} could not be extended between {1} and {2}", hexCoord, hexRiver.Hexes.Last<HexCoord>(), hexCoord3));
						return;
					}
				}
				foreach (HexCoord coord in hexRiver.Hexes)
				{
					context.Board[coord].SetTerrainType(TerrainType.River);
				}
				hexRiver.TerrainTypeData = database.GetTerrainData(TerrainType.River);
				context.Features.Add(hexRiver);
				hexRiver.Connected = true;
				GameGenerator.UpdateValidGenerationHexes(context, new TerrainType[]
				{
					TerrainType.River
				});
				num++;
			}
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x00032E2C File Offset: 0x0003102C
		private void SpawnPlacesOfPower(TurnProcessContext processContext, GameGenerationContext context, int numPops)
		{
			GameGeneratorPathfinder pathfinder = new GameGeneratorPathfinder(context);
			List<GamePieceStaticData> source = IEnumerableExtensions.ToList<GamePieceStaticData>(processContext.EnumerateSpawnablePoPs());
			List<GamePieceStaticData> list = IEnumerableExtensions.ToList<GamePieceStaticData>((from p in source
			where p._level <= 3
			select p).Shuffle(context.Rand));
			int num = 0;
			List<GamePieceStaticData> list2 = IEnumerableExtensions.ToList<GamePieceStaticData>((from p in source
			where p._level > 3
			select p).Shuffle(context.Rand));
			int num2 = 0;
			TurnState turn = processContext.CurrentTurn;
			List<GamePiece> allPersonalGuards = IEnumerableExtensions.ToList<GamePiece>(from player in turn.EnumeratePlayerStatesInTurnOrder(false, false)
			select turn.FetchGameItem<GamePiece>(player.PersonalGuardId));
			HexBoard board = context.Board;
			List<GamePiece> list3 = IEnumerableExtensions.ToList<GamePiece>(allPersonalGuards);
			Func<HexCoord, bool> <>9__3;
			Func<HexCoord, bool> <>9__4;
			while (list3.Count > 0)
			{
				IEnumerable<HexCoord> availableHexes = context.AvailableHexes;
				Func<HexCoord, bool> predicate;
				if ((predicate = <>9__3) == null)
				{
					predicate = (<>9__3 = ((HexCoord hexCoord) => pathfinder.DistanceToClosest(hexCoord, allPersonalGuards) >= 3));
				}
				IEnumerable<HexCoord> source2 = availableHexes.Where(predicate);
				Func<HexCoord, bool> predicate2;
				if ((predicate2 = <>9__4) == null)
				{
					predicate2 = (<>9__4 = ((HexCoord hexCoord) => !board.GetNeighbours(hexCoord, true).Any(new Func<HexCoord, bool>(context.Structures.Contains))));
				}
				List<HexCoord> list4 = IEnumerableExtensions.ToList<HexCoord>(source2.Where(predicate2));
				if (list4.Count == 0)
				{
					break;
				}
				GamePiece personalGuard = ListExtensions.PopLast<GamePiece>(list3);
				list4.Shuffle<HexCoord>();
				HexCoord hexCoord3 = list4.SelectMaxOrDefault((HexCoord hexCoord) => pathfinder.DistanceToClosest(hexCoord, context.Structures) - 2 * pathfinder.GetDistance(hexCoord, personalGuard.Location), HexCoord.Invalid);
				if (!hexCoord3.IsValid)
				{
					break;
				}
				GamePieceStaticData gamePieceStaticData = null;
				if (list.Count > 0)
				{
					num++;
					gamePieceStaticData = ListExtensions.PopLast<GamePieceStaticData>(list);
				}
				else if (list2.Count > 0)
				{
					num2++;
					gamePieceStaticData = ListExtensions.PopLast<GamePieceStaticData>(list2);
				}
				if (gamePieceStaticData == null)
				{
					break;
				}
				processContext.SpawnFixture(gamePieceStaticData, null, hexCoord3);
				context.Structures.Add(hexCoord3);
				context.AvailableHexes.Remove(hexCoord3);
				GameGenerator.InvalidateNeighbours(context, hexCoord3, 3);
				List<HexCoord> collection;
				if (pathfinder.TryFindPath(hexCoord3, personalGuard.Location, out collection))
				{
					context.PathsFromPlayersToStructures.AddRange(collection);
				}
				int i = 0;
				while (i < list3.Count)
				{
					GamePiece gamePiece = list3[i];
					if (pathfinder.GetDistance(hexCoord3, gamePiece.Location) <= 3)
					{
						list3.RemoveAt(i);
						List<HexCoord> collection2;
						if (pathfinder.TryFindPath(hexCoord3, personalGuard.Location, out collection2))
						{
							context.PathsFromPlayersToStructures.AddRange(collection2);
						}
					}
					else
					{
						i++;
					}
				}
			}
			Func<HexCoord, bool> <>9__6;
			Func<HexCoord, int> <>9__7;
			for (int j = num + num2; j < numPops; j++)
			{
				IEnumerable<HexCoord> availableHexes2 = context.AvailableHexes;
				Func<HexCoord, bool> predicate3;
				if ((predicate3 = <>9__6) == null)
				{
					predicate3 = (<>9__6 = ((HexCoord hexCoord) => !board.GetNeighbours(hexCoord, true).Any(new Func<HexCoord, bool>(context.Structures.Contains))));
				}
				List<HexCoord> source3 = IEnumerableExtensions.ToList<HexCoord>(availableHexes2.Where(predicate3));
				source3.Shuffle<HexCoord>();
				Func<HexCoord, int> @delegate;
				if ((@delegate = <>9__7) == null)
				{
					@delegate = (<>9__7 = ((HexCoord hexCoord) => pathfinder.DistanceToClosest(hexCoord, context.Structures)));
				}
				HexCoord hexCoord2 = source3.SelectMaxOrDefault(@delegate, HexCoord.Invalid);
				if (!hexCoord2.IsValid)
				{
					break;
				}
				bool flag = num <= num2 && list.Count > 0;
				GamePieceStaticData gamePieceStaticData2 = null;
				if (flag)
				{
					gamePieceStaticData2 = ListExtensions.PopLast<GamePieceStaticData>(list);
					num++;
				}
				else if (list2.Count > 0)
				{
					gamePieceStaticData2 = ListExtensions.PopLast<GamePieceStaticData>(list2);
					num2++;
				}
				if (gamePieceStaticData2 == null)
				{
					break;
				}
				processContext.SpawnFixture(gamePieceStaticData2, null, hexCoord2);
				context.Structures.Add(hexCoord2);
				context.AvailableHexes.Remove(hexCoord2);
				GameGenerator.InvalidateNeighbours(context, hexCoord2, 2);
			}
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x00033284 File Offset: 0x00031484
		private List<HexCoord> FilterFeatureCandidates(TurnProcessContext processContext, GameDatabase database, GameGenerationContext generationContext, IEnumerable<HexCoord> candidates, TerrainStaticData terrainTypeData, HexIsland island = null)
		{
			HexBoard board = generationContext.Board;
			bool impassible = terrainTypeData.MoveCost == MoveCostType.NonTraversible;
			bool damageDealing = terrainTypeData.LegacyType == TerrainType.Lava;
			bool teleportBlocking = terrainTypeData.LegacyType == TerrainType.Ruin;
			return IEnumerableExtensions.ToList<HexCoord>(from hex in candidates
			where !impassible || !generationContext.PathsFromPlayersToStructures.Contains(hex)
			where board.GetNeighbours(hex, false).All(delegate(HexCoord neighbour)
			{
				TerrainStaticData terrainStaticData;
				if (!database.TryFindTerrainData(board[neighbour], out terrainStaticData))
				{
					return false;
				}
				if (terrainStaticData.LegacyType == terrainTypeData.LegacyType)
				{
					return hex == neighbour || (island != null && island.Hexes.Contains(neighbour));
				}
				if (impassible && terrainStaticData.MoveCost == MoveCostType.NonTraversible)
				{
					return false;
				}
				if (damageDealing | teleportBlocking)
				{
					GamePiece gamePieceAt = processContext.CurrentTurn.GetGamePieceAt(neighbour);
					if (gamePieceAt != null && gamePieceAt.IsFixture())
					{
						if (teleportBlocking)
						{
							return false;
						}
						if (damageDealing && gamePieceAt.SubCategory == GamePieceCategory.Stronghold)
						{
							return false;
						}
					}
				}
				return true;
			})
			select hex);
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x00033334 File Offset: 0x00031534
		private bool TrySpawnFeatures(TurnProcessContext processContext, GameGenerationContext context, GameDatabase database, FeatureData featureData, TerrainStaticData terrainTypeData)
		{
			float frequency = featureData.Frequency;
			TerrainType legacyType = terrainTypeData.LegacyType;
			if (context.AvailableHexes.Count == 0)
			{
				return false;
			}
			int num = (int)((float)context.AvailableHexes.Count * frequency);
			float num2 = (float)(terrainTypeData.PatchMinSize + terrainTypeData.PatchMaxSize) / 2f;
			int count = (int)MathF.Ceiling((float)num / num2);
			List<HexIsland> list = IEnumerableExtensions.ToList<HexIsland>(this.CreateFeatureIslands(processContext, context, database, terrainTypeData, count));
			if (list.Count == 0)
			{
				return false;
			}
			list.ShuffleContents(context.Rand);
			int maxSize = Math.Min(terrainTypeData.PatchMinSize, num / list.Count);
			int num3 = this.CompleteHexIslands(processContext, context, database, list, maxSize);
			num -= num3;
			List<HexIsland> list2 = IEnumerableExtensions.ToList<HexIsland>(from island in list
			where island.SuccessfullyCompleted && island.CanExpandFurther
			select island);
			list2.ShuffleContents(context.Rand);
			int num4 = this.ExpandHexIslands(processContext, context, database, list2, num);
			num -= num4;
			return true;
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x00033437 File Offset: 0x00031637
		private IEnumerable<HexIsland> CreateFeatureIslands(TurnProcessContext processContext, GameGenerationContext context, GameDatabase database, TerrainStaticData terrainTypeData, int count)
		{
			HexBoard board = context.Board;
			TerrainType featureType = terrainTypeData.LegacyType;
			List<HexCoord> validHexes = this.FilterFeatureCandidates(processContext, database, context, context.AvailableHexes, terrainTypeData, null);
			int i = 0;
			while (i < count && validHexes.Count > 0)
			{
				HexCoord hexCoord = validHexes.Random(context.Rand);
				board[hexCoord].SetTerrainType(featureType);
				context.AvailableHexes.Remove(hexCoord);
				foreach (HexCoord item in board.GetNeighbours(hexCoord, true))
				{
					validHexes.Remove(item);
				}
				yield return new HexIsland
				{
					Hexes = 
					{
						hexCoord
					},
					TerrainTypeData = terrainTypeData
				};
				int num = i;
				i = num + 1;
			}
			yield break;
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x0003346C File Offset: 0x0003166C
		private int CompleteHexIslands(TurnProcessContext processContext, GameGenerationContext context, GameDatabase database, IEnumerable<HexIsland> islandsToComplete, int maxSize)
		{
			int num = 0;
			foreach (HexIsland hexIsland in islandsToComplete)
			{
				this.ExpandHexIsland(processContext, context, database, context.Board, hexIsland, maxSize);
				TerrainType legacyType = hexIsland.TerrainTypeData.LegacyType;
				if (hexIsland.SuccessfullyCompleted)
				{
					context.Features.Add(hexIsland);
					using (List<HexCoord>.Enumerator enumerator2 = hexIsland.Hexes.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							HexCoord hexCoord = enumerator2.Current;
							context.Board[hexCoord].SetTerrainType(legacyType);
							context.AvailableHexes.Remove(hexCoord);
							num++;
						}
						continue;
					}
				}
				foreach (HexCoord hexCoord2 in hexIsland.Hexes)
				{
					context.Board[hexCoord2].SetTerrainType(TerrainType.Plain);
					context.AvailableHexes.Add(hexCoord2);
				}
			}
			return num;
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x000335AC File Offset: 0x000317AC
		private int ExpandHexIslands(TurnProcessContext processContext, GameGenerationContext context, GameDatabase database, List<HexIsland> islandsToComplete, int maxTilesToAdd)
		{
			int num = 0;
			while (num < maxTilesToAdd && islandsToComplete.Count > 0)
			{
				int index = context.Rand.Next(0, islandsToComplete.Count - 1);
				HexIsland hexIsland = islandsToComplete[index];
				if (this.ExpandHexIsland(processContext, context, database, context.Board, hexIsland, hexIsland.Size + 1) != 0)
				{
					List<HexCoord> hexes = hexIsland.Hexes;
					int index2 = hexes.Count - 1;
					HexCoord hexCoord = hexes[index2];
					context.Board[hexCoord].SetTerrainType(hexIsland.TerrainTypeData.LegacyType);
					context.AvailableHexes.Remove(hexCoord);
					num++;
				}
				else
				{
					islandsToComplete.RemoveAt(index);
				}
			}
			return num;
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x0003365C File Offset: 0x0003185C
		private int ExpandHexIsland(TurnProcessContext processContext, GameGenerationContext context, GameDatabase database, HexBoard board, HexIsland island, int maxSize = -1)
		{
			if (maxSize < 0)
			{
				maxSize = island.TerrainTypeData.PatchMaxSize;
			}
			int num = maxSize - island.Size;
			Func<HexCoord, bool> <>9__0;
			Func<HexCoord, bool> <>9__1;
			Func<HexCoord, bool> <>9__6;
			Predicate<HexCoord> <>9__2;
			Func<HexCoord, bool> <>9__7;
			Predicate<HexCoord> <>9__3;
			Func<HexCoord, bool> <>9__4;
			Func<HexCoord, bool> <>9__8;
			Predicate<HexCoord> <>9__5;
			int i;
			for (i = 0; i < num; i++)
			{
				if (!island.CanExpandFurther)
				{
					return i;
				}
				IslandMethod islandMethod = (island.Size < 2) ? IslandMethod.Random : island.TerrainTypeData.PatchExpansionMethod;
				if (island.TerrainTypeData.LegacyType == TerrainType.Ravine && island.Size > 1)
				{
					islandMethod = IslandMethod.Strips;
				}
				List<HexCoord> list = null;
				switch (islandMethod)
				{
				case IslandMethod.Strips:
				{
					List<HexCoord> list2 = IEnumerableExtensions.ToList<HexCoord>(island.Hexes.Skip(island.Hexes.Count - 2));
					List<HexCoord> list3 = list2;
					Predicate<HexCoord> match;
					if ((match = <>9__3) == null)
					{
						match = (<>9__3 = delegate(HexCoord x)
						{
							IEnumerable<HexCoord> neighbours4 = board.GetNeighbours(x, false);
							Func<HexCoord, bool> predicate4;
							if ((predicate4 = <>9__7) == null)
							{
								predicate4 = (<>9__7 = ((HexCoord y) => island.Hexes.Contains(y)));
							}
							return neighbours4.Count(predicate4) > 1;
						});
					}
					list3.RemoveAll(match);
					if (list2.Count <= 0)
					{
						return i;
					}
					HexCoord hexCoord = list2.Random(context.Rand);
					IEnumerable<HexCoord> neighbours = board.GetNeighbours(hexCoord, false);
					Func<HexCoord, bool> predicate;
					if ((predicate = <>9__4) == null)
					{
						predicate = (<>9__4 = ((HexCoord x) => context.AvailableHexes.Contains(x)));
					}
					list = IEnumerableExtensions.ToList<HexCoord>(neighbours.Where(predicate));
					List<HexCoord> list4 = list;
					Predicate<HexCoord> match2;
					if ((match2 = <>9__5) == null)
					{
						match2 = (<>9__5 = delegate(HexCoord x)
						{
							IEnumerable<HexCoord> neighbours4 = board.GetNeighbours(x, false);
							Func<HexCoord, bool> predicate4;
							if ((predicate4 = <>9__8) == null)
							{
								predicate4 = (<>9__8 = ((HexCoord y) => island.Hexes.Contains(y)));
							}
							return neighbours4.Count(predicate4) > 1;
						});
					}
					list4.RemoveAll(match2);
					list = this.FilterFeatureCandidates(processContext, database, context, list, island.TerrainTypeData, island);
					if (list.Count <= 0)
					{
						island.DeadEnds.Add(hexCoord);
						continue;
					}
					break;
				}
				case IslandMethod.Bunching:
				{
					HexCoord hexCoord = island.Hexes.Random(context.Rand);
					IEnumerable<HexCoord> neighbours2 = board.GetNeighbours(hexCoord, false);
					Func<HexCoord, bool> predicate2;
					if ((predicate2 = <>9__1) == null)
					{
						predicate2 = (<>9__1 = ((HexCoord x) => context.AvailableHexes.Contains(x)));
					}
					list = IEnumerableExtensions.ToList<HexCoord>(neighbours2.Where(predicate2));
					List<HexCoord> list5 = list;
					Predicate<HexCoord> match3;
					if ((match3 = <>9__2) == null)
					{
						match3 = (<>9__2 = delegate(HexCoord x)
						{
							IEnumerable<HexCoord> neighbours4 = board.GetNeighbours(x, false);
							Func<HexCoord, bool> predicate4;
							if ((predicate4 = <>9__6) == null)
							{
								predicate4 = (<>9__6 = ((HexCoord y) => island.Hexes.Contains(y)));
							}
							return neighbours4.Count(predicate4) < 2;
						});
					}
					list5.RemoveAll(match3);
					list = this.FilterFeatureCandidates(processContext, database, context, list, island.TerrainTypeData, island);
					if (list.Count <= 0)
					{
						island.DeadEnds.Add(hexCoord);
						continue;
					}
					break;
				}
				case IslandMethod.Random:
				{
					HexCoord hexCoord = island.Hexes.Random(context.Rand);
					IEnumerable<HexCoord> neighbours3 = board.GetNeighbours(hexCoord, false);
					Func<HexCoord, bool> predicate3;
					if ((predicate3 = <>9__0) == null)
					{
						predicate3 = (<>9__0 = ((HexCoord x) => context.AvailableHexes.Contains(x)));
					}
					list = IEnumerableExtensions.ToList<HexCoord>(neighbours3.Where(predicate3));
					list = this.FilterFeatureCandidates(processContext, database, context, list, island.TerrainTypeData, island);
					if (list.Count <= 0)
					{
						island.DeadEnds.Add(hexCoord);
						continue;
					}
					break;
				}
				}
				HexCoord hexCoord2 = list.Random(context.Rand);
				context.AvailableHexes.Remove(hexCoord2);
				island.Add(hexCoord2);
			}
			return i;
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x000339E0 File Offset: 0x00031BE0
		private static void InvalidateNeighbours(GameGenerationContext context, HexCoord hex, int neighboursToInvalidate = 6)
		{
			foreach (HexCoord item in context.Board.GetNeighbours(hex, false).Shuffle(context.Rand))
			{
				if (context.AvailableHexes.Contains(item))
				{
					context.AvailableHexes.Remove(item);
					if (--neighboursToInvalidate == 0)
					{
						break;
					}
				}
			}
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x00033A60 File Offset: 0x00031C60
		private static void UpdateValidGenerationHexes(GameGenerationContext context, IEnumerable<HexCoord> hexes)
		{
			foreach (HexCoord centre in hexes)
			{
				foreach (HexCoord item in context.Board.GetNeighbours(centre, true))
				{
					context.AvailableHexes.Remove(item);
				}
			}
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x00033AEC File Offset: 0x00031CEC
		private static void UpdateValidGenerationHexes(GameGenerationContext context, params TerrainType[] types)
		{
			IEnumerable<Hex> hexes = context.Board.Hexes;
			Func<Hex, bool> <>9__0;
			Func<Hex, bool> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = ((Hex x) => IEnumerableExtensions.Contains<TerrainType>(types, x.Type)));
			}
			foreach (Hex hex in hexes.Where(predicate))
			{
				foreach (HexCoord item in context.Board.GetNeighbours(hex.HexCoord, true))
				{
					context.AvailableHexes.Remove(item);
				}
			}
		}

		// Token: 0x040005B6 RID: 1462
		[Inject]
		private DiContainer _container;

		// Token: 0x040005B7 RID: 1463
		[Inject]
		private GameDatabase _database;
	}
}
