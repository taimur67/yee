using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000388 RID: 904
	[Serializable]
	public class AbilityContext : ModifierContext
	{
		// Token: 0x06001138 RID: 4408 RVA: 0x00042B85 File Offset: 0x00040D85
		[JsonConstructor]
		public AbilityContext()
		{
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x00042B8D File Offset: 0x00040D8D
		public AbilityContext(string sourceId)
		{
			this.SourceId = sourceId;
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x00042B9C File Offset: 0x00040D9C
		public override string ToString()
		{
			return this.SourceId;
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x00042BA4 File Offset: 0x00040DA4
		public override void DeepClone(out ModifierContext modifierContext)
		{
			modifierContext = new AbilityContext
			{
				SourceId = this.SourceId.DeepClone()
			};
		}

		// Token: 0x040007F7 RID: 2039
		[JsonProperty]
		public string SourceId;
	}
}
