using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x02000610 RID: 1552
	public class RemovePrestigeEventProcessor : GrandEventActionProcessor<RemovePrestigeEventOrder, RemovePrestigeEventStaticData>
	{
		// Token: 0x06001CD4 RID: 7380 RVA: 0x000637B8 File Offset: 0x000619B8
		protected override Result ProcessInternal(PlayGrandEventOrder order)
		{
			List<PlayerState> list = IEnumerableExtensions.ToList<PlayerState>(from x in base._currentTurn.EnumeratePlayerStates(false, false)
			where x.Id != this._player.Id
			select x);
			if (IEnumerableExtensions.Any<PlayerState>(list))
			{
				PlayerState playerState = list.WeightedRandom((PlayerState player) => (float)player.OrderSlots, base._random, false);
				int prestige = base._random.Next(base.data.PrestigeMin, base.data.PrestigeMax);
				PaymentRemovedEvent paymentRemovedEvent = this.TurnProcessContext.RemovePayment(playerState, new Payment
				{
					Prestige = prestige
				}, null);
				paymentRemovedEvent.Visibility = GameEventVisibility.Public;
				base.GameEvent.AddChildEvent<PaymentRemovedEvent>(paymentRemovedEvent);
				base.GameEvent.AddAffectedPlayerId(playerState.Id);
				return Result.Success;
			}
			return new NoValidTargetsProblem();
		}
	}
}
