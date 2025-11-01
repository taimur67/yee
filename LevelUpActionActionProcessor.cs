using System;
using System.Linq;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000639 RID: 1593
	public class LevelUpActionActionProcessor : ActionProcessor<OrderLevelUp>
	{
		// Token: 0x06001D74 RID: 7540 RVA: 0x00065B8C File Offset: 0x00063D8C
		public override Result Validate()
		{
			if (base.request.PowerType == PowerType.None)
			{
				return Result.Failure;
			}
			return Result.Success;
		}

		// Token: 0x06001D75 RID: 7541 RVA: 0x00065BA8 File Offset: 0x00063DA8
		public override Result Process(ActionProcessContext context)
		{
			if (base.request.PowerType == PowerType.None)
			{
				return Result.Failure;
			}
			ArchfiendStat archfiendStat = base.request.PowerType.ToArchfiendStat();
			GameEvent ev = this.TurnProcessContext.PermanentlyAdjustArchfiendStat(this._player, this._player, archfiendStat, 1, 1);
			int value = this._player.Get(archfiendStat).Value;
			PowerUpgradedEvent powerUpgradedEvent = base._currentTurn.AddGameEvent<PowerUpgradedEvent>(new PowerUpgradedEvent(this._player.Id, base.request.PowerType, value));
			powerUpgradedEvent.AddChildEvent(ev);
			PowersStaticData powerLevelData = base._database.GetPowerLevelData(base.request.PowerType, value);
			if (powerLevelData != null)
			{
				powerUpgradedEvent.Unlocks = IEnumerableExtensions.ToList<ConfigRef>(powerLevelData.Unlocks.Cast<ConfigRef>());
				powerUpgradedEvent.ConfigRef = powerLevelData.ConfigRef;
			}
			this.TurnProcessContext.RecalculateAllModifiersFor(this._player);
			return Result.Success;
		}

		// Token: 0x06001D76 RID: 7542 RVA: 0x00065C94 File Offset: 0x00063E94
		public override Cost CalculateCost()
		{
			PowersStaticData powerLevelData = base._database.GetPowerLevelData(base.request.PowerType, base.request.Level);
			CostStaticData costStaticData = (powerLevelData != null) ? powerLevelData.Cost : null;
			if (costStaticData == null)
			{
				return Cost.None;
			}
			return costStaticData;
		}
	}
}
