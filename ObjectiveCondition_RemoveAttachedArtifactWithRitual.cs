using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x0200059F RID: 1439
	[Serializable]
	public class ObjectiveCondition_RemoveAttachedArtifactWithRitual : ObjectiveCondition_EventFilter<RitualCastEvent>
	{
		// Token: 0x06001B25 RID: 6949 RVA: 0x0005E898 File Offset: 0x0005CA98
		protected override bool Filter(TurnContext context, RitualCastEvent @event, PlayerState owner, PlayerState target, out int strength)
		{
			if (!base.Filter(context, @event, owner, target, out strength))
			{
				return false;
			}
			IEnumerable<Identifier> ids = from t in @event.Enumerate<AttachmentRemovedEvent>()
			select t.RemovedAttachment;
			strength = context.CurrentTurn.EnumerateGameItems<Artifact>(ids).Count<Artifact>();
			return strength > 0;
		}
	}
}
