using System;
using System.Linq;
using Game.Simulation.StaticData;
using Game.StaticData;
using LoG.Simulation.Extensions;

namespace LoG
{
	// Token: 0x020006A2 RID: 1698
	public class SummonLegionRitualProcessor : TargetedRitualActionProcessor<SummonLegionRitualOrder, SummonLegionRitualData, RitualCastEvent>
	{
		// Token: 0x06001F39 RID: 7993 RVA: 0x0006BC9C File Offset: 0x00069E9C
		public override Cost CalculateBaseCost()
		{
			Cost a = this.AbilityData.Cost;
			int a2 = base._currentTurn.GetActiveGamePieces().Count((GamePiece x) => x.StaticDataId == base.data.LegionStaticData.Id);
			CostStaticData costStaticData = a2 * base.data.AdditionalCostPerSummon;
			return a + costStaticData;
		}

		// Token: 0x06001F3A RID: 7994 RVA: 0x0006BCF4 File Offset: 0x00069EF4
		private bool IsTargetCurrentlyActive()
		{
			GamePiece gamePiece;
			return this.IsTargetCurrentlyActive(out gamePiece);
		}

		// Token: 0x06001F3B RID: 7995 RVA: 0x0006BD09 File Offset: 0x00069F09
		private bool IsTargetCurrentlyActive(out GamePiece summonedGamePiece)
		{
			return SummonLegionRitualProcessor.IsTargetCurrentlyActive(base._currentTurn, base.data, out summonedGamePiece);
		}

		// Token: 0x06001F3C RID: 7996 RVA: 0x0006BD20 File Offset: 0x00069F20
		public static bool IsTargetCurrentlyActive(TurnState turn, SummonLegionRitualData data, out GamePiece summonedGamePiece)
		{
			return turn.GetActiveGamePieces().TryFirst(out summonedGamePiece, (GamePiece gamePiece) => gamePiece.StaticDataId == data.LegionStaticData.Id);
		}

		// Token: 0x06001F3D RID: 7997 RVA: 0x0006BD52 File Offset: 0x00069F52
		private bool HasBeenSummonedPreviously()
		{
			return base._currentTurn.DeadItemReferences.Contains(base.data.LegionStaticData) || base._currentTurn.EnumerateAllGamePieces().Any((GamePiece t) => t.StaticDataId == base.data.LegionStaticData.Id);
		}

		// Token: 0x06001F3E RID: 7998 RVA: 0x0006BD8F File Offset: 0x00069F8F
		private bool ShouldConvertGamePiece(GamePiece summonedGamePiece, PlayerState prospectiveOwner)
		{
			return base.data.DuplicationConstraints == SummonLegionDuplicationConstraints.ConvertAndTeleportSummonIfAlive && summonedGamePiece.ControllingPlayerId != prospectiveOwner.Id;
		}

		// Token: 0x06001F3F RID: 7999 RVA: 0x0006BDB4 File Offset: 0x00069FB4
		private bool ShouldTeleportExistingGamePiece(out GamePiece summonedGamePiece)
		{
			SummonLegionDuplicationConstraints duplicationConstraints = base.data.DuplicationConstraints;
			if (duplicationConstraints - SummonLegionDuplicationConstraints.TeleportSummonIfAlive <= 1)
			{
				return this.IsTargetCurrentlyActive(out summonedGamePiece);
			}
			summonedGamePiece = null;
			return false;
		}

		// Token: 0x06001F40 RID: 8000 RVA: 0x0006BDE0 File Offset: 0x00069FE0
		public override Result Validate()
		{
			Problem problem = base.Validate() as Problem;
			if (problem != null)
			{
				return problem;
			}
			bool flag = this.IsTargetCurrentlyActive();
			SummonLegionDuplicationConstraints duplicationConstraints = base.data.DuplicationConstraints;
			if (duplicationConstraints != SummonLegionDuplicationConstraints.OneTimeSummoning)
			{
				if (duplicationConstraints == SummonLegionDuplicationConstraints.AllowRespawnIfSummonIsKilled)
				{
					if (flag)
					{
						return Result.SimulationError("Game piece with id " + base.data.LegionStaticData.Id + " cannot be summoned while another copy is still alive.");
					}
				}
			}
			else if (this.HasBeenSummonedPreviously())
			{
				return Result.SimulationError("Only one game piece with id " + base.data.LegionStaticData.Id + " can ever be summoned.");
			}
			Problem problem2 = base.request.ValidateAuraOverlap(this.TurnProcessContext, base.request.TargetHex) as Problem;
			if (problem2 != null)
			{
				return problem2;
			}
			int commandRatingCost = base.request.GetCommandRatingCost(this._player, base._currentTurn, base.data, base._database);
			int legionCount;
			if (commandRatingCost > 0 && !base._currentTurn.HasSufficientCommandRating(this._player, commandRatingCost, out legionCount, 0))
			{
				return new Result.CommandRatingTooLowToSummonProblem(base.data.ConfigRef, legionCount, this._player.CommandRating);
			}
			return Result.Success;
		}

		// Token: 0x06001F41 RID: 8001 RVA: 0x0006BF00 File Offset: 0x0006A100
		private Result ConvertGamePiece(GamePiece summonedLegion, PlayerState owner, GameEvent ritualCastEvent)
		{
			ritualCastEvent.AddAffectedPlayerId(summonedLegion.ControllingPlayerId);
			GameItemOwnershipChanged ev = new GameItemOwnershipChanged(summonedLegion.ControllingPlayerId, owner.Id, summonedLegion, summonedLegion.Category);
			ritualCastEvent.AddChildEvent<GameItemOwnershipChanged>(ev);
			summonedLegion.ControllingPlayerId = owner.Id;
			summonedLegion.LastConvertedTurn = base._currentTurn.TurnValue;
			summonedLegion.NextUpkeepTurn = base._currentTurn.TurnValue + 1;
			summonedLegion.UseUpkeep(true);
			return Result.Success;
		}

		// Token: 0x06001F42 RID: 8002 RVA: 0x0006BF7C File Offset: 0x0006A17C
		private Result TeleportGamePiece(GamePiece summonedLegion, PlayerState owner, HexCoord relativeHex, GameEvent ritualCastEvent)
		{
			if (this.ShouldConvertGamePiece(summonedLegion, owner))
			{
				this.ConvertGamePiece(summonedLegion, owner, ritualCastEvent);
			}
			int num = summonedLegion.TotalHP - summonedLegion.HP;
			if (num > 0)
			{
				HealGamePieceEvent ev = summonedLegion.Heal(num);
				ritualCastEvent.AddChildEvent<HealGamePieceEvent>(ev);
			}
			if (summonedLegion.Location == relativeHex)
			{
				return Result.Success;
			}
			LegionMoveEvent legionMoveEvent = LegionMovementProcessor.TeleportMove(this.TurnProcessContext, summonedLegion, base.request.TargetHex, true, AttackOutcomeIntent.Default);
			ritualCastEvent.AddChildEvent<LegionMoveEvent>(legionMoveEvent);
			Problem problem = legionMoveEvent.Result as Problem;
			if (problem != null)
			{
				return problem;
			}
			return Result.Success;
		}

		// Token: 0x06001F43 RID: 8003 RVA: 0x0006C01C File Offset: 0x0006A21C
		private Result CreateGamePiece(GamePieceStaticData legionData, PlayerState owner, HexCoord relativeHex, GameEvent ritualCastEvent, out GamePiece summonedLegion)
		{
			summonedLegion = this.TurnProcessContext.SpawnLegion(legionData, owner, relativeHex);
			if (summonedLegion == null)
			{
				return new Result.CastRitualOnInvalidHexProblem(this.AbilityData.ConfigRef, relativeHex);
			}
			LegionSpawnedEvent legionSpawnedEvent = new LegionSpawnedEvent(this._player.Id, summonedLegion.Id, base.request.TargetHex, LegionSpawnedEvent.LegionSpawnType.Ritual, summonedLegion);
			legionSpawnedEvent.AddAffectedPlayerId(base._currentTurn.HexBoard.GetOwnership(base.request.TargetHex));
			ritualCastEvent.AddChildEvent<LegionSpawnedEvent>(legionSpawnedEvent);
			return Result.Success;
		}

		// Token: 0x06001F44 RID: 8004 RVA: 0x0006C0A8 File Offset: 0x0006A2A8
		private Result CreateOrTeleportGamePiece(GamePieceStaticData legionData, PlayerState owner, HexCoord relativeHex, GameEvent ritualCastEvent, out GamePiece summonedGamePiece)
		{
			if (!this.ShouldTeleportExistingGamePiece(out summonedGamePiece))
			{
				return this.CreateGamePiece(legionData, owner, relativeHex, ritualCastEvent, out summonedGamePiece);
			}
			return this.TeleportGamePiece(summonedGamePiece, owner, relativeHex, ritualCastEvent);
		}

		// Token: 0x06001F45 RID: 8005 RVA: 0x0006C0D0 File Offset: 0x0006A2D0
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			HexCoord targetHex = base.request.TargetHex;
			int playerId = (targetHex == HexCoord.Invalid) ? this._player.Id : base._currentTurn.HexBoard.GetOwnership(base.request.TargetHex);
			RitualCastEvent ritualCastEvent;
			Problem problem = base.CheckPlayerRitualResistance(playerId, out ritualCastEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			GamePieceStaticData legionData;
			if (!base._database.TryFetch<GamePieceStaticData>(base.data.LegionStaticData.Id, out legionData))
			{
				return Result.SimulationError("Legion " + base.data.LegionStaticData.Id + " does not exist");
			}
			HexBoard hexBoard = base._currentTurn.HexBoard;
			targetHex = base.request.TargetHex;
			HexCoord hexCoord = hexBoard.ToRelativeHex(targetHex);
			PlayerState playerState = base.data.PlayerOwned ? this._player : base._currentTurn.ForceMajeurePlayer;
			GamePiece gamePiece;
			Problem problem2 = this.CreateOrTeleportGamePiece(legionData, playerState, hexCoord, ritualCastEvent, out gamePiece) as Problem;
			if (problem2 != null)
			{
				return problem2;
			}
			if (base.data.OverrideLevel > gamePiece.Level && !gamePiece.TryForceRandomLevelUp(base._database, base._currentTurn, base.data.OverrideLevel, true))
			{
				return new Result.CastRitualOnInvalidHexProblem(this.AbilityData.ConfigRef, hexCoord);
			}
			if (playerState == base._currentTurn.ForceMajeurePlayer && base.data.Behaviour != null)
			{
				NeutralForceTurnModuleStaticData data = base._database.Fetch(base.data.Behaviour);
				NeutralForceTurnModuleInstance neutralForceTurnModuleInstance = (NeutralForceTurnModuleInstance)TurnModuleInstanceFactory.CreateInstance(base._currentTurn, data);
				base._currentTurn.AddActiveTurnModule(this.TurnProcessContext, neutralForceTurnModuleInstance);
				neutralForceTurnModuleInstance.GamePieceId = gamePiece.Id;
			}
			return Result.Success;
		}
	}
}
