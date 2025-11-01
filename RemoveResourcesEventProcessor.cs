using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x02000614 RID: 1556
	public class RemoveResourcesEventProcessor : GrandEventActionProcessor<RemoveResourcesEventOrder, RemoveResourcesEventStaticData>
	{
		// Token: 0x06001CDE RID: 7390 RVA: 0x000639B4 File Offset: 0x00061BB4
		protected override Result ProcessInternal(PlayGrandEventOrder order)
		{
			foreach (PlayerState playerState in base._currentTurn.EnumeratePlayerStates(false, false))
			{
				int count = (int)Math.Round((double)((float)playerState.Resources.Count * base.data.PercentToRemove), MidpointRounding.AwayFromZero);
				IEnumerable<ResourceNFT> random = playerState.Resources.GetRandom(base._random, count);
				PaymentRemovedEvent ev = this.TurnProcessContext.RemovePayment(playerState, new Payment(random, 0), null);
				base.GameEvent.AddChildEvent<PaymentRemovedEvent>(ev);
			}
			return Result.Success;
		}
	}
}
