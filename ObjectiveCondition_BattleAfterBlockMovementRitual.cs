using System;
using System.Linq;

namespace LoG
{
	// Token: 0x0200054C RID: 1356
	[Serializable]
	public class ObjectiveCondition_BattleAfterBlockMovementRitual : ObjectiveCondition_EventFilter<BattleEvent>
	{
		// Token: 0x06001A48 RID: 6728 RVA: 0x0005B980 File Offset: 0x00059B80
		protected override bool Filter(TurnContext context, BattleEvent @event, PlayerState owner, PlayerState target)
		{
			if (!base.Filter(context, @event, owner, target))
			{
				return false;
			}
			if (@event.BattleResult.AttackingPlayerId != owner.Id)
			{
				return false;
			}
			GamePiece theirPiece;
			GamePiece gamePiece;
			if (!@event.BattleResult.TryGetPiecesForPlayer(owner.Id, true, out gamePiece, out theirPiece))
			{
				return false;
			}
			return (from castRitual in context.CurrentTurn.GetGameEvents().OfType<RitualCastEvent>()
			where castRitual.Succeeded && castRitual.TriggeringPlayerID == owner.Id
			select castRitual).SelectMany((RitualCastEvent castRitual) => castRitual.Enumerate<BlockMovementEvent>()).Any((BlockMovementEvent blockEvent) => blockEvent.Target == theirPiece.Id && !blockEvent.TargetHadAlreadyMoved);
		}
	}
}
