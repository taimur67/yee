using System;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000319 RID: 793
	public class MessageTrigger_OnPoPLiberatedTargeted : MessageTriggerCondition
	{
		// Token: 0x06000F6D RID: 3949 RVA: 0x0003D1EC File Offset: 0x0003B3EC
		public override string GetDescription()
		{
			return string.Format("Message <{0}> captured <{1}> that is claimed by <{2}>", this.RecipientArchfiendId, this.TargetPoP, this.TargetArchfiendId);
		}

		// Token: 0x06000F6E RID: 3950 RVA: 0x0003D20A File Offset: 0x0003B40A
		public override bool Evaluate(TurnState newTurn, TurnProcessContext context, GameDatabase database)
		{
			return false;
		}

		// Token: 0x04000736 RID: 1846
		public GamePiece TargetPoP;

		// Token: 0x04000737 RID: 1847
		public string TargetArchfiendId;
	}
}
