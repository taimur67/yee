using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200020D RID: 525
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class UnholyCrusadeLegionDefeatEvent : GameEvent
	{
		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000A3E RID: 2622 RVA: 0x0002DF2A File Offset: 0x0002C12A
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x0002DF2D File Offset: 0x0002C12D
		[JsonConstructor]
		private UnholyCrusadeLegionDefeatEvent()
		{
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x0002DF35 File Offset: 0x0002C135
		public UnholyCrusadeLegionDefeatEvent(int triggeringPlayerID) : base(triggeringPlayerID)
		{
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x0002DF3E File Offset: 0x0002C13E
		public override string GetDebugName(TurnContext context)
		{
			return "A legion has been defeated during the Unholy Crusade.";
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x0002DF45 File Offset: 0x0002C145
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			return TurnLogEntryType.UnholyCrusadeContinueLegionLost;
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x0002DF4C File Offset: 0x0002C14C
		public override void DeepClone(out GameEvent clone)
		{
			UnholyCrusadeLegionDefeatEvent unholyCrusadeLegionDefeatEvent = new UnholyCrusadeLegionDefeatEvent();
			base.DeepCloneGameEventParts<UnholyCrusadeLegionDefeatEvent>(unholyCrusadeLegionDefeatEvent);
			clone = unholyCrusadeLegionDefeatEvent;
		}
	}
}
