using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000369 RID: 873
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public abstract class ModifiableTargetGroup
	{
		// Token: 0x040007BD RID: 1981
		[JsonProperty]
		public Guid Id = Guid.NewGuid();

		// Token: 0x040007BE RID: 1982
		[JsonProperty]
		public List<IModifier> Modifiers = new List<IModifier>();

		// Token: 0x040007BF RID: 1983
		[JsonProperty]
		public List<Ability> Abilities = new List<Ability>();
	}
}
