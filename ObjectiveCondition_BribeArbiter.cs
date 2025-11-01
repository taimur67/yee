using System;

namespace LoG
{
	// Token: 0x02000551 RID: 1361
	[Serializable]
	public class ObjectiveCondition_BribeArbiter : ObjectiveCondition_EventFilter<PraetorDuelOutcomeEvent>
	{
		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06001A53 RID: 6739 RVA: 0x0005BDBB File Offset: 0x00059FBB
		public override string LocalizationKey
		{
			get
			{
				if (!base.IsTargeted)
				{
					return "BribeArbiter";
				}
				return "BribeArbiter.Targeted";
			}
		}

		// Token: 0x06001A54 RID: 6740 RVA: 0x0005BDD0 File Offset: 0x00059FD0
		protected override bool Filter(TurnContext context, PraetorDuelOutcomeEvent @event, PlayerState owner, PlayerState target)
		{
			ArbiterInterventionEvent arbiterInterventionEvent;
			return base.Filter(context, @event, owner, target) && @event.TryGet<ArbiterInterventionEvent>(out arbiterInterventionEvent) && arbiterInterventionEvent.FavoredPlayer == owner.Id;
		}
	}
}
