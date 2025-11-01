using System;
using System.Linq;
using Game.StaticData;

namespace LoG
{
	// Token: 0x0200068E RID: 1678
	public class RaiseVolcanoRitualProcessor : TargetedRitualActionProcessor<RaiseVolcanoRitualOrder, ChangeTerrainTypeRitualData, RitualCastEvent>
	{
		// Token: 0x06001ECE RID: 7886 RVA: 0x0006A00C File Offset: 0x0006820C
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			int ownership = base._currentTurn.HexBoard.GetOwnership(base.request.TargetHex);
			RitualCastEvent ritualCastEvent;
			Problem problem = base.CheckPlayerRitualResistance(ownership, out ritualCastEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			if (base._currentTurn.HexBoard[base.request.TargetHex].Type != TerrainType.Plain)
			{
				return Result.Failure;
			}
			RaiseVolcanoActiveRitual raiseVolcanoActiveRitual = this.TurnProcessContext.CreateActiveRitual(base.data, base.request, this.CalculateCost(), base.GameEvent.MaskingContext);
			this._player.RitualState.SlottedItems.Add(raiseVolcanoActiveRitual.Id);
			raiseVolcanoActiveRitual.InitialTerrainType = base._currentTurn.HexBoard[base.request.TargetHex].Type;
			raiseVolcanoActiveRitual.StartRitual(this.TurnProcessContext, this._player, ritualCastEvent);
			this.TurnProcessContext.RecalculateAllModifiersFor(this._player);
			return Result.Success;
		}

		// Token: 0x06001ECF RID: 7887 RVA: 0x0006A108 File Offset: 0x00068308
		protected override int GetPrestigeReward()
		{
			int num = 0;
			if (IEnumerableExtensions.Any<BattleProcessor.DamageEvent>(base.GameEvent.LocalChildEvents.OfType<BattleProcessor.DamageEvent>()))
			{
				num = base.GetPrestigeReward();
			}
			if (IEnumerableExtensions.Any<LegionKilledEvent>(base.GameEvent.LocalChildEvents.OfType<LegionKilledEvent>()))
			{
				num += base.GetPrestigeReward() * base.data.KillPrestigeMultiplier;
			}
			return num;
		}
	}
}
