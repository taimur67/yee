using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000201 RID: 513
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class TurnStartEvent : GameEvent
	{
		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000A06 RID: 2566 RVA: 0x0002DA14 File Offset: 0x0002BC14
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x0002DA17 File Offset: 0x0002BC17
		public TurnStartEvent() : base(-1)
		{
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x0002DA20 File Offset: 0x0002BC20
		public override void DeepClone(out GameEvent clone)
		{
			TurnStartEvent turnStartEvent = new TurnStartEvent();
			base.DeepCloneGameEventParts<TurnStartEvent>(turnStartEvent);
			clone = turnStartEvent;
		}
	}
}
