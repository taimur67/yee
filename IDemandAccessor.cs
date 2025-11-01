using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x020005CD RID: 1485
	public interface IDemandAccessor : ISelectionAccessor
	{
		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06001BEB RID: 7147
		// (set) Token: 0x06001BEC RID: 7148
		DemandOptions DemandOption { get; set; }

		// Token: 0x06001BED RID: 7149
		List<DemandOptions> GetValidOptions(PlayerState player, TurnState turnState);

		// Token: 0x06001BEE RID: 7150
		List<string> GetValidOptionStrings(PlayerState player, TurnState turnState);

		// Token: 0x06001BEF RID: 7151
		int GetCurrentSelectionInt(PlayerState player, TurnState turnState);
	}
}
