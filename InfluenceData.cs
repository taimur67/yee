using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Game.StaticData;
using LoG.Simulation.Extensions;

namespace LoG
{
	// Token: 0x020001A0 RID: 416
	public class InfluenceData
	{
		// Token: 0x0600079C RID: 1948 RVA: 0x00023600 File Offset: 0x00021800
		public bool TryGetPylonDesirability(out float pylonDesirability)
		{
			return (pylonDesirability = this._pylonDesirability) >= 0f;
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x00023624 File Offset: 0x00021824
		public float PylonDesirabilityOrDefault(float defaultValue)
		{
			float result;
			if (this.TryGetPylonDesirability(out result))
			{
				return result;
			}
			return defaultValue;
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x00023640 File Offset: 0x00021840
		public bool TryGetEntranceCount(out int entranceCount)
		{
			return (entranceCount = this._entranceCount) >= 0;
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x00023660 File Offset: 0x00021860
		public int EntranceCountOrDefault(int defaultValue)
		{
			int result;
			if (this.TryGetEntranceCount(out result))
			{
				return result;
			}
			return defaultValue;
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x0002367C File Offset: 0x0002187C
		public bool TryGetNearestFixtureDistance(out int nearestFixtureDistance)
		{
			return (nearestFixtureDistance = this._nearestFixtureDistance) >= 0;
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x0002369C File Offset: 0x0002189C
		public int NearestFixtureDistanceOrDefault(int defaultValue)
		{
			int result;
			if (this.TryGetNearestFixtureDistance(out result))
			{
				return result;
			}
			return defaultValue;
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x000236B8 File Offset: 0x000218B8
		public bool TryGetSumOfAllSquaredFixtureDistances(out float sumOfDist2ToFixtures)
		{
			return (sumOfDist2ToFixtures = this._sumOfAllSquaredFixtureDistances) >= 0f;
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x000236DC File Offset: 0x000218DC
		public float SumOfAllSquaredFixtureDistancesOrDefault(float defaultValue)
		{
			float result;
			if (this.TryGetSumOfAllSquaredFixtureDistances(out result))
			{
				return result;
			}
			return defaultValue;
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x000236F8 File Offset: 0x000218F8
		public bool TryGetCentrality(out float centrality)
		{
			return (centrality = this._centrality) >= 0f;
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x0002371C File Offset: 0x0002191C
		public float CentralityOrDefault(float defaultValue)
		{
			float result;
			if (this.TryGetCentrality(out result))
			{
				return result;
			}
			return defaultValue;
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x00023738 File Offset: 0x00021938
		public bool TryGetCongestion(out float congestion)
		{
			return (congestion = this._congestion) >= 0f;
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x0002375C File Offset: 0x0002195C
		public float CongestionOrDefault(float defaultValue)
		{
			float result;
			if (this.TryGetCongestion(out result))
			{
				return result;
			}
			return defaultValue;
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x00023776 File Offset: 0x00021976
		public bool IsAdjacentToPlayer(int playerID)
		{
			return this._adjacentToPlayer.Contains(playerID);
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x00023784 File Offset: 0x00021984
		public bool TryGetControl(int playerID, out float controlValue)
		{
			controlValue = 0f;
			InfluenceData.PlayerData playerData;
			if (!this._dataPerPlayer.TryGetValue(playerID, out playerData))
			{
				return false;
			}
			controlValue = playerData.ControlValue;
			return true;
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x000237B4 File Offset: 0x000219B4
		public float ControlOrDefault(int playerID, float defaultValue)
		{
			float result;
			if (this.TryGetControl(playerID, out result))
			{
				return result;
			}
			return defaultValue;
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x000237D0 File Offset: 0x000219D0
		public bool TryGetCohesionContribution(int playerID, out float cohesionContribution)
		{
			cohesionContribution = 0f;
			InfluenceData.PlayerData playerData;
			if (!this._dataPerPlayer.TryGetValue(playerID, out playerData))
			{
				return false;
			}
			cohesionContribution = playerData.CohesionContribution;
			return true;
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x00023800 File Offset: 0x00021A00
		public float CohesionContributionOrDefault(int playerID, float defaultValue)
		{
			float result;
			if (this.TryGetCohesionContribution(playerID, out result))
			{
				return result;
			}
			return defaultValue;
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x0002381C File Offset: 0x00021A1C
		public bool TryGetTurnsToReach(Identifier gamePieceID, out int turnsToReach, bool ignoreDiplomacyWithOwner = false)
		{
			turnsToReach = int.MaxValue;
			InfluenceData.LegionDistanceData legionDistanceData;
			if ((ignoreDiplomacyWithOwner ? this._reachableIfOwnerTerritoryCouldBeCrossedLegionData : this._reachableLegionData).TryGetValue(gamePieceID, out legionDistanceData))
			{
				turnsToReach = legionDistanceData.TurnsToReach;
				return true;
			}
			return false;
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x00023858 File Offset: 0x00021A58
		public bool TryGetShortestPathLength(Identifier gamePieceID, out int pathLength, bool ignoreDiplomacyWithOwner = false)
		{
			pathLength = int.MaxValue;
			InfluenceData.LegionDistanceData legionDistanceData;
			if ((ignoreDiplomacyWithOwner ? this._reachableIfOwnerTerritoryCouldBeCrossedLegionData : this._reachableLegionData).TryGetValue(gamePieceID, out legionDistanceData))
			{
				pathLength = legionDistanceData.ShortestPathLength;
				return true;
			}
			return false;
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x00023894 File Offset: 0x00021A94
		public bool CanReach(Identifier gamePieceID, bool ignoreDiplomacyWithOwner = false)
		{
			int num;
			return this.TryGetTurnsToReach(gamePieceID, out num, ignoreDiplomacyWithOwner);
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x000238AC File Offset: 0x00021AAC
		public float TurnsToReachOrDefault(Identifier gamePieceID, float defaultValue, bool ignoreDiplomacyWithOwner = false)
		{
			int num;
			if (this.TryGetTurnsToReach(gamePieceID, out num, ignoreDiplomacyWithOwner))
			{
				return (float)num;
			}
			return defaultValue;
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x000238CC File Offset: 0x00021ACC
		public bool TryGetSpawnProximity(int playerID, out float spawnProximity)
		{
			spawnProximity = 0f;
			InfluenceData.PlayerData playerData;
			if (!this._dataPerPlayer.TryGetValue(playerID, out playerData))
			{
				return false;
			}
			spawnProximity = playerData.SpawnProximity;
			return true;
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x000238FC File Offset: 0x00021AFC
		public float SpawnProximityOrDefault(int playerID, float defaultValue)
		{
			float result;
			if (this.TryGetSpawnProximity(playerID, out result))
			{
				return result;
			}
			return defaultValue;
		}

		// Token: 0x0400037A RID: 890
		public const int InvalidData = -1;

		// Token: 0x0400037B RID: 891
		private int _entranceCount = -1;

		// Token: 0x0400037C RID: 892
		private int _nearestFixtureDistance = -1;

		// Token: 0x0400037D RID: 893
		private float _sumOfAllSquaredFixtureDistances = -1f;

		// Token: 0x0400037E RID: 894
		private float _centrality = -1f;

		// Token: 0x0400037F RID: 895
		private float _congestion = -1f;

		// Token: 0x04000380 RID: 896
		private float _pylonDesirability = -1f;

		// Token: 0x04000381 RID: 897
		private Dictionary<int, InfluenceData.PlayerData> _dataPerPlayer = new Dictionary<int, InfluenceData.PlayerData>();

		// Token: 0x04000382 RID: 898
		private readonly Dictionary<Identifier, InfluenceData.LegionDistanceData> _reachableLegionData = new Dictionary<Identifier, InfluenceData.LegionDistanceData>();

		// Token: 0x04000383 RID: 899
		private readonly Dictionary<Identifier, InfluenceData.LegionDistanceData> _reachableIfOwnerTerritoryCouldBeCrossedLegionData = new Dictionary<Identifier, InfluenceData.LegionDistanceData>();

		// Token: 0x04000384 RID: 900
		private HashSet<int> _adjacentToPlayer = new HashSet<int>();

		// Token: 0x0200086C RID: 2156
		public class GamePieceDistance
		{
			// Token: 0x060027E3 RID: 10211 RVA: 0x00083BFF File Offset: 0x00081DFF
			public void Record(Identifier id, int distance)
			{
				this.Distances[id] = distance;
				if (distance < this.MinimumDistance)
				{
					this.MinimumDistance = distance;
				}
				this.SumOfSquaredDistances += (float)(distance * distance);
			}

			// Token: 0x060027E4 RID: 10212 RVA: 0x00083C2F File Offset: 0x00081E2F
			public int GetMinimumDistance()
			{
				if (this.MinimumDistance == 2147483647)
				{
					return -1;
				}
				return this.MinimumDistance;
			}

			// Token: 0x060027E5 RID: 10213 RVA: 0x00083C46 File Offset: 0x00081E46
			public float SumOfSquaredDistancesToGamePieces()
			{
				return this.SumOfSquaredDistances;
			}

			// Token: 0x040011F1 RID: 4593
			public readonly Dictionary<Identifier, int> Distances = new Dictionary<Identifier, int>();

			// Token: 0x040011F2 RID: 4594
			public int MinimumDistance = int.MaxValue;

			// Token: 0x040011F3 RID: 4595
			public float SumOfSquaredDistances;
		}

		// Token: 0x0200086D RID: 2157
		public class GamePieceFloodFillSolver
		{
			// Token: 0x170005B1 RID: 1457
			// (get) Token: 0x060027E7 RID: 10215 RVA: 0x00083C6C File Offset: 0x00081E6C
			private TurnState _turnState
			{
				get
				{
					return this._context.CurrentTurn;
				}
			}

			// Token: 0x060027E8 RID: 10216 RVA: 0x00083C79 File Offset: 0x00081E79
			public Dictionary<HexCoord, InfluenceData.GamePieceDistance> DoAllGamePieceDistancesFloodFillIgnoringAllDiplomacy(TurnContext context, IEnumerable<GamePiece> gamePieces)
			{
				return this.DoAllGamePieceDistancesFloodFill(context, gamePieces, IEnumerableExtensions.ToList<int>(from p in context.CurrentTurn.EnumeratePlayerStates(true, false)
				select p.Id));
			}

			// Token: 0x060027E9 RID: 10217 RVA: 0x00083CBC File Offset: 0x00081EBC
			public Dictionary<HexCoord, InfluenceData.GamePieceDistance> DoAllGamePieceDistancesFloodFill(TurnContext context, IEnumerable<GamePiece> gamePieces, List<int> ignoreDiplomacyWithPlayers = null)
			{
				this._context = context;
				foreach (Hex hex in this._turnState.HexBoard.Hexes)
				{
					this._distances.Add(hex.HexCoord, new InfluenceData.GamePieceDistance());
				}
				HashSet<HexCoord> hashSet = this.FindIgnorableHexes(context);
				foreach (GamePiece gamePiece in gamePieces)
				{
					this._oldPerimeter.Clear();
					this._perimeter.Clear();
					this._currentGamePiece = gamePiece;
					this._closedList.Clear();
					CollectionExtensions.AddRange<HexCoord>(this._closedList, hashSet);
					this._perimeter.Add(gamePiece.Location);
					this.DoFloodFill(ignoreDiplomacyWithPlayers);
				}
				return this._distances;
			}

			// Token: 0x060027EA RID: 10218 RVA: 0x00083DC0 File Offset: 0x00081FC0
			private HashSet<HexCoord> FindIgnorableHexes(TurnContext turnContext)
			{
				return (from hex in turnContext.HexBoard.Hexes
				where !this._context.IsCapturableTileType(hex)
				select hex into t
				select t.HexCoord).ToHashSet<HexCoord>();
			}

			// Token: 0x060027EB RID: 10219 RVA: 0x00083E14 File Offset: 0x00082014
			private void AdvancePerimeter(List<int> ignoreDiplomacyWithPlayers, int iteration)
			{
				this._oldPerimeter.Clear();
				CollectionExtensions.AddRange<HexCoord>(this._oldPerimeter, this._perimeter);
				this._perimeter.Clear();
				foreach (HexCoord coord in this._oldPerimeter)
				{
					if (this._turnState.HexBoard.TryGetNeighboursNormalized(coord, this._neighbourBuffer))
					{
						foreach (HexCoord hexCoord in this._neighbourBuffer)
						{
							if (!this._closedList.Contains(hexCoord) && !this._oldPerimeter.Contains(hexCoord))
							{
								bool flag = false;
								if (LegionMovementProcessor.CanEnterCanton(this._context, this._currentGamePiece, hexCoord, PathMode.March, ignoreDiplomacyWithPlayers))
								{
									flag = true;
									GamePiece gamePiece;
									if (LegionMovementProcessor.IsOccupied(this._context.CurrentTurn, hexCoord, out gamePiece) && gamePiece.IsFixture() && gamePiece.ControllingPlayerId != this._currentGamePiece.ControllingPlayerId)
									{
										this.RecordDistance(hexCoord, this._currentGamePiece.Id, iteration + 1);
										flag = false;
									}
								}
								if (flag)
								{
									this._perimeter.Add(hexCoord);
								}
								else
								{
									this._closedList.Add(hexCoord);
								}
							}
						}
					}
				}
			}

			// Token: 0x060027EC RID: 10220 RVA: 0x00083F90 File Offset: 0x00082190
			private void DoFloodFill(List<int> ignoreDiplomacyWithPlayers = null)
			{
				int num = 0;
				while (num < this._timeoutMaxAttempts && this._perimeter.Count != 0)
				{
					foreach (HexCoord location in this._perimeter)
					{
						this.RecordDistance(location, this._currentGamePiece.Id, num);
					}
					this.AdvancePerimeter(ignoreDiplomacyWithPlayers, num);
					num++;
				}
			}

			// Token: 0x060027ED RID: 10221 RVA: 0x00084018 File Offset: 0x00082218
			private void RecordDistance(HexCoord location, Identifier fixtureID, int distance)
			{
				location = this._context.HexBoard.ToRelativeHex(location);
				this._distances[location].Record(fixtureID, distance);
				this._closedList.Add(location);
			}

			// Token: 0x060027EE RID: 10222 RVA: 0x00084050 File Offset: 0x00082250
			public float FindLargestSumDistanceToFixturesValue()
			{
				float num = 0f;
				foreach (KeyValuePair<HexCoord, InfluenceData.GamePieceDistance> keyValuePair in this._distances)
				{
					float num2 = keyValuePair.Value.SumOfSquaredDistancesToGamePieces();
					if (num2 > num)
					{
						num = num2;
					}
				}
				return num;
			}

			// Token: 0x040011F4 RID: 4596
			private TurnContext _context;

			// Token: 0x040011F5 RID: 4597
			private readonly HashSet<HexCoord> _closedList = new HashSet<HexCoord>();

			// Token: 0x040011F6 RID: 4598
			private readonly HashSet<HexCoord> _perimeter = new HashSet<HexCoord>();

			// Token: 0x040011F7 RID: 4599
			private readonly HashSet<HexCoord> _oldPerimeter = new HashSet<HexCoord>();

			// Token: 0x040011F8 RID: 4600
			private HexCoord[] _neighbourBuffer = new HexCoord[6];

			// Token: 0x040011F9 RID: 4601
			private Dictionary<HexCoord, InfluenceData.GamePieceDistance> _distances = new Dictionary<HexCoord, InfluenceData.GamePieceDistance>();

			// Token: 0x040011FA RID: 4602
			private GamePiece _currentGamePiece;

			// Token: 0x040011FB RID: 4603
			private int _timeoutMaxAttempts = 100;
		}

		// Token: 0x0200086E RID: 2158
		public class PlayerData
		{
			// Token: 0x040011FC RID: 4604
			public float SpawnProximity;

			// Token: 0x040011FD RID: 4605
			public float ControlValue;

			// Token: 0x040011FE RID: 4606
			public float CohesionContribution;
		}

		// Token: 0x0200086F RID: 2159
		public readonly struct LegionDistanceData
		{
			// Token: 0x060027F2 RID: 10226 RVA: 0x00084124 File Offset: 0x00082324
			public LegionDistanceData(int shortestPathLength, int turnsToReach)
			{
				this.ShortestPathLength = shortestPathLength;
				this.TurnsToReach = turnsToReach;
			}

			// Token: 0x040011FF RID: 4607
			public readonly int ShortestPathLength;

			// Token: 0x04001200 RID: 4608
			public readonly int TurnsToReach;
		}

		// Token: 0x02000870 RID: 2160
		public class TerrainInfluenceMap : InfluenceMap
		{
			// Token: 0x060027F3 RID: 10227 RVA: 0x00084134 File Offset: 0x00082334
			protected TerrainInfluenceMap()
			{
			}

			// Token: 0x060027F4 RID: 10228 RVA: 0x00084154 File Offset: 0x00082354
			public static Task<InfluenceData.TerrainInfluenceMap> Generate(TurnContext context)
			{
				InfluenceData.TerrainInfluenceMap.<Generate>d__1 <Generate>d__;
				<Generate>d__.<>t__builder = AsyncTaskMethodBuilder<InfluenceData.TerrainInfluenceMap>.Create();
				<Generate>d__.context = context;
				<Generate>d__.<>1__state = -1;
				<Generate>d__.<>t__builder.Start<InfluenceData.TerrainInfluenceMap.<Generate>d__1>(ref <Generate>d__);
				return <Generate>d__.<>t__builder.Task;
			}

			// Token: 0x170005B2 RID: 1458
			// (get) Token: 0x060027F5 RID: 10229 RVA: 0x00084197 File Offset: 0x00082397
			// (set) Token: 0x060027F6 RID: 10230 RVA: 0x0008419F File Offset: 0x0008239F
			public int Width { get; protected set; }

			// Token: 0x170005B3 RID: 1459
			// (get) Token: 0x060027F7 RID: 10231 RVA: 0x000841A8 File Offset: 0x000823A8
			// (set) Token: 0x060027F8 RID: 10232 RVA: 0x000841B0 File Offset: 0x000823B0
			public int Height { get; protected set; }

			// Token: 0x170005B4 RID: 1460
			public InfluenceData this[HexCoord coord]
			{
				get
				{
					InfluenceData result;
					if (!this.InfMap.TryGetValue(coord.Normalized(this.Width, this.Height), out result))
					{
						return null;
					}
					return result;
				}
				set
				{
					this.InfMap[coord] = value;
				}
			}

			// Token: 0x060027FB RID: 10235 RVA: 0x00084200 File Offset: 0x00082400
			public float MaxFor(IEnumerable<HexCoord> hexCoords, Func<InfluenceData, float> f)
			{
				return hexCoords.Max((HexCoord h) => f(this[h]));
			}

			// Token: 0x060027FC RID: 10236 RVA: 0x00084234 File Offset: 0x00082434
			public float MinFor(IEnumerable<HexCoord> hexCoords, Func<InfluenceData, float> f)
			{
				return hexCoords.Min((HexCoord h) => f(this[h]));
			}

			// Token: 0x060027FD RID: 10237 RVA: 0x00084268 File Offset: 0x00082468
			public int MaxFor(IEnumerable<HexCoord> hexCoords, Func<InfluenceData, int> f)
			{
				return hexCoords.Max((HexCoord h) => f(this[h]));
			}

			// Token: 0x060027FE RID: 10238 RVA: 0x0008429C File Offset: 0x0008249C
			public int MinFor(IEnumerable<HexCoord> hexCoords, Func<InfluenceData, int> f)
			{
				return hexCoords.Min((HexCoord h) => f(this[h]));
			}

			// Token: 0x060027FF RID: 10239 RVA: 0x000842D0 File Offset: 0x000824D0
			protected Task Generate_Internal(TurnContext context)
			{
				InfluenceData.TerrainInfluenceMap.<Generate_Internal>d__25 <Generate_Internal>d__;
				<Generate_Internal>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
				<Generate_Internal>d__.<>4__this = this;
				<Generate_Internal>d__.context = context;
				<Generate_Internal>d__.<>1__state = -1;
				<Generate_Internal>d__.<>t__builder.Start<InfluenceData.TerrainInfluenceMap.<Generate_Internal>d__25>(ref <Generate_Internal>d__);
				return <Generate_Internal>d__.<>t__builder.Task;
			}

			// Token: 0x06002800 RID: 10240 RVA: 0x0008431C File Offset: 0x0008251C
			public bool TryGetPylonDesirability(HexCoord hexCoord, out float pylonDesirability)
			{
				pylonDesirability = -1f;
				InfluenceData influenceData;
				return this.InfMap.TryGetValue(hexCoord, out influenceData) && influenceData.TryGetPylonDesirability(out pylonDesirability);
			}

			// Token: 0x06002801 RID: 10241 RVA: 0x0008434C File Offset: 0x0008254C
			public bool TryGetExternalBorderCantons(int playerId, out HashSet<HexCoord> externalBorder)
			{
				externalBorder = null;
				PlayerTerritory playerTerritory;
				if (this.TryGetPlayerTerritory(playerId, out playerTerritory))
				{
					externalBorder = playerTerritory.ExternalBorder;
				}
				return externalBorder != null;
			}

			// Token: 0x06002802 RID: 10242 RVA: 0x00084374 File Offset: 0x00082574
			public bool TryGetOwnedTerritoryForPlayer(int playerId, out HashSet<HexCoord> ownedTerritory)
			{
				ownedTerritory = null;
				PlayerTerritory playerTerritory;
				if (this.TryGetPlayerTerritory(playerId, out playerTerritory))
				{
					ownedTerritory = playerTerritory.OwnedTerritory;
				}
				return ownedTerritory != null;
			}

			// Token: 0x06002803 RID: 10243 RVA: 0x0008439C File Offset: 0x0008259C
			public bool TryGetUnclaimedBorderForPlayer(int playerId, out HashSet<HexCoord> unclaimedBorder)
			{
				unclaimedBorder = null;
				PlayerTerritory playerTerritory;
				if (this.TryGetPlayerTerritory(playerId, out playerTerritory))
				{
					unclaimedBorder = playerTerritory.UnclaimedExternalBorder;
				}
				return unclaimedBorder != null;
			}

			// Token: 0x06002804 RID: 10244 RVA: 0x000843C4 File Offset: 0x000825C4
			private bool TryGetPlayerTerritory(int playerId, out PlayerTerritory territory)
			{
				territory = this.GetPlayerTerritory(playerId);
				return territory != null;
			}

			// Token: 0x06002805 RID: 10245 RVA: 0x000843D4 File Offset: 0x000825D4
			private PlayerTerritory GetPlayerTerritory(int playerId)
			{
				if (playerId == -1 || playerId == -2147483648)
				{
					return null;
				}
				PlayerTerritory result;
				if (!this.PlayerTerritories.TryGetValue(playerId, out result))
				{
					result = (this.PlayerTerritories[playerId] = new PlayerTerritory());
				}
				return result;
			}

			// Token: 0x06002806 RID: 10246 RVA: 0x00084414 File Offset: 0x00082614
			public Task CalculatePlayerTerritory(TurnContext context)
			{
				InfluenceData.TerrainInfluenceMap.<CalculatePlayerTerritory>d__32 <CalculatePlayerTerritory>d__;
				<CalculatePlayerTerritory>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
				<CalculatePlayerTerritory>d__.<>4__this = this;
				<CalculatePlayerTerritory>d__.context = context;
				<CalculatePlayerTerritory>d__.<>1__state = -1;
				<CalculatePlayerTerritory>d__.<>t__builder.Start<InfluenceData.TerrainInfluenceMap.<CalculatePlayerTerritory>d__32>(ref <CalculatePlayerTerritory>d__);
				return <CalculatePlayerTerritory>d__.<>t__builder.Task;
			}

			// Token: 0x06002807 RID: 10247 RVA: 0x00084460 File Offset: 0x00082660
			public bool GetHexIsAdjacentTo(HexCoord hex, int playerID)
			{
				InfluenceData influenceData;
				return this.InfMap.TryGetValue(hex, out influenceData) && influenceData.IsAdjacentToPlayer(playerID);
			}

			// Token: 0x06002808 RID: 10248 RVA: 0x00084488 File Offset: 0x00082688
			public Task<float> CalculateCongestionAsync(TurnContext context, HexCoord coord)
			{
				Hex hex = context.HexBoard[coord];
				if (hex.Type == TerrainType.LandBridge)
				{
					return Task.FromResult<float>(1f);
				}
				if (context.IsCapturableTileType(hex))
				{
					float num = (float)this.InfMap[coord]._entranceCount / 3f;
					return Task.FromResult<float>(num * num);
				}
				return Task.FromResult<float>(0f);
			}

			// Token: 0x06002809 RID: 10249 RVA: 0x000844EC File Offset: 0x000826EC
			public Task<int> CalcEntranceCountAsync(TurnContext context, HexCoord centreHex)
			{
				IEnumerable<HexCoord> enumerable = context.HexBoard.EnumerateNeighbours(centreHex);
				int num = 0;
				bool flag = false;
				foreach (HexCoord coord in enumerable)
				{
					if (LegionMovementProcessor.IsTraversable(context, null, coord, PathMode.March))
					{
						if (!flag)
						{
							num++;
							flag = true;
						}
					}
					else
					{
						flag = false;
					}
				}
				return Task.FromResult<int>(num);
			}

			// Token: 0x0600280A RID: 10250 RVA: 0x0008455C File Offset: 0x0008275C
			public Task DoSpawnProximityFloodFillTask(TurnContext context)
			{
				Task completedTask;
				using (SimProfilerBlock.ProfilerBlock(""))
				{
					InfluenceData.GamePieceFloodFillSolver gamePieceFloodFillSolver = new InfluenceData.GamePieceFloodFillSolver();
					float num = MathF.Pow((float)context.HexBoard.Rows * 0.5f, 2f) + MathF.Pow((float)context.HexBoard.Columns * 0.5f, 2f);
					foreach (KeyValuePair<HexCoord, InfluenceData.GamePieceDistance> keyValuePair in gamePieceFloodFillSolver.DoAllGamePieceDistancesFloodFillIgnoringAllDiplomacy(context, context.CurrentTurn.GetAllStrongholds()))
					{
						HexCoord hexCoord;
						InfluenceData.GamePieceDistance gamePieceDistance;
						keyValuePair.Deconstruct(out hexCoord, out gamePieceDistance);
						HexCoord key = hexCoord;
						InfluenceData.GamePieceDistance gamePieceDistance2 = gamePieceDistance;
						foreach (PlayerState playerState in context.CurrentTurn.EnumeratePlayerStates(false, false))
						{
							GamePiece stronghold = context.CurrentTurn.GetStronghold(playerState.Id);
							int num2;
							if (stronghold != null && gamePieceDistance2.Distances.TryGetValue(stronghold.Id, out num2))
							{
								float num3 = (float)num2 * (float)num2;
								float spawnProximity = Math.Clamp(1f - num3 / num, 0f, 1f);
								this.InfMap[key]._dataPerPlayer.GetOrCreate(playerState.Id).SpawnProximity = spawnProximity;
							}
						}
					}
					completedTask = Task.CompletedTask;
				}
				return completedTask;
			}

			// Token: 0x0600280B RID: 10251 RVA: 0x0008471C File Offset: 0x0008291C
			public Task DoTurnsToReachFloodFillTask(TurnContext context)
			{
				Task completedTask;
				using (SimProfilerBlock.ProfilerBlock(""))
				{
					foreach (KeyValuePair<HexCoord, InfluenceData.GamePieceDistance> keyValuePair in new InfluenceData.GamePieceFloodFillSolver().DoAllGamePieceDistancesFloodFill(context, context.CurrentTurn.GetAllActiveLegions(), null))
					{
						foreach (KeyValuePair<Identifier, int> keyValuePair2 in keyValuePair.Value.Distances)
						{
							GamePiece gamePiece = context.CurrentTurn.FetchGameItem<GamePiece>(keyValuePair2.Key);
							ModifiableValue groundMoveDistance = gamePiece.GroundMoveDistance;
							int value = keyValuePair2.Value;
							int turnsToReach = (int)Math.Ceiling((double)((float)value / (float)groundMoveDistance));
							if (this.InfMap[keyValuePair.Key]._reachableLegionData.ContainsKey(keyValuePair2.Key))
							{
								SimLogger logger = SimLogger.Logger;
								if (logger != null)
								{
									logger.Error(string.Format("Adding more than one distance for legion {0} on {1}", gamePiece, keyValuePair.Key));
								}
							}
							else
							{
								this.InfMap[keyValuePair.Key]._reachableLegionData[keyValuePair2.Key] = new InfluenceData.LegionDistanceData(value, turnsToReach);
							}
						}
					}
					completedTask = Task.CompletedTask;
				}
				return completedTask;
			}

			// Token: 0x0600280C RID: 10252 RVA: 0x000848D0 File Offset: 0x00082AD0
			public Task DoTurnsToReachIfOwnerTerritoryCouldBeCrossedFloodFillTask(TurnContext context)
			{
				Task completedTask;
				using (SimProfilerBlock.ProfilerBlock(""))
				{
					foreach (PlayerState playerState in context.CurrentTurn.EnumeratePlayerStates(true, false))
					{
						foreach (KeyValuePair<HexCoord, InfluenceData.GamePieceDistance> keyValuePair in new InfluenceData.GamePieceFloodFillSolver().DoAllGamePieceDistancesFloodFill(context, context.CurrentTurn.GetAllActiveLegions(), new List<int>
						{
							playerState.Id
						}))
						{
							if (context.HexBoard[keyValuePair.Key].ControllingPlayerID == playerState.Id)
							{
								foreach (KeyValuePair<Identifier, int> keyValuePair2 in keyValuePair.Value.Distances)
								{
									GamePiece gamePiece = context.CurrentTurn.FetchGameItem<GamePiece>(keyValuePair2.Key);
									int value = keyValuePair2.Value;
									int turnsToReach = (int)Math.Ceiling((double)((float)value / (float)gamePiece.GroundMoveDistance));
									if (this.InfMap[keyValuePair.Key]._reachableIfOwnerTerritoryCouldBeCrossedLegionData.ContainsKey(keyValuePair2.Key))
									{
										SimLogger logger = SimLogger.Logger;
										if (logger != null)
										{
											logger.Error(string.Format("Adding more than one distance for legion {0} on {1}", gamePiece, keyValuePair.Key));
										}
									}
									else
									{
										this.InfMap[keyValuePair.Key]._reachableIfOwnerTerritoryCouldBeCrossedLegionData[keyValuePair2.Key] = new InfluenceData.LegionDistanceData(value, turnsToReach);
									}
								}
							}
						}
					}
					completedTask = Task.CompletedTask;
				}
				return completedTask;
			}

			// Token: 0x0600280D RID: 10253 RVA: 0x00084B00 File Offset: 0x00082D00
			public void DoNearestFixtureFloodFillTask(TurnContext context)
			{
				using (SimProfilerBlock.ProfilerBlock(""))
				{
					InfluenceData.GamePieceFloodFillSolver gamePieceFloodFillSolver = new InfluenceData.GamePieceFloodFillSolver();
					Dictionary<HexCoord, InfluenceData.GamePieceDistance> dictionary = gamePieceFloodFillSolver.DoAllGamePieceDistancesFloodFillIgnoringAllDiplomacy(context, context.CurrentTurn.GetAllActiveFixtures());
					foreach (KeyValuePair<HexCoord, InfluenceData.GamePieceDistance> keyValuePair in dictionary)
					{
						int minimumDistance = keyValuePair.Value.GetMinimumDistance();
						this.InfMap[keyValuePair.Key]._nearestFixtureDistance = minimumDistance;
						float sumOfAllSquaredFixtureDistances = keyValuePair.Value.SumOfSquaredDistancesToGamePieces();
						this.InfMap[keyValuePair.Key]._sumOfAllSquaredFixtureDistances = sumOfAllSquaredFixtureDistances;
					}
					float num = gamePieceFloodFillSolver.FindLargestSumDistanceToFixturesValue();
					if (num > 0f)
					{
						foreach (KeyValuePair<HexCoord, InfluenceData.GamePieceDistance> keyValuePair2 in dictionary)
						{
							float sumOfAllSquaredFixtureDistances2 = this.InfMap[keyValuePair2.Key]._sumOfAllSquaredFixtureDistances;
							this.InfMap[keyValuePair2.Key]._centrality = sumOfAllSquaredFixtureDistances2 / num;
						}
					}
				}
			}

			// Token: 0x0600280E RID: 10254 RVA: 0x00084C74 File Offset: 0x00082E74
			public void CalculateCohesionValuesFast(TurnContext context)
			{
				using (SimProfilerBlock.ProfilerBlock(""))
				{
					foreach (PlayerState playerState in context.CurrentTurn.EnumeratePlayerStates(false, false))
					{
						PlayerTerritory playerTerritory;
						if (this.TryGetPlayerTerritory(playerState.Id, out playerTerritory))
						{
							int count = playerTerritory.OwnedTerritory.Count;
							int num = context.CalcFrontierCount(playerTerritory.OwnedTerritory);
							int count2 = playerTerritory.Enclaves.Count;
							float num2 = CaptureExtensions.CalculateTerritorialCohesion(count, num, count2);
							HashSet<HexCoord> hashSet = playerTerritory.OwnedTerritory.ToHashSet<HexCoord>();
							foreach (Hex hex in context.HexBoard.Hexes)
							{
								HexCoord hexCoord = hex.HexCoord;
								float num3 = num2;
								if (context.IsCapturableTileType(hex))
								{
									if (hex.ControllingPlayerID == playerState.Id)
									{
										int cantons = count - 1;
										int num4 = num;
										foreach (HexCoord coord in context.HexBoard.EnumerateNeighboursNormalized(hexCoord))
										{
											if (context.IsCapturableTileType(coord))
											{
												if (context.HexBoard[coord].ControllingPlayerID == playerState.Id)
												{
													num4++;
												}
												else
												{
													num4--;
												}
											}
										}
										hashSet.Remove(hexCoord);
										int enclaves = context.HexBoard.CalculateEnclaveCount(hashSet);
										hashSet.Add(hexCoord);
										num3 -= CaptureExtensions.CalculateTerritorialCohesion(cantons, num4, enclaves);
									}
									else
									{
										int cantons2 = count + 1;
										bool flag = playerTerritory.ExternalBorder.Contains(hexCoord);
										int num5 = num;
										foreach (HexCoord coord2 in context.HexBoard.EnumerateNeighboursNormalized(hexCoord))
										{
											if (context.IsCapturableTileType(coord2))
											{
												if (context.HexBoard[coord2].ControllingPlayerID == playerState.Id)
												{
													num5--;
												}
												else
												{
													num5++;
												}
											}
										}
										int num6 = count2;
										if (flag)
										{
											int num7 = 0;
											foreach (HashSet<HexCoord> @object in playerTerritory.Enclaves)
											{
												if (context.HexBoard.EnumerateNeighboursNormalized(hexCoord).Any(new Func<HexCoord, bool>(@object.Contains)))
												{
													num7++;
												}
											}
											if (num7 > 0)
											{
												num6 -= num7 - 1;
											}
										}
										else
										{
											num6++;
										}
										num3 = CaptureExtensions.CalculateTerritorialCohesion(cantons2, num5, num6) - num3;
									}
									this.InfMap[hexCoord]._dataPerPlayer.GetOrCreate(playerState.Id).CohesionContribution = num3;
								}
							}
						}
					}
				}
			}

			// Token: 0x0600280F RID: 10255 RVA: 0x00084FF8 File Offset: 0x000831F8
			public Task CalculateCohesionValueAsync(TurnContext context)
			{
				InfluenceData.TerrainInfluenceMap.<CalculateCohesionValueAsync>d__41 <CalculateCohesionValueAsync>d__;
				<CalculateCohesionValueAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
				<CalculateCohesionValueAsync>d__.<>4__this = this;
				<CalculateCohesionValueAsync>d__.context = context;
				<CalculateCohesionValueAsync>d__.<>1__state = -1;
				<CalculateCohesionValueAsync>d__.<>t__builder.Start<InfluenceData.TerrainInfluenceMap.<CalculateCohesionValueAsync>d__41>(ref <CalculateCohesionValueAsync>d__);
				return <CalculateCohesionValueAsync>d__.<>t__builder.Task;
			}

			// Token: 0x06002810 RID: 10256 RVA: 0x00085044 File Offset: 0x00083244
			public Task CalculateControlHeuristicAsync(TurnContext context)
			{
				Task completedTask;
				using (SimProfilerBlock.ProfilerBlock(""))
				{
					float num = 1f;
					float num2 = 2f;
					float num3 = 3f;
					float num4 = 4f;
					float num5 = 5f;
					foreach (PlayerState playerState in context.CurrentTurn.EnumeratePlayerStates(false, false))
					{
						foreach (Hex hex in context.HexBoard.Hexes)
						{
							if (context.IsCapturableTileType(hex))
							{
								float num6 = 0f;
								float num7 = 0f;
								num6 += num5 * ((this.InfMap[hex.HexCoord]._nearestFixtureDistance == 1) ? 1f : 0f);
								num7 += num5;
								num6 += num * this.InfMap[hex.HexCoord]._centrality;
								num7 += num;
								num6 += num2 * this.InfMap[hex.HexCoord]._congestion;
								num7 += num2;
								float num8;
								if (this.InfMap[hex.HexCoord].TryGetSpawnProximity(playerState.Id, out num8))
								{
									num6 += num4 * num8;
									num7 += num4;
								}
								float num9;
								if (this.InfMap[hex.HexCoord].TryGetCohesionContribution(playerState.Id, out num9))
								{
									num6 += num3 * num9;
									num7 += num3;
								}
								num6 /= num7;
								num6 = Math.Clamp(num6, 0f, 1f);
								this.InfMap[hex.HexCoord]._dataPerPlayer.GetOrCreate(playerState.Id).ControlValue = num6;
							}
						}
					}
					completedTask = Task.CompletedTask;
				}
				return completedTask;
			}

			// Token: 0x06002811 RID: 10257 RVA: 0x00085298 File Offset: 0x00083498
			public Task CalculateSpawnProximityAsync(TurnContext context)
			{
				TurnState currentTurn = context.CurrentTurn;
				HexBoard hexBoard = context.HexBoard;
				float num = MathF.Pow((float)hexBoard.Rows * 0.5f, 2f) + MathF.Pow((float)hexBoard.Columns * 0.5f, 2f);
				PathfinderHexboard pathfinderHexboard = new PathfinderHexboard();
				pathfinderHexboard.PopulateMap(context);
				List<int> ignoreDiplomacyWithPlayers = IEnumerableExtensions.ToList<int>(from p in currentTurn.EnumeratePlayerStates(true, true)
				select p.Id);
				foreach (PlayerState playerState in currentTurn.EnumeratePlayerStates(false, false))
				{
					GamePiece stronghold = currentTurn.GetStronghold(playerState.Id);
					foreach (Hex hex in hexBoard.Hexes)
					{
						InfluenceData.PlayerData orCreate = this.InfMap[hex.HexCoord]._dataPerPlayer.GetOrCreate(playerState.Id);
						orCreate.SpawnProximity = float.MaxValue;
						if (stronghold != null)
						{
							PFAgentGamePiece agentData = new PFAgentGamePiece(null)
							{
								GamePiece = stronghold,
								AvoidanceType = GamePieceAvoidance.None,
								DestinationAlwaysValid = true,
								AllowRedeployToDestination = true,
								IgnoreDiplomacyWithPlayers = ignoreDiplomacyWithPlayers
							};
							List<HexCoord> list = pathfinderHexboard.FindPath(stronghold.Location, hex.HexCoord, agentData);
							if (list != null && list.Count != 0)
							{
								float num2 = (float)(list.Count * list.Count);
								orCreate.SpawnProximity = Math.Clamp(1f - num2 / num, 0f, 1f);
							}
						}
					}
				}
				return Task.CompletedTask;
			}

			// Token: 0x06002812 RID: 10258 RVA: 0x00085498 File Offset: 0x00083698
			public Task CalcUnclaimedAsync(TurnContext context)
			{
				float num = (float)context.HexBoard.Hexes.Count((Hex t) => t.IsUnclaimed());
				int num2 = context.HexBoard.Columns * context.HexBoard.Rows;
				float value = num / (float)num2;
				this.UnclaimedCantonProportion = Math.Clamp(value, 0f, 1f);
				return Task.CompletedTask;
			}

			// Token: 0x06002813 RID: 10259 RVA: 0x0008550C File Offset: 0x0008370C
			public Task CalcAdjacentToFriendlyAsync(TurnContext context)
			{
				Task completedTask;
				using (SimProfilerBlock.ProfilerBlock(""))
				{
					foreach (PlayerState playerState in context.CurrentTurn.EnumeratePlayerStates(false, false))
					{
						foreach (GamePiece gamePiece in context.CurrentTurn.GetActiveGamePiecesForPlayer(playerState.Id))
						{
							if (gamePiece.Location != HexCoord.Invalid)
							{
								foreach (HexCoord coord in context.HexBoard.GetNeighbours(gamePiece.Location, false))
								{
									this[coord]._adjacentToPlayer.Add(playerState.Id);
								}
							}
						}
					}
					completedTask = Task.CompletedTask;
				}
				return completedTask;
			}

			// Token: 0x06002814 RID: 10260 RVA: 0x00085640 File Offset: 0x00083840
			public Task CalcTerrainTypeCounts(TurnContext context)
			{
				foreach (Hex hex in context.HexBoard.Hexes)
				{
					TerrainType type = hex.Type;
					if (type <= TerrainType.Swamp)
					{
						if (type != TerrainType.Ravine)
						{
							if (type == TerrainType.Swamp)
							{
								this.NumSwampHexes++;
							}
						}
						else
						{
							this.NumRavineHexes++;
						}
					}
					else if (type != TerrainType.Lava)
					{
						if (type == TerrainType.Vent)
						{
							this.NumSulphurHexes++;
						}
					}
					else
					{
						this.NumLavaHexes++;
					}
				}
				return Task.CompletedTask;
			}

			// Token: 0x06002815 RID: 10261 RVA: 0x000856F4 File Offset: 0x000838F4
			public void CalcPylonValue(TurnContext context)
			{
				using (SimProfilerBlock.ProfilerBlock(""))
				{
					PlayerState playerState = context.CurrentTurn.FindPlayerState("Belphegor");
					if (playerState != null)
					{
						SummonLegionRitualData summonLegionRitualData = context.Database.Fetch<SummonLegionRitualData>("belphegor_raise_dark_pylon");
						AuraStaticData auraStaticData;
						if (summonLegionRitualData.AuraBlacklist.Count != 1)
						{
							SimLogger logger = SimLogger.Logger;
							if (logger != null)
							{
								logger.Error("Expecting 1 aura from dark pylon");
							}
						}
						else if (!context.Database.TryFetch<AuraStaticData>(summonLegionRitualData.AuraBlacklist[0], out auraStaticData))
						{
							SimLogger logger2 = SimLogger.Logger;
							if (logger2 != null)
							{
								logger2.Error("Unable to find aura data");
							}
						}
						else
						{
							GamePiece pandaemonium = context.CurrentTurn.GetPandaemonium();
							if (pandaemonium == null)
							{
								SimLogger logger3 = SimLogger.Logger;
								if (logger3 != null)
								{
									logger3.Error("Could not find Pandaemonium");
								}
							}
							else
							{
								Dictionary<HexCoord, GamePiece> dictionary = context.CurrentTurn.GenerateHexCoordToGamePieceLookup();
								foreach (Hex hex in context.CurrentTurn.HexBoard.Hexes)
								{
									if (summonLegionRitualData.HexTargetSettings.ValidTerrainTypes.IsSet((int)hex.Type) && (summonLegionRitualData.HexTargetSettings.CanTargetAdjacentToPoP || this.InfMap[hex.HexCoord]._nearestFixtureDistance > 1))
									{
										bool flag = false;
										using (IEnumerator<Aura> enumerator2 = context.CurrentTurn.GetAurasOverlapping(hex.HexCoord).GetEnumerator())
										{
											while (enumerator2.MoveNext())
											{
												if (enumerator2.Current.AuraSourceId == auraStaticData.Id)
												{
													flag = true;
													break;
												}
											}
										}
										if (!flag && context.HexBoard.ShortestDistance(pandaemonium.Location, hex.HexCoord) > auraStaticData.Radius)
										{
											float pylonDesirability = 0f;
											foreach (HexCoord key in context.HexBoard.EnumerateRangeNormalized(hex.HexCoord, auraStaticData.Radius))
											{
												GamePiece gamePiece;
												if (dictionary.TryGetValue(key, out gamePiece) && gamePiece.IsFixture())
												{
													float amount;
													if (gamePiece.ControllingPlayerId == playerState.Id)
													{
														amount = 0.3f;
													}
													else if (gamePiece.ControllingPlayerId == -1)
													{
														amount = 0.4f;
													}
													else
													{
														amount = 0.5f;
														PlayerState playerState2;
														if (context.CurrentTurn.TryGetNemesis(playerState, out playerState2))
														{
															amount = 0.6f;
														}
													}
													ref pylonDesirability.LerpTo01(amount);
												}
											}
											this.InfMap[hex.HexCoord]._pylonDesirability = pylonDesirability;
										}
									}
								}
							}
						}
					}
				}
			}

			// Token: 0x04001201 RID: 4609
			public Dictionary<HexCoord, InfluenceData> InfMap = new Dictionary<HexCoord, InfluenceData>();

			// Token: 0x04001202 RID: 4610
			public SparseArray<PlayerTerritory> PlayerTerritories = new SparseArray<PlayerTerritory>();

			// Token: 0x04001203 RID: 4611
			public bool IsGenerated;

			// Token: 0x04001206 RID: 4614
			public float UnclaimedCantonProportion;

			// Token: 0x04001207 RID: 4615
			public int NumLavaHexes;

			// Token: 0x04001208 RID: 4616
			public int NumRavineHexes;

			// Token: 0x04001209 RID: 4617
			public int NumSwampHexes;

			// Token: 0x0400120A RID: 4618
			public int NumSulphurHexes;
		}
	}
}
