using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000236 RID: 566
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class MarchedEvent : MoveStepEvent
	{
		// Token: 0x06000B13 RID: 2835 RVA: 0x0002F4BB File Offset: 0x0002D6BB
		[JsonConstructor]
		protected MarchedEvent()
		{
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x0002F4C3 File Offset: 0x0002D6C3
		public MarchedEvent(LegionMovementProcessor.MoveQuery query) : this(query.GamePiece.ControllingPlayerId, query.GamePiece, query.StartingCoord, query.DestinationCoord)
		{
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x0002F4ED File Offset: 0x0002D6ED
		public MarchedEvent(int playerId, Identifier legionId, HexCoord from, HexCoord to) : base(playerId, legionId, from, to, PathMode.March)
		{
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x0002F4FC File Offset: 0x0002D6FC
		public override void DeepClone(out GameEvent clone)
		{
			MarchedEvent marchedEvent = new MarchedEvent();
			base.DeepCloneMoveStepEventParts(marchedEvent);
			clone = marchedEvent;
		}
	}
}
