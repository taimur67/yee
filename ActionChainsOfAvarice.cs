using System;

namespace LoG
{
	// Token: 0x02000104 RID: 260
	public class ActionChainsOfAvarice : ActionOrderGOAPNode<OrderRequestChainsOfAvarice>
	{
		// Token: 0x170000EB RID: 235
		// (get) Token: 0x0600046B RID: 1131 RVA: 0x00013697 File Offset: 0x00011897
		public override bool ReducePriorityWhenTitansNeedActions
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600046C RID: 1132 RVA: 0x0001369A File Offset: 0x0001189A
		public static string ArchfiendId
		{
			get
			{
				return "Mammon";
			}
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x000136A1 File Offset: 0x000118A1
		public static bool CanBeUsedByArchfiend(PlayerState wouldBeUser)
		{
			return wouldBeUser.ArchfiendId == ActionChainsOfAvarice.ArchfiendId;
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x0600046E RID: 1134 RVA: 0x000136B3 File Offset: 0x000118B3
		public override ActionID ID
		{
			get
			{
				return ActionID.Diplo_Chains_Of_Avarice;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x0600046F RID: 1135 RVA: 0x000136B7 File Offset: 0x000118B7
		public override string ActionName
		{
			get
			{
				return "Chains of Avarice vs  " + base.Context.DebugName(this.TargetPlayerID);
			}
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x000136D4 File Offset: 0x000118D4
		public ActionChainsOfAvarice(int targetPlayerID)
		{
			this.TargetPlayerID = targetPlayerID;
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x000136EC File Offset: 0x000118EC
		public override void Prepare()
		{
			base.AddConstraint(new WPIsActionPlannedThisTurn(this.ID)
			{
				InvertLogic = true
			});
			if (this.OwningPlanner.IsEndGame && !this.OwningPlanner.IsWinning(-2147483648) && this.OwningPlanner.IsWinning(this.TargetPlayerID))
			{
				base.Disable("Don't make it harder to attack the winning player");
				return;
			}
			base.AddConstraint(new WPDiplomaticCooldownConstraint(DiplomaticCooldownType.ChainsOfAvarice, this.Cooldown));
			base.AddEffect(new WPReducedDiplomaticStress());
			Cost cost = new Cost();
			cost[ResourceTypes.Souls] = WorldProperty.MaxWeight;
			cost[ResourceTypes.Ichor] = WorldProperty.MaxWeight;
			cost[ResourceTypes.Hellfire] = WorldProperty.MaxWeight;
			cost[ResourceTypes.Darkness] = WorldProperty.MaxWeight;
			base.AddEffect(new WPTribute(cost));
			base.AddScalarCostModifier(-0.4f, PFCostModifier.Archfiend_Bonus);
			base.Prepare();
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x000137C1 File Offset: 0x000119C1
		protected override OrderRequestChainsOfAvarice GenerateOrder()
		{
			return new OrderRequestChainsOfAvarice(this.TargetPlayerID)
			{
				TargetID = this.TargetPlayerID
			};
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x000137DA File Offset: 0x000119DA
		public override Result SubmitAction(TurnContext context, PlayerState playerState)
		{
			Result result = base.SubmitAction(context, playerState);
			if (result.successful)
			{
				this.OwningPlanner.AIPersistentData.RecordDiplomaticAbilityUsed(DiplomaticCooldownType.ChainsOfAvarice, context.CurrentTurn);
			}
			return result;
		}

		// Token: 0x04000251 RID: 593
		public int TargetPlayerID;

		// Token: 0x04000252 RID: 594
		public int Cooldown = 3;
	}
}
