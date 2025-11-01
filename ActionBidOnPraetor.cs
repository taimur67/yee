using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x020000E0 RID: 224
	public class ActionBidOnPraetor : ActionOrderGOAPNode<OrderMakeBid>
	{
		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000339 RID: 825 RVA: 0x0000EEE7 File Offset: 0x0000D0E7
		public override bool ReducePriorityWhenTitansNeedActions
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600033A RID: 826 RVA: 0x0000EEEA File Offset: 0x0000D0EA
		public override ActionID ID
		{
			get
			{
				return ActionID.Bid_On_Praetor;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600033B RID: 827 RVA: 0x0000EEEE File Offset: 0x0000D0EE
		public override bool DoDynamicScoring
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600033C RID: 828 RVA: 0x0000EEF1 File Offset: 0x0000D0F1
		public override string ActionName
		{
			get
			{
				return "Bid on Praetor " + base.Context.DebugName(this.PraetorID);
			}
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0000EF0E File Offset: 0x0000D10E
		public ActionBidOnPraetor(Identifier praetorID)
		{
			this.PraetorID = praetorID;
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000EF24 File Offset: 0x0000D124
		private void AddDuellingEffects()
		{
			PlayerState playerState = this.OwningPlanner.PlayerState;
			TurnState trueTurn = this.OwningPlanner.TrueTurn;
			Praetor praetor = trueTurn.FetchGameItem<Praetor>(this.PraetorID);
			PlayerState playerState2;
			bool flag = trueTurn.TryGetNemesis(playerState, out playerState2);
			PraetorHeuristics praetorHeuristics = this.OwningPlanner.PraetorHeuristics;
			if (!WPHasAnyPraetor.Check(trueTurn, this.OwningPlanner.PlayerId))
			{
				using (IEnumerator<PlayerState> enumerator = trueTurn.EnumeratePlayerStates(false, false).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PlayerState playerState3 = enumerator.Current;
						base.AddEffect(new WPDuelAdvantage(playerState3.Id));
						float amount;
						if (playerState3.Id != playerState.Id && praetorHeuristics.TryGetPraetorAdvantage(praetor, playerState3, out amount) && flag && playerState3.Id == playerState2.Id)
						{
							base.AddScalarCostReduction(amount, PFCostModifier.Heuristic_Bonus);
						}
					}
					return;
				}
			}
			foreach (PlayerState playerState4 in trueTurn.EnumeratePlayerStates(false, false))
			{
				float num;
				Praetor praetor2;
				float num2;
				if (playerState4.Id != playerState.Id && praetorHeuristics.TryGetPraetorAdvantage(praetor, playerState4, out num) && praetorHeuristics.TryGetBestPraetor(playerState, playerState4, out praetor2, out num2, true) && num >= num2 * 1.1f)
				{
					base.AddEffect(new WPDuelAdvantage(playerState4.Id));
					if (flag && playerState4.Id == playerState2.Id)
					{
						base.AddScalarCostReduction(num, PFCostModifier.Heuristic_Bonus);
					}
				}
			}
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0000F0B4 File Offset: 0x0000D2B4
		public override void Prepare()
		{
			PlayerState playerState = this.OwningPlanner.PlayerState;
			TurnState trueTurn = this.OwningPlanner.TrueTurn;
			if (trueTurn.FetchGameItem<Praetor>(this.PraetorID) == null)
			{
				base.Disable(string.Format("Invalid Praetor {0}", this.PraetorID));
				return;
			}
			base.AddEffect(new WPHasAnyPraetor());
			base.AddEffect(new WPIdlePraetor(this.PraetorID));
			if (!trueTurn.GetGameItemsControlledBy<Praetor>(playerState.Id).Any(new Func<Praetor, bool>(AIExtensions.IsWorthyTargetForManual)))
			{
				this.AddDuellingEffects();
			}
			base.Prepare();
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000F14A File Offset: 0x0000D34A
		protected override OrderMakeBid GenerateOrder()
		{
			return new OrderMakeBid(this.PraetorID);
		}

		// Token: 0x04000201 RID: 513
		private Identifier PraetorID = Identifier.Invalid;
	}
}
