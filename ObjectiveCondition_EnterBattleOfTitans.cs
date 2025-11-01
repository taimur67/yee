using System;

namespace LoG
{
	// Token: 0x02000580 RID: 1408
	[Serializable]
	public class ObjectiveCondition_EnterBattleOfTitans : ObjectiveCondition_EventFilter<BattleEvent>
	{
		// Token: 0x06001AD1 RID: 6865 RVA: 0x0005DA40 File Offset: 0x0005BC40
		protected override bool Filter(TurnContext context, BattleEvent @event, PlayerState owner, PlayerState target)
		{
			BattleResult battleResult = @event.BattleResult;
			return battleResult.Attacker_StartState.SubCategory == GamePieceCategory.Titan && battleResult.Defender_StartState.SubCategory == GamePieceCategory.Titan && base.Filter(context, @event, owner, target);
		}
	}
}
