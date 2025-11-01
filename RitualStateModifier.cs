using System;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000393 RID: 915
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public sealed class RitualStateModifier : ArchfiendModifier
	{
		// Token: 0x0600118E RID: 4494 RVA: 0x00043995 File Offset: 0x00041B95
		[JsonConstructor]
		private RitualStateModifier()
		{
		}

		// Token: 0x0600118F RID: 4495 RVA: 0x0004399D File Offset: 0x00041B9D
		public RitualStateModifier(RitualStateModifierStaticData data) : base(data)
		{
		}
	}
}
