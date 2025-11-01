using System;

namespace LoG
{
	// Token: 0x020004F5 RID: 1269
	public interface IDiplomaticDecisionRequest
	{
		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06001811 RID: 6161
		int RequestingPlayerId { get; }

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06001812 RID: 6162
		int AffectedPlayerId { get; }

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06001813 RID: 6163
		int PrestigeWager { get; }

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06001814 RID: 6164
		OrderTypes OrderType { get; }

		// Token: 0x06001815 RID: 6165
		bool RelatesToPlayers(PlayerPair pair);
	}
}
