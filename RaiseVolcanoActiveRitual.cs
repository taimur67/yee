using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002DE RID: 734
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class RaiseVolcanoActiveRitual : ActiveRitual
	{
		// Token: 0x06000E58 RID: 3672 RVA: 0x00039038 File Offset: 0x00037238
		public override Result StartRitual(TurnProcessContext context, PlayerState player, RitualCastEvent ritualCastEvent)
		{
			this.GameEvent = ritualCastEvent;
			context.HexBoard[base.TargetContext.Location].SetTerrainType(TerrainType.Volcano);
			TurnState currentTurn = context.CurrentTurn;
			foreach (HexCoord hexCoord in context.HexBoard.GetNeighbours(base.TargetContext.Location, false))
			{
				if (context.HexBoard[hexCoord].Type == TerrainType.Plain)
				{
					GamePiece gamePieceAt = currentTurn.GetGamePieceAt(hexCoord);
					if (gamePieceAt == null || !gamePieceAt.IsFixture())
					{
						this._lavaHexes.Add(hexCoord);
						context.HexBoard[hexCoord].SetTerrainType(TerrainType.Lava);
					}
				}
			}
			GamePiece gamePieceAt2 = currentTurn.GetGamePieceAt(base.TargetContext.Location);
			if (gamePieceAt2 != null && gamePieceAt2.IsLegionOrTitan())
			{
				LegionMoveEvent legionMoveEvent = LegionMovementProcessor.RetreatTeleport(context, gamePieceAt2, base.TargetContext.Location);
				if (legionMoveEvent != null)
				{
					BattleProcessor.DamageEvent ev = context.DealDamage(gamePieceAt2, player.Id, 2, DamageType.True, false, AttackOutcomeIntent.Default);
					this.GameEvent.AddChildEvent<LegionMoveEvent>(legionMoveEvent);
					this.GameEvent.AddChildEvent<BattleProcessor.DamageEvent>(ev);
				}
				else
				{
					LegionKilledEvent ev2 = context.KillGamePiece(gamePieceAt2, player.Id);
					this.GameEvent.AddChildEvent<LegionKilledEvent>(ev2);
				}
				this.GameEvent.AddAffectedPlayerId(gamePieceAt2.ControllingPlayerId);
			}
			return base.StartRitual(context, player, ritualCastEvent);
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x000391A0 File Offset: 0x000373A0
		public override Result EndRitual(TurnProcessContext context, PlayerState player, ItemBanishedEvent banishedEvent)
		{
			context.HexBoard[base.TargetContext.Location].SetTerrainType(this.InitialTerrainType);
			foreach (HexCoord coord in this._lavaHexes)
			{
				context.HexBoard[coord].SetTerrainType(TerrainType.Plain);
			}
			return base.EndRitual(context, player, banishedEvent);
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x00039228 File Offset: 0x00037428
		public override Result ValidateRitualTargetHex(TurnProcessContext context, PlayerState caster, Hex target)
		{
			Problem problem = base.ValidateRitualTargetHex(context, caster, target) as Problem;
			if (problem != null)
			{
				return problem;
			}
			if (target.Type != TerrainType.Volcano)
			{
				return Result.Failure;
			}
			return Result.Success;
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x00039260 File Offset: 0x00037460
		public sealed override void DeepClone(out GameItem gameItem)
		{
			RaiseVolcanoActiveRitual raiseVolcanoActiveRitual = new RaiseVolcanoActiveRitual();
			base.DeepCloneActiveRitualParts(raiseVolcanoActiveRitual);
			raiseVolcanoActiveRitual._lavaHexes = this._lavaHexes.DeepClone();
			raiseVolcanoActiveRitual.InitialTerrainType = this.InitialTerrainType;
			raiseVolcanoActiveRitual.GameEvent = this.GameEvent.DeepClone(CloneFunction.FastClone);
			gameItem = raiseVolcanoActiveRitual;
		}

		// Token: 0x04000657 RID: 1623
		[JsonProperty]
		private List<HexCoord> _lavaHexes = new List<HexCoord>();

		// Token: 0x04000658 RID: 1624
		[JsonProperty]
		public TerrainType InitialTerrainType;

		// Token: 0x04000659 RID: 1625
		[JsonProperty]
		public RitualCastEvent GameEvent;
	}
}
