using System;
using System.Collections.Generic;
using System.Linq;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020001AE RID: 430
	public class PathfinderHexboard : Pathfinder<PFNodeHexCoord, PFAgentGamePiece>
	{
		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060007EB RID: 2027 RVA: 0x00024512 File Offset: 0x00022712
		private TurnState _turnState
		{
			get
			{
				return this._context.CurrentTurn;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060007EC RID: 2028 RVA: 0x0002451F File Offset: 0x0002271F
		public HexBoard HexBoard
		{
			get
			{
				return this._context.HexBoard;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060007ED RID: 2029 RVA: 0x0002452C File Offset: 0x0002272C
		// (set) Token: 0x060007EE RID: 2030 RVA: 0x00024534 File Offset: 0x00022734
		public int ImmuneToFumes { get; private set; } = int.MinValue;

		// Token: 0x060007EF RID: 2031 RVA: 0x00024540 File Offset: 0x00022740
		public PathfinderHexboard()
		{
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x00024598 File Offset: 0x00022798
		public PathfinderHexboard(TurnContext turnState)
		{
			this.PopulateMap(turnState);
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x000245F4 File Offset: 0x000227F4
		public void PopulateMap(TurnContext context)
		{
			this._context = context;
			ListExtensions.ClearFast<PFNodeHexCoord>(this.Map);
			foreach (Hex hex in this.HexBoard.GetAllHexes())
			{
				HexCoord location = this.HexBoard.ToRelativeHex(hex.HexCoord);
				this.Map.Add(new PFNodeHexCoord(location, this._turnState.HexBoard));
			}
			this.LegionHexes.Clear();
			this.FixtureHexes.Clear();
			foreach (GamePiece gamePiece in this._turnState.GetActiveGamePieces())
			{
				PathfinderHexboard.GamePiecePosition item = new PathfinderHexboard.GamePiecePosition(gamePiece.Location, gamePiece.ControllingPlayerId);
				if (gamePiece.IsFixture())
				{
					this.FixtureHexes.Add(item);
				}
				else
				{
					this.LegionHexes.Add(item);
				}
			}
			List<TerrainStaticData> list = new List<TerrainStaticData>();
			foreach (TerrainType legacyType in PathfinderHexboard.TerrainTypesToQuery)
			{
				TerrainStaticData item2;
				if (this._context.Database.TryFindTerrainData(legacyType, out item2))
				{
					list.Add(item2);
				}
			}
			foreach (GamePiece gamePiece2 in this._turnState.GetAllActiveLegions())
			{
				this.CalculateTerrainCostsForGamePiece(gamePiece2, list);
				using (IEnumerator<Aura> enumerator3 = this._turnState.GetAurasAffecting(gamePiece2).GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						if (enumerator3.Current.AbilitySourceId == "Noxious Fumes")
						{
							this.LegionsInFumes.Add(gamePiece2);
						}
					}
				}
			}
			foreach (Hex hex2 in this.HexBoard.Hexes)
			{
				foreach (Aura aura in this._turnState.GetAurasOverlapping(hex2.HexCoord))
				{
					if (aura.AbilitySourceId == "Noxious Fumes")
					{
						if (this.ImmuneToFumes == -2147483648)
						{
							GamePiece gamePiece3;
							if (this._turnState.TryFetchGameItem<GamePiece>(aura.ProviderId, out gamePiece3))
							{
								this.ImmuneToFumes = gamePiece3.ControllingPlayerId;
							}
							else
							{
								SimLogger logger = SimLogger.Logger;
								if (logger != null)
								{
									logger.Error(string.Format("Unable to find aura source piece {0} for aura {1}", aura.ProviderId, aura.AuraSourceId));
								}
							}
						}
						this.TileInFumes.Add(hex2.HexCoord);
						break;
					}
				}
			}
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x00024920 File Offset: 0x00022B20
		public bool DoesHexContainFixtureForPlayer(HexCoord hexCoord, int playerId)
		{
			return this.FixtureHexes.Contains(new PathfinderHexboard.GamePiecePosition(hexCoord, playerId));
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x00024934 File Offset: 0x00022B34
		public bool IsHexDangerous(GamePiece gamePiece, HexCoord hexCoord)
		{
			if (this.TileInFumes.Contains(hexCoord) && gamePiece.ControllingPlayerId != this.ImmuneToFumes)
			{
				return true;
			}
			Hex hex = this.HexBoard[hexCoord];
			Dictionary<TerrainType, AITerrainCostData> dictionary;
			if (!this.TerrainCostsPerUnit.TryGetValue(gamePiece.Id, out dictionary))
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Error("Unable to find terrain costs for game piece " + gamePiece.NameKey);
				}
				return false;
			}
			AITerrainCostData aiterrainCostData;
			if (!dictionary.TryGetValue(hex.Type, out aiterrainCostData))
			{
				SimLogger logger2 = SimLogger.Logger;
				if (logger2 != null)
				{
					logger2.Error(string.Format("No terrain cost for terrain type {0}", hex.Type));
				}
				return false;
			}
			return aiterrainCostData.IsDangerous;
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x000249E0 File Offset: 0x00022BE0
		public float GetTerrainModifierCostForGamePiece(GamePiece gamePiece, TerrainType terrainType)
		{
			Dictionary<TerrainType, AITerrainCostData> dictionary;
			if (!this.TerrainCostsPerUnit.TryGetValue(gamePiece.Id, out dictionary))
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Error("Unable to find terrain costs for game piece " + gamePiece.NameKey);
				}
				return 0f;
			}
			AITerrainCostData aiterrainCostData;
			if (!dictionary.TryGetValue(terrainType, out aiterrainCostData))
			{
				SimLogger logger2 = SimLogger.Logger;
				if (logger2 != null)
				{
					logger2.Error(string.Format("No terrain cost for terrain type {0}", terrainType));
				}
				return 0f;
			}
			float result;
			switch (aiterrainCostData.MoveCostType)
			{
			case MoveCostType.NonTraversible:
				result = 10f;
				break;
			case MoveCostType.MovePoints:
				result = 0f;
				break;
			case MoveCostType.RemainderOfMovement:
				result = 1f;
				break;
			case MoveCostType.AllOfMovement:
				result = 1f;
				break;
			case MoveCostType.FreeMovement:
				result = 0f;
				break;
			default:
				result = 0f;
				break;
			}
			return result;
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x00024AAC File Offset: 0x00022CAC
		private void CalculateTerrainCostsForGamePiece(GamePiece gamePiece, List<TerrainStaticData> terrainStaticDataList)
		{
			using (SimProfilerBlock.ProfilerBlock(""))
			{
				Dictionary<TerrainType, AITerrainCostData> dictionary;
				if (!this.TerrainCostsPerUnit.TryGetValue(gamePiece.Id, out dictionary))
				{
					dictionary = new Dictionary<TerrainType, AITerrainCostData>();
					this.TerrainCostsPerUnit.Add(gamePiece.Id, dictionary);
				}
				else
				{
					SimLogger logger = SimLogger.Logger;
					if (logger != null)
					{
						logger.Error("Calculating Terrain costs for game piece " + gamePiece.NameKey + " more than once");
					}
				}
				foreach (TerrainStaticData terrainStaticData in terrainStaticDataList)
				{
					TerrainType legacyType = terrainStaticData.LegacyType;
					AITerrainCostData value;
					if (!dictionary.TryGetValue(legacyType, out value))
					{
						MoveCostType moveCostType = LegionMovementProcessor.CalculateMovementCostType(terrainStaticData, gamePiece);
						bool isDangerous = false;
						foreach (ConfigRef<ItemAbilityStaticData> cref in terrainStaticData.ProvidedAbilities)
						{
							foreach (AbilityEffect abilityEffect in this._context.Database.Fetch(cref).Effects)
							{
								if (abilityEffect is TurnEffect_Damage && !abilityEffect.IsNullified(this._context, gamePiece))
								{
									isDangerous = true;
								}
							}
						}
						value = new AITerrainCostData(moveCostType, isDangerous);
						dictionary.Add(legacyType, value);
					}
					else
					{
						SimLogger logger2 = SimLogger.Logger;
						if (logger2 != null)
						{
							logger2.Error(string.Format("Calculating Terrain costs for terrain type {0} more than once", terrainStaticData.LegacyType));
						}
					}
				}
			}
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x00024CA4 File Offset: 0x00022EA4
		public bool TryFindPath(HexCoord start, HexCoord end, PFAgentGamePiece agentData, out List<HexCoord> path)
		{
			path = new List<HexCoord>();
			start = this.HexBoard.ToRelativeHex(start);
			end = this.HexBoard.ToRelativeHex(end);
			if (!start.IsValid || !end.IsValid)
			{
				return false;
			}
			PFNodeHexCoord start2 = this[start];
			PFNodeHexCoord destination = this[end];
			List<PFNodeHexCoord> source;
			bool flag = base.TryFindPath(start2, destination, agentData, out source, null);
			path = IEnumerableExtensions.ToList<HexCoord>(from t in source
			select t.Location);
			bool flag2;
			if (path.Count > 1)
			{
				HexCoord hexCoord = path.Last<HexCoord>();
				flag2 = (hexCoord == end);
			}
			else
			{
				flag2 = false;
			}
			return flag && flag2;
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x00024D58 File Offset: 0x00022F58
		public bool TryFindPathToAny(HexCoord start, Func<HexCoord, bool> isEnd, PFAgentGamePiece agentData, out List<HexCoord> path)
		{
			path = new List<HexCoord>();
			start = this.HexBoard.ToRelativeHex(start);
			PathfinderHexboard.HexboardDestinationChecker destinationChecker = new PathfinderHexboard.HexboardDestinationChecker(isEnd);
			if (!start.IsValid)
			{
				return false;
			}
			PFNodeHexCoord start2 = this[start];
			List<PFNodeHexCoord> source;
			bool flag = base.TryFindPathToAny(start2, destinationChecker, agentData, out source);
			path = IEnumerableExtensions.ToList<HexCoord>(from t in source
			select t.Location);
			return flag & (path.Count > 1 && isEnd(path.Last<HexCoord>()));
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x00024DEC File Offset: 0x00022FEC
		public List<HexCoord> FindPath(HexCoord start, HexCoord end, PFAgentGamePiece agentData)
		{
			List<HexCoord> result;
			this.TryFindPath(start, end, agentData, out result);
			return result;
		}

		// Token: 0x1700019A RID: 410
		private PFNodeHexCoord this[HexCoord coord]
		{
			get
			{
				return this.Map[this.HexBoard.ToIndex(coord)];
			}
			set
			{
				this.Map[this.HexBoard.ToIndex(coord)] = value;
			}
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x00024E39 File Offset: 0x00023039
		public override IEnumerable<PFNodeHexCoord> GetNeighbours(PFNodeHexCoord currentNode, PFNodeHexCoord destination, PFAgentGamePiece agent)
		{
			return from t in this.ValidNeighbours(currentNode.Location, destination.Location, agent)
			select this[t];
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x00024E5F File Offset: 0x0002305F
		public override IEnumerable<PFNodeHexCoord> GetNeighbours(PFNodeHexCoord currentNode, PFAgentGamePiece agent)
		{
			return from t in this.ValidNeighbours(currentNode.Location, agent)
			select this[t];
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x00024E80 File Offset: 0x00023080
		public IEnumerable<HexCoord> ValidNeighbours(HexCoord coord, HexCoord finalDestination, PFAgentGamePiece agent)
		{
			return from t in this.HexBoard.EnumerateNeighboursNormalized(coord)
			where this.CanMoveTo(coord, t, finalDestination, agent)
			select t;
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x00024ED4 File Offset: 0x000230D4
		public IEnumerable<HexCoord> ValidNeighbours(HexCoord coord, PFAgentGamePiece agent)
		{
			return from t in this.HexBoard.EnumerateNeighboursNormalized(coord)
			where this.CanMoveTo(t, agent)
			select t;
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x00024F14 File Offset: 0x00023114
		public bool CanMoveTo(HexCoord fromCoord, HexCoord toCoord, HexCoord finalDestination, PFAgentGamePiece agent)
		{
			PathfinderHexboard.<>c__DisplayClass34_0 CS$<>8__locals1 = new PathfinderHexboard.<>c__DisplayClass34_0();
			CS$<>8__locals1.fromCoord = fromCoord;
			if (!agent.DestinationAlwaysValid || !(toCoord == finalDestination))
			{
				return this.CanMoveTo(toCoord, agent);
			}
			if (agent.AllowRedeployToDestination)
			{
				return true;
			}
			PathfinderHexboard.<>c__DisplayClass34_0 CS$<>8__locals2 = CS$<>8__locals1;
			GamePiece gamePiece = agent.GamePiece;
			CS$<>8__locals2.ownerTeam = ((gamePiece != null) ? gamePiece.ControllingPlayerId : int.MinValue);
			return !this.FixtureHexes.Any((PathfinderHexboard.GamePiecePosition p) => p.Coord == CS$<>8__locals1.fromCoord && p.PlayerId == CS$<>8__locals1.ownerTeam);
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x00024F90 File Offset: 0x00023190
		public bool CanMoveTo(HexCoord toCoord, PFAgentGamePiece agent)
		{
			PathfinderHexboard.<>c__DisplayClass35_0 CS$<>8__locals1 = new PathfinderHexboard.<>c__DisplayClass35_0();
			CS$<>8__locals1.toCoord = toCoord;
			PathfinderHexboard.<>c__DisplayClass35_0 CS$<>8__locals2 = CS$<>8__locals1;
			GamePiece gamePiece = agent.GamePiece;
			CS$<>8__locals2.ownerTeam = ((gamePiece != null) ? gamePiece.ControllingPlayerId : int.MinValue);
			return ((agent.AvoidanceType & GamePieceAvoidance.EnemyLegion) != GamePieceAvoidance.EnemyLegion || !this.LegionHexes.Any((PathfinderHexboard.GamePiecePosition p) => p.Coord == CS$<>8__locals1.toCoord && p.PlayerId != CS$<>8__locals1.ownerTeam)) && ((agent.AvoidanceType & GamePieceAvoidance.FriendlyLegion) != GamePieceAvoidance.FriendlyLegion || !this.LegionHexes.Any((PathfinderHexboard.GamePiecePosition p) => p.Coord == CS$<>8__locals1.toCoord && p.PlayerId == CS$<>8__locals1.ownerTeam)) && ((agent.AvoidanceType & GamePieceAvoidance.EnemyFixture) != GamePieceAvoidance.EnemyFixture || !this.FixtureHexes.Any((PathfinderHexboard.GamePiecePosition p) => p.Coord == CS$<>8__locals1.toCoord && p.PlayerId != CS$<>8__locals1.ownerTeam)) && ((agent.AvoidanceType & GamePieceAvoidance.FriendlyFixture) != GamePieceAvoidance.FriendlyFixture || !this.FixtureHexes.Any((PathfinderHexboard.GamePiecePosition p) => p.Coord == CS$<>8__locals1.toCoord && p.PlayerId == CS$<>8__locals1.ownerTeam)) && LegionMovementProcessor.CanEnterCanton(this._context, agent.GamePiece, CS$<>8__locals1.toCoord, PathMode.March, agent.IgnoreDiplomacyWithPlayers).successful;
		}

		// Token: 0x040003AB RID: 939
		private static TerrainType[] TerrainTypesToQuery = new TerrainType[]
		{
			TerrainType.Plain,
			TerrainType.Ravine,
			TerrainType.Swamp,
			TerrainType.River,
			TerrainType.LandBridge,
			TerrainType.Lava,
			TerrainType.Vent,
			TerrainType.Ruin
		};

		// Token: 0x040003AC RID: 940
		private TurnContext _context;

		// Token: 0x040003AD RID: 941
		private readonly HashSet<PathfinderHexboard.GamePiecePosition> LegionHexes = new HashSet<PathfinderHexboard.GamePiecePosition>();

		// Token: 0x040003AE RID: 942
		private readonly HashSet<PathfinderHexboard.GamePiecePosition> FixtureHexes = new HashSet<PathfinderHexboard.GamePiecePosition>();

		// Token: 0x040003AF RID: 943
		private readonly Dictionary<Identifier, Dictionary<TerrainType, AITerrainCostData>> TerrainCostsPerUnit = new Dictionary<Identifier, Dictionary<TerrainType, AITerrainCostData>>();

		// Token: 0x040003B0 RID: 944
		public readonly HashSet<Identifier> LegionsInFumes = new HashSet<Identifier>();

		// Token: 0x040003B1 RID: 945
		public readonly HashSet<HexCoord> TileInFumes = new HashSet<HexCoord>();

		// Token: 0x02000876 RID: 2166
		private struct GamePiecePosition
		{
			// Token: 0x06002825 RID: 10277 RVA: 0x00085AFC File Offset: 0x00083CFC
			public GamePiecePosition(HexCoord coord, int playerId)
			{
				this.Coord = coord;
				this.PlayerId = playerId;
			}

			// Token: 0x04001218 RID: 4632
			public HexCoord Coord;

			// Token: 0x04001219 RID: 4633
			public int PlayerId;
		}

		// Token: 0x02000877 RID: 2167
		public class HexboardDestinationChecker : IPathDestinationChecker<PFNodeHexCoord>
		{
			// Token: 0x06002826 RID: 10278 RVA: 0x00085B0C File Offset: 0x00083D0C
			public HexboardDestinationChecker(Func<HexCoord, bool> isEnd)
			{
				this._isEnd = isEnd;
			}

			// Token: 0x06002827 RID: 10279 RVA: 0x00085B1B File Offset: 0x00083D1B
			public bool IsDestination(PFNodeHexCoord node)
			{
				return this._isEnd(node.Location);
			}

			// Token: 0x0400121A RID: 4634
			private Func<HexCoord, bool> _isEnd;
		}
	}
}
