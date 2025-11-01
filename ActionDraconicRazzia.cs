using System;

namespace LoG
{
	// Token: 0x02000106 RID: 262
	public class ActionDraconicRazzia : ActionOrderGOAPNode<OrderDeclareDraconicRazzia>
	{
		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000479 RID: 1145 RVA: 0x00013881 File Offset: 0x00011A81
		public static string ArchfiendId
		{
			get
			{
				return "Astaroth";
			}
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00013888 File Offset: 0x00011A88
		public static bool CanBeUsedByArchfiend(PlayerState wouldBeUser)
		{
			return wouldBeUser.ArchfiendId == ActionDraconicRazzia.ArchfiendId;
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0001389A File Offset: 0x00011A9A
		public static bool IsPlanned(GOAPPlanner planner)
		{
			return planner.IsActionPlannedThisTurn(ActionDraconicRazzia._actionID);
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x0600047C RID: 1148 RVA: 0x000138A7 File Offset: 0x00011AA7
		public override ActionID ID
		{
			get
			{
				return ActionDraconicRazzia._actionID;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x0600047D RID: 1149 RVA: 0x000138AE File Offset: 0x00011AAE
		public override string ActionName
		{
			get
			{
				return "Draconic Razzia vs " + base.Context.DebugName(this.TargetPlayerID);
			}
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x000138CB File Offset: 0x00011ACB
		public ActionDraconicRazzia(int archfiendID)
		{
			this.TargetPlayerID = archfiendID;
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x000138DC File Offset: 0x00011ADC
		public override void Prepare()
		{
			base.AddConstraint(new WPNeutralTitanOnWarpath
			{
				InvertLogic = true
			});
			bool flag = this.OwningPlanner.IsDogPileTarget(this.TargetPlayerID);
			if (!flag)
			{
				base.AddPrecondition(new WPMilitarySuperiority(this.OwningPlanner.PlayerId, this.TargetPlayerID, 0.5f));
			}
			base.AddPrecondition(new WPThreaten(this.OwningPlanner.PlayerId, this.TargetPlayerID));
			base.AddEffect(new WPCanAttack(this.TargetPlayerID, false));
			if (flag)
			{
				base.AddEffect(new WPUndermineArchfiend(this.TargetPlayerID));
			}
			base.AddScalarCostModifier(-1f, PFCostModifier.Archfiend_Bonus);
			base.Prepare();
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00013983 File Offset: 0x00011B83
		protected override OrderDeclareDraconicRazzia GenerateOrder()
		{
			return new OrderDeclareDraconicRazzia
			{
				TargetID = this.TargetPlayerID
			};
		}

		// Token: 0x04000254 RID: 596
		private static readonly ActionID _actionID = ActionID.Diplo_Draconic_Razzia;

		// Token: 0x04000255 RID: 597
		public int TargetPlayerID;
	}
}
