using System;

namespace LoG
{
	// Token: 0x02000105 RID: 261
	public class ActionDeclareBloodFeud : ActionOrderGOAPNode<OrderDeclareBloodFeud>
	{
		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000474 RID: 1140 RVA: 0x00013804 File Offset: 0x00011A04
		public override ActionID ID
		{
			get
			{
				return ActionID.Diplo_Declare_Blood_Feud;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000475 RID: 1141 RVA: 0x00013808 File Offset: 0x00011A08
		public override string ActionName
		{
			get
			{
				return "Blood Feud vs " + base.Context.DebugName(this.ArchfiendID);
			}
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00013825 File Offset: 0x00011A25
		public ActionDeclareBloodFeud(int archfiendID)
		{
			this.ArchfiendID = archfiendID;
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00013834 File Offset: 0x00011A34
		public override void Prepare()
		{
			base.AddPrecondition(new WPThreaten(this.OwningPlanner.PlayerState.Id, this.ArchfiendID));
			base.AddEffect(new WPCanEliminate(this.ArchfiendID));
			base.Prepare();
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x0001386E File Offset: 0x00011A6E
		protected override OrderDeclareBloodFeud GenerateOrder()
		{
			return new OrderDeclareBloodFeud
			{
				TargetID = this.ArchfiendID
			};
		}

		// Token: 0x04000253 RID: 595
		public int ArchfiendID;
	}
}
