using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000680 RID: 1664
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class MartyrGamePieceRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001E98 RID: 7832 RVA: 0x00069641 File Offset: 0x00067841
		public MartyrGamePieceRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001E99 RID: 7833 RVA: 0x0006964E File Offset: 0x0006784E
		public MartyrGamePieceRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001E9A RID: 7834 RVA: 0x00069657 File Offset: 0x00067857
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			yield return new ActionPhase_TargetGamePiece(delegate(GamePiece x)
			{
				this.SacrificedGamePiece = x;
			}, new ActionPhase_Target<GamePiece>.IsValidFunc(this.IsValidSacrifice), new ActionPhase_TargetGamePiece.IsValidArchfiendFunc(this.IsValidArchfiend), new ActionPhase_TargetGamePiece.IsSelectableGamePieceFunc(base.FilterGamePiece), 1, ActionPhaseType.SacrificeGamePiece);
			yield return new ActionPhase_TargetGamePiece(delegate(GamePiece x)
			{
				this.TargetContext.SetTargetGamePiece(x);
			}, new ActionPhase_Target<GamePiece>.IsValidFunc(this.IsValidBuffTarget), new ActionPhase_TargetGamePiece.IsValidArchfiendFunc(this.IsValidArchfiend), new ActionPhase_TargetGamePiece.IsSelectableGamePieceFunc(base.FilterGamePiece), 1, ActionPhaseType.BuffGamePiece);
			yield break;
		}

		// Token: 0x06001E9B RID: 7835 RVA: 0x00069668 File Offset: 0x00067868
		private Result IsValidSacrifice(TurnContext context, List<GamePiece> selected, GamePiece gamePiece, int castingPlayerId)
		{
			MartyrGamePieceRitualData martyrGamePieceRitualData = context.Database.Fetch<MartyrGamePieceRitualData>(base.RitualId);
			Problem problem = base.IsValidGamePiece(context, selected, gamePiece, castingPlayerId, martyrGamePieceRitualData.SacrificeTargetSettings) as Problem;
			if (problem != null)
			{
				return problem;
			}
			return Result.Success;
		}

		// Token: 0x06001E9C RID: 7836 RVA: 0x000696A8 File Offset: 0x000678A8
		private Result IsValidBuffTarget(TurnContext context, List<GamePiece> selected, GamePiece gamePiece, int castingPlayerId)
		{
			Problem problem = this.IsValidGamePiece(context, selected, gamePiece, castingPlayerId) as Problem;
			if (problem != null)
			{
				return problem;
			}
			MartyrGamePieceRitualData martyrGamePieceRitualData = context.Database.Fetch<MartyrGamePieceRitualData>(base.RitualId);
			if (gamePiece.Id == this.SacrificedGamePiece)
			{
				return new Result.CastRitualOnPlayerItemProblem(martyrGamePieceRitualData.ConfigRef, castingPlayerId, gamePiece);
			}
			return Result.Success;
		}

		// Token: 0x04000CDF RID: 3295
		[JsonProperty]
		public Identifier SacrificedGamePiece;
	}
}
