using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005B9 RID: 1465
	[Serializable]
	public class ObjectiveCondition_WinBattlesWithTrespassing : ObjectiveCondition_WinBattlesWithActiveRitual
	{
		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06001B65 RID: 7013 RVA: 0x0005F24E File Offset: 0x0005D44E
		public override string LocalizationKey
		{
			get
			{
				return "WinBattlesWithTrespassing";
			}
		}

		// Token: 0x06001B66 RID: 7014 RVA: 0x0005F258 File Offset: 0x0005D458
		protected override bool Filter(TurnContext context, BattleEvent @event, PlayerState owner, PlayerState target)
		{
			if (!base.Filter(context, @event, owner, target))
			{
				return false;
			}
			GamePiece gamePiece;
			GamePiece gamePiece2;
			if (!@event.BattleResult.TryGetPiecesForPlayer(owner.Id, true, out gamePiece, out gamePiece2))
			{
				return false;
			}
			Hex hex = context.HexBoard[gamePiece.Location];
			int controllingPlayerID = hex.ControllingPlayerID;
			if (controllingPlayerID == -1 || @event.AffectedPlayerIds.Contains(controllingPlayerID))
			{
				return false;
			}
			DiplomaticState diplomaticState = context.CurrentTurn.CurrentDiplomaticTurn.GetDiplomaticState(new PlayerPair(owner.Id, hex.ControllingPlayerID));
			return !diplomaticState.AllowCombat(context.CurrentTurn.CurrentDiplomaticTurn, owner.Id, hex.ControllingPlayerID) && !diplomaticState.AllowMovementIntoTerritory(context.CurrentTurn.CurrentDiplomaticTurn, owner.Id, hex.ControllingPlayerID);
		}

		// Token: 0x04000C5A RID: 3162
		[JsonProperty]
		public bool MustBeAgainstPlayers;
	}
}
