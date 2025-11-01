using System;

namespace LoG
{
	// Token: 0x0200014E RID: 334
	public class GoalVendettaDestroyLegions : GoalVendettaBase
	{
		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060006A1 RID: 1697 RVA: 0x00021450 File Offset: 0x0001F650
		protected override bool IsFulfilledByMovingOutOfDanger
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060006A2 RID: 1698 RVA: 0x00021453 File Offset: 0x0001F653
		public override ActionID ID
		{
			get
			{
				return ActionID.Goal_Vendetta_Destroy_Legions;
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060006A3 RID: 1699 RVA: 0x00021457 File Offset: 0x0001F657
		public override string ActionName
		{
			get
			{
				return "Goal - Vendetta: destroy legions of " + base.Context.DebugName(this.ArchfiendID);
			}
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x00021474 File Offset: 0x0001F674
		public GoalVendettaDestroyLegions(int archfiendID)
		{
			this.ArchfiendID = archfiendID;
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x00021483 File Offset: 0x0001F683
		public override void Prepare()
		{
			base.AddPrecondition(new WPOpportunisticSupport());
			base.AddPrecondition(new WPCombatVsPlayer(this.ArchfiendID));
			base.Prepare();
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x000214A7 File Offset: 0x0001F6A7
		public override float CalcGoalSelectorRelevance(TurnState playerViewOfTurnState, PlayerState playerState)
		{
			return 1f;
		}
	}
}
