using System;

namespace LoG
{
	// Token: 0x02000351 RID: 849
	public class TurnEffect_Revive : TurnAbilityEffect
	{
		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06001025 RID: 4133 RVA: 0x0003FC5D File Offset: 0x0003DE5D
		public override TurnProcessStage HasEffectInStage
		{
			get
			{
				return TurnProcessStage.TurnModule_Healing;
			}
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x0003FC60 File Offset: 0x0003DE60
		public int GetReviveTurnsForGamePiece(TurnState turn, GamePiece piece)
		{
			int num = piece.Level * 2;
			PlayerState playerState;
			if (!turn.TryFindControllingPlayer(piece, out playerState))
			{
				return num;
			}
			if (playerState.PersonalGuardId != piece.Id)
			{
				return num;
			}
			return Math.Min(num, playerState.MaxReviveDuration.Value);
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x0003FCA4 File Offset: 0x0003DEA4
		protected override void OnStageOfTurnIfActive(Ability ability, TurnProcessContext context, GamePiece piece)
		{
			if (piece.DeathTurn < 0)
			{
				return;
			}
			PlayerState playerState = context.CurrentTurn.FindPlayerState(piece.ControllingPlayerId, null);
			int reviveTurnsForGamePiece = this.GetReviveTurnsForGamePiece(context.CurrentTurn, piece);
			if (context.CurrentTurn.TurnValue - piece.DeathTurn < reviveTurnsForGamePiece)
			{
				return;
			}
			if (piece.Status == GameItemStatus.Banished)
			{
				HexCoord coord;
				if (LegionMovementProcessor.TryFindSpawnPointFor(context, playerState, piece, out coord))
				{
					context.ReviveGamePiece(piece, coord);
					context.CurrentTurn.AddGameEvent<GamePieceReformedEvent>(new GamePieceReformedEvent(playerState.Id, piece.Id, true));
					return;
				}
				context.CurrentTurn.AddGameEvent<GamePieceReformedEvent>(new GamePieceReformedEvent(playerState.Id, piece.Id, false));
			}
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x0003FD50 File Offset: 0x0003DF50
		public override void DeepClone(out AbilityEffect clone)
		{
			TurnEffect_Revive turnEffect_Revive = new TurnEffect_Revive();
			base.DeepCloneTurnAbilityEffectParts(turnEffect_Revive);
			clone = turnEffect_Revive;
		}
	}
}
