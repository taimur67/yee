using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200025C RID: 604
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class SchemeInvalidatedEvent : GameEvent
	{
		// Token: 0x06000BD5 RID: 3029 RVA: 0x000304F5 File Offset: 0x0002E6F5
		[JsonConstructor]
		private SchemeInvalidatedEvent()
		{
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x000304FD File Offset: 0x0002E6FD
		public SchemeInvalidatedEvent(int playerId, SchemeObjective scheme) : base(playerId)
		{
			this.Scheme = scheme;
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x0003050D File Offset: 0x0002E70D
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("Scheme {0} can no longer be completed", this.Scheme.Id);
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x00030529 File Offset: 0x0002E729
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (forPlayerID == this.TriggeringPlayerID)
			{
				return TurnLogEntryType.SchemeInvalidated;
			}
			return TurnLogEntryType.None;
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x0003053C File Offset: 0x0002E73C
		public override void DeepClone(out GameEvent clone)
		{
			SchemeInvalidatedEvent schemeInvalidatedEvent = new SchemeInvalidatedEvent
			{
				Scheme = this.Scheme.DeepClone<SchemeObjective>()
			};
			base.DeepCloneGameEventParts<SchemeInvalidatedEvent>(schemeInvalidatedEvent);
			clone = schemeInvalidatedEvent;
		}

		// Token: 0x0400052D RID: 1325
		[JsonProperty]
		[BindableValue(null, BindingOption.None)]
		public SchemeObjective Scheme;
	}
}
