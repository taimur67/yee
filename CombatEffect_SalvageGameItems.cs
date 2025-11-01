using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x02000345 RID: 837
	public class CombatEffect_SalvageGameItems : CombatPhaseAbilityEffect
	{
		// Token: 0x06000FF7 RID: 4087 RVA: 0x0003F0D1 File Offset: 0x0003D2D1
		protected override GameEvent OnWinnerPreCapture(Ability source, CombatAbilityContext context, BattlePhaseResult phaseResult)
		{
			this.RecoverItems(context, IEnumerableExtensions.ToList<Identifier>(context.Opponent.Slots));
			return null;
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x0003F0EC File Offset: 0x0003D2EC
		protected override GameEvent OnPostBattle(Ability source, CombatAbilityContext context, BattleEvent battleEvent)
		{
			LegionKilledEvent legionKilledEvent;
			if (battleEvent.BattleResult.DidWin(context.Actor.Id) && battleEvent.TryGet<LegionKilledEvent>(out legionKilledEvent))
			{
				this.RecoverItems(context, legionKilledEvent.HeldItems);
			}
			return null;
		}

		// Token: 0x06000FF9 RID: 4089 RVA: 0x0003F12C File Offset: 0x0003D32C
		private void RecoverItems(CombatAbilityContext context, List<Identifier> items)
		{
			TurnState turn = context.Turn;
			PlayerState playerState = turn.FindPlayerState(context.Actor.ControllingPlayerId, null);
			RecoverGameItemsEvent recoverGameItemsEvent = new RecoverGameItemsEvent(context.Actor, context.Opponent, Array.Empty<Identifier>());
			foreach (Identifier id in items)
			{
				GameItem gameItem = turn.FetchGameItem(id);
				if (gameItem.Category == this.Category && turn.Random.NextFloat() < this.SalvageChance)
				{
					context.TurnContext.RemoveItemFromAnySlot(gameItem);
					playerState.AddToVault(gameItem);
					gameItem.Status = GameItemStatus.InPlay;
					recoverGameItemsEvent.RecoveredItems.Add(gameItem.Id);
				}
			}
			List<Identifier> recoveredItems = recoverGameItemsEvent.RecoveredItems;
			if (recoveredItems != null && recoveredItems.Count > 0)
			{
				context.Turn.AddGameEvent<RecoverGameItemsEvent>(recoverGameItemsEvent);
				recoverGameItemsEvent.AddChildEvent<ItemsReceivedEvent>(new ItemsReceivedEvent(playerState.Id, recoverGameItemsEvent.RecoveredItems));
			}
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x0003F248 File Offset: 0x0003D448
		public override void DeepClone(out AbilityEffect clone)
		{
			CombatEffect_SalvageGameItems combatEffect_SalvageGameItems = new CombatEffect_SalvageGameItems
			{
				Category = this.Category,
				SalvageChance = this.SalvageChance
			};
			base.DeepCloneCombatPhaseAbilityEffectParts(combatEffect_SalvageGameItems);
			clone = combatEffect_SalvageGameItems;
		}

		// Token: 0x04000767 RID: 1895
		public GameItemCategory Category;

		// Token: 0x04000768 RID: 1896
		public float SalvageChance = 1f;
	}
}
