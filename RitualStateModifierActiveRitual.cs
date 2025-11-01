using System;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002DF RID: 735
	[Serializable]
	public class RitualStateModifierActiveRitual : ActiveRitual
	{
		// Token: 0x06000E5D RID: 3677 RVA: 0x000392C0 File Offset: 0x000374C0
		public override Result StartRitual(TurnProcessContext context, PlayerState player, RitualCastEvent ritualCastEvent)
		{
			TurnState currentTurn = context.CurrentTurn;
			PlayerTargetGroup modifiableTargetGroup = new PlayerTargetGroup(new RitualStateModifier(context.Database.Fetch<ModifyRitualStateRitualData>(base.StaticDataId).Modifiers)
			{
				Source = new RitualContext(player.Id, player.ArchfiendId, base.StaticDataId)
			}, new int[]
			{
				base.TargetContext.PlayerId
			});
			this._globalModifierId = currentTurn.PushGlobalModifier(modifiableTargetGroup);
			context.RecalculateAllModifiersFor(currentTurn.FindPlayerState(base.TargetContext.PlayerId, null));
			return base.StartRitual(context, player, ritualCastEvent);
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x00039354 File Offset: 0x00037554
		public override Result EndRitual(TurnProcessContext context, PlayerState player, ItemBanishedEvent banishedEvent)
		{
			TurnState currentTurn = context.CurrentTurn;
			currentTurn.PopGlobalModifier(this._globalModifierId);
			context.RecalculateAllModifiersFor(currentTurn.FindPlayerState(base.TargetContext.PlayerId, null));
			return base.EndRitual(context, player, banishedEvent);
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x00039395 File Offset: 0x00037595
		protected void DeepCloneRitualStateModifierParts(RitualStateModifierActiveRitual clone)
		{
			clone._globalModifierId = this._globalModifierId;
			base.DeepCloneActiveRitualParts(clone);
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x000393AC File Offset: 0x000375AC
		public override void DeepClone(out GameItem gameItem)
		{
			RitualStateModifierActiveRitual ritualStateModifierActiveRitual = new RitualStateModifierActiveRitual();
			this.DeepCloneRitualStateModifierParts(ritualStateModifierActiveRitual);
			gameItem = ritualStateModifierActiveRitual;
		}

		// Token: 0x0400065A RID: 1626
		[JsonProperty]
		private Guid _globalModifierId;
	}
}
