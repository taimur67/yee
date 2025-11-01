using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002A1 RID: 673
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EntityTag : IDeepClone<EntityTag>
	{
		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000D04 RID: 3332 RVA: 0x0003467D File Offset: 0x0003287D
		// (set) Token: 0x06000D05 RID: 3333 RVA: 0x00034685 File Offset: 0x00032885
		[JsonProperty]
		public ModifierContext Source { get; set; }

		// Token: 0x06000D06 RID: 3334 RVA: 0x0003468E File Offset: 0x0003288E
		public override string ToString()
		{
			return StringExtensions.RemoveStart(base.GetType().Name, "EntityTag_");
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x000346A5 File Offset: 0x000328A5
		protected void DeepCloneEntityTagParts(EntityTag clone)
		{
			clone.Source = this.Source.DeepClone<ModifierContext>();
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x000346B8 File Offset: 0x000328B8
		public virtual void DeepClone(out EntityTag clone)
		{
			clone = new EntityTag();
			this.DeepCloneEntityTagParts(clone);
		}
	}
}
