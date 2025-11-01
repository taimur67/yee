using System;

namespace LoG
{
	// Token: 0x020000DD RID: 221
	public class ActionBidOnArtifact : ActionOrderGOAPNode<OrderMakeBid>
	{
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000323 RID: 803 RVA: 0x0000E973 File Offset: 0x0000CB73
		public override bool ReducePriorityWhenTitansNeedActions
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000324 RID: 804 RVA: 0x0000E976 File Offset: 0x0000CB76
		public override bool DoDynamicScoring
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000325 RID: 805 RVA: 0x0000E979 File Offset: 0x0000CB79
		public override string ActionName
		{
			get
			{
				return "Bid on artifact: " + base.Context.DebugName(this.ArtifactID);
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000326 RID: 806 RVA: 0x0000E996 File Offset: 0x0000CB96
		public override ActionID ID
		{
			get
			{
				return ActionID.Bid_On_Artifact;
			}
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000E99A File Offset: 0x0000CB9A
		public ActionBidOnArtifact(Identifier identifier)
		{
			this.ArtifactID = identifier;
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000E9B0 File Offset: 0x0000CBB0
		public override void Prepare()
		{
			base.AddEffect(new WPArtifactInVault(this.ArtifactID));
			base.Prepare();
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000E9C9 File Offset: 0x0000CBC9
		protected override OrderMakeBid GenerateOrder()
		{
			return new OrderMakeBid(this.ArtifactID);
		}

		// Token: 0x040001FB RID: 507
		public Identifier ArtifactID = Identifier.Invalid;
	}
}
