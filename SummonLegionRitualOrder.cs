using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020006A1 RID: 1697
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class SummonLegionRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001F31 RID: 7985 RVA: 0x0006BA6A File Offset: 0x00069C6A
		public SummonLegionRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001F32 RID: 7986 RVA: 0x0006BA77 File Offset: 0x00069C77
		public SummonLegionRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001F33 RID: 7987 RVA: 0x0006BA8C File Offset: 0x00069C8C
		public override int GetCommandRatingCost(PlayerState user, TurnState turn, AbilityStaticData data, GameDatabase database)
		{
			SummonLegionRitualData summonLegionRitualData = data as SummonLegionRitualData;
			if (summonLegionRitualData == null)
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Error(string.Format("Order {0} should be associated with {1}", this, typeof(SummonLegionRitualData)));
				}
				return 0;
			}
			if (!summonLegionRitualData.PlayerOwned)
			{
				return 0;
			}
			GamePiece gamePiece;
			if (SummonLegionRitualProcessor.IsTargetCurrentlyActive(turn, summonLegionRitualData, out gamePiece) && gamePiece != null)
			{
				SummonLegionDuplicationConstraints duplicationConstraints = summonLegionRitualData.DuplicationConstraints;
				int result;
				if (duplicationConstraints != SummonLegionDuplicationConstraints.TeleportSummonIfAlive)
				{
					if (duplicationConstraints != SummonLegionDuplicationConstraints.ConvertAndTeleportSummonIfAlive)
					{
						result = gamePiece.CommandCost;
					}
					else
					{
						result = ((gamePiece.ControllingPlayerId == user.Id) ? 0 : gamePiece.CommandCost);
					}
				}
				else
				{
					result = 0;
				}
				return result;
			}
			GamePieceStaticData gamePieceStaticData = database.Fetch(summonLegionRitualData.LegionStaticData);
			if (gamePieceStaticData == null)
			{
				return 0;
			}
			return gamePieceStaticData.CommandCost;
		}

		// Token: 0x06001F34 RID: 7988 RVA: 0x0006BB32 File Offset: 0x00069D32
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			SummonLegionRitualData ritualData = database.Fetch<SummonLegionRitualData>(base.RitualId);
			if (ritualData.AllowSelectSpawnPoint)
			{
				yield return new ActionPhase_TargetHex(delegate(HexCoord x)
				{
					this.TargetContext.SetTargetHex(x, turn.HexBoard.GetOwnership(x));
				}, new ActionPhase_Target<HexCoord>.IsValidFunc(this.IsValidHex), 1);
			}
			if (ritualData.PandaemoniumWarningRange > 0)
			{
				yield return new ActionPhase_MessageBox(ActionMessageType.PylonNearPandaemonium, null, null)
				{
					ShowCondition = ((TurnContext x) => this.IsNearPandaemonium(x, ritualData.PandaemoniumWarningRange))
				};
			}
			yield break;
		}

		// Token: 0x06001F35 RID: 7989 RVA: 0x0006BB50 File Offset: 0x00069D50
		private bool IsNearPandaemonium(TurnContext context, int range)
		{
			return context.HexBoard.ShortestDistance(this.TargetContext.Location, context.CurrentTurn.GetPandaemonium().Location) <= range;
		}

		// Token: 0x06001F36 RID: 7990 RVA: 0x0006BB80 File Offset: 0x00069D80
		public override Result IsValidHex(TurnContext context, List<HexCoord> selected, HexCoord hexCoord, int castingPlayerId)
		{
			Problem problem = base.IsValidHex(context, selected, hexCoord, castingPlayerId) as Problem;
			if (problem != null)
			{
				return problem;
			}
			AbilityStaticData dataForRequest = context.GetDataForRequest(this);
			if (!(dataForRequest is IHexTargetAbility))
			{
				return new SimulationError("Expected IHexTargetAbility data but could not find it.");
			}
			Problem problem2 = this.ValidateAuraOverlap(context, hexCoord) as Problem;
			if (problem2 != null)
			{
				return problem2;
			}
			if (!LegionMovementProcessor.IsTraversable(context, null, hexCoord, PathMode.March))
			{
				return new Result.CastRitualOnInvalidHexProblem(dataForRequest.ConfigRef, hexCoord);
			}
			return Result.Success;
		}

		// Token: 0x06001F37 RID: 7991 RVA: 0x0006BBEE File Offset: 0x00069DEE
		public override IEnumerable<ActionConflict> GeneratePotentialConflicts()
		{
			yield return new SummonLegionConflict(this.TargetCoord);
			yield break;
		}

		// Token: 0x06001F38 RID: 7992 RVA: 0x0006BC00 File Offset: 0x00069E00
		public Result ValidateAuraOverlap(TurnContext context, HexCoord hex)
		{
			SummonLegionRitualData dataForRequest = context.GetDataForRequest(this);
			using (IEnumerator<Aura> enumerator = context.CurrentTurn.GetAurasOverlapping(hex).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Aura aura = enumerator.Current;
					if (dataForRequest.AuraBlacklist.Any((ConfigRef<AuraStaticData> x) => x.Id == aura.AuraSourceId))
					{
						return new Result.CastRitualWithinAuraProblem(dataForRequest.ConfigRef, hex, aura.AbilitySourceId);
					}
				}
			}
			return Result.Success;
		}

		// Token: 0x04000CEC RID: 3308
		[JsonProperty]
		[DefaultValue(2147483647)]
		public HexCoord TargetCoord = HexCoord.Invalid;
	}
}
