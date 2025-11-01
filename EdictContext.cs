using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000380 RID: 896
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EdictContext : ModifierContext
	{
		// Token: 0x0600111C RID: 4380 RVA: 0x00042968 File Offset: 0x00040B68
		[JsonConstructor]
		public EdictContext()
		{
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x00042970 File Offset: 0x00040B70
		public EdictContext(string edictId, string effectId)
		{
			this.EdictId = edictId;
			this.EdictEffect = effectId;
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x00042986 File Offset: 0x00040B86
		public override string ToString()
		{
			return this.EdictId + ":" + this.EdictEffect;
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x0004299E File Offset: 0x00040B9E
		public override void DeepClone(out ModifierContext modifierContext)
		{
			modifierContext = new EdictContext
			{
				EdictId = this.EdictId.DeepClone(),
				EdictEffect = this.EdictEffect.DeepClone()
			};
		}

		// Token: 0x040007ED RID: 2029
		[JsonProperty]
		public string EdictId;

		// Token: 0x040007EE RID: 2030
		[JsonProperty]
		public string EdictEffect;
	}
}
