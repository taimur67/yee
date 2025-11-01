using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020001F7 RID: 503
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class LegacyEvent : GameEvent
	{
		// Token: 0x060009C2 RID: 2498 RVA: 0x0002D2BF File Offset: 0x0002B4BF
		[JsonConstructor]
		public LegacyEvent() : base(int.MinValue)
		{
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x0002D2CC File Offset: 0x0002B4CC
		public override string GetDebugName(TurnContext context)
		{
			return "[Obsolete Event]";
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x0002D2D4 File Offset: 0x0002B4D4
		public override void DeepClone(out GameEvent clone)
		{
			LegacyEvent legacyEvent = new LegacyEvent();
			base.DeepCloneGameEventParts<LegacyEvent>(legacyEvent);
			clone = legacyEvent;
		}
	}
}
