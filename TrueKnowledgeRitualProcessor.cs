using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020006C1 RID: 1729
	public class TrueKnowledgeRitualProcessor : TargetedRitualActionProcessor<TrueKnowledgeRitualOrder, TrueKnowledgeRitualData, RitualCastEvent>
	{
		// Token: 0x06001FA0 RID: 8096 RVA: 0x0006C8C8 File Offset: 0x0006AAC8
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			RitualCastEvent ritualCastEvent;
			Problem problem = base.CheckPlayerRitualResistance(base.request.TargetPlayerId, out ritualCastEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			TrueKnowledgeActiveRitual trueKnowledgeActiveRitual = this.TurnProcessContext.CreateActiveRitual(base.data, base.request, this.CalculateCost(), base.GameEvent.MaskingContext);
			this._player.RitualState.SlottedItems.Add(trueKnowledgeActiveRitual.Id);
			trueKnowledgeActiveRitual.VulnerabilityValue = base.data.RitualStrengthIncrease;
			trueKnowledgeActiveRitual.StartRitual(this.TurnProcessContext, this._player, ritualCastEvent);
			return Result.Success;
		}
	}
}
