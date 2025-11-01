using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x020005EF RID: 1519
	public class BlockRitualsEventProcessor : GrandEventActionProcessor<BlockRitualsEventOrder, BlockRitualsEventStaticData>
	{
		// Token: 0x06001C7D RID: 7293 RVA: 0x0006223C File Offset: 0x0006043C
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
			foreach (PlayerState player in base._currentTurn.EnumeratePlayerStates(false, false))
			{
				List<ActiveRitual> list = IEnumerableExtensions.ToList<ActiveRitual>(base._currentTurn.GetActiveRituals(player));
				for (int i = list.Count - 1; i >= 0; i--)
				{
					ActiveRitual activeRitual = list[i];
					this.TurnProcessContext.BanishGameItem(activeRitual.Id, int.MinValue);
				}
			}
			base._currentTurn.AddActiveTurnModule(this.TurnProcessContext, modifyArchfiendTurnModuleInstance);
			return Result.Success;
		}
	}
}
