using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000202 RID: 514
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class TurnEndEvent : GameEvent
	{
		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000A09 RID: 2569 RVA: 0x0002DA3E File Offset: 0x0002BC3E
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x0002DA41 File Offset: 0x0002BC41
		public TurnEndEvent() : base(-1)
		{
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x0002DA4C File Offset: 0x0002BC4C
		public override void DeepClone(out GameEvent clone)
		{
			TurnEndEvent turnEndEvent = new TurnEndEvent();
			base.DeepCloneGameEventParts<TurnEndEvent>(turnEndEvent);
			clone = turnEndEvent;
		}
	}
}
