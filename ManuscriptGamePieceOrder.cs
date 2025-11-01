using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000621 RID: 1569
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ManuscriptGamePieceOrder : InvokeManuscriptOrder
	{
		// Token: 0x06001D01 RID: 7425 RVA: 0x00064408 File Offset: 0x00062608
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			yield return new ActionPhase_TargetGamePiece(delegate(GamePiece x)
			{
				this.TargetContext.SetTargetGamePiece(x);
			}, new ActionPhase_Target<GamePiece>.IsValidFunc(this.IsValidGamePiece), new ActionPhase_TargetGamePiece.IsValidArchfiendFunc(this.IsValidArchfiend), new ActionPhase_TargetGamePiece.IsSelectableGamePieceFunc(base.FilterGamePiece), 1, ActionPhaseType.None);
			yield break;
		}

		// Token: 0x06001D02 RID: 7426 RVA: 0x00064418 File Offset: 0x00062618
		public override Result IsValidGamePiece(TurnContext context, List<GamePiece> selected, GamePiece gamePiece, int triggeringPlayerId)
		{
			Result result = base.IsValidGamePiece(context, selected, gamePiece, triggeringPlayerId, null);
			if (!result.successful)
			{
				return result;
			}
			if (gamePiece.LearntManuscriptsCount >= gamePiece.Level)
			{
				return new Result.InvokeManuscriptOnGameItemAboveCapacityProblem(this.ManuscriptId, gamePiece.Id, gamePiece.LearntManuscriptsCount, gamePiece.Level);
			}
			return Result.Success;
		}
	}
}
