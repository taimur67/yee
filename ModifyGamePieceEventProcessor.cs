using System;

namespace LoG
{
	// Token: 0x02000605 RID: 1541
	public class ModifyGamePieceEventProcessor : GrandEventActionProcessor<ModifyGamePieceEventOrder, ModifyGamePieceEventStaticData>
	{
		// Token: 0x06001CB5 RID: 7349 RVA: 0x0006308C File Offset: 0x0006128C
		protected override Result ProcessInternal(PlayGrandEventOrder order)
		{
			IModifier modifier = new GamePieceModifier(base.data.Modifiers);
			modifier.Source = new GrandEventContext(this._player.Id, this._player.ArchfiendId, this.AbilityData.Id);
			ModifyArchfiendTurnModuleInstance modifyArchfiendTurnModuleInstance = TurnModuleInstanceFactory.CreateInstance<ModifyArchfiendTurnModuleInstance>(base._currentTurn);
			modifyArchfiendTurnModuleInstance.AffectedPlayerIds = base.data.Targets.GetPlayerIds(base._currentTurn, this._player);
			modifyArchfiendTurnModuleInstance.Lifetime = base._random.Next(base.data.MinDuration, base.data.MaxDuration);
			modifyArchfiendTurnModuleInstance.Modifier = modifier;
			modifyArchfiendTurnModuleInstance.EventEffectId = base.data.Id;
			base.GameEvent.Duration = modifyArchfiendTurnModuleInstance.Lifetime;
			base._currentTurn.AddActiveTurnModule(this.TurnProcessContext, modifyArchfiendTurnModuleInstance);
			return Result.Success;
		}
	}
}
