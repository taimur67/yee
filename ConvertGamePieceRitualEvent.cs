using System;

namespace LoG
{
	// Token: 0x0200027E RID: 638
	[Serializable]
	public class ConvertGamePieceRitualEvent : RitualCastEvent
	{
		// Token: 0x06000C78 RID: 3192 RVA: 0x00031674 File Offset: 0x0002F874
		public override void DeepClone(out GameEvent clone)
		{
			ConvertGamePieceRitualEvent convertGamePieceRitualEvent = new ConvertGamePieceRitualEvent();
			base.DeepCloneRitualCastEventParts(convertGamePieceRitualEvent);
			clone = convertGamePieceRitualEvent;
		}
	}
}
