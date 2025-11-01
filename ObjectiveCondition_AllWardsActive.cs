using System;
using System.Collections.Generic;
using System.Linq;
using Core.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000547 RID: 1351
	[Serializable]
	public class ObjectiveCondition_AllWardsActive : ObjectiveCondition
	{
		// Token: 0x06001A34 RID: 6708 RVA: 0x0005B60C File Offset: 0x0005980C
		protected override int CalculateTotalProgress(TurnContext context, PlayerState owner, bool isInitialProgress)
		{
			ObjectiveCondition_AllWardsActive.<>c__DisplayClass1_0 CS$<>8__locals1 = new ObjectiveCondition_AllWardsActive.<>c__DisplayClass1_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.owner = owner;
			if (isInitialProgress)
			{
				return 0;
			}
			TurnState currentTurn = context.CurrentTurn;
			CS$<>8__locals1.currentlyOnRitualTable = IEnumerableExtensions.ToList<ActiveRitual>(from current in currentTurn.GetActiveRituals(CS$<>8__locals1.owner)
			where CS$<>8__locals1.<>4__this.AcceptedRituals.Any((ConfigRef<RitualStaticData> accepted) => accepted.Id == current.StaticDataId)
			select current);
			IEnumerable<RitualCastEvent> source = currentTurn.GetGameEvents().OfType<RitualCastEvent>().Where(new Func<RitualCastEvent, bool>(CS$<>8__locals1.<CalculateTotalProgress>g__IsValidCastingRitual|1));
			return CS$<>8__locals1.currentlyOnRitualTable.Count + source.Count<RitualCastEvent>();
		}

		// Token: 0x04000BE2 RID: 3042
		public List<ConfigRef<RitualStaticData>> AcceptedRituals = new List<ConfigRef<RitualStaticData>>();
	}
}
