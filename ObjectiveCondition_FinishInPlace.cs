using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000588 RID: 1416
	public class ObjectiveCondition_FinishInPlace : ObjectiveCondition_GameOver
	{
		// Token: 0x06001AF1 RID: 6897 RVA: 0x0005DFAC File Offset: 0x0005C1AC
		public override bool CheckCompleteStatus(TurnContext context, PlayerState owner, GameVictory victory)
		{
			if (victory.PrimaryVictoryType == this.RequiredType)
			{
				return false;
			}
			int placeIndex = this.PlaceIndex;
			List<int> playerIdsInWinningOrder = victory.PlayerIdsInWinningOrder;
			int? num = (playerIdsInWinningOrder != null) ? new int?(playerIdsInWinningOrder.Count) : null;
			return !(placeIndex > num.GetValueOrDefault() & num != null) && owner.Id == victory.PlayerIdsInWinningOrder[this.PlaceIndex] && base.CheckCompleteStatus(context, owner, victory);
		}

		// Token: 0x04000C40 RID: 3136
		[JsonProperty]
		[DefaultValue(VictoryType.Prestige)]
		public VictoryType RequiredType;

		// Token: 0x04000C41 RID: 3137
		[JsonProperty]
		public int PlaceIndex;
	}
}
