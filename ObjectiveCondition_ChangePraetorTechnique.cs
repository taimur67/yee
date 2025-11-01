using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x0200055E RID: 1374
	[Serializable]
	public class ObjectiveCondition_ChangePraetorTechnique : ObjectiveCondition_EventFilter<ManuscriptEvent>
	{
		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06001A7A RID: 6778 RVA: 0x0005C3B3 File Offset: 0x0005A5B3
		protected override bool CanSupportTargets
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001A7B RID: 6779 RVA: 0x0005C3B8 File Offset: 0x0005A5B8
		protected override bool Filter(TurnContext context, ManuscriptEvent @event, PlayerState owner, PlayerState target)
		{
			if (!base.Filter(context, @event, owner, target))
			{
				return false;
			}
			if (@event.Category != ManuscriptCategory.Manual)
			{
				return false;
			}
			PraetorCombatMoveTypeChangedEvent praetorCombatMoveTypeChangedEvent;
			if (!@event.TryGet<PraetorCombatMoveTypeChangedEvent>(out praetorCombatMoveTypeChangedEvent))
			{
				return false;
			}
			PraetorCombatMoveStaticData praetorCombatMoveStaticData;
			if (!context.Database.TryFetch<PraetorCombatMoveStaticData>(praetorCombatMoveTypeChangedEvent.OldValue.CombatMoveReference, out praetorCombatMoveStaticData))
			{
				return false;
			}
			PraetorCombatMoveStaticData praetorCombatMoveStaticData2;
			if (!context.Database.TryFetch<PraetorCombatMoveStaticData>(praetorCombatMoveTypeChangedEvent.NewValue.CombatMoveReference, out praetorCombatMoveStaticData2))
			{
				return false;
			}
			if (praetorCombatMoveStaticData.TechniqueType.Id == praetorCombatMoveStaticData2.TechniqueType.Id)
			{
				return false;
			}
			if (this.MinimumStylesKnown > 1)
			{
				Praetor praetor;
				if (!context.CurrentTurn.TryFetchGameItem<Praetor>(@event.TargetId, out praetor))
				{
					return false;
				}
				HashSet<string> hashSet = new HashSet<string>();
				foreach (PraetorCombatMoveInstance praetorCombatMoveInstance in praetor.CombatMoves)
				{
					PraetorCombatMoveStaticData praetorCombatMoveStaticData3;
					if (context.Database.TryFetch<PraetorCombatMoveStaticData>(praetorCombatMoveInstance.CombatMoveReference, out praetorCombatMoveStaticData3))
					{
						hashSet.Add(praetorCombatMoveStaticData3.TechniqueType.Id);
					}
				}
				if (hashSet.Count < this.MinimumStylesKnown)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04000BFD RID: 3069
		public int MinimumStylesKnown;
	}
}
