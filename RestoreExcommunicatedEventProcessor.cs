using System;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000618 RID: 1560
	public class RestoreExcommunicatedEventProcessor : GrandEventActionProcessor<RestoreExcommunicatedEventOrder, RestoreExcommunicatedEventStaticData>
	{
		// Token: 0x06001CE7 RID: 7399 RVA: 0x00063BE8 File Offset: 0x00061DE8
		protected override Result ProcessInternal(PlayGrandEventOrder order)
		{
			int controllingPlayerId = base._currentTurn.GetPandaemonium().ControllingPlayerId;
			foreach (PlayerState playerState in from x in base._currentTurn.EnumeratePlayerStates(false, false)
			where x.Excommunicated
			select x)
			{
				if (controllingPlayerId != playerState.Id)
				{
					base.GameEvent.AddChildEvent(base._currentTurn.CurrentDiplomaticTurn.ReinstateExcommunicatedPlayer(this.TurnProcessContext, playerState));
				}
			}
			return Result.Success;
		}
	}
}
