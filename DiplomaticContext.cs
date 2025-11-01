using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x020006CD RID: 1741
	public class DiplomaticContext
	{
		// Token: 0x06001FE4 RID: 8164 RVA: 0x0006DAD0 File Offset: 0x0006BCD0
		public void AddDiplomaticAction(int actor, int target, PlayerDiplomaticAction diplomaticAction)
		{
			PlayerPair key = new PlayerPair(actor, target);
			List<PlayerDiplomaticAction> list;
			if (!this.DiplomaticActions.TryGetValue(key, out list))
			{
				list = new List<PlayerDiplomaticAction>();
				this.DiplomaticActions[key] = list;
			}
			list.Add(diplomaticAction);
		}

		// Token: 0x04000D1C RID: 3356
		public Dictionary<PlayerPair, List<PlayerDiplomaticAction>> DiplomaticActions = new Dictionary<PlayerPair, List<PlayerDiplomaticAction>>();
	}
}
