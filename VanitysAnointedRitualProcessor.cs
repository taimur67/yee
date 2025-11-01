using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020006C5 RID: 1733
	public class VanitysAnointedRitualProcessor : TargetedRitualActionProcessor<VanitysAnointedRitualOrder, VanitysAnointedRitualData, RitualCastEvent>
	{
		// Token: 0x06001FB1 RID: 8113 RVA: 0x0006CB94 File Offset: 0x0006AD94
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			GameItem item = base._currentTurn.FetchGameItem(base.request.TargetItemId);
			RitualCastEvent ritualCastEvent;
			Problem problem = base.CheckGameItemRitualResistance(item, base.request.TargetPlayerId, out ritualCastEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			VanitysAnointedActiveRitual vanitysAnointedActiveRitual = this.TurnProcessContext.CreateActiveRitual(base.data, base.request, this.CalculateCost(), base.GameEvent.MaskingContext);
			this._player.RitualState.SlottedItems.Add(vanitysAnointedActiveRitual.Id);
			vanitysAnointedActiveRitual.StartRitual(this.TurnProcessContext, this._player, ritualCastEvent);
			return Result.Success;
		}
	}
}
