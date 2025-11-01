using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200024C RID: 588
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class ItemAcquiredEvent : ItemsAcquiredEvent
	{
		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000B7F RID: 2943 RVA: 0x0002FEAB File Offset: 0x0002E0AB
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Private;
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000B80 RID: 2944 RVA: 0x0002FEAE File Offset: 0x0002E0AE
		[JsonIgnore]
		public override IEnumerable<Identifier> AcquiredItems
		{
			get
			{
				return IEnumerableExtensions.ToEnumerable<Identifier>(this.Item);
			}
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x0002FEBB File Offset: 0x0002E0BB
		[JsonConstructor]
		protected ItemAcquiredEvent()
		{
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x0002FEC3 File Offset: 0x0002E0C3
		public ItemAcquiredEvent(int owningPlayer, Identifier item) : base(owningPlayer)
		{
			this.Item = item;
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x0002FED3 File Offset: 0x0002E0D3
		protected new T DeepCloneGameEventParts<T>(T gameEvent) where T : ItemAcquiredEvent
		{
			base.DeepCloneGameEventParts<T>(gameEvent).Item = this.Item;
			return gameEvent;
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x0002FEED File Offset: 0x0002E0ED
		public override void DeepClone(out GameEvent clone)
		{
			clone = this.DeepCloneGameEventParts<ItemAcquiredEvent>(new ItemAcquiredEvent());
		}

		// Token: 0x04000519 RID: 1305
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Identifier Item;
	}
}
