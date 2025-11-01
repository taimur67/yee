using System;

namespace LoG
{
	// Token: 0x02000103 RID: 259
	public class ActionBecomeVassal : ActionOrderGOAPNode<OrderRequestToBeVassalizedByTarget>
	{
		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x000134D9 File Offset: 0x000116D9
		public override bool ReducePriorityWhenTitansNeedActions
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000465 RID: 1125 RVA: 0x000134DC File Offset: 0x000116DC
		public override string ActionName
		{
			get
			{
				return "Become Vassal to " + base.Context.DebugName(this.BloodLordID);
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x000134F9 File Offset: 0x000116F9
		public override ActionID ID
		{
			get
			{
				return ActionID.Diplo_Propose_Become_Vassal;
			}
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x000134FD File Offset: 0x000116FD
		public ActionBecomeVassal(int bloodLordID)
		{
			this.BloodLordID = bloodLordID;
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x00013514 File Offset: 0x00011714
		public override void Prepare()
		{
			base.AddConstraint(new WPDiplomaticCooldownConstraint(DiplomaticCooldownType.BecomeVassal, this.Cooldown));
			base.AddEffect(new WPReducedDiplomaticStress());
			PlayerState playerState = this.OwningPlanner.PlayerState;
			TurnState currentTurn = this.OwningPlanner.TrueContext.CurrentTurn;
			bool flag = RivalryProcessor.CaresAboutPrestige(currentTurn, playerState) && currentTurn.ConclaveFavouriteId == playerState.Id;
			bool flag2 = this.OwningPlanner.GameProgress >= 0.8f;
			bool flag3 = GoalAvoidElimination.AnyThreat(this.OwningPlanner);
			PlayerState conclaveFavourite = currentTurn.GetConclaveFavourite();
			if (conclaveFavourite == null)
			{
				base.Disable("There is no favourite");
				return;
			}
			PlayerState playerState2 = currentTurn.FindPlayerState(this.BloodLordID, null);
			if (playerState2 == null)
			{
				base.Disable(string.Format("Invalid player {0}", this.BloodLordID));
				return;
			}
			bool flag4 = conclaveFavourite.Id == playerState2.Id;
			bool flag5 = !flag4 && playerState2.SpendablePrestige + playerState.SpendablePrestige > conclaveFavourite.SpendablePrestige;
			bool flag6 = flag2 && !flag && (flag4 || flag5);
			if (!flag3 && !flag6)
			{
				base.Disable(string.Format("Becoming {0}'s vassal will not help us survive or win", this.BloodLordID));
				return;
			}
			if (!flag4 && !flag3)
			{
				base.AddScalarCostIncrease(0.5f, PFCostModifier.Heuristic_Bonus);
			}
			base.Prepare();
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00013660 File Offset: 0x00011860
		protected override OrderRequestToBeVassalizedByTarget GenerateOrder()
		{
			return new OrderRequestToBeVassalizedByTarget(this.BloodLordID, null);
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x0001366E File Offset: 0x0001186E
		public override Result SubmitAction(TurnContext context, PlayerState playerState)
		{
			Result result = base.SubmitAction(context, playerState);
			if (result.successful)
			{
				this.OwningPlanner.AIPersistentData.RecordDiplomaticAbilityUsed(DiplomaticCooldownType.BecomeVassal, context.CurrentTurn);
			}
			return result;
		}

		// Token: 0x0400024F RID: 591
		public int BloodLordID;

		// Token: 0x04000250 RID: 592
		public int Cooldown = 10;
	}
}
