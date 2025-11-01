using System;
using System.Linq;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000690 RID: 1680
	public class ReassignTerritoryRitualProcessor : TargetedRitualActionProcessor<ReassignTerritoryRitualOrder, ReassignTerritoryRitualData, RitualCastEvent>
	{
		// Token: 0x06001ED9 RID: 7897 RVA: 0x0006A360 File Offset: 0x00068560
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			int ownership = base._currentTurn.HexBoard.GetOwnership(base.request.TargetHex);
			RitualCastEvent ritualCastEvent;
			Problem problem = base.CheckPlayerRitualResistance(ownership, out ritualCastEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			Hex hex = base._currentTurn.HexBoard[base.request.TargetHex];
			if (hex == null)
			{
				return new Result.CastRitualOnInvalidHexProblem(this.AbilityData.ConfigRef, base.request.TargetHex);
			}
			int existingOwner = base._currentTurn.HexBoard.GetOwnership(base.request.TargetHex);
			ReassignMode reassignMode = base.data.ReassignMode;
			int num;
			if (reassignMode != ReassignMode.AssignRandomly)
			{
				if (reassignMode != ReassignMode.AssignToSelected)
				{
					num = this._player.Id;
				}
				else
				{
					num = base.request.BeneficiaryPlayerId;
				}
			}
			else
			{
				num = (from x in base._currentTurn.PlayerStates
				where x.Id != existingOwner
				select x).GetRandom(base.Random).Id;
			}
			int playerId = num;
			CantonClaimedEvent cantonClaimedEvent = this.TurnProcessContext.ClaimCanton(hex.HexCoord, playerId);
			cantonClaimedEvent.AddAffectedPlayerId(this._player.Id);
			ritualCastEvent.AddChildEvent<CantonClaimedEvent>(cantonClaimedEvent);
			return Result.Success;
		}
	}
}
