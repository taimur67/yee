using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000553 RID: 1363
	public class ObjectiveCondition_BribeArbiterTotal : ObjectiveCondition_EventFilter<PraetorDuelOutcomeEvent>
	{
		// Token: 0x06001A56 RID: 6742 RVA: 0x0005BE14 File Offset: 0x0005A014
		protected override bool Filter(TurnContext context, PraetorDuelOutcomeEvent @event, PlayerState owner, PlayerState target, out int strength)
		{
			if (!base.Filter(context, @event, owner, target, out strength))
			{
				return false;
			}
			strength = 0;
			PraetorDuelParticipantData praetorDuelParticipantData;
			if (!@event.TryGetParticipantData(owner.Id, out praetorDuelParticipantData))
			{
				return false;
			}
			if (praetorDuelParticipantData.Bribe == null || praetorDuelParticipantData.Bribe.Total.IsZero)
			{
				return false;
			}
			if (this.Factor == BribeContributionFactor.ValueSum)
			{
				strength = praetorDuelParticipantData.Bribe.Total.ValueSum;
			}
			else if (this.Factor == BribeContributionFactor.TotalCards)
			{
				strength = praetorDuelParticipantData.Bribe.Resources.Count;
			}
			return true;
		}

		// Token: 0x04000BF0 RID: 3056
		[JsonProperty]
		public BribeContributionFactor Factor;
	}
}
