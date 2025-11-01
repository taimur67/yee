using System;
using System.Collections.Generic;
using System.ComponentModel;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004FD RID: 1277
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class UnholyCrusadeEventDecisionResponse : GrandEventDecisionResponse
	{
		// Token: 0x06001831 RID: 6193 RVA: 0x00056D4F File Offset: 0x00054F4F
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			yield return new ActionPhase_TargetGamePiece(delegate(GamePiece x)
			{
				this.SubmitLegion(x);
			}, new ActionPhase_Target<GamePiece>.IsValidFunc(this.IsValidGamePiece), new ActionPhase_TargetGamePiece.IsValidArchfiendFunc(this.IsValidArchfiend), new ActionPhase_TargetGamePiece.IsSelectableGamePieceFunc(this.IsSelectableArchfiend), 1, ActionPhaseType.None);
			yield break;
		}

		// Token: 0x06001832 RID: 6194 RVA: 0x00056D5F File Offset: 0x00054F5F
		private Result IsSelectableArchfiend(TurnContext context, PlayerState player, GamePiece candidate)
		{
			if (candidate.SubCategory != GamePieceCategory.Legion)
			{
				return Result.Failure;
			}
			return Result.Success;
		}

		// Token: 0x06001833 RID: 6195 RVA: 0x00056D74 File Offset: 0x00054F74
		private Result IsValidArchfiend(TurnContext context, int target, int castingPlayerId)
		{
			if (target != castingPlayerId)
			{
				return Result.Failure;
			}
			return Result.Success;
		}

		// Token: 0x06001834 RID: 6196 RVA: 0x00056D85 File Offset: 0x00054F85
		private Result IsValidGamePiece(TurnContext context, List<GamePiece> selected, GamePiece target, int castingPlayerId)
		{
			if (!this.IsSelectableArchfiend(context, context.CurrentTurn.FindPlayerState(castingPlayerId, null), target))
			{
				return Result.Failure;
			}
			if (target.HasTag<EntityTag_CannotJoinCrusade>())
			{
				return new GameItemProblem(target.Id);
			}
			return Result.Success;
		}

		// Token: 0x06001835 RID: 6197 RVA: 0x00056DC3 File Offset: 0x00054FC3
		public void SubmitLegion(Identifier id)
		{
			this.SubmittedLegionId = id;
		}

		// Token: 0x06001836 RID: 6198 RVA: 0x00056DCC File Offset: 0x00054FCC
		public bool ValidSubmission()
		{
			return this.SubmittedLegionId != Identifier.Invalid;
		}

		// Token: 0x06001837 RID: 6199 RVA: 0x00056DDC File Offset: 0x00054FDC
		public override void DeepClone(out DecisionResponse clone)
		{
			UnholyCrusadeEventDecisionResponse unholyCrusadeEventDecisionResponse = new UnholyCrusadeEventDecisionResponse
			{
				SubmittedLegionId = this.SubmittedLegionId
			};
			base.DeepCloneGrandEventDecisionResponseParts(unholyCrusadeEventDecisionResponse);
			clone = unholyCrusadeEventDecisionResponse;
		}

		// Token: 0x04000B7B RID: 2939
		[JsonProperty]
		[DefaultValue(Identifier.Invalid)]
		public Identifier SubmittedLegionId = Identifier.Invalid;
	}
}
