using System;

namespace LoG
{
	// Token: 0x02000102 RID: 258
	public class ActionAssertWeakness : ActionOrderGOAPNode<OrderAssertWeakness>
	{
		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x0600045F RID: 1119 RVA: 0x000132B4 File Offset: 0x000114B4
		public override ActionID ID
		{
			get
			{
				return ActionID.Diplo_Assert_Weakness;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000460 RID: 1120 RVA: 0x000132B8 File Offset: 0x000114B8
		public override string ActionName
		{
			get
			{
				return "Assert weakness of  " + base.Context.DebugName(this.TargetArchfiendID);
			}
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x000132D5 File Offset: 0x000114D5
		public ActionAssertWeakness(int targetArchfiendID)
		{
			this.TargetArchfiendID = targetArchfiendID;
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x000132E4 File Offset: 0x000114E4
		protected override OrderAssertWeakness GenerateOrder()
		{
			if (this._grievance == null)
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Error("Generating assertion of weakness order with no grievance!");
				}
			}
			return new OrderAssertWeakness
			{
				TargetID = this.TargetArchfiendID,
				GrievanceResponse = this._grievance
			};
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00013320 File Offset: 0x00011520
		public override void Prepare()
		{
			PlayerState playerState = this.OwningPlanner.PlayerState;
			PlayerState playerState2 = this.OwningPlanner.TrueTurn.FindPlayerState(this.TargetArchfiendID, null);
			bool flag;
			float num;
			if (!this.OwningPlanner.TryGenerateGrievance(playerState2, out this._grievance, out flag, out num) || this._grievance == null)
			{
				base.Disable("Assert Weakness couldn't generate a good grievance.");
				return;
			}
			bool flag2 = this._grievance is VendettaContext;
			base.AddConstraint(new WPIsActionPlannedThisTurn(this.ID)
			{
				InvertLogic = true
			});
			base.AddConstraint(new WPNeutralTitanOnWarpath
			{
				InvertLogic = true
			});
			bool flag3 = this.OwningPlanner.IsDogPileTarget(this.TargetArchfiendID);
			if (flag2)
			{
				base.AddPrecondition(new WPThreaten(playerState.Id, this.TargetArchfiendID));
				if (!flag3)
				{
					base.AddPrecondition(new WPMilitarySuperiority(playerState.Id, this.TargetArchfiendID, 0.5f));
				}
				base.AddEffect(new WPCanAttack(this.TargetArchfiendID, false));
				base.AddEffect(new WPPrestigeProduction(WorldProperty.MaxWeight));
				base.AddScalarCostModifier(num - 1f, PFCostModifier.Heuristic_Bonus);
			}
			else
			{
				base.AddPrecondition(new WPHasAnyPraetor());
				base.AddEffect(new WPCanDuel(this.TargetArchfiendID));
				if (!flag3)
				{
					base.AddPrecondition(new WPDuelAdvantage(this.TargetArchfiendID));
				}
				base.AddScalarCostModifier(num - 1f, PFCostModifier.Heuristic_Bonus);
			}
			base.AddEffect(new WPUndermineArchfiend(playerState2.Id));
			int num2;
			switch (this.OwningPlanner.PlayerState.Rank)
			{
			case Rank.Marquis:
				num2 = 15;
				break;
			case Rank.Duke:
				num2 = 20;
				break;
			case Rank.Prince:
				num2 = 25;
				break;
			default:
				num2 = 10;
				break;
			}
			int prestigeAmount = num2;
			base.AddEffect(WPTribute.PrestigeOnly(prestigeAmount));
			base.Prepare();
		}

		// Token: 0x0400024D RID: 589
		public int TargetArchfiendID;

		// Token: 0x0400024E RID: 590
		private GrievanceContext _grievance;
	}
}
