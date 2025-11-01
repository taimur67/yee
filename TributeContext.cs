using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x020006D1 RID: 1745
	public class TributeContext
	{
		// Token: 0x06001FEB RID: 8171 RVA: 0x0006DBA4 File Offset: 0x0006BDA4
		public void IncrementDraw(PlayerState player)
		{
			if (!this.PlayerDraws.ContainsKey(player.Id))
			{
				this.PlayerDraws[player.Id] = 0;
			}
			Dictionary<int, int> playerDraws = this.PlayerDraws;
			int id = player.Id;
			int num = playerDraws[id];
			playerDraws[id] = num + 1;
		}

		// Token: 0x06001FEC RID: 8172 RVA: 0x0006DBF4 File Offset: 0x0006BDF4
		public int GetNumDraws(PlayerState player)
		{
			int result;
			if (!this.PlayerDraws.TryGetValue(player.Id, out result))
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x04000D21 RID: 3361
		public Dictionary<int, int> PlayerDraws = new Dictionary<int, int>();
	}
}
