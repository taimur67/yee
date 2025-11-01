using System;

namespace LoG
{
	// Token: 0x0200010B RID: 267
	public class ActionLureOfExcess : ActionOrderGOAPNode<OrderRequestLureOfExcess>
	{
		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000496 RID: 1174 RVA: 0x0001424C File Offset: 0x0001244C
		public override bool ReducePriorityWhenTitansNeedActions
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000497 RID: 1175 RVA: 0x0001424F File Offset: 0x0001244F
		public static string ArchfiendId
		{
			get
			{
				return "Erzsebet";
			}
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x00014256 File Offset: 0x00012456
		public static bool CanBeUsedByArchfiend(PlayerState wouldBeUser)
		{
			return wouldBeUser.ArchfiendId == ActionLureOfExcess.ArchfiendId;
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000499 RID: 1177 RVA: 0x00014268 File Offset: 0x00012468
		public override string ActionName
		{
			get
			{
				return string.Format("Use Lure Of Excess on player {0}", this.TargetPlayer);
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x0600049A RID: 1178 RVA: 0x0001427F File Offset: 0x0001247F
		public override ActionID ID
		{
			get
			{
				return ActionID.Diplo_Vile_LureOfExcess;
			}
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x00014283 File Offset: 0x00012483
		public ActionLureOfExcess(int targetPlayer)
		{
			this.TargetPlayer = targetPlayer;
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x000142A0 File Offset: 0x000124A0
		public override void Prepare()
		{
			base.AddConstraint(new WPIsActionPlannedThisTurn(this.ID)
			{
				InvertLogic = true
			});
			if (this.OwningPlanner.IsEndGame && !this.OwningPlanner.IsWinning(-2147483648) && this.OwningPlanner.IsWinning(this.TargetPlayer))
			{
				base.Disable("Don't make it harder to attack the winning player");
				return;
			}
			base.AddEffect(new WPReducedDiplomaticStress());
			base.AddEffect(new WPUndermineArchfiend(this.TargetPlayer));
			base.AddScalarCostModifier(-1f, PFCostModifier.Heuristic_Bonus);
			if (this.OwningPlanner.PlayerState.Animosity.GetValue(this.TargetPlayer) < 0.3f)
			{
				base.AddScalarCostModifier(0.3f, PFCostModifier.Heuristic_Bonus);
			}
			base.AddConstraint(new WPDiplomaticCooldownConstraint(DiplomaticCooldownType.LureOfExcess, 1));
			base.Prepare();
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0001436D File Offset: 0x0001256D
		protected override OrderRequestLureOfExcess GenerateOrder()
		{
			return new OrderRequestLureOfExcess(this.TargetPlayer);
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0001437A File Offset: 0x0001257A
		public override Result SubmitAction(TurnContext context, PlayerState playerState)
		{
			Result result = base.SubmitAction(context, playerState);
			if (result.successful)
			{
				this.OwningPlanner.AIPersistentData.RecordDiplomaticAbilityUsed(DiplomaticCooldownType.LureOfExcess, context.CurrentTurn);
			}
			return result;
		}

		// Token: 0x04000263 RID: 611
		public int TargetPlayer = int.MinValue;
	}
}
