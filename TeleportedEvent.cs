using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000237 RID: 567
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class TeleportedEvent : MoveStepEvent
	{
		// Token: 0x06000B17 RID: 2839 RVA: 0x0002F519 File Offset: 0x0002D719
		[JsonConstructor]
		public TeleportedEvent()
		{
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x0002F521 File Offset: 0x0002D721
		public TeleportedEvent(LegionMovementProcessor.MoveQuery query) : this(query.GamePiece.ControllingPlayerId, query.GamePiece, query.StartingCoord, query.DestinationCoord)
		{
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x0002F54B File Offset: 0x0002D74B
		public TeleportedEvent(int playerId, Identifier legionId, HexCoord from, HexCoord to) : base(playerId, legionId, from, to, PathMode.Teleport)
		{
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x0002F55C File Offset: 0x0002D75C
		public override void DeepClone(out GameEvent clone)
		{
			TeleportedEvent teleportedEvent = new TeleportedEvent();
			base.DeepCloneMoveStepEventParts(teleportedEvent);
			clone = teleportedEvent;
		}
	}
}
