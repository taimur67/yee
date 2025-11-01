using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000220 RID: 544
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class RevealOrdersEvent : GameEvent
	{
		// Token: 0x06000A9F RID: 2719 RVA: 0x0002E97C File Offset: 0x0002CB7C
		[JsonConstructor]
		private RevealOrdersEvent()
		{
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x0002E984 File Offset: 0x0002CB84
		public RevealOrdersEvent(int turnNumber, IEnumerable<ActionableOrder> orders, int triggeringPlayerID, int targetPlayer) : base(triggeringPlayerID)
		{
			this.Orders = IEnumerableExtensions.ToList<ActionableOrder>(orders);
			this.TurnNumber = turnNumber;
			base.AddAffectedPlayerId(targetPlayer);
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x0002E9A8 File Offset: 0x0002CBA8
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0}'s orders were revealed to {1}", base.AffectedPlayerID, this.TriggeringPlayerID);
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x0002E9CA File Offset: 0x0002CBCA
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (this.TriggeringPlayerID != forPlayerID)
			{
				return TurnLogEntryType.None;
			}
			return TurnLogEntryType.OrdersRevealed;
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x0002E9DC File Offset: 0x0002CBDC
		public override void DeepClone(out GameEvent clone)
		{
			RevealOrdersEvent revealOrdersEvent = new RevealOrdersEvent
			{
				TurnNumber = this.TurnNumber,
				Orders = this.Orders.DeepClone(CloneFunction.FastClone)
			};
			base.DeepCloneGameEventParts<RevealOrdersEvent>(revealOrdersEvent);
			clone = revealOrdersEvent;
		}

		// Token: 0x040004E6 RID: 1254
		[JsonProperty]
		public int TurnNumber;

		// Token: 0x040004E7 RID: 1255
		[JsonProperty]
		public List<ActionableOrder> Orders;
	}
}
