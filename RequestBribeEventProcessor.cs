using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000616 RID: 1558
	public class RequestBribeEventProcessor : GrandEventActionProcessor<RequestBribeEventOrder, RequestBribeEventStaticData, RequestBribeEvent>
	{
		// Token: 0x06001CE2 RID: 7394 RVA: 0x00063A78 File Offset: 0x00061C78
		protected override Result ProcessInternal(PlayGrandEventOrder order)
		{
			PlayerState playerState = IEnumerableExtensions.ToList<PlayerState>(from x in base._currentTurn.EnumeratePlayerStates(false, false)
			where x.Id != this._player.Id
			select x).WeightedRandom((PlayerState player) => (float)player.OrderSlots, base._random, false);
			Cost cost = new Cost();
			cost.RequiredTokenCount = base._random.Next(base.data.MinTribute, base.data.MaxTribute);
			RequestBribeEventDecisionRequest decisionRequest = new RequestBribeEventDecisionRequest(base._currentTurn, this.AbilityData.Id)
			{
				Cost = cost,
				PrestigeLossPercent = base.data.PrestigeLossPercent,
				PlayerId = playerState.Id
			};
			playerState.AddDecisionRequest(decisionRequest);
			IEnumerable<int> playerIds = from x in IEnumerableExtensions.ExceptFor<PlayerState>(base._currentTurn.EnumeratePlayerStates(false, false), new PlayerState[]
			{
				playerState
			})
			select x.Id;
			base.GameEvent.Cost = cost;
			base.GameEvent.PlayerId = playerState.Id;
			base.GameEvent.AddAffectedPlayerIds(playerIds);
			return Result.Success;
		}
	}
}
