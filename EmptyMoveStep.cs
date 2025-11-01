using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000234 RID: 564
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EmptyMoveStep : MoveStepEvent
	{
		// Token: 0x06000B0B RID: 2827 RVA: 0x0002F3D6 File Offset: 0x0002D5D6
		[JsonConstructor]
		protected EmptyMoveStep()
		{
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x0002F3DE File Offset: 0x0002D5DE
		public EmptyMoveStep(LegionMovementProcessor.MoveQuery query) : this(query.GamePiece.ControllingPlayerId, query.GamePiece, query.StartingCoord, query.DestinationCoord, query.MovementMode)
		{
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x0002F40E File Offset: 0x0002D60E
		public EmptyMoveStep(int playerId, Identifier legionId, HexCoord from, HexCoord to, PathMode pathMode) : base(playerId, legionId, from, to, pathMode)
		{
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x0002F420 File Offset: 0x0002D620
		public override void DeepClone(out GameEvent clone)
		{
			EmptyMoveStep emptyMoveStep = new EmptyMoveStep();
			base.DeepCloneMoveStepEventParts(emptyMoveStep);
			clone = emptyMoveStep;
		}
	}
}
