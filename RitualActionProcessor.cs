using System;
using System.Linq;
using Game.Simulation.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020005C6 RID: 1478
	public abstract class RitualActionProcessor<T, Q> : ActionProcessor<T, Q>, IRitualProcessor where T : CastRitualOrder, new() where Q : RitualStaticData
	{
		// Token: 0x06001BB2 RID: 7090 RVA: 0x0005FCDD File Offset: 0x0005DEDD
		public override Result IsAvailable()
		{
			if (!this._player.BlockRitualTable)
			{
				return Result.Success;
			}
			return Result.Failure;
		}

		// Token: 0x06001BB3 RID: 7091 RVA: 0x0005FCFC File Offset: 0x0005DEFC
		public override Result Validate()
		{
			if (this._player.BlockRitualTable)
			{
				return new Result.RitualTableBlockedProblem(base.data.ConfigRef);
			}
			if (!base._ritualState.HasSpace)
			{
				return new Result.NoFreeRitualSlotProblem(base.data.ConfigRef);
			}
			switch (base.request.RitualMaskingSettings.MaskingMode)
			{
			case RitualMaskingMode.Masked:
				if (!this._player.IsRitualMaskingAvailable())
				{
					return new Result.RitualMaskingUnavailableProblem(base.data.ConfigRef);
				}
				break;
			case RitualMaskingMode.Framed:
				if (!this._player.IsRitualFramingAvailable())
				{
					return new Result.RitualFramingUnavailableProblem(base.data.ConfigRef);
				}
				break;
			}
			if (!base._currentTurn.GetAvailableRituals(base._database, this._player, true).All((RitualStaticData ritual) => ritual.Id != base.data.Id))
			{
				return Result.Success;
			}
			if (base._database.IsArchfiendSpecificAbility(base.data.ConfigRef))
			{
				return Result.Success;
			}
			if (base.data.RitualType != RitualType.Artifact)
			{
				return new Result.PowerTooLowForRitualProblem(base.data.ConfigRef, base.data.Category);
			}
			return new Result.ArtifactNotEquippedProblem(base.data.ConfigRef);
		}

		// Token: 0x06001BB4 RID: 7092 RVA: 0x0005FE70 File Offset: 0x0005E070
		public override Result Preview(ActionProcessContext context)
		{
			AbilityPlaceholder abilityPlaceholder = this.CreateToken(base.request);
			return this.TurnProcessContext.AttachItemToRitualSlot(this._player, abilityPlaceholder.Id);
		}

		// Token: 0x06001BB5 RID: 7093 RVA: 0x0005FEA1 File Offset: 0x0005E0A1
		public override Result Process(ActionProcessContext context)
		{
			return this.Validate();
		}

		// Token: 0x06001BB6 RID: 7094 RVA: 0x0005FEA9 File Offset: 0x0005E0A9
		protected virtual AbilityPlaceholder CreateToken(T request)
		{
			return base._currentTurn.AddGameItem<AbilityPlaceholder>().SetId(request.ActionInstanceId).SetStaticDataId(request.RitualId).SetItemType(GameItemCategory.ActiveRitual).SetAttachableTo(SlotType.Ritual);
		}

		// Token: 0x06001BB7 RID: 7095 RVA: 0x0005FEE2 File Offset: 0x0005E0E2
		public virtual RitualMaskingMode GetRitualMaskingMode()
		{
			return RitualMaskingMode.NoMasking;
		}

		// Token: 0x06001BB8 RID: 7096 RVA: 0x0005FEE8 File Offset: 0x0005E0E8
		public override Cost CalculateCost()
		{
			Cost cost = this.CalculateBaseCost();
			Cost a = cost * this._player.RitualCostMultiplier.Value;
			Cost cost2 = new Cost();
			cost2[ResourceTypes.Souls] = ((cost[ResourceTypes.Souls] > 0) ? this._player.RitualCostOffset : 0);
			cost2[ResourceTypes.Ichor] = ((cost[ResourceTypes.Ichor] > 0) ? this._player.RitualCostOffset : 0);
			cost2[ResourceTypes.Hellfire] = ((cost[ResourceTypes.Hellfire] > 0) ? this._player.RitualCostOffset : 0);
			cost2[ResourceTypes.Darkness] = ((cost[ResourceTypes.Darkness] > 0) ? this._player.RitualCostOffset : 0);
			Cost cost3 = a + cost2;
			RitualMaskingMode ritualMaskingMode = this.GetRitualMaskingMode();
			if (ritualMaskingMode == RitualMaskingMode.NoMasking)
			{
				return cost3;
			}
			RitualsEconomyData ritualsEconomyData = base._database.FetchSingle<RitualsEconomyData>();
			Cost a2 = cost3;
			CostStaticData costStaticData = (ritualMaskingMode == RitualMaskingMode.Masked) ? ritualsEconomyData.MaskingCost : ritualsEconomyData.FramingCost;
			return a2 + costStaticData;
		}
	}
}
