using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200025A RID: 602
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class DecisionAddedEvent : GameEvent
	{
		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000BC8 RID: 3016 RVA: 0x000303D6 File Offset: 0x0002E5D6
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Private;
			}
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x000303D9 File Offset: 0x0002E5D9
		[JsonConstructor]
		private DecisionAddedEvent()
		{
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x000303E1 File Offset: 0x0002E5E1
		public DecisionAddedEvent(int forPlayerId, DecisionId id)
		{
			this.DecisionId = id;
			base.AddAffectedPlayerId(forPlayerId);
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x000303F8 File Offset: 0x0002E5F8
		public override void DeepClone(out GameEvent clone)
		{
			DecisionAddedEvent decisionAddedEvent = new DecisionAddedEvent
			{
				DecisionId = this.DecisionId
			};
			base.DeepCloneGameEventParts<DecisionAddedEvent>(decisionAddedEvent);
			clone = decisionAddedEvent;
		}

		// Token: 0x04000528 RID: 1320
		[JsonProperty]
		public DecisionId DecisionId;
	}
}
