using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000671 RID: 1649
	public class ElocutionRitualProcessor : TargetedRitualActionProcessor<ElocutionRitualOrder, ElocutionRitualData, RitualCastEvent>
	{
		// Token: 0x06001E69 RID: 7785 RVA: 0x00068E58 File Offset: 0x00067058
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			RitualCastEvent ritualCastEvent;
			Problem problem = base.CheckPlayerRitualResistance(base.request.TargetPlayerId, out ritualCastEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			ElocutionActiveRitual elocutionActiveRitual = this.TurnProcessContext.CreateActiveRitual(base.data, base.request, this.CalculateCost(), base.GameEvent.MaskingContext);
			this._player.RitualState.SlottedItems.Add(elocutionActiveRitual.Id);
			elocutionActiveRitual.StartRitual(this.TurnProcessContext, this._player, ritualCastEvent);
			return Result.Success;
		}
	}
}
