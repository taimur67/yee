using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x02000612 RID: 1554
	public class RemoveResourceOfTypeEventProcessor : GrandEventActionProcessor<RemoveResourceOfTypeEventOrder, RemoveResourceOfTypeEventStaticData>
	{
		// Token: 0x06001CD9 RID: 7385 RVA: 0x000638BC File Offset: 0x00061ABC
		protected override Result ProcessInternal(PlayGrandEventOrder order)
		{
			foreach (PlayerState playerState in base._currentTurn.EnumeratePlayerStates(false, false))
			{
				List<ResourceNFT> list = IEnumerableExtensions.ToList<ResourceNFT>(from x in playerState.Resources
				where x.Values[base.data.ResourceType] > 0
				select x);
				int num = (int)Math.Round((double)((float)list.Count * base.data.PercentToRemove), MidpointRounding.AwayFromZero);
				if (num > 0)
				{
					IEnumerable<ResourceNFT> random = list.GetRandom(base._random, num);
					PaymentRemovedEvent ev = this.TurnProcessContext.RemovePayment(playerState, new Payment(random, 0), null);
					base.GameEvent.AddChildEvent<PaymentRemovedEvent>(ev);
				}
			}
			return Result.Success;
		}
	}
}
