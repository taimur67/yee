using System;
using System.Collections.Generic;
using System.Linq;
using Game.StaticData;

namespace LoG
{
	// Token: 0x0200040D RID: 1037
	public abstract class HostileForceProcessor<T, Q> : NeutralForceTurnModuleProcessor<T, Q> where T : HostileForceTurnModuleInstance where Q : HostileForceTurnModuleStaticData
	{
		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06001492 RID: 5266 RVA: 0x0004E865 File Offset: 0x0004CA65
		protected GamePiece NeutralForce
		{
			get
			{
				return base._currentTurn.FetchGameItem<GamePiece>(base.Instance.GamePieceId);
			}
		}

		// Token: 0x06001493 RID: 5267 RVA: 0x0004E882 File Offset: 0x0004CA82
		public override void Initialize()
		{
			base.Initialize();
			base.SubscribeTo(TurnProcessStage.TurnModule_NeutralForces, new TurnModuleProcessor.ProcessEvent(this.Process));
			base.SubscribeTo(TurnProcessStage.TurnModule_TurnEnd, new TurnModuleProcessor.ProcessEvent(this.OnTurnEnd));
		}

		// Token: 0x06001494 RID: 5268
		protected abstract List<int> CalculatePlayerPriorityList(List<PlayerState> playerStates);

		// Token: 0x06001495 RID: 5269 RVA: 0x0004E8B0 File Offset: 0x0004CAB0
		protected virtual bool TryGetOverrideTarget(out Identifier overrideTarget)
		{
			overrideTarget = Identifier.Invalid;
			return false;
		}

		// Token: 0x06001496 RID: 5270 RVA: 0x0004E8B8 File Offset: 0x0004CAB8
		private void Process()
		{
			int num = base.Instance.CreatedTurn + base.Instance.TurnDurationLimit;
			if (base._currentTurn.TurnValue >= num)
			{
				this.EndEvent();
				return;
			}
			bool flag = false;
			List<HexCoord> list = null;
			Identifier targetId;
			if (this.TryGetOverrideTarget(out targetId))
			{
				flag = this.IsValidTarget(targetId, out list, true);
			}
			if (!flag)
			{
				if (!this.IsValidTarget(base.Instance.CurrentTarget, out list, true))
				{
					if (base.Instance.CurrentTarget != Identifier.Invalid)
					{
						this.MoveToNextPlayer();
					}
					base.Instance.CurrentTarget = Identifier.Invalid;
					GamePiece gamePiece;
					if (this.TryGetNextTarget(out gamePiece, out list, true))
					{
						base.Instance.CurrentTarget = gamePiece.Id;
					}
				}
				if (base.Instance.CurrentTarget == Identifier.Invalid)
				{
					list = null;
					int num2 = base.Instance.CreatedTurn + base.Instance.MinTurnDuration;
					if (base._currentTurn.TurnValue > num2)
					{
						this.EndEvent();
						return;
					}
				}
			}
			if (list != null)
			{
				if (this.TryAndAttackTarget(list) && !flag)
				{
					base.Instance.CurrentTarget = Identifier.Invalid;
					this.MoveToNextPlayer();
					return;
				}
			}
			else
			{
				List<HexCoord> list2 = IEnumerableExtensions.ToList<HexCoord>(base._currentTurn.HexBoard.EnumerateRange(this.NeutralForce.Location, this.NeutralForce.GroundMoveDistance));
				for (int i = list2.Count - 1; i >= 0; i--)
				{
					HexCoord coord = list2[i];
					GamePiece gamePieceAt = base._currentTurn.GetGamePieceAt(coord);
					if (gamePieceAt != null && gamePieceAt.IsFixture())
					{
						list2.RemoveAt(i);
					}
				}
				PathfinderHexboard pathfinderHexboard = new PathfinderHexboard(this.TurnProcessContext);
				while (list2.Count > 0)
				{
					HexCoord end = list2.PopRandom(base._currentTurn.Random);
					List<HexCoord> movePath;
					if (pathfinderHexboard.TryFindPath(this.NeutralForce.Location, end, this.PathFindingAgent(), out movePath))
					{
						LegionMoveEvent gameEvent = LegionMovementProcessor.GroundMove(this.TurnProcessContext, this.NeutralForce.Id, movePath, AttackOutcomeIntent.Default);
						base._currentTurn.AddGameEvent<LegionMoveEvent>(gameEvent);
						return;
					}
				}
			}
		}

		// Token: 0x06001497 RID: 5271 RVA: 0x0004EAF0 File Offset: 0x0004CCF0
		private void OnTurnEnd()
		{
			if (this.NeutralForce == null || !this.NeutralForce.IsAlive())
			{
				this.EndEvent();
				return;
			}
			if (base.Instance.EventEffectId != null)
			{
				base._currentTurn.AddGameEvent<GrandEventContinued>(new GrandEventContinued(base.Instance.EventEffectId, Array.Empty<int>()));
			}
		}

		// Token: 0x06001498 RID: 5272 RVA: 0x0004EB51 File Offset: 0x0004CD51
		private void MoveToNextPlayer()
		{
			if (base.Instance.OrderedPlayerIdsToTarget.Count > 0)
			{
				base.Instance.OrderedPlayerIdsToTarget.RemoveAt(0);
			}
		}

		// Token: 0x06001499 RID: 5273
		protected abstract List<GamePiece> GetPrioritisedTargetList(int playerId);

		// Token: 0x0600149A RID: 5274 RVA: 0x0004EB84 File Offset: 0x0004CD84
		private bool TryGetNextTarget(out GamePiece gamePiece, out List<HexCoord> path, bool pathNeeded)
		{
			while (base.Instance.OrderedPlayerIdsToTarget.Count > 0)
			{
				foreach (GamePiece gamePiece2 in this.GetPrioritisedTargetList(base.Instance.OrderedPlayerIdsToTarget[0]))
				{
					if (this.IsValidTarget(gamePiece2, out path, pathNeeded))
					{
						gamePiece = gamePiece2;
						return true;
					}
				}
				this.MoveToNextPlayer();
			}
			gamePiece = null;
			path = null;
			return false;
		}

		// Token: 0x0600149B RID: 5275 RVA: 0x0004EC24 File Offset: 0x0004CE24
		private void EndEvent()
		{
			this.TurnProcessContext.BanishGameItem(base.Instance.GamePieceId, int.MinValue);
			if (base.Instance.EventEffectId != null)
			{
				base._currentTurn.AddGameEvent<GrandEventEnded>(new GrandEventEnded(base.Instance.EventEffectId, Array.Empty<int>()));
			}
			base.RemoveSelf();
		}

		// Token: 0x0600149C RID: 5276 RVA: 0x0004EC90 File Offset: 0x0004CE90
		private bool IsValidTarget(GamePiece gamePiece, out List<HexCoord> path, bool pathNeeded)
		{
			path = null;
			return gamePiece != null && gamePiece.IsAlive() && gamePiece.Status == GameItemStatus.InPlay && (!pathNeeded || new PathfinderHexboard(this.TurnProcessContext).TryFindPath(this.NeutralForce.Location, gamePiece.Location, this.PathFindingAgent(), out path));
		}

		// Token: 0x0600149D RID: 5277 RVA: 0x0004ECEC File Offset: 0x0004CEEC
		private bool IsValidTarget(Identifier targetId, out List<HexCoord> path, bool pathNeeded)
		{
			if (targetId == Identifier.Invalid)
			{
				path = null;
				return false;
			}
			GamePiece gamePiece = base._currentTurn.FetchGameItem<GamePiece>(targetId);
			return this.IsValidTarget(gamePiece, out path, pathNeeded);
		}

		// Token: 0x0600149E RID: 5278 RVA: 0x0004ED18 File Offset: 0x0004CF18
		protected virtual PFAgentGamePiece PathFindingAgent()
		{
			return new PFAgentGamePiece(this.NeutralForce)
			{
				AvoidanceType = (GamePieceAvoidance.FriendlyFixture | GamePieceAvoidance.EnemyFixture)
			};
		}

		// Token: 0x0600149F RID: 5279 RVA: 0x0004ED30 File Offset: 0x0004CF30
		private bool TryAndAttackTarget(List<HexCoord> path)
		{
			GamePiece item = base._currentTurn.FetchGameItem<GamePiece>(base.Instance.CurrentTarget);
			LegionMoveEvent legionMoveEvent = LegionMovementProcessor.GroundMove(this.TurnProcessContext, this.NeutralForce, path, AttackOutcomeIntent.Default);
			base._currentTurn.AddGameEvent<LegionMoveEvent>(legionMoveEvent);
			if (!legionMoveEvent.Result)
			{
				return false;
			}
			using (IEnumerator<BattleResult> enumerator = (from t in legionMoveEvent.Enumerate<BattleEvent>()
			select t.BattleResult).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsBetween(this.NeutralForce, item))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060014A0 RID: 5280 RVA: 0x0004EE04 File Offset: 0x0004D004
		private HexCoord GetSpawnHexToFirstTarget(List<HexCoord> fallbackCoords)
		{
			GamePiece initialTarget;
			List<HexCoord> list;
			if (this.TryGetNextTarget(out initialTarget, out list, false))
			{
				HexCoord hexCoord;
				if (IEnumerableExtensions.ToList<HexCoord>(from x in base._currentTurn.HexBoard.EnumerateRange(initialTarget.Location, this._staticData.SpawnRadius)
				where this.TurnProcessContext.IsValidSpawnPoint(this._currentTurn.ForceMajeurePlayer, x, null)
				select x).TryGetRandom(base._currentTurn.Random, out hexCoord))
				{
					return base._currentTurn.HexBoard.ToRelativeHex(hexCoord);
				}
				if (fallbackCoords.Count > 0)
				{
					ListExtensions.OrderBy<HexCoord, int>(fallbackCoords, (HexCoord x) => this._currentTurn.HexBoard.ShortestDistance(x, initialTarget.Location));
					HexBoard hexBoard = base._currentTurn.HexBoard;
					HexCoord hexCoord2 = fallbackCoords[0];
					return hexBoard.ToRelativeHex(hexCoord2);
				}
			}
			return HexCoord.Invalid;
		}

		// Token: 0x060014A1 RID: 5281 RVA: 0x0004EED8 File Offset: 0x0004D0D8
		private List<HexCoord> GetPossibleSpawnPoint()
		{
			return IEnumerableExtensions.ToList<HexCoord>(from t in base._currentTurn.HexBoard.GetAllHexes()
			select t.HexCoord into x
			where this.TurnProcessContext.IsValidSpawnPoint(base._currentTurn.ForceMajeurePlayer, x, null)
			select x);
		}

		// Token: 0x060014A2 RID: 5282 RVA: 0x0004EF30 File Offset: 0x0004D130
		public override void OnAdded()
		{
			List<HexCoord> possibleSpawnPoint = this.GetPossibleSpawnPoint();
			if (possibleSpawnPoint.Count <= 0)
			{
				base.RemoveSelf();
				return;
			}
			base.Instance.MinTurnDuration = this._staticData.MinTurnDuration;
			base.Instance.TurnDurationLimit = base._currentTurn.Random.Next(this._staticData.TurnDurationLimitStart, this._staticData.TurnDurationLimitEnd + 1);
			HexCoord location = possibleSpawnPoint[0];
			GamePieceStaticData staticData = base._database.Fetch<GamePieceStaticData>(this._staticData.GamePieceRef.Id);
			GamePiece gamePiece = this.TurnProcessContext.SpawnLegion(staticData, base._currentTurn.ForceMajeurePlayer, location);
			base.Instance.GamePieceId = gamePiece.Id;
			base.Instance.OrderedPlayerIdsToTarget = this.CalculatePlayerPriorityList(IEnumerableExtensions.ToList<PlayerState>(base._currentTurn.EnumeratePlayerStates(false, false)));
			HexCoord spawnHexToFirstTarget = this.GetSpawnHexToFirstTarget(possibleSpawnPoint);
			if (spawnHexToFirstTarget != HexCoord.Invalid)
			{
				location = spawnHexToFirstTarget;
				this.TurnProcessContext.Place(gamePiece, location, false);
			}
			LegionSpawnedEvent gameEvent = new LegionSpawnedEvent(-1, gamePiece.Id, location, LegionSpawnedEvent.LegionSpawnType.Event, gamePiece);
			base._currentTurn.AddGameEvent<LegionSpawnedEvent>(gameEvent);
		}
	}
}
