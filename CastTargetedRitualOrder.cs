using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005C3 RID: 1475
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public abstract class CastTargetedRitualOrder : CastRitualOrder
	{
		// Token: 0x06001B83 RID: 7043 RVA: 0x0005F8B4 File Offset: 0x0005DAB4
		protected CastTargetedRitualOrder()
		{
		}

		// Token: 0x06001B84 RID: 7044 RVA: 0x0005F8BC File Offset: 0x0005DABC
		protected CastTargetedRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001B85 RID: 7045 RVA: 0x0005F8C8 File Offset: 0x0005DAC8
		public override Result IsValidGamePiece(TurnContext context, List<GamePiece> selected, GamePiece gamePiece, int castingPlayerId)
		{
			Problem problem = base.IsValidGamePiece(context, selected, gamePiece, castingPlayerId) as Problem;
			if (problem != null)
			{
				return problem;
			}
			EntityTag_BlockRitualMasking entityTag_BlockRitualMasking;
			if (this.RitualMaskingSettings.MaskingMode != RitualMaskingMode.NoMasking && gamePiece.TryGetTag<EntityTag_BlockRitualMasking>(out entityTag_BlockRitualMasking) && !entityTag_BlockRitualMasking.AllowDuringPhase.Contains(context.CurrentTurn.TurnPhase))
			{
				return new Result.TargetBlocksRitualMaskingProblem(context.GetDataForRequest(this).ConfigRef, gamePiece);
			}
			return Result.Success;
		}
	}
}
