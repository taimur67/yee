using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x02000156 RID: 342
	public class WPCanAttack : WorldProperty<WPCanAttack>
	{
		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060006BA RID: 1722 RVA: 0x000216F2 File Offset: 0x0001F8F2
		public override bool MustBeSuccessfullFulfilledAsPrecondition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x000216F5 File Offset: 0x0001F8F5
		public WPCanAttack()
		{
			this.ArchfiendID = int.MinValue;
			this.Anybody = true;
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0002170F File Offset: 0x0001F90F
		public WPCanAttack(int archfiendID, bool anybody = false)
		{
			this.ArchfiendID = archfiendID;
			this.Anybody = anybody;
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x00021728 File Offset: 0x0001F928
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			if (this.Anybody)
			{
				using (IEnumerator<PlayerState> enumerator = viewContext.CurrentTurn.EnumeratePlayerStates(false, false).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PlayerState playerState2 = enumerator.Current;
						if (playerState2.Id != playerState.Id && viewContext.CurrentTurn.CombatAuthorizedBetween(playerState.Id, playerState2.Id))
						{
							return true;
						}
					}
					return false;
				}
			}
			return playerState.Id != this.ArchfiendID && viewContext.CurrentTurn.CombatAuthorizedBetween(playerState.Id, this.ArchfiendID);
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x000217D4 File Offset: 0x0001F9D4
		public override WPProvidesEffect ProvidesEffectInternal(WPCanAttack vendettaPrecondition)
		{
			if (this.ArchfiendID != vendettaPrecondition.ArchfiendID)
			{
				return WPProvidesEffect.No;
			}
			return WPProvidesEffect.Yes;
		}

		// Token: 0x0400030C RID: 780
		public int ArchfiendID;

		// Token: 0x0400030D RID: 781
		public bool Anybody;
	}
}
