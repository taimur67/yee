using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000660 RID: 1632
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ConvertGamePieceRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001E21 RID: 7713 RVA: 0x00067E5A File Offset: 0x0006605A
		public ConvertGamePieceRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001E22 RID: 7714 RVA: 0x00067E67 File Offset: 0x00066067
		public ConvertGamePieceRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001E23 RID: 7715 RVA: 0x00067E70 File Offset: 0x00066070
		public override int GetCommandRatingCost(PlayerState user, TurnState turn, AbilityStaticData data, GameDatabase database)
		{
			GamePiece gamePiece = turn.FetchGameItem<GamePiece>(base.TargetItemId);
			if (gamePiece == null)
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Error(string.Format("{0} is not a Game Piece", base.TargetItemId));
				}
				return 1;
			}
			return gamePiece.CommandCost;
		}

		// Token: 0x06001E24 RID: 7716 RVA: 0x00067EBA File Offset: 0x000660BA
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			yield return new ActionPhase_TargetGamePiece(delegate(GamePiece x)
			{
				this.TargetContext.SetTargetGamePiece(x);
			}, new ActionPhase_Target<GamePiece>.IsValidFunc(this.IsValidGamePiece), new ActionPhase_TargetGamePiece.IsValidArchfiendFunc(this.IsValidArchfiend), new ActionPhase_TargetGamePiece.IsSelectableGamePieceFunc(base.FilterGamePiece), 1, ActionPhaseType.None);
			yield break;
		}

		// Token: 0x06001E25 RID: 7717 RVA: 0x00067ECC File Offset: 0x000660CC
		public override Result IsValidGamePiece(TurnContext context, List<GamePiece> selected, GamePiece gamePiece, int castingPlayerId)
		{
			Problem problem = base.IsValidGamePiece(context, selected, gamePiece, castingPlayerId) as Problem;
			if (problem != null)
			{
				return problem;
			}
			if (gamePiece.CanBeConverted)
			{
				return Result.Success;
			}
			return new Result.CannotBeStolenByRitualsProblem(context.GetDataForRequest(this).ConfigRef, gamePiece);
		}
	}
}
