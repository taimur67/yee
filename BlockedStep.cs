using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000235 RID: 565
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class BlockedStep : MoveStepEvent
	{
		// Token: 0x06000B0F RID: 2831 RVA: 0x0002F43D File Offset: 0x0002D63D
		[JsonConstructor]
		protected BlockedStep()
		{
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x0002F445 File Offset: 0x0002D645
		public BlockedStep(LegionMovementProcessor.MoveQuery query, Problem reason) : this(query.GamePiece.ControllingPlayerId, query.GamePiece, query.StartingCoord, query.DestinationCoord, query.MovementMode)
		{
			this.Reason = reason;
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x0002F47C File Offset: 0x0002D67C
		public BlockedStep(int playerId, Identifier legionId, HexCoord from, HexCoord to, PathMode pathMode) : base(playerId, legionId, from, to, pathMode)
		{
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x0002F48C File Offset: 0x0002D68C
		public override void DeepClone(out GameEvent clone)
		{
			BlockedStep blockedStep = new BlockedStep
			{
				Reason = this.Reason.DeepClone(CloneFunction.FastClone)
			};
			base.DeepCloneMoveStepEventParts(blockedStep);
			clone = blockedStep;
		}

		// Token: 0x0400050C RID: 1292
		[JsonProperty]
		public Problem Reason;
	}
}
