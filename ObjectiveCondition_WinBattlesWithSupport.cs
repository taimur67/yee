using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x020005B8 RID: 1464
	[Serializable]
	public class ObjectiveCondition_WinBattlesWithSupport : ObjectiveCondition_WinBattles
	{
		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06001B62 RID: 7010 RVA: 0x0005F200 File Offset: 0x0005D400
		public override string LocalizationKey
		{
			get
			{
				return "WinBattlesWithSupport";
			}
		}

		// Token: 0x06001B63 RID: 7011 RVA: 0x0005F208 File Offset: 0x0005D408
		protected override bool Filter(TurnContext context, BattleEvent @event, PlayerState owner, PlayerState target)
		{
			IReadOnlyList<Identifier> readOnlyList;
			IReadOnlyList<Identifier> readOnlyList2;
			return base.Filter(context, @event, owner, target) && @event.BattleResult.TryGetSupportingPiecesForPlayer(owner.Id, out readOnlyList, out readOnlyList2) && readOnlyList.Count > 0;
		}
	}
}
