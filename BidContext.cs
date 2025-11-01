using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x020006CC RID: 1740
	public class BidContext
	{
		// Token: 0x06001FE2 RID: 8162 RVA: 0x0006DA84 File Offset: 0x0006BC84
		public void AddBid(GameItem item, PlayerBid playerBid)
		{
			List<PlayerBid> list;
			if (!this.Bids.TryGetValue(item, out list))
			{
				list = new List<PlayerBid>();
				this.Bids[item] = list;
			}
			list.Add(playerBid);
		}

		// Token: 0x04000D1B RID: 3355
		public Dictionary<GameItem, List<PlayerBid>> Bids = new Dictionary<GameItem, List<PlayerBid>>();
	}
}
