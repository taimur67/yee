using System;

namespace LoG
{
	// Token: 0x02000503 RID: 1283
	public static class DiplomaticExtensions
	{
		// Token: 0x06001859 RID: 6233 RVA: 0x000575D0 File Offset: 0x000557D0
		public static bool HasDiplomaticMarkerUI(this DiplomaticState state)
		{
			DiplomaticStateValue type = state.Type;
			return type != DiplomaticStateValue.Neutral && type != DiplomaticStateValue.Excommunicated && type != DiplomaticStateValue.BloodVassalage && type != DiplomaticStateValue.Vassalised;
		}
	}
}
