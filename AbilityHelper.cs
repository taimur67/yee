using System;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;
using LoG.Simulation;

namespace LoG
{
	// Token: 0x020003DA RID: 986
	public struct AbilityHelper
	{
		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x0600132D RID: 4909 RVA: 0x00048DA2 File Offset: 0x00046FA2
		private GameRules _gameRules
		{
			get
			{
				return this._context.Rules;
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x0600132E RID: 4910 RVA: 0x00048DAF File Offset: 0x00046FAF
		private TurnState _turn
		{
			get
			{
				return this._context.CurrentTurn;
			}
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x0600132F RID: 4911 RVA: 0x00048DBC File Offset: 0x00046FBC
		private GameDatabase _database
		{
			get
			{
				return this._context.Database;
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06001330 RID: 4912 RVA: 0x00048DCC File Offset: 0x00046FCC
		private PlayerTurn PlayerTurn
		{
			get
			{
				PlayerTurn result;
				if ((result = this._playerTurn) == null)
				{
					result = (this._playerTurn = this._player.PlayerTurn);
				}
				return result;
			}
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x00048DF7 File Offset: 0x00046FF7
		public AbilityHelper(TurnContext context, PlayerState player, PlayerTurn turn = null)
		{
			this._context = context;
			this._player = player;
			this._playerTurn = turn;
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x00048E10 File Offset: 0x00047010
		public Result IsUnlocked(ActionableOrder order)
		{
			Problem problem = this.GetProcessor(order).IsUnLocked(order) as Problem;
			if (problem != null)
			{
				return problem;
			}
			return Result.Success;
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x00048E3A File Offset: 0x0004703A
		public Result IsValid(ActionableOrder order)
		{
			return this.GetProcessor(order).Validate();
		}

		// Token: 0x06001334 RID: 4916 RVA: 0x00048E48 File Offset: 0x00047048
		public Result IsAvailableAndAffordable(ActionableOrder order)
		{
			Problem problem = this.IsAvailable(order, true) as Problem;
			if (problem != null)
			{
				return problem;
			}
			return this.IsAffordable(order);
		}

		// Token: 0x06001335 RID: 4917 RVA: 0x00048E70 File Offset: 0x00047070
		public Result IsAvailable(ActionableOrder order, bool checkUnlock = true)
		{
			if (this._player.Eliminated)
			{
				return new ActionUnavailablePlayerEliminated
				{
					PlayerId = this._player.Id
				};
			}
			Problem problem = this.PlayerTurn.CheckConflicts(order) as Problem;
			if (problem != null)
			{
				return problem;
			}
			Problem problem2 = this.IsActionAvailable(order, checkUnlock) as Problem;
			if (problem2 != null)
			{
				return problem2;
			}
			bool flag = false;
			foreach (ActionableOrder actionableOrder in this.PlayerTurn.Orders)
			{
				if (order.ActionInstanceId == actionableOrder.ActionInstanceId)
				{
					flag = true;
					break;
				}
			}
			if (this._player.OrderSlots.Value <= IEnumerableExtensions.ToList<ActionableOrder>(this._player.GetOrders<ActionableOrder>()).Count && !flag)
			{
				return Result.NotEnoughSlots();
			}
			return Result.Success;
		}

		// Token: 0x06001336 RID: 4918 RVA: 0x00048F60 File Offset: 0x00047160
		public Result IsActionAvailableAndAffordable(ActionableOrder order, bool checkUnlock = true)
		{
			Problem problem = this.IsActionAvailable(order, checkUnlock) as Problem;
			if (problem != null)
			{
				return problem;
			}
			return this.IsAffordable(order);
		}

		// Token: 0x06001337 RID: 4919 RVA: 0x00048F88 File Offset: 0x00047188
		public Result IsActionAvailable(ActionableOrder order, bool checkUnlock = true)
		{
			ActionProcessor processor = this.GetProcessor(order);
			AbilityStaticData abilityData = processor.AbilityData;
			ConfigRef abilityId = (abilityData != null) ? abilityData.ConfigRef : null;
			int turnsRemaining;
			if (this._player.TryGetCooldown(abilityId, out turnsRemaining))
			{
				int targetPlayerID = this._player.Id;
				DiplomaticOrder diplomaticOrder = order as DiplomaticOrder;
				if (diplomaticOrder != null)
				{
					targetPlayerID = diplomaticOrder.TargetID;
				}
				ITarget target = order as ITarget;
				if (target != null)
				{
					targetPlayerID = target.Target.PlayerId;
				}
				return new Result.DiplomacyCooldownProblem(targetPlayerID, order.OrderType, turnsRemaining);
			}
			Problem problem = processor.IsAvailable() as Problem;
			if (problem != null)
			{
				return problem;
			}
			if (checkUnlock)
			{
				Problem problem2 = processor.IsUnLocked(order) as Problem;
				if (problem2 != null)
				{
					return problem2;
				}
			}
			return Result.Success;
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x0004903C File Offset: 0x0004723C
		public Result IsAffordable(ActionableOrder order)
		{
			Cost cost = this.CalculateCost(order);
			return this._player.CanAfford(cost);
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x0004905D File Offset: 0x0004725D
		public AbilityStaticData GetAbilityData(ActionableOrder order)
		{
			return this._context.GetDataForRequest(order);
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x0004906B File Offset: 0x0004726B
		public Cost CalculateCost(ActionableOrder order)
		{
			if (order == null)
			{
				return Cost.None;
			}
			ActionProcessor processor = this.GetProcessor(order);
			return ((processor != null) ? processor.CalculateCost() : null) ?? Cost.None;
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x00049092 File Offset: 0x00047292
		public Cost CalculateBaseCost(ActionableOrder order)
		{
			if (order == null)
			{
				return Cost.None;
			}
			ActionProcessor processor = this.GetProcessor(order);
			return ((processor != null) ? processor.CalculateBaseCost() : null) ?? Cost.None;
		}

		// Token: 0x0600133C RID: 4924 RVA: 0x000490BC File Offset: 0x000472BC
		public bool TryAutoPay(ActionableOrder order)
		{
			Cost cost = this.CalculateCost(order);
			return this.TryAutoPay(order, cost);
		}

		// Token: 0x0600133D RID: 4925 RVA: 0x000490DC File Offset: 0x000472DC
		public bool TryAutoPay(ActionableOrder order, Cost cost)
		{
			if (cost.IsZero)
			{
				return true;
			}
			Payment payment = PaymentUtils.DeducePayment(this._player, cost, PaymentUtils.AutoPayMethods.Optimal, 8);
			if (payment == null)
			{
				return false;
			}
			if (!payment.IsSufficient(cost))
			{
				return false;
			}
			order.Payment = payment;
			return true;
		}

		// Token: 0x0600133E RID: 4926 RVA: 0x00049120 File Offset: 0x00047320
		public bool TryAutoPay_AICheat(PlayerState playerState, ActionableOrder order, Cost cost)
		{
			if (cost.IsZero)
			{
				return true;
			}
			Payment payment = PaymentUtils.DeducePayment(playerState, cost, PaymentUtils.AutoPayMethods.GreedyPermutation, 8);
			if (payment.Total.GreaterThanOrEqualTo(cost))
			{
				order.Payment = payment;
				return true;
			}
			payment = PaymentUtils.DeducePayment(playerState, cost, PaymentUtils.AutoPayMethods.GreedyDistance, playerState.Resources.Count);
			if (payment.IsSufficient(cost))
			{
				order.Payment = payment;
				return true;
			}
			return false;
		}

		// Token: 0x0600133F RID: 4927 RVA: 0x00049184 File Offset: 0x00047384
		private ActionProcessor GetProcessor(ActionableOrder order)
		{
			return ActionProcessorFactory.PrepareProcessor(this._gameRules, this._turn, this._player, order, this._database);
		}

		// Token: 0x040008DC RID: 2268
		private TurnContext _context;

		// Token: 0x040008DD RID: 2269
		private PlayerState _player;

		// Token: 0x040008DE RID: 2270
		private PlayerTurn _playerTurn;
	}
}
