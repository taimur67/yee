using System;
using System.Collections.Generic;
using Core.StaticData;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x0200018B RID: 395
	public class WPPraetorWithMoveType : WorldProperty<WPPraetorWithMoveType>
	{
		// Token: 0x0600075B RID: 1883 RVA: 0x00022F3E File Offset: 0x0002113E
		public WPPraetorWithMoveType(string allowedMoveType)
		{
			this.AllowedMoveType = allowedMoveType;
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x00022F50 File Offset: 0x00021150
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			foreach (Praetor praetor in viewContext.CurrentTurn.GetGameItemsControlledBy<Praetor>(playerState.Id))
			{
				foreach (PraetorCombatMoveInstance praetorCombatMoveInstance in praetor.CombatMoves)
				{
					ConfigRef<PraetorCombatMoveStaticData> combatMoveReference = praetorCombatMoveInstance.CombatMoveReference;
					PraetorCombatMoveStaticData praetorCombatMoveStaticData = viewContext.Database.Fetch(combatMoveReference);
					if (viewContext.Database.Fetch(praetorCombatMoveStaticData.TechniqueType).Id == this.AllowedMoveType)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x00023014 File Offset: 0x00021214
		public override WPProvidesEffect ProvidesEffectInternal(WPPraetorWithMoveType otherPrecondition)
		{
			if (otherPrecondition.AllowedMoveType == this.AllowedMoveType)
			{
				return WPProvidesEffect.Yes;
			}
			return WPProvidesEffect.No;
		}

		// Token: 0x0400035F RID: 863
		public string AllowedMoveType;

		// Token: 0x04000360 RID: 864
		private List<string> moveTypes;
	}
}
