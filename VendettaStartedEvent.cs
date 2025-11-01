using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200021B RID: 539
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class VendettaStartedEvent : VendettaGameEvent
	{
		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000A83 RID: 2691 RVA: 0x0002E5DA File Offset: 0x0002C7DA
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x0002E5DD File Offset: 0x0002C7DD
		private VendettaStartedEvent()
		{
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x0002E5E5 File Offset: 0x0002C7E5
		public VendettaStartedEvent(int triggeringPlayerId, int targetPlayerId, VendettaObjective objective, int prestige, int turns) : base(triggeringPlayerId, objective, prestige, turns)
		{
			base.AddAffectedPlayerId(targetPlayerId);
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x0002E5FA File Offset: 0x0002C7FA
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("A vendetta between {0} and {1} has begun.", this.TriggeringPlayerID, base.AffectedPlayerID);
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x0002E61C File Offset: 0x0002C81C
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (base.Cancelled)
			{
				return TurnLogEntryType.None;
			}
			if (forPlayerID == this.TriggeringPlayerID)
			{
				return TurnLogEntryType.VendettaStartedInitiator;
			}
			if (forPlayerID == base.AffectedPlayerID)
			{
				return TurnLogEntryType.VendettaStartedRecipient;
			}
			if (!this.VendettaKnowledgeHolders.IsSet(forPlayerID))
			{
				return TurnLogEntryType.VendettaStartedWitness;
			}
			return TurnLogEntryType.VendettaStartedWitness_HasKnowledge;
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x0002E658 File Offset: 0x0002C858
		public override void DeepClone(out GameEvent clone)
		{
			VendettaStartedEvent vendettaStartedEvent = new VendettaStartedEvent
			{
				VendettaKnowledgeHolders = this.VendettaKnowledgeHolders
			};
			base.DeepCloneVendettaGameEventParts(vendettaStartedEvent);
			clone = vendettaStartedEvent;
		}

		// Token: 0x040004DF RID: 1247
		[JsonProperty]
		public BitMask VendettaKnowledgeHolders;
	}
}
