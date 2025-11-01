using System;
using System.ComponentModel;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005D7 RID: 1495
	public class DiplomaticAbility_DeclareDraconicRazzia : DiplomaticDarkArtAbilityStaticData
	{
		// Token: 0x04000C77 RID: 3191
		[JsonProperty]
		[DefaultValue(1)]
		public int TurnDelay = 1;

		// Token: 0x04000C78 RID: 3192
		[JsonProperty]
		[DefaultValue(1)]
		public int Duration = 1;
	}
}
