using System;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200070F RID: 1807
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class DiscountBazaarEffectStaticData : EdictEffectStaticData
	{
		// Token: 0x04000F33 RID: 3891
		[JsonProperty]
		public int DiscountAmount;
	}
}
