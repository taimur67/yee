using System;
using System.Linq;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000600 RID: 1536
	public class IncreaseRitualCostsEventProcessor : GrandEventActionProcessor<IncreaseRitualCostsEventOrder, IncreaseRitualCostsEventStaticData>
	{
		// Token: 0x06001CAC RID: 7340 RVA: 0x00062D48 File Offset: 0x00060F48
		protected override Result ProcessInternal(PlayGrandEventOrder order)
		{
			ArchfiendModifierStaticData archfiendModifierStaticData = new ArchfiendModifierStaticData();
			archfiendModifierStaticData.SetValue(ArchfiendStat.RitualCostOffset, (float)base._random.Next(base.data.MinIncrease, base.data.MaxIncrease), ModifierTarget.ValueOffset);
			IModifier modifier = new ArchfiendModifier(archfiendModifierStaticData);
			modifier.Source = new GrandEventContext(this._player.Id, this._player.ArchfiendId, this.AbilityData.Id);
			ModifyArchfiendTurnModuleInstance modifyArchfiendTurnModuleInstance = TurnModuleInstanceFactory.CreateInstance<ModifyArchfiendTurnModuleInstance>(base._currentTurn);
			modifyArchfiendTurnModuleInstance.AffectedPlayerIds = IEnumerableExtensions.ToArray<int>(from x in base._currentTurn.EnumeratePlayerStates(false, false)
			select x.Id);
			modifyArchfiendTurnModuleInstance.Lifetime = base._random.Next(base.data.MinDuration, base.data.MaxDuration);
			modifyArchfiendTurnModuleInstance.Modifier = modifier;
			modifyArchfiendTurnModuleInstance.EventEffectId = base.data.Id;
			base.GameEvent.Duration = modifyArchfiendTurnModuleInstance.Lifetime;
			base._currentTurn.AddActiveTurnModule(this.TurnProcessContext, modifyArchfiendTurnModuleInstance);
			return Result.Success;
		}
	}
}
