using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000696 RID: 1686
	public class RevealVendettaRitualProcessor : TargetedRitualActionProcessor<RevealVendettaRitualOrder, RevealVendettaRitualData, RitualCastEvent>
	{
		// Token: 0x06001EEF RID: 7919 RVA: 0x0006A800 File Offset: 0x00068A00
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			RitualCastEvent ritualCastEvent;
			Problem problem = base.CheckPlayerRitualResistance(base.request.TargetPlayerId, out ritualCastEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			DiplomaticState diplomaticState = base._currentTurn.GetDiplomaticStatus(base.request.VendettaPair).DiplomaticState;
			RevealVendettaEvent gameEvent = new RevealVendettaEvent(this._player.Id, base.request.VendettaPair, diplomaticState as VendettaState);
			base._currentTurn.AddGameEvent<RevealVendettaEvent>(gameEvent);
			return Result.Success;
		}
	}
}
