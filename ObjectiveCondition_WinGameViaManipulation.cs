using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200058D RID: 1421
	[Serializable]
	public class ObjectiveCondition_WinGameViaManipulation : ObjectiveCondition_GameOver
	{
		// Token: 0x06001B00 RID: 6912 RVA: 0x0005E46E File Offset: 0x0005C66E
		public override bool CheckCompleteStatus(TurnContext context, PlayerState owner, GameVictory victory)
		{
			return victory.WinningPlayerID == owner.Id && victory.ManipulationType != VictoryManipulationType.None && (this.RequiredType == VictoryManipulationType.None || this.RequiredType == victory.ManipulationType) && base.CheckCompleteStatus(context, owner, victory);
		}

		// Token: 0x04000C44 RID: 3140
		[JsonProperty]
		[DefaultValue(VictoryManipulationType.None)]
		public VictoryManipulationType RequiredType;
	}
}
