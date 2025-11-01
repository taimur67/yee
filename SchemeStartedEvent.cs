using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200025D RID: 605
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class SchemeStartedEvent : SchemeEvent
	{
		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000BDA RID: 3034 RVA: 0x0003056B File Offset: 0x0002E76B
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

		// Token: 0x06000BDB RID: 3035 RVA: 0x0003057D File Offset: 0x0002E77D
		[JsonConstructor]
		private SchemeStartedEvent()
		{
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x00030585 File Offset: 0x0002E785
		public SchemeStartedEvent(int triggeringPlayerID, SchemeObjective scheme) : base(triggeringPlayerID, scheme)
		{
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x0003058F File Offset: 0x0002E78F
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (forPlayerID == this.TriggeringPlayerID)
			{
				return TurnLogEntryType.SchemeStarted;
			}
			if (this.Scheme.IsPublic)
			{
				return TurnLogEntryType.SchemeStartedWitness;
			}
			return TurnLogEntryType.None;
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x000305B4 File Offset: 0x0002E7B4
		public override void DeepClone(out GameEvent clone)
		{
			SchemeStartedEvent schemeStartedEvent = new SchemeStartedEvent();
			base.DeepCloneSchemeEventParts(schemeStartedEvent);
			clone = schemeStartedEvent;
		}
	}
}
