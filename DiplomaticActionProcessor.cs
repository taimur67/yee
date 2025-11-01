using System;
using System.Linq;
using Game.Simulation.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020005CF RID: 1487
	public abstract class DiplomaticActionProcessor<T, Q> : ActionProcessor<T, Q>, IDiplomaticActionProcessor where T : DiplomaticOrder, new() where Q : DiplomaticAbilityStaticData
	{
		// Token: 0x06001BF2 RID: 7154 RVA: 0x00060C81 File Offset: 0x0005EE81
		public Result Enact(DiplomaticOrder request)
		{
			Result result = this.Enact((T)((object)request));
			if (result)
			{
				this.TurnProcessContext.CurrentTurn.AddGameEvent<TargetedByDiplomaticActionEvent>(new TargetedByDiplomaticActionEvent(this._player, (PlayerIndex)request.TargetID));
			}
			return result;
		}

		// Token: 0x06001BF3 RID: 7155 RVA: 0x00060CC0 File Offset: 0x0005EEC0
		public sealed override Result Process(ActionProcessContext context)
		{
			this.TurnProcessContext.DiplomaticContext.AddDiplomaticAction(this._player.Id, base.request.TargetID, new PlayerDiplomaticOrder
			{
				Player = this._player,
				Payment = base.request.Payment,
				Request = base.request,
				Processor = this,
				OrderSlotIndex = context.OrderSlotIndex
			});
			return Result.Success;
		}

		// Token: 0x06001BF4 RID: 7156
		public abstract Result Enact(T order);

		// Token: 0x06001BF5 RID: 7157 RVA: 0x00060D48 File Offset: 0x0005EF48
		public override Result IsAvailable()
		{
			return DiplomaticStateProcessor.ValidateOrderType(base._currentTurn, base.PlayerId, base.request.TargetID, base.request.OrderType, false);
		}

		// Token: 0x06001BF6 RID: 7158 RVA: 0x00060D7C File Offset: 0x0005EF7C
		public override Cost CalculateCost()
		{
			RankCostPair rankCostPair = base.data.RankCostOverrides.FirstOrDefault(delegate(RankCostPair t)
			{
				ArchfiendRankStaticData archfiendRankStaticData = base._database.Fetch(t.Rank);
				return ((archfiendRankStaticData != null) ? archfiendRankStaticData.RankValue : int.MaxValue) == this._player.RankValue;
			});
			if (rankCostPair != null)
			{
				return rankCostPair.Cost;
			}
			return this.CalculateBaseCost();
		}

		// Token: 0x06001BF7 RID: 7159 RVA: 0x00060DC0 File Offset: 0x0005EFC0
		public override Result Validate()
		{
			return this.IsAvailable();
		}
	}
}
