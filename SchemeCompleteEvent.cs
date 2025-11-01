using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000260 RID: 608
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class SchemeCompleteEvent : SchemeEvent
	{
		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000BE8 RID: 3048 RVA: 0x00030671 File Offset: 0x0002E871
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				if (!this.Scheme.IsPublic)
				{
					return GameEventVisibility.Private;
				}
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x00030683 File Offset: 0x0002E883
		[JsonConstructor]
		private SchemeCompleteEvent()
		{
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x0003068B File Offset: 0x0002E88B
		public SchemeCompleteEvent(int playerId, SchemeObjective scheme) : base(playerId, scheme)
		{
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x00030695 File Offset: 0x0002E895
		public override string GetDebugName(TurnContext context)
		{
			return "Scheme Completed";
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x0003069C File Offset: 0x0002E89C
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (this.Scheme.IsPrivate)
			{
				if (forPlayerID != this.TriggeringPlayerID)
				{
					return TurnLogEntryType.None;
				}
				return TurnLogEntryType.PrivateSchemeComplete;
			}
			else
			{
				if (forPlayerID != this.TriggeringPlayerID)
				{
					return TurnLogEntryType.SchemeCompleteWitness;
				}
				return TurnLogEntryType.SchemeComplete;
			}
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x000306D0 File Offset: 0x0002E8D0
		public override void DeepClone(out GameEvent clone)
		{
			SchemeCompleteEvent schemeCompleteEvent = new SchemeCompleteEvent
			{
				PrestigeAward = this.PrestigeAward
			};
			base.DeepCloneSchemeEventParts(schemeCompleteEvent);
			clone = schemeCompleteEvent;
		}

		// Token: 0x04000531 RID: 1329
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public int PrestigeAward;
	}
}
