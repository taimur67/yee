using System;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x0200032A RID: 810
	public class MessageTrigger_OnTargetInvokeManuscriptCategory : MessageTriggerCondition
	{
		// Token: 0x06000FA1 RID: 4001 RVA: 0x0003DE86 File Offset: 0x0003C086
		public override string GetDescription()
		{
			return string.Format("Message <{0}> when <{1}> invokes manuscript of type <{2}>", this.RecipientArchfiendId, this.TargetArchfiendId, this.ManuscriptCategory);
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x0003DEAC File Offset: 0x0003C0AC
		public override bool Evaluate(TurnState newTurn, TurnProcessContext context, GameDatabase database)
		{
			PlayerState playerState = context.CurrentTurn.FindPlayerState(this.RecipientArchfiendId);
			foreach (ManuscriptEvent manuscriptEvent in context.CurrentTurn.GetGameEvents().OfType<ManuscriptEvent>())
			{
				Manuscript manuscript;
				if (manuscriptEvent.TriggeringPlayerID == playerState.Id && context.CurrentTurn.TryFetchGameItem<Manuscript>(manuscriptEvent.ManuscriptId, out manuscript) && manuscript.GetCategory(database) == this.ManuscriptCategory)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000749 RID: 1865
		public string TargetArchfiendId;

		// Token: 0x0400074A RID: 1866
		public ManuscriptCategory ManuscriptCategory;
	}
}
