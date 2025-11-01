using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000683 RID: 1667
	public class OngoingArchfiendModifierRitualProcessor : TargetedRitualActionProcessor<ModifyArchfiendRitualOrder, ModifyArchfiendRitualData, RitualCastEvent>
	{
		// Token: 0x06001EA8 RID: 7848 RVA: 0x00069A14 File Offset: 0x00067C14
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			RitualCastEvent ritualCastEvent;
			Problem problem = base.CheckPlayerRitualResistance(base.request.TargetPlayerId, out ritualCastEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			ArchfiendModifierActiveRitual archfiendModifierActiveRitual = this.TurnProcessContext.CreateActiveRitual(base.data, base.request, this.CalculateCost(), base.GameEvent.MaskingContext);
			this._player.RitualState.SlottedItems.Add(archfiendModifierActiveRitual.Id);
			archfiendModifierActiveRitual.StartRitual(this.TurnProcessContext, this._player, ritualCastEvent);
			return Result.Success;
		}
	}
}
