using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200058E RID: 1422
	[Serializable]
	public class ObjectiveCondition_LoseGameViaManipulation : ObjectiveCondition_GameOver
	{
		// Token: 0x06001B02 RID: 6914 RVA: 0x0005E4B4 File Offset: 0x0005C6B4
		public override bool CheckCompleteStatus(TurnContext context, PlayerState owner, GameVictory victory)
		{
			return victory.WinningPlayerID != owner.Id && victory.PuppetPlayerID == owner.Id && (this.RequiredType == VictoryManipulationType.None || this.RequiredType == victory.ManipulationType) && base.CheckCompleteStatus(context, owner, victory);
		}

		// Token: 0x04000C45 RID: 3141
		[JsonProperty]
		[DefaultValue(VictoryManipulationType.None)]
		public VictoryManipulationType RequiredType;
	}
}
