using System;

namespace LoG
{
	// Token: 0x02000128 RID: 296
	public class RespondToInsult : DecisionResponseGOAPNode<InsultDecisionRequest, InsultDecisionResponse>
	{
		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000568 RID: 1384 RVA: 0x0001A21F File Offset: 0x0001841F
		public override string ActionName
		{
			get
			{
				return string.Format("Respond to Insult: {0}", this.YesNoResponse);
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000569 RID: 1385 RVA: 0x0001A236 File Offset: 0x00018436
		public override ActionID ID
		{
			get
			{
				return ActionID.Decision_RespondToInsult;
			}
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x0001A23A File Offset: 0x0001843A
		public RespondToInsult(YesNo response, InsultDecisionRequest request)
		{
			this.YesNoResponse = response;
			this.Request = request;
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x0001A250 File Offset: 0x00018450
		public override void Prepare()
		{
			InsultDecisionRequest request = this.Request;
			base.AddConstraint(new WPDecisionNeedsMaking(this.Request));
			if (this.YesNoResponse == YesNo.Yes)
			{
				Cost cost = new Cost();
				cost.Add(ResourceTypes.Prestige, request.PrestigeWager);
				base.AddPrecondition(new WPTribute(cost));
				return;
			}
			base.AddEffect(new WPCanAttack(request.RequestingPlayerId, false));
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x0001A2B1 File Offset: 0x000184B1
		protected override InsultDecisionResponse GenerateDecision()
		{
			InsultDecisionResponse insultDecisionResponse = base.GenerateDecision();
			insultDecisionResponse.Choice = this.YesNoResponse;
			return insultDecisionResponse;
		}

		// Token: 0x040002B4 RID: 692
		public YesNo YesNoResponse;
	}
}
