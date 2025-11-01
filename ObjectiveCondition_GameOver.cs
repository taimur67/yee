using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200058B RID: 1419
	[Serializable]
	public class ObjectiveCondition_GameOver : BooleanStateObjectiveCondition
	{
		// Token: 0x06001AFB RID: 6907 RVA: 0x0005E3E4 File Offset: 0x0005C5E4
		protected override bool CheckCompleteStatus(TurnContext context, PlayerState owner, bool isInitialProgress)
		{
			GameVictory victory = context.CurrentTurn.Victory;
			return victory != null && (!this.MustBeMultiplayer || context.IsMultiplayer()) && this.CheckCompleteStatus(context, owner, victory);
		}

		// Token: 0x06001AFC RID: 6908 RVA: 0x0005E41D File Offset: 0x0005C61D
		public virtual bool CheckCompleteStatus(TurnContext context, PlayerState owner, GameVictory victory)
		{
			return true;
		}

		// Token: 0x04000C42 RID: 3138
		[JsonProperty]
		public bool MustBeMultiplayer;
	}
}
