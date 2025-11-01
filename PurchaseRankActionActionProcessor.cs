using System;
using System.Collections.Generic;
using System.Linq;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000641 RID: 1601
	public class PurchaseRankActionActionProcessor : ActionProcessor<OrderPurchaseRank>
	{
		// Token: 0x06001D90 RID: 7568 RVA: 0x0006605A File Offset: 0x0006425A
		public override Result Validate()
		{
			if (base.request.TargetRank <= this._player.Rank)
			{
				return Result.Failure;
			}
			return Result.Success;
		}

		// Token: 0x06001D91 RID: 7569 RVA: 0x0006607F File Offset: 0x0006427F
		public override Result IsAvailable()
		{
			if (this._player.Excommunicated)
			{
				return Result.Failure;
			}
			if (base._database.GetArchfiendRank(base.request.TargetRank) == null)
			{
				return Result.Failure;
			}
			return Result.Success;
		}

		// Token: 0x06001D92 RID: 7570 RVA: 0x000660B8 File Offset: 0x000642B8
		public override Cost CalculateCost()
		{
			ArchfiendRankStaticData archfiendRank = base._database.GetArchfiendRank(base.request.TargetRank);
			if (archfiendRank == null)
			{
				return Cost.None;
			}
			Cost cost = new Cost(archfiendRank.Cost);
			int prestige = (int)((float)(cost._prestige * this._player.RankCostPercent) / 100f);
			cost._prestige = prestige;
			return cost;
		}

		// Token: 0x06001D93 RID: 7571 RVA: 0x00066118 File Offset: 0x00064318
		public override Result Process(ActionProcessContext context)
		{
			Problem problem = base.Process(context) as Problem;
			if (problem != null)
			{
				return problem;
			}
			ArchfiendRankStaticData archfiendRank = base._database.GetArchfiendRank(base.request.TargetRank);
			if (archfiendRank == null)
			{
				return Result.Failure;
			}
			int value = base.request.TargetRank - this._player.Rank;
			this._player.RankValue.AddInstalledModifier(new StatModifier(value, null, ModifierTarget.ValueOffset));
			this.TurnProcessContext.RecalculateAllModifiersFor(this._player);
			RankIncreaseEvent rankIncreaseEvent = new RankIncreaseEvent(this._player.Id, (Rank)archfiendRank.RankValue);
			IEnumerable<ConfigRef<AbilityStaticData>> unlocks = archfiendRank.Unlocks;
			rankIncreaseEvent.Unlocks = IEnumerableExtensions.ToList<ConfigRef>(unlocks.Cast<ConfigRef>());
			base._currentTurn.AddGameEvent<RankIncreaseEvent>(rankIncreaseEvent);
			return Result.Success;
		}
	}
}
