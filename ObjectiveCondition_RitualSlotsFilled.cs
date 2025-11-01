using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x020005A4 RID: 1444
	[Serializable]
	public class ObjectiveCondition_RitualSlotsFilled : ObjectiveCondition
	{
		// Token: 0x06001B2D RID: 6957 RVA: 0x0005E96C File Offset: 0x0005CB6C
		protected override int CalculateTotalProgress(TurnContext context, PlayerState owner, bool isInitialProgress)
		{
			ObjectiveCondition_RitualSlotsFilled.<>c__DisplayClass0_0 CS$<>8__locals1 = new ObjectiveCondition_RitualSlotsFilled.<>c__DisplayClass0_0();
			CS$<>8__locals1.owner = owner;
			if (isInitialProgress)
			{
				return 0;
			}
			TurnState currentTurn = context.CurrentTurn;
			CS$<>8__locals1.currentlyOnRitualTable = IEnumerableExtensions.ToList<GameItem>(CS$<>8__locals1.owner.RitualState.SlottedItems.Select(new Func<Identifier, GameItem>(currentTurn.FetchGameItem)));
			IEnumerable<RitualCastEvent> source = currentTurn.GetGameEvents().OfType<RitualCastEvent>().Where(new Func<RitualCastEvent, bool>(CS$<>8__locals1.<CalculateTotalProgress>g__IsValidCastingRitual|0));
			return CS$<>8__locals1.currentlyOnRitualTable.Count + source.Count<RitualCastEvent>();
		}
	}
}
