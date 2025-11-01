using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200020C RID: 524
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class UnholyCrusadeFailureEvent : GameEvent
	{
		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000A38 RID: 2616 RVA: 0x0002DEEA File Offset: 0x0002C0EA
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x0002DEED File Offset: 0x0002C0ED
		[JsonConstructor]
		private UnholyCrusadeFailureEvent()
		{
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x0002DEF5 File Offset: 0x0002C0F5
		public UnholyCrusadeFailureEvent(int triggeringPlayerID) : base(triggeringPlayerID)
		{
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x0002DEFE File Offset: 0x0002C0FE
		public override string GetDebugName(TurnContext context)
		{
			return "The Unholy Crusade has failed, and all legions were lost";
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x0002DF05 File Offset: 0x0002C105
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			return TurnLogEntryType.UnholyCrusadeReturnAllLost;
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x0002DF0C File Offset: 0x0002C10C
		public override void DeepClone(out GameEvent clone)
		{
			UnholyCrusadeFailureEvent unholyCrusadeFailureEvent = new UnholyCrusadeFailureEvent();
			base.DeepCloneGameEventParts<UnholyCrusadeFailureEvent>(unholyCrusadeFailureEvent);
			clone = unholyCrusadeFailureEvent;
		}
	}
}
