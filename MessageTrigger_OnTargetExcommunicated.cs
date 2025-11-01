using System;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000328 RID: 808
	public class MessageTrigger_OnTargetExcommunicated : MessageTriggerCondition
	{
		// Token: 0x06000F9A RID: 3994 RVA: 0x0003DD96 File Offset: 0x0003BF96
		public override string GetDescription()
		{
			return "Message <" + this.RecipientArchfiendId + "> when they have been Excommunicated";
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x0003DDAD File Offset: 0x0003BFAD
		public override bool Evaluate(TurnState newTurn, TurnProcessContext context, GameDatabase database)
		{
			return context.CurrentTurn.FindPlayerState(this.RecipientArchfiendId).Excommunicated;
		}
	}
}
