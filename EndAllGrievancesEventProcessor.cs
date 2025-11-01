using System;

namespace LoG
{
	// Token: 0x020005F7 RID: 1527
	public class EndAllGrievancesEventProcessor : GrandEventActionProcessor<EndAllGrievancesEventOrder, EndAllGrievancesEventStaticData>
	{
		// Token: 0x06001C91 RID: 7313 RVA: 0x00062810 File Offset: 0x00060A10
		public override Result ProcessImmediate(ActionProcessContext context)
		{
			this.TurnProcessContext.PaperworkRestructurePlayed = true;
			return Result.Success;
		}

		// Token: 0x06001C92 RID: 7314 RVA: 0x00062824 File Offset: 0x00060A24
		protected override Result ProcessInternal(PlayGrandEventOrder order)
		{
			foreach (DiplomaticPairStatus diplomaticPairStatus in base._currentTurn.CurrentDiplomaticTurn.Standings)
			{
				bool flag = false;
				if (diplomaticPairStatus.DiplomaticState is VendettaState)
				{
					flag = true;
				}
				else
				{
					PendingDiplomacy_Grievance pendingDiplomacy_Grievance = diplomaticPairStatus.DiplomaticState as PendingDiplomacy_Grievance;
					if (pendingDiplomacy_Grievance != null)
					{
						OrderTypes cause = pendingDiplomacy_Grievance.Cause;
						if (cause == OrderTypes.Demand || cause == OrderTypes.Extort || cause == OrderTypes.Insult || cause == OrderTypes.Humiliate || cause == OrderTypes.VileCalumny)
						{
							flag = true;
						}
					}
					else
					{
						PendingDiplomacyState pendingDiplomacyState = diplomaticPairStatus.DiplomaticState as PendingDiplomacyState;
						if (pendingDiplomacyState != null && (pendingDiplomacyState.DiplomaticPendingValue & (DiplomaticPendingValue.MakeDemand | DiplomaticPendingValue.Insult | DiplomaticPendingValue.Extort | DiplomaticPendingValue.Humiliate | DiplomaticPendingValue.VileCalumny)) != DiplomaticPendingValue.None)
						{
							flag = true;
						}
					}
				}
				if (flag)
				{
					diplomaticPairStatus.SetNeutral(this.TurnProcessContext, true);
					diplomaticPairStatus.ResetCooldowns();
				}
			}
			return Result.Success;
		}
	}
}
