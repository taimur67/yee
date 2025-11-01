using System;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000315 RID: 789
	public class MessageTrigger_OnTargetDestroyedPoP : MessageTriggerCondition
	{
		// Token: 0x06000F5F RID: 3935 RVA: 0x0003CF74 File Offset: 0x0003B174
		public override string GetDescription()
		{
			return string.Format("Message <{0}> when they destroy <{1}>", this.RecipientArchfiendId, this.TargetPoP);
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x0003CF8C File Offset: 0x0003B18C
		public override bool Evaluate(TurnState newTurn, TurnProcessContext context, GameDatabase database)
		{
			return context.CurrentTurn.GetGameEvents().OfType<ItemBanishedEvent>().Any((ItemBanishedEvent t) => t.ItemId == this.TargetPoP.Id);
		}

		// Token: 0x04000732 RID: 1842
		public GamePiece TargetPoP;
	}
}
