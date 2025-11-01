using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Game.Simulation.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200068F RID: 1679
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ReassignTerritoryRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001ED1 RID: 7889 RVA: 0x0006A16A File Offset: 0x0006836A
		public ReassignTerritoryRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001ED2 RID: 7890 RVA: 0x0006A177 File Offset: 0x00068377
		public ReassignTerritoryRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001ED3 RID: 7891 RVA: 0x0006A18B File Offset: 0x0006838B
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			ReassignTerritoryRitualData data = database.Fetch<ReassignTerritoryRitualData>(base.RitualId);
			yield return new ActionPhase_TargetHex(delegate(HexCoord x)
			{
				this.TargetContext.SetTargetHex(x);
			}, new ActionPhase_Target<HexCoord>.IsValidFunc(this.IsValidHex), 1);
			if (data.ReassignMode == ReassignMode.AssignToSelected)
			{
				yield return new ActionPhase_TargetArchfiend(delegate(int x)
				{
					this.BeneficiaryPlayerId = x;
				}, new ActionPhase_SingleTarget<int>.IsValidFunc(this.IsValidBeneficiary));
			}
			yield break;
		}

		// Token: 0x06001ED4 RID: 7892 RVA: 0x0006A1A4 File Offset: 0x000683A4
		public override Result IsValidHex(TurnContext context, List<HexCoord> selected, HexCoord hexCoord, int castingPlayerId)
		{
			Problem problem = base.IsValidHex(context, selected, hexCoord, castingPlayerId) as Problem;
			if (problem != null)
			{
				return problem;
			}
			AbilityStaticData dataForRequest = context.GetDataForRequest(this);
			if (!(dataForRequest is IPlayerTargetAbility))
			{
				return new SimulationError("Expected IPlayerTargetAbility data but could not find it.");
			}
			bool beneficiaryIsDefined = this.BeneficiaryPlayerId != int.MinValue;
			if (!context.HexBoard.GetNeighbours(hexCoord, false).Any(delegate(HexCoord neighbour)
			{
				int ownership = context.HexBoard.GetOwnership(neighbour);
				return (!beneficiaryIsDefined || ownership == this.BeneficiaryPlayerId) && this.IsValidBeneficiary(context, context.HexBoard.GetOwnership(neighbour), castingPlayerId, hexCoord);
			}))
			{
				return new Result.CastRitualTargetIsBorderProblem(dataForRequest.ConfigRef, hexCoord, this.BeneficiaryPlayerId, true);
			}
			return Result.Success;
		}

		// Token: 0x06001ED5 RID: 7893 RVA: 0x0006A276 File Offset: 0x00068476
		private Result IsValidBeneficiary(TurnContext context, int targetPlayerId, int castingPlayerId)
		{
			return this.IsValidBeneficiary(context, targetPlayerId, castingPlayerId, HexCoord.Invalid);
		}

		// Token: 0x06001ED6 RID: 7894 RVA: 0x0006A288 File Offset: 0x00068488
		private Result IsValidBeneficiary(TurnContext context, int targetPlayerId, int castingPlayerId, HexCoord targetHexCoord)
		{
			if (targetHexCoord == HexCoord.Invalid)
			{
				HexCoord targetHex = base.TargetHex;
				if (targetHex == HexCoord.Invalid)
				{
					return new SimulationError(string.Format("IsValidBeneficiary is being invoked with invalid HexCoord {0}.", targetHexCoord));
				}
				targetHexCoord = base.TargetHex;
			}
			AbilityStaticData dataForRequest = context.GetDataForRequest(this);
			if (!(dataForRequest is IPlayerTargetAbility))
			{
				return new SimulationError("Expected IPlayerTargetAbility data but could not find it.");
			}
			HexBoard hexBoard = context.HexBoard;
			if (hexBoard.GetOwnership(targetHexCoord) == targetPlayerId)
			{
				return new Result.CastRitualOnPlayerProblem(dataForRequest.ConfigRef, targetPlayerId);
			}
			if (!hexBoard.CantonBordersPlayersRealm(targetPlayerId, targetHexCoord, false))
			{
				return new Result.CastRitualNoAdjacentHexOwnedProblem(dataForRequest.ConfigRef, targetPlayerId, targetHexCoord);
			}
			if (targetPlayerId != castingPlayerId)
			{
				Problem problem = base.IsValidArchfiend(context, targetPlayerId, castingPlayerId) as Problem;
				if (problem != null)
				{
					return problem;
				}
			}
			return Result.Success;
		}

		// Token: 0x04000CE5 RID: 3301
		[JsonProperty]
		[DefaultValue(-2147483648)]
		public int BeneficiaryPlayerId = int.MinValue;
	}
}
