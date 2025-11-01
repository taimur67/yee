using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200021D RID: 541
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class VendettaCompletedEvent : VendettaGameEvent
	{
		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000A8E RID: 2702 RVA: 0x0002E705 File Offset: 0x0002C905
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x0002E708 File Offset: 0x0002C908
		[JsonConstructor]
		private VendettaCompletedEvent()
		{
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x0002E710 File Offset: 0x0002C910
		public VendettaCompletedEvent(int triggeringPlayerId, int targetPlayerId, VendettaObjective objective, int prestigeWager, int prestigeBonus, int turns) : base(triggeringPlayerId, objective, prestigeWager, turns)
		{
			this.PrestigeBonus = prestigeBonus;
			base.AddAffectedPlayerId(targetPlayerId);
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x0002E730 File Offset: 0x0002C930
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("The vendetta between {0} and {1} has been settled. ", this.TriggeringPlayerID, base.AffectedPlayerID) + (this.Successful ? string.Format("{0} succeeded.", this.TriggeringPlayerID) : string.Format("{0} failed.", this.TriggeringPlayerID));
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x0002E798 File Offset: 0x0002C998
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (base.Cancelled)
			{
				return TurnLogEntryType.None;
			}
			if (forPlayerID == this.TriggeringPlayerID)
			{
				if (!this.Successful)
				{
					return TurnLogEntryType.VendettaFailedInitiator;
				}
				return TurnLogEntryType.VendettaSuccessfulInitiator;
			}
			else if (forPlayerID == base.AffectedPlayerID)
			{
				if (!this.Successful)
				{
					return TurnLogEntryType.VendettaFailedRecipient;
				}
				return TurnLogEntryType.VendettaSuccessfulRecipient;
			}
			else
			{
				if (!this.Successful)
				{
					return TurnLogEntryType.VendettaFailedWitness;
				}
				return TurnLogEntryType.VendettaSuccessfulWitness;
			}
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x0002E7F8 File Offset: 0x0002C9F8
		public override void DeepClone(out GameEvent clone)
		{
			VendettaCompletedEvent vendettaCompletedEvent = new VendettaCompletedEvent
			{
				Successful = this.Successful,
				PrestigeBonus = this.PrestigeBonus,
				TurnsTaken = this.TurnsTaken
			};
			base.DeepCloneVendettaGameEventParts(vendettaCompletedEvent);
			clone = vendettaCompletedEvent;
		}

		// Token: 0x040004E0 RID: 1248
		[BindableValue("success", BindingOption.None)]
		[JsonProperty]
		public bool Successful;

		// Token: 0x040004E1 RID: 1249
		[BindableValue("bonus", BindingOption.None)]
		[JsonProperty]
		public int PrestigeBonus;

		// Token: 0x040004E2 RID: 1250
		[JsonProperty]
		public int TurnsTaken;
	}
}
