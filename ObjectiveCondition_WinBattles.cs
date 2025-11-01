using System;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005B4 RID: 1460
	[Serializable]
	public class ObjectiveCondition_WinBattles : ObjectiveCondition_EventFilter<BattleEvent>
	{
		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06001B53 RID: 6995 RVA: 0x0005EDD9 File Offset: 0x0005CFD9
		public override string LocalizationKey
		{
			get
			{
				if (!base.IsTargeted)
				{
					return "WinBattles";
				}
				return "WinBattles.Targeted";
			}
		}

		// Token: 0x06001B54 RID: 6996 RVA: 0x0005EDF0 File Offset: 0x0005CFF0
		protected override bool Filter(TurnContext context, BattleEvent @event, PlayerState owner, PlayerState target)
		{
			BattleOutcome outcome = @event.BattleResult.Outcome;
			if (outcome == BattleOutcome.Stalemate || outcome == BattleOutcome.Undecided || outcome == BattleOutcome.Both_Destroyed)
			{
				return false;
			}
			GamePiece gamePiece;
			if (!@event.BattleResult.TryGetWinningPiece_EndState(out gamePiece))
			{
				return false;
			}
			if (gamePiece.ControllingPlayerId != owner.Id)
			{
				return false;
			}
			if (this.AcceptedOpponentTypes != null && !this.AcceptedOpponentTypes.ValidGamePieceTypes.IsEmpty())
			{
				GamePiece gamePiece2;
				GamePiece gamePiece3;
				if (!@event.BattleResult.TryGetPiecesForPlayer(owner.Id, true, out gamePiece2, out gamePiece3))
				{
					return false;
				}
				if (!this.AcceptedOpponentTypes.ValidGamePieceTypes.IsSet((int)gamePiece3.SubCategory))
				{
					return false;
				}
			}
			return base.Filter(context, @event, owner, target);
		}

		// Token: 0x04000C56 RID: 3158
		[JsonProperty]
		public GamePieceTargetSettings AcceptedOpponentTypes = new GamePieceTargetSettings();
	}
}
