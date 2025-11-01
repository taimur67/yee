using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;
using LoG.Simulation.Extensions;

namespace LoG
{
	// Token: 0x02000400 RID: 1024
	public class AbyssalStriderProcessor : NeutralForceTurnModuleProcessor<AbyssalStriderTurnModuleInstance, AbyssalStriderTurnModuleStaticData>
	{
		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06001461 RID: 5217 RVA: 0x0004DC57 File Offset: 0x0004BE57
		public GamePiece Strider
		{
			get
			{
				return base._currentTurn.FetchGameItem<GamePiece>(base.Instance.GamePieceId);
			}
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x0004DC6F File Offset: 0x0004BE6F
		public static int CarryingCapacity(TurnState turn, AbyssalStriderTurnModuleStaticData data)
		{
			return (int)Math.Round((double)(data.SpawnsPerNeutralCanton * (float)turn.HexBoard.GetNeutralHexes().Count<Hex>() * (float)turn.StriderCarryCapacityMultiplier));
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x0004DC98 File Offset: 0x0004BE98
		public static int PopulationCount(TurnState turn, AbyssalStriderTurnModuleStaticData data)
		{
			return turn.EnumerateAllGamePieces().Count((GamePiece x) => x.StaticDataId == data.GamePieceRef.Id && x.IsAlive());
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06001464 RID: 5220 RVA: 0x0004DCC9 File Offset: 0x0004BEC9
		public int TurnsSinceOffspringSpawned
		{
			get
			{
				return base._currentTurn.TurnValue - base.Instance.LastSpawnTurn;
			}
		}

		// Token: 0x06001465 RID: 5221 RVA: 0x0004DCE4 File Offset: 0x0004BEE4
		public static int SpawnPeriod(TurnState turn, AbyssalStriderTurnModuleStaticData data)
		{
			int num = AbyssalStriderProcessor.PopulationCount(turn, data);
			float num2 = (float)AbyssalStriderProcessor.CarryingCapacity(turn, data) * 0.5f;
			if ((float)num < num2)
			{
				double num3 = Math.Pow((double)((float)num / num2), 1.0);
				return (int)Math.Ceiling((double)data.BaseSpawnPeriod * num3);
			}
			float num4 = (float)num - num2;
			double num5 = Math.Pow(2.0, (double)num4);
			return (int)Math.Ceiling((double)data.BaseSpawnPeriod + num5);
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x0004DD58 File Offset: 0x0004BF58
		public override void Initialize()
		{
			base.Initialize();
			base.SubscribeTo(TurnProcessStage.TurnModule_NeutralForces, new TurnModuleProcessor.ProcessEvent(this.Process));
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x0004DD74 File Offset: 0x0004BF74
		public override void OnAdded()
		{
			base.Instance.LastSpawnTurn = base._currentTurn.TurnValue - base._currentTurn.Random.Next(0, AbyssalStriderProcessor.SpawnPeriod(base._currentTurn, this._staticData));
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x0004DDB0 File Offset: 0x0004BFB0
		private void TrySpawnOffspring()
		{
			if (!this.Strider.IsAlive())
			{
				return;
			}
			int num = AbyssalStriderProcessor.PopulationCount(base._currentTurn, this._staticData);
			int num2 = AbyssalStriderProcessor.CarryingCapacity(base._currentTurn, this._staticData);
			if (num >= num2)
			{
				base.Instance.LastSpawnTurn = base._currentTurn.TurnValue;
				return;
			}
			int num3 = AbyssalStriderProcessor.SpawnPeriod(base._currentTurn, this._staticData);
			if (this.TurnsSinceOffspringSpawned <= num3)
			{
				return;
			}
			HexCoord[] array = IEnumerableExtensions.ToArray<HexCoord>((from hexCoord in base._currentTurn.HexBoard.GetNeighbours(this.Strider.Location, false)
			where this.TurnProcessContext.IsValidSpawnPoint(base._currentTurn.ForceMajeurePlayer, hexCoord, null)
			select hexCoord).Where(new Func<HexCoord, bool>(this.IsValidRoamTarget)));
			if (!IEnumerableExtensions.Any<HexCoord>(array))
			{
				return;
			}
			HexCoord location = array.GetRandom(this.TurnProcessContext.Random);
			location = base._currentTurn.HexBoard.ToRelativeHex(location);
			GamePieceStaticData staticData = base._database.Fetch<GamePieceStaticData>(this._staticData.GamePieceRef.Id);
			GamePiece gamePiece = this.TurnProcessContext.SpawnLegionWithBehaviour(staticData, base._currentTurn.ForceMajeurePlayer, location, this._staticData);
			LegionSpawnedEvent gameEvent = new LegionSpawnedEvent(-1, gamePiece.Id, location, LegionSpawnedEvent.LegionSpawnType.Event, gamePiece);
			base._currentTurn.AddGameEvent<LegionSpawnedEvent>(gameEvent);
			base.Instance.LastSpawnTurn = base._currentTurn.TurnValue;
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x0004DF0C File Offset: 0x0004C10C
		protected virtual void Process()
		{
			if (this.Strider == null)
			{
				base.RemoveSelf();
				return;
			}
			if (this.Strider == null || !this.Strider.IsAlive())
			{
				this.Cleanup();
				return;
			}
			List<GamePiece> list = IEnumerableExtensions.ToList<GamePiece>(this.GetPotentialTargets());
			List<HexCoord> list2;
			if (this.GetNextValidAttackTarget(list, out list2) != null)
			{
				this.AttackTarget(list2);
				this.TrySpawnOffspring();
				return;
			}
			List<GamePiece> list3 = IEnumerableExtensions.ToList<GamePiece>(list.Where(delegate(GamePiece x)
			{
				List<HexCoord> list4;
				return this.IsValidFleeTarget(x, out list4);
			}));
			HexCoord random;
			if (IEnumerableExtensions.Any<GamePiece>(list3))
			{
				List<HexCoord> source = IEnumerableExtensions.ToList<HexCoord>(list3.Select(delegate(GamePiece x)
				{
					CubeCoord cubeCoord = base._currentTurn.HexBoard.Diff(x.Location, this.Strider.Location);
					return (HexCoord)cubeCoord;
				}));
				Vector2 left = new Vector2((float)source.Average((HexCoord x) => x.row), (float)source.Average((HexCoord x) => x.column));
				Vector2 vector = new Vector2((float)this.Strider.Location.row, (float)this.Strider.Location.column) - left * (float)this.Strider.GroundMoveDistance * 2f;
				random = new HexCoord((int)Math.Round((double)vector.X), (int)Math.Round((double)vector.Y));
			}
			else
			{
				random = base._currentTurn.HexBoard.EnumerateRange(this.Strider.Location, this.Strider.GroundMoveDistance).Where(new Func<HexCoord, bool>(this.IsValidRoamTarget)).GetRandom(base._currentTurn.Random);
			}
			if (new PathfinderHexboard(this.TurnProcessContext).TryFindPath(this.Strider.Location, random, new PFAgentGamePiece(this.Strider), out list2))
			{
				LegionMoveEvent gameEvent = LegionMovementProcessor.GroundMove(this.TurnProcessContext, this.Strider.Id, list2, AttackOutcomeIntent.Default);
				base._currentTurn.AddGameEvent<LegionMoveEvent>(gameEvent);
			}
			this.TrySpawnOffspring();
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x0004E118 File Offset: 0x0004C318
		private IEnumerable<GamePiece> GetPotentialTargets()
		{
			return from x in (from x in base._currentTurn.HexBoard.EnumerateRange(this.Strider.Location, this.Strider.GroundMoveDistance)
			select base._currentTurn.GetGamePieceAt(x)).ExcludeNull<GamePiece>()
			where x.IsLegionOrTitan()
			where x.ControllingPlayerId != -1
			select x;
		}

		// Token: 0x0600146B RID: 5227 RVA: 0x0004E1B0 File Offset: 0x0004C3B0
		private bool IsValidRoamTarget(HexCoord hexCoord)
		{
			Hex hex = base._currentTurn.HexBoard[hexCoord];
			TerrainStaticData terrainStaticData;
			if (hex == null || !base._database.TryFindTerrainData(hex, out terrainStaticData))
			{
				return false;
			}
			if (!LegionMovementProcessor.IsWalkable(terrainStaticData, this.Strider))
			{
				return false;
			}
			return !(from configRef in terrainStaticData.ProvidedAbilities
			select base._database.Fetch(configRef) into abilityStaticData
			where abilityStaticData != null
			select abilityStaticData).SelectMany((ItemAbilityStaticData abilityStaticData) => abilityStaticData.Effects).Any((AbilityEffect effect) => effect is TurnEffect_Damage);
		}

		// Token: 0x0600146C RID: 5228 RVA: 0x0004E27A File Offset: 0x0004C47A
		private bool IsValidFleeTarget(GamePiece target, out List<HexCoord> path)
		{
			return this.IsValidTarget(target, out path) && !this.CanDefeat(target);
		}

		// Token: 0x0600146D RID: 5229 RVA: 0x0004E292 File Offset: 0x0004C492
		private bool IsValidAttackTarget(GamePiece target, out List<HexCoord> path)
		{
			return this.IsValidTarget(target, out path) && this.CanDefeat(target);
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x0004E2A8 File Offset: 0x0004C4A8
		private bool IsValidTarget(GamePiece target, out List<HexCoord> path)
		{
			path = new List<HexCoord>();
			return target.IsAlive() && target.Status == GameItemStatus.InPlay && target.ControllingPlayerId != -1 && new PathfinderHexboard(this.TurnProcessContext).TryFindPath(this.Strider.Location, target.Location, new PFAgentGamePiece(this.Strider), out path) && path.Count - 1 <= this.Strider.GroundMoveDistance;
		}

		// Token: 0x0600146F RID: 5231 RVA: 0x0004E32C File Offset: 0x0004C52C
		private GamePiece GetNextValidAttackTarget(IEnumerable<GamePiece> potentialTargets, out List<HexCoord> path)
		{
			foreach (GamePiece gamePiece in potentialTargets)
			{
				if (this.IsValidAttackTarget(gamePiece, out path))
				{
					return gamePiece;
				}
			}
			path = new List<HexCoord>();
			return null;
		}

		// Token: 0x06001470 RID: 5232 RVA: 0x0004E388 File Offset: 0x0004C588
		private bool CanDefeat(GamePiece target)
		{
			return GamePiece.CalcCombatAdvantageAtPosition(this.TurnProcessContext, this.Strider, target) > 0.5f;
		}

		// Token: 0x06001471 RID: 5233 RVA: 0x0004E3A3 File Offset: 0x0004C5A3
		protected void Cleanup()
		{
			if (this.Strider != null && this.Strider.Status != GameItemStatus.Banished)
			{
				this.TurnProcessContext.BanishGameItem(base.Instance.GamePieceId, int.MinValue);
			}
			base.RemoveSelf();
		}

		// Token: 0x06001472 RID: 5234 RVA: 0x0004E3E0 File Offset: 0x0004C5E0
		private void AttackTarget(List<HexCoord> path)
		{
			LegionMoveEvent gameEvent = LegionMovementProcessor.GroundMove(this.TurnProcessContext, this.Strider.Id, path, AttackOutcomeIntent.Default);
			base._currentTurn.AddGameEvent<LegionMoveEvent>(gameEvent);
		}
	}
}
