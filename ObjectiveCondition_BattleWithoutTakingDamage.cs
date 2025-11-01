using System;

namespace LoG
{
	// Token: 0x0200054D RID: 1357
	[Serializable]
	public class ObjectiveCondition_BattleWithoutTakingDamage : ObjectiveCondition_EventFilter<BattleEvent>
	{
		// Token: 0x06001A4A RID: 6730 RVA: 0x0005BA4C File Offset: 0x00059C4C
		protected override bool Filter(TurnContext context, BattleEvent @event, PlayerState owner, PlayerState target)
		{
			if (!base.Filter(context, @event, owner, target))
			{
				return false;
			}
			if (owner != null)
			{
				GamePiece gamePiece;
				GamePiece gamePiece2;
				if (!@event.BattleResult.TryGetPiecesForPlayer(owner.Id, true, out gamePiece, out gamePiece2))
				{
					return false;
				}
				bool flag = false;
				foreach (BattlePhaseEvent battlePhaseEvent in @event.Enumerate<BattlePhaseEvent>())
				{
					if (battlePhaseEvent.PhaseResult.LosingLegionId == gamePiece.Id && battlePhaseEvent.PhaseResult.HPDamage > 0)
					{
						flag = true;
						break;
					}
				}
				return !flag;
			}
			else
			{
				if (target == null)
				{
					return true;
				}
				GamePiece gamePiece2;
				GamePiece gamePiece3;
				if (!@event.BattleResult.TryGetPiecesForPlayer(target.Id, true, out gamePiece3, out gamePiece2))
				{
					return false;
				}
				bool flag2 = false;
				foreach (BattlePhaseEvent battlePhaseEvent2 in @event.Enumerate<BattlePhaseEvent>())
				{
					if (battlePhaseEvent2.PhaseResult.LosingLegionId == gamePiece3.Id && battlePhaseEvent2.PhaseResult.HPDamage > 0)
					{
						flag2 = true;
						break;
					}
				}
				return !flag2;
			}
		}
	}
}
