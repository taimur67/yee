using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;
using Zenject;

namespace LoG
{
	// Token: 0x020006D3 RID: 1747
	public class TurnContext
	{
		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x06001FEF RID: 8175 RVA: 0x0006DC84 File Offset: 0x0006BE84
		public TurnState CurrentTurn
		{
			get
			{
				return this._currentTurn;
			}
		}

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x06001FF0 RID: 8176 RVA: 0x0006DC8C File Offset: 0x0006BE8C
		public GameDatabase Database
		{
			get
			{
				return this._database;
			}
		}

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x06001FF1 RID: 8177 RVA: 0x0006DC94 File Offset: 0x0006BE94
		public GameRules Rules
		{
			get
			{
				return this._rules;
			}
		}

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x06001FF2 RID: 8178 RVA: 0x0006DC9C File Offset: 0x0006BE9C
		public SimulationRandom Random
		{
			get
			{
				return this._currentTurn.Random;
			}
		}

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x06001FF3 RID: 8179 RVA: 0x0006DCA9 File Offset: 0x0006BEA9
		public HexBoard HexBoard
		{
			get
			{
				return this._currentTurn.HexBoard;
			}
		}

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x06001FF4 RID: 8180 RVA: 0x0006DCB6 File Offset: 0x0006BEB6
		public DiplomaticTurnState Diplomacy
		{
			get
			{
				return this._currentTurn.CurrentDiplomaticTurn;
			}
		}

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x06001FF5 RID: 8181 RVA: 0x0006DCC4 File Offset: 0x0006BEC4
		public SimLogger Logger
		{
			get
			{
				SimLogger result;
				if ((result = this._logger) == null)
				{
					result = (this._logger = SimLogger.Logger);
				}
				return result;
			}
		}

		// Token: 0x06001FF6 RID: 8182 RVA: 0x0006DCE9 File Offset: 0x0006BEE9
		public TurnContext()
		{
		}

		// Token: 0x06001FF7 RID: 8183 RVA: 0x0006DCF1 File Offset: 0x0006BEF1
		public TurnContext(GameRules rules, TurnState turn, GameDatabase database)
		{
			this._rules = rules;
			this._currentTurn = turn;
			this._database = database;
		}

		// Token: 0x06001FF8 RID: 8184 RVA: 0x0006DD10 File Offset: 0x0006BF10
		public bool TryFindTeleportDestination(GamePiece legion, HexCoord intendedDestination, out HexCoord teleportToHex, List<HexCoord> blacklist = null)
		{
			teleportToHex = HexCoord.Invalid;
			if (!legion.CanTeleport || legion.TeleportDistance == 0)
			{
				return false;
			}
			int num = legion.TeleportDistance;
			if (this.HexBoard.ShortestDistance(intendedDestination, legion.Location) <= num)
			{
				teleportToHex = intendedDestination;
				return true;
			}
			List<Hex> list = IEnumerableExtensions.ToList<Hex>(from t in this.HexBoard.GetAllHexes()
			where LegionMovementProcessor.CanEnterCanton(this, legion, t.HexCoord, PathMode.Teleport, null)
			select t);
			if (blacklist != null)
			{
				list.RemoveAll((Hex t) => blacklist.Contains(t.HexCoord));
			}
			Dictionary<HexCoord, int> dictionary = new Dictionary<HexCoord, int>();
			foreach (Hex hex in list)
			{
				if (this.HexBoard.ShortestDistance(hex.HexCoord, legion.Location) <= num)
				{
					int value = this.HexBoard.ShortestDistance(hex.HexCoord, intendedDestination);
					dictionary.Add(hex.HexCoord, value);
				}
			}
			List<HexCoord> coord = IEnumerableExtensions.ToList<HexCoord>(from hexDistance in dictionary
			orderby hexDistance.Value
			select hexDistance.Key);
			HexCoord hexCoord = this.CurrentTurn.FirstVacantTraversableHexCoord(coord);
			if (hexCoord != HexCoord.Invalid)
			{
				teleportToHex = hexCoord;
				return true;
			}
			return false;
		}

		// Token: 0x04000D22 RID: 3362
		[Inject]
		protected TurnState _currentTurn;

		// Token: 0x04000D23 RID: 3363
		[Inject]
		protected GameDatabase _database;

		// Token: 0x04000D24 RID: 3364
		[Inject]
		protected GameRules _rules;

		// Token: 0x04000D25 RID: 3365
		[InjectOptional]
		protected SimLogger _logger;
	}
}
