using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000207 RID: 519
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class ItemsReceivedEvent : ItemsAcquiredEvent
	{
		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000A1F RID: 2591 RVA: 0x0002DC78 File Offset: 0x0002BE78
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Private;
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000A20 RID: 2592 RVA: 0x0002DC7B File Offset: 0x0002BE7B
		[JsonIgnore]
		public override IEnumerable<Identifier> AcquiredItems
		{
			get
			{
				return this.ItemsReceived;
			}
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x0002DC83 File Offset: 0x0002BE83
		[JsonConstructor]
		private ItemsReceivedEvent()
		{
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x0002DC8B File Offset: 0x0002BE8B
		public ItemsReceivedEvent(int playerId, List<Identifier> items)
		{
			this.TriggeringPlayerID = playerId;
			base.AddAffectedPlayerId(playerId);
			this.ItemsReceived = items;
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x0002DCA8 File Offset: 0x0002BEA8
		public override string GetDebugName(TurnContext context)
		{
			return "Items received: " + ListExtensions.Join<Identifier>(this.ItemsReceived, ", ");
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x0002DCC4 File Offset: 0x0002BEC4
		public override void DeepClone(out GameEvent clone)
		{
			ItemsReceivedEvent itemsReceivedEvent = new ItemsReceivedEvent
			{
				ItemsReceived = this.ItemsReceived.DeepClone()
			};
			base.DeepCloneGameEventParts<ItemsReceivedEvent>(itemsReceivedEvent);
			clone = itemsReceivedEvent;
		}

		// Token: 0x040004C7 RID: 1223
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public List<Identifier> ItemsReceived;
	}
}
