using System;

namespace LoG
{
	// Token: 0x02000127 RID: 295
	public class RespondToGrievanceChoice : DecisionResponseGOAPNode<GrievanceDecisionRequest, GrievanceDecisionResponse>
	{
		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000561 RID: 1377 RVA: 0x0001A131 File Offset: 0x00018331
		public override ActionID ID
		{
			get
			{
				return ActionID.Decision_RespondToGrievanceChoice;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000562 RID: 1378 RVA: 0x0001A135 File Offset: 0x00018335
		public override string ActionName
		{
			get
			{
				return string.Format("Respond to {0} Grievance: {1}", this.GrievanceType, this.YesNo);
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000563 RID: 1379 RVA: 0x0001A157 File Offset: 0x00018357
		public override bool ConsumesActionSlot
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x0001A15A File Offset: 0x0001835A
		public RespondToGrievanceChoice(GrievanceDecisionRequest request, GrievanceContext grievanceContext, GrievanceType nodeType, YesNo yesNo)
		{
			this.Request = request;
			this.GrievanceContext = grievanceContext;
			this.GrievanceType = nodeType;
			this.YesNo = yesNo;
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x0001A180 File Offset: 0x00018380
		public override void Prepare()
		{
			base.AddConstraint(new WPDecisionNeedsMaking(this.Request));
			GrievanceType grievanceType = this.GrievanceType;
			if (grievanceType != GrievanceType.Combat)
			{
				if (grievanceType == GrievanceType.Duel)
				{
					if (this.YesNo == YesNo.Yes)
					{
						base.AddEffect(new WPCanDuel(this.Request.RequestingPlayerId));
					}
				}
			}
			else if (this.YesNo == YesNo.Yes)
			{
				base.AddEffect(new WPCanAttack(this.Request.RequestingPlayerId, false));
			}
			base.Prepare();
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x0001A1F5 File Offset: 0x000183F5
		protected override GrievanceDecisionResponse GenerateDecision()
		{
			GrievanceDecisionResponse grievanceDecisionResponse = base.GenerateDecision();
			grievanceDecisionResponse.GrievanceResponse = this.GrievanceContext;
			grievanceDecisionResponse.Choice = this.YesNo;
			return grievanceDecisionResponse;
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0001A215 File Offset: 0x00018415
		public override Result SubmitAction(TurnContext context, PlayerState playerState)
		{
			return base.SubmitAction(context, playerState);
		}

		// Token: 0x040002B1 RID: 689
		public GrievanceContext GrievanceContext;

		// Token: 0x040002B2 RID: 690
		public GrievanceType GrievanceType;

		// Token: 0x040002B3 RID: 691
		public YesNo YesNo;
	}
}
