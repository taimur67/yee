using System;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000317 RID: 791
	public class MessageTrigger_OnTargetInvokedManuscript : MessageTriggerCondition
	{
		// Token: 0x06000F67 RID: 3943 RVA: 0x0003D06E File Offset: 0x0003B26E
		public override string GetDescription()
		{
			return string.Concat(new string[]
			{
				"Message <",
				this.RecipientArchfiendId,
				"> after they invoke the <",
				this.ManuscriptNameKey,
				"> Manuscript"
			});
		}

		// Token: 0x06000F68 RID: 3944 RVA: 0x0003D0A8 File Offset: 0x0003B2A8
		public override bool Evaluate(TurnState newTurn, TurnProcessContext context, GameDatabase database)
		{
			PlayerState playerState = context.CurrentTurn.FindPlayerState(this.RecipientArchfiendId);
			foreach (ManuscriptEvent manuscriptEvent in context.CurrentTurn.GetGameEvents().OfType<ManuscriptEvent>())
			{
				Manuscript manuscript;
				if (manuscriptEvent.TriggeringPlayerID == playerState.Id && context.CurrentTurn.TryFetchGameItem<Manuscript>(manuscriptEvent.ManuscriptId, out manuscript) && !(this.ManuscriptNameKey != manuscript.NameKey))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000735 RID: 1845
		public string ManuscriptNameKey;
	}
}
