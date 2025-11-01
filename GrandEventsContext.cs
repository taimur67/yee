using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x020006CE RID: 1742
	public class GrandEventsContext
	{
		// Token: 0x06001FE6 RID: 8166 RVA: 0x0006DB23 File Offset: 0x0006BD23
		public void AddGrandEventAction(PlayerGrandEventAction eventAction)
		{
			this.GrandEventOrders.Add(eventAction);
		}

		// Token: 0x04000D1D RID: 3357
		public List<PlayerGrandEventAction> GrandEventOrders = new List<PlayerGrandEventAction>();
	}
}
