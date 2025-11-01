using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000216 RID: 534
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ConsolidateTributeEvent : GameEvent
	{
		// Token: 0x06000A6A RID: 2666 RVA: 0x0002E359 File Offset: 0x0002C559
		[JsonConstructor]
		private ConsolidateTributeEvent()
		{
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x0002E36C File Offset: 0x0002C56C
		public ConsolidateTributeEvent(int triggeringPlayerID) : base(triggeringPlayerID)
		{
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x0002E380 File Offset: 0x0002C580
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0} consolidated {1} resources into {2}", this.TriggeringPlayerID, this.ConsolidatedResources.Count, this.ResultResource);
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x0002E3B0 File Offset: 0x0002C5B0
		public override void DeepClone(out GameEvent clone)
		{
			ConsolidateTributeEvent consolidateTributeEvent = new ConsolidateTributeEvent
			{
				ConsolidatedResources = this.ConsolidatedResources.DeepClone<ResourceNFT>(),
				ResultResource = this.ResultResource.DeepClone<ResourceNFT>()
			};
			base.DeepCloneGameEventParts<ConsolidateTributeEvent>(consolidateTributeEvent);
			clone = consolidateTributeEvent;
		}

		// Token: 0x040004D6 RID: 1238
		[JsonProperty]
		public List<ResourceNFT> ConsolidatedResources = new List<ResourceNFT>();

		// Token: 0x040004D7 RID: 1239
		[JsonProperty]
		public ResourceNFT ResultResource;
	}
}
