using System;

namespace LoG
{
	// Token: 0x020005F1 RID: 1521
	public class BlockStratagemsEventProcessor : GrandEventActionProcessor<BlockStratagemsEventOrder, BlockStratagemsEventStaticData>
	{
		// Token: 0x06001C81 RID: 7297 RVA: 0x000623C4 File Offset: 0x000605C4
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
			foreach (Stratagem item in base._currentTurn.EnumerateGameItems<Stratagem>())
			{
				ItemBanishedEvent gameEvent = this.TurnProcessContext.BanishGameItem(item, this._player.Id);
				base._currentTurn.AddGameEvent<ItemBanishedEvent>(gameEvent);
			}
			base._currentTurn.AddActiveTurnModule(this.TurnProcessContext, modifyArchfiendTurnModuleInstance);
			return Result.Success;
		}
	}
}
