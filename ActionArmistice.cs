using System;
using System.Linq;
using LoG.Simulation;

namespace LoG
{
	// Token: 0x02000101 RID: 257
	public class ActionArmistice : ActionOrderGOAPNode<OrderSendEmissary>
	{
		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000458 RID: 1112 RVA: 0x00012F54 File Offset: 0x00011154
		public override bool ReducePriorityWhenTitansNeedActions
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x00012F57 File Offset: 0x00011157
		public override string ActionName
		{
			get
			{
				return string.Format("Armistice with player {0}", this._targetPlayerId);
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x0600045A RID: 1114 RVA: 0x00012F6E File Offset: 0x0001116E
		public override ActionID ID
		{
			get
			{
				return ActionID.Diplo_Armistice;
			}
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00012F72 File Offset: 0x00011172
		public ActionArmistice(int targetPlayerId)
		{
			this._targetPlayerId = targetPlayerId;
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00012F88 File Offset: 0x00011188
		public override void Prepare()
		{
			base.AddConstraint(new WPIsActionPlannedThisTurn(this.ID)
			{
				InvertLogic = true
			});
			base.AddConstraint(new WPDangerOfWar(this._targetPlayerId));
			if (this.OwningPlanner.IsEndGame && !this.OwningPlanner.IsWinning(-2147483648) && this.OwningPlanner.IsWinning(this._targetPlayerId))
			{
				base.Disable("Don't make it harder to attack the winning player");
				return;
			}
			PlayerState playerState = this.OwningPlanner.TrueTurn.FindPlayerState(this._targetPlayerId, null);
			if (playerState == null)
			{
				base.Disable(string.Format("Invalid target player {0}", this._targetPlayerId));
				return;
			}
			float num;
			float num2;
			this.OwningPlanner.GetGrievanceRisks(playerState, this.OwningPlanner.PlayerState, out num, out num2);
			float num3 = (3f * num2 + num) / 4f;
			PlayerState playerState2 = this.OwningPlanner.PlayerState;
			float num4;
			if (!this.OwningPlanner.ArchfiendHeuristics.TryGetDiplomaticStress(playerState2.Id, out num4))
			{
				base.Disable(string.Format("Could not retrieve Diplomatic stress for {0}", playerState2.Id));
				return;
			}
			int num5 = Math.Min(this.OwningPlanner.TrueContext.Rules.NumResourceSlots, playerState2.Resources.Count<ResourceNFT>());
			if (num5 == 0)
			{
				base.Disable(string.Format("{0} has no tribute so cannot send an emissary", playerState2.Id));
				return;
			}
			float num6 = num4 * (1f - num3);
			this._cardsToGive = Math.Clamp((int)((float)num5 * num6), 1, 8);
			int num7 = (int)(this._cardsToGive + playerState2.Rank);
			if (num7 < 3)
			{
				base.Disable(string.Format("{0} cards would mean a duration of {1}, which is not worth wasting an order on", this._cardsToGive, num7));
				return;
			}
			base.AddConstraint(new WPNumTributeTokens(this._cardsToGive));
			base.AddEffect(new WPReducedDiplomaticStress());
			PlayerState playerState3;
			if (this.OwningPlanner.AIPreviewContext.CurrentTurn.TryGetNemesis(this.OwningPlanner.PlayerState, out playerState3) && playerState3.Id == this._targetPlayerId)
			{
				base.AddScalarCostModifier(1f, PFCostModifier.Heuristic_Bonus);
			}
			if (this.OwningPlanner.PlayerState.Animosity.GetValue(this._targetPlayerId) < 0.5f)
			{
				base.AddScalarCostModifier(0.5f, PFCostModifier.Heuristic_Bonus);
			}
			if (this.OwningPlanner.ArchfiendHeuristics.GetThreatens(this._targetPlayerId, this.OwningPlanner.PlayerState.Id))
			{
				base.AddScalarCostModifier(-0.8f, PFCostModifier.Heuristic_Bonus);
			}
			base.AddConstraint(new WPDiplomaticCooldownConstraint(DiplomaticCooldownType.Armistice, 3));
			base.Prepare();
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00013215 File Offset: 0x00011415
		protected override OrderSendEmissary GenerateOrder()
		{
			return new OrderSendEmissary(this._targetPlayerId, null);
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00013224 File Offset: 0x00011424
		public override Result SubmitAction(TurnContext context, PlayerState playerState)
		{
			Cost cost = new Cost(this._cardsToGive);
			if (!playerState.CanAfford(cost))
			{
				return Result.Failure;
			}
			Payment payment = PaymentUtils.DeducePayment(this.OwningPlanner.PlayerState, cost, PaymentUtils.AutoPayMethods.Optimal, this._cardsToGive);
			if (payment.IsEmpty)
			{
				return Result.Failure;
			}
			base.Order.SetOffering(payment);
			Problem problem = base.SubmitAction(context, playerState) as Problem;
			if (problem != null)
			{
				return problem;
			}
			this.OwningPlanner.AIPersistentData.RecordDiplomaticAbilityUsed(DiplomaticCooldownType.Armistice, context.CurrentTurn);
			return Result.Success;
		}

		// Token: 0x0400024A RID: 586
		private const int Cooldown = 3;

		// Token: 0x0400024B RID: 587
		private int _targetPlayerId;

		// Token: 0x0400024C RID: 588
		private int _cardsToGive = 3;
	}
}
