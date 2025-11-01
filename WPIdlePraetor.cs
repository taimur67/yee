using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x02000173 RID: 371
	public class WPIdlePraetor : WorldProperty
	{
		// Token: 0x06000719 RID: 1817 RVA: 0x000226C2 File Offset: 0x000208C2
		public WPIdlePraetor(Identifier praetorID)
		{
			this.PraetorID = praetorID;
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x000226D4 File Offset: 0x000208D4
		public static bool Check(TurnState turn, PlayerState player, Identifier praetorId = Identifier.Invalid)
		{
			IEnumerable<Praetor> enumerable = player.VaultedItems.Select(new Func<Identifier, GameItem>(turn.FetchGameItem)).OfType<Praetor>();
			if (praetorId != Identifier.Invalid)
			{
				return enumerable.Any((Praetor p) => p.Id == praetorId);
			}
			return IEnumerableExtensions.Any<Praetor>(enumerable);
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x0002272D File Offset: 0x0002092D
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return WPIdlePraetor.Check(viewContext.CurrentTurn, playerState, this.PraetorID);
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x00022744 File Offset: 0x00020944
		public override WPProvidesEffect ProvidesEffect(WorldProperty precondition)
		{
			WPIdlePraetor wpidlePraetor = precondition as WPIdlePraetor;
			if (wpidlePraetor == null)
			{
				return WPProvidesEffect.No;
			}
			bool flag = wpidlePraetor.PraetorID != Identifier.Invalid;
			bool flag2 = this.PraetorID != Identifier.Invalid;
			if (flag && flag2 && this.PraetorID != wpidlePraetor.PraetorID)
			{
				return WPProvidesEffect.No;
			}
			if (WPIdlePraetor.Check(this.OwningPlanner.PlayerViewOfTurnState, this.OwningPlanner.PlayerState, this.PraetorID))
			{
				return WPProvidesEffect.Redundant;
			}
			return WPProvidesEffect.Yes;
		}

		// Token: 0x04000341 RID: 833
		public Identifier PraetorID;
	}
}
