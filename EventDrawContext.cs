using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x020006D0 RID: 1744
	public class EventDrawContext
	{
		// Token: 0x06001FE8 RID: 8168 RVA: 0x0006DB44 File Offset: 0x0006BD44
		public int GetNumDraws(int playerId)
		{
			return this.PlayerDraws.GetValueOrDefault(playerId, 0);
		}

		// Token: 0x06001FE9 RID: 8169 RVA: 0x0006DB54 File Offset: 0x0006BD54
		public void AddDraws(int playerId, int drawCount)
		{
			int num;
			if (this.PlayerDraws.TryGetValue(playerId, out num))
			{
				this.PlayerDraws[playerId] = num + drawCount;
				return;
			}
			this.PlayerDraws[playerId] = drawCount;
		}

		// Token: 0x04000D20 RID: 3360
		public Dictionary<int, int> PlayerDraws = new Dictionary<int, int>();
	}
}
