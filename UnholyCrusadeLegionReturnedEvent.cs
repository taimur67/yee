using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200020B RID: 523
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class UnholyCrusadeLegionReturnedEvent : GameEvent
	{
		// Token: 0x06000A33 RID: 2611 RVA: 0x0002DE6A File Offset: 0x0002C06A
		[JsonConstructor]
		private UnholyCrusadeLegionReturnedEvent()
		{
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x0002DE72 File Offset: 0x0002C072
		public UnholyCrusadeLegionReturnedEvent(int triggeringPlayerID, Identifier legion, bool survived) : base(triggeringPlayerID)
		{
			this.LegionId = legion;
			this.Survived = survived;
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x0002DE89 File Offset: 0x0002C089
		public override string GetDebugName(TurnContext context)
		{
			if (!this.Survived)
			{
				return "The Unholy Crusade has ended and your legion was too weak to survive";
			}
			return "The Unholy Crusade has ended and your legion has returned stronger";
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x0002DE9E File Offset: 0x0002C09E
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (!this.Survived)
			{
				return TurnLogEntryType.UnholyCrusadeReturnLegionLost;
			}
			return TurnLogEntryType.UnholyCrusadeReturnLegionSurvive;
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x0002DEB4 File Offset: 0x0002C0B4
		public override void DeepClone(out GameEvent clone)
		{
			UnholyCrusadeLegionReturnedEvent unholyCrusadeLegionReturnedEvent = new UnholyCrusadeLegionReturnedEvent
			{
				Survived = this.Survived,
				LegionId = this.LegionId
			};
			base.DeepCloneGameEventParts<UnholyCrusadeLegionReturnedEvent>(unholyCrusadeLegionReturnedEvent);
			clone = unholyCrusadeLegionReturnedEvent;
		}

		// Token: 0x040004CC RID: 1228
		[BindableValue("survived", BindingOption.None)]
		[JsonProperty]
		public bool Survived;

		// Token: 0x040004CD RID: 1229
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Identifier LegionId;
	}
}
