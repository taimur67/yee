using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000287 RID: 647
	[Serializable]
	public class StealGameItemRitualEvent : RitualCastEvent
	{
		// Token: 0x06000C94 RID: 3220 RVA: 0x00031C9C File Offset: 0x0002FE9C
		public override void DeepClone(out GameEvent clone)
		{
			StealGameItemRitualEvent stealGameItemRitualEvent = new StealGameItemRitualEvent
			{
				ItemCategory = this.ItemCategory
			};
			base.DeepCloneRitualCastEventParts(stealGameItemRitualEvent);
			clone = stealGameItemRitualEvent;
		}

		// Token: 0x04000591 RID: 1425
		[JsonProperty]
		public GameItemCategory ItemCategory;
	}
}
