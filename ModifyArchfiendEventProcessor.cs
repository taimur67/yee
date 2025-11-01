using System;

namespace LoG
{
	// Token: 0x02000602 RID: 1538
	public class ModifyArchfiendEventProcessor : GrandEventActionProcessor<ModifyArchfiendEventOrder, ModifyArchfiendEventStaticData, ModifyOfferingQualityEvent>
	{
		// Token: 0x06001CB0 RID: 7344 RVA: 0x00062E80 File Offset: 0x00061080
		protected override Result ProcessInternal(PlayGrandEventOrder order)
		{
			IModifier modifier = new ArchfiendModifier(base.data.Modifiers);
			modifier.Source = new GrandEventContext(this._player.Id, this._player.ArchfiendId, this.AbilityData.Id);
			ModifyArchfiendTurnModuleInstance modifyArchfiendTurnModuleInstance = TurnModuleInstanceFactory.CreateInstance<ModifyArchfiendTurnModuleInstance>(base._currentTurn);
			modifyArchfiendTurnModuleInstance.AffectedPlayerIds = base.data.Targets.GetPlayerIds(base._currentTurn, this._player);
			modifyArchfiendTurnModuleInstance.Lifetime = base._random.Next(base.data.MinDuration, base.data.MaxDuration);
			modifyArchfiendTurnModuleInstance.Modifier = modifier;
			modifyArchfiendTurnModuleInstance.EventEffectId = base.data.Id;
			base.GameEvent.Duration = modifyArchfiendTurnModuleInstance.Lifetime;
			base.GameEvent.AddAffectedPlayerIds(modifyArchfiendTurnModuleInstance.AffectedPlayerIds);
			base._currentTurn.AddActiveTurnModule(this.TurnProcessContext, modifyArchfiendTurnModuleInstance);
			return Result.Success;
		}
	}
}
