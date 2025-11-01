using System;

namespace LoG
{
	// Token: 0x020004A9 RID: 1193
	public class MaintainRitualDecisionProcessor : DecisionProcessor<MaintainRitualDecisionRequest, MaintainRitualDecisionResponse>
	{
		// Token: 0x06001664 RID: 5732 RVA: 0x00052B50 File Offset: 0x00050D50
		protected override Result Validate(MaintainRitualDecisionResponse response)
		{
			Result result;
			switch (response.Choice)
			{
			case YesNo.Undefined:
				result = Result.Failure;
				break;
			case YesNo.Yes:
				if (!base.request.RequiredPayment.IsZero)
				{
					result = this._player.CanAfford(base.request.RequiredPayment);
				}
				else
				{
					result = Result.Success;
				}
				break;
			case YesNo.No:
				result = Result.Success;
				break;
			default:
				result = Result.Failure;
				break;
			}
			return result;
		}

		// Token: 0x06001665 RID: 5733 RVA: 0x00052BC4 File Offset: 0x00050DC4
		protected override Result Preview(MaintainRitualDecisionResponse response)
		{
			YesNo choice = response.Choice;
			Result result;
			if (choice != YesNo.Undefined)
			{
				if (choice != YesNo.Yes)
				{
					result = Result.Success;
				}
				else
				{
					result = this._player.AcceptPayment(response.Payment);
				}
			}
			else
			{
				result = Result.Failure;
			}
			return result;
		}

		// Token: 0x06001666 RID: 5734 RVA: 0x00052C04 File Offset: 0x00050E04
		protected override Result Process(MaintainRitualDecisionResponse response)
		{
			ActiveRitual activeRitual = base._currentTurn.FetchGameItem<ActiveRitual>(base.request.ActiveRitualId);
			if (activeRitual == null)
			{
				return Result.SimulationError(string.Format("Requested maintainRitual for invalid game item {0}", base.request.ActiveRitualId));
			}
			if (response.Choice == YesNo.No)
			{
				this.CancelRitual(activeRitual);
				return Result.Success;
			}
			if (base.request.RequiredPayment.IsZero)
			{
				return Result.Success;
			}
			Problem problem = this._player.RemovePayment(response.Payment) as Problem;
			if (problem != null)
			{
				this.CancelRitual(activeRitual);
				return problem;
			}
			activeRitual.PayUpkeep(response.Payment);
			return Result.Success;
		}

		// Token: 0x06001667 RID: 5735 RVA: 0x00052CAD File Offset: 0x00050EAD
		public void CancelRitual(ActiveRitual activeRitual)
		{
			this.TurnProcessContext.ProcessContexts.RitualsToEnd.Add(new ValueTuple<PlayerState, ActiveRitual>(this._player, activeRitual));
		}
	}
}
