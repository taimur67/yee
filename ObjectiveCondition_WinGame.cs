using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200058C RID: 1420
	[Serializable]
	public class ObjectiveCondition_WinGame : ObjectiveCondition_GameOver
	{
		// Token: 0x06001AFE RID: 6910 RVA: 0x0005E428 File Offset: 0x0005C628
		public override bool CheckCompleteStatus(TurnContext context, PlayerState owner, GameVictory victory)
		{
			return victory.WinningPlayerID == owner.Id && (this.RequiredType == VictoryType.None || this.RequiredType == victory.PrimaryVictoryType) && victory.ManipulationType == VictoryManipulationType.None && base.CheckCompleteStatus(context, owner, victory);
		}

		// Token: 0x04000C43 RID: 3139
		[JsonProperty]
		[DefaultValue(VictoryType.Prestige)]
		public VictoryType RequiredType;
	}
}
