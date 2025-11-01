using System;

namespace LoG
{
	// Token: 0x02000125 RID: 293
	internal class RespondToDemand : DecisionResponseGOAPNode<MakeDemandDecisionRequest, MakeDemandDecisionResponse>
	{
		// Token: 0x17000135 RID: 309
		// (get) Token: 0x0600055C RID: 1372 RVA: 0x0001A086 File Offset: 0x00018286
		public override string ActionName
		{
			get
			{
				return string.Format("Respond to Demand: {0}", this.YesNoResponse);
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x0600055D RID: 1373 RVA: 0x0001A09D File Offset: 0x0001829D
		public override ActionID ID
		{
			get
			{
				return ActionID.Decision_RespondToDemand;
			}
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x0001A0A1 File Offset: 0x000182A1
		public RespondToDemand(YesNo yesNoResponse, MakeDemandDecisionRequest request)
		{
			this.YesNoResponse = yesNoResponse;
			this.Request = request;
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x0001A0B8 File Offset: 0x000182B8
		public override void Prepare()
		{
			base.AddConstraint(new WPDecisionNeedsMaking(this.Request));
			if (this.YesNoResponse != YesNo.Yes)
			{
				base.AddEffect(new WPCanAttack(this.Request.RequestingPlayerId, false));
				Cost cost = new Cost();
				int prestigeWager = this.Request.PrestigeWager;
				cost.Set(ResourceTypes.Prestige, prestigeWager);
				base.AddEffect(new WPTribute(cost));
			}
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x0001A11D File Offset: 0x0001831D
		protected override MakeDemandDecisionResponse GenerateDecision()
		{
			MakeDemandDecisionResponse makeDemandDecisionResponse = base.GenerateDecision();
			makeDemandDecisionResponse.Choice = this.YesNoResponse;
			return makeDemandDecisionResponse;
		}

		// Token: 0x040002AD RID: 685
		public YesNo YesNoResponse;
	}
}
