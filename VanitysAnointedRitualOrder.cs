using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020006C4 RID: 1732
	public class VanitysAnointedRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001FAC RID: 8108 RVA: 0x0006CACA File Offset: 0x0006ACCA
		public VanitysAnointedRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001FAD RID: 8109 RVA: 0x0006CAD7 File Offset: 0x0006ACD7
		public VanitysAnointedRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001FAE RID: 8110 RVA: 0x0006CAE0 File Offset: 0x0006ACE0
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			yield return new ActionPhase_TargetGameItem(delegate(Identifier x)
			{
				this.TargetContext.SetTargetGameItem(x, turn.FindControllingPlayer(x).Id);
			}, new ActionPhase_Target<Identifier>.IsValidFunc(this.IsValidGameItem), new ActionPhase_TargetGameItem.IsValidArchfiendFunc(this.IsValidArchfiend), new ActionPhase_TargetGameItem.IsSelectableGameItemFunc(base.FilterGameItem), 1, ActionPhaseType.None);
			yield break;
		}

		// Token: 0x06001FAF RID: 8111 RVA: 0x0006CAF8 File Offset: 0x0006ACF8
		public override Result IsValidGameItem(TurnContext context, List<Identifier> selected, Identifier gameItemId, int castingPlayerId)
		{
			Problem problem = base.IsValidGameItem(context, selected, gameItemId, castingPlayerId) as Problem;
			if (problem != null)
			{
				return problem;
			}
			AbilityStaticData dataForRequest = context.GetDataForRequest(this);
			if (!(dataForRequest is IGameItemTargetAbility))
			{
				return new SimulationError("Expected IGameItemTargetAbility data but could not find it.");
			}
			Praetor praetor = context.CurrentTurn.FetchGameItem<Praetor>(gameItemId);
			if (praetor == null)
			{
				return new Result.CastRitualOnPlayerProblem(dataForRequest.ConfigRef, castingPlayerId);
			}
			GamePiece controllingPiece = context.CurrentTurn.GetControllingPiece(gameItemId);
			if (controllingPiece == null || !controllingPiece.IsLegionOrTitan())
			{
				return new Result.CastRitualItemControllingGamePieceProblem(dataForRequest.ConfigRef, castingPlayerId, praetor, GamePieceCategory.Legion);
			}
			return Result.Success;
		}

		// Token: 0x06001FB0 RID: 8112 RVA: 0x0006CB81 File Offset: 0x0006AD81
		public override IEnumerable<ActionConflict> GeneratePotentialConflicts()
		{
			yield return new VanitysAnointedConflict(base.TargetItemId);
			yield break;
		}
	}
}
