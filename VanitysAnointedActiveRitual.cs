using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002E4 RID: 740
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class VanitysAnointedActiveRitual : ActiveRitual
	{
		// Token: 0x06000E74 RID: 3700 RVA: 0x00039904 File Offset: 0x00037B04
		public override Result StartRitual(TurnProcessContext context, PlayerState player, RitualCastEvent ritualCastEvent)
		{
			TurnState currentTurn = context.CurrentTurn;
			GameDatabase database = context.Database;
			VanitysAnointedRitualData vanitysAnointedRitualData = database.Fetch<VanitysAnointedRitualData>(base.StaticDataId);
			ItemAbilityStaticData itemAbilityStaticData = database.Fetch<ItemAbilityStaticData>(vanitysAnointedRitualData.ProvidedAbility.Id);
			Ability ability = new Ability(itemAbilityStaticData);
			ability.Name = itemAbilityStaticData.Id;
			ability.ProviderId = base.TargetContext.ItemId;
			ability.SourceId = itemAbilityStaticData.Id;
			GameItemTargetGroup gameItemTargetGroup = new GameItemTargetGroup();
			GamePiece controllingPiece = currentTurn.GetControllingPiece(base.TargetContext.ItemId);
			this._hostGamePieceId = controllingPiece.Id;
			gameItemTargetGroup.Targets.Add(controllingPiece);
			gameItemTargetGroup.Abilities.Add(ability);
			IEnumerable<GamePieceModifierStaticData> source = itemAbilityStaticData.GetModifiers().OfType<GamePieceModifierStaticData>();
			gameItemTargetGroup.Modifiers = IEnumerableExtensions.ToList<IModifier>(from x in source
			select new GamePieceModifier(x)
			{
				Source = ability
			});
			this._globalModifierId = currentTurn.PushGlobalModifier(gameItemTargetGroup);
			this.RecalculateTargetModifiers(context);
			GameItem gameItem = currentTurn.FetchGameItem<GameItem>(base.TargetContext.ItemId);
			ritualCastEvent.AddChildEvent<AbilitySetOnGameItemEvent>(new AbilitySetOnGameItemEvent(player.Id, gameItem.Id, controllingPiece.ControllingPlayerId, vanitysAnointedRitualData.ProvidedAbility, true));
			return base.StartRitual(context, player, ritualCastEvent);
		}

		// Token: 0x06000E75 RID: 3701 RVA: 0x00039A58 File Offset: 0x00037C58
		public override Result EndRitual(TurnProcessContext context, PlayerState player, ItemBanishedEvent banishedEvent)
		{
			TurnState currentTurn = context.CurrentTurn;
			VanitysAnointedRitualData vanitysAnointedRitualData = context.Database.Fetch<VanitysAnointedRitualData>(base.StaticDataId);
			currentTurn.PopGlobalModifier(this._globalModifierId);
			this.RecalculateTargetModifiers(context);
			int num = int.MinValue;
			GameItem item;
			if (currentTurn.TryFetchGameItem<GameItem>(base.TargetContext.ItemId, out item))
			{
				PlayerState playerState = context.CurrentTurn.FindControllingPlayer(item);
				num = ((playerState != null) ? playerState.Id : num);
			}
			if (num == -2147483648)
			{
				num = base.TargetContext.PlayerId;
			}
			banishedEvent.AddChildEvent<AbilitySetOnGameItemEvent>(new AbilitySetOnGameItemEvent(player.Id, base.TargetContext.ItemId, num, vanitysAnointedRitualData.ProvidedAbility, false));
			return base.EndRitual(context, player, banishedEvent);
		}

		// Token: 0x06000E76 RID: 3702 RVA: 0x00039B08 File Offset: 0x00037D08
		public override Result ValidateRitualTargetItem(TurnProcessContext context, PlayerState caster, GameItem target)
		{
			Problem problem = base.ValidateRitualTargetItem(context, caster, target) as Problem;
			if (problem != null)
			{
				return problem;
			}
			if (context.CurrentTurn.GetControllingPiece(target) != this._hostGamePieceId)
			{
				return Result.Failure;
			}
			return Result.Success;
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x00039B54 File Offset: 0x00037D54
		private void RecalculateTargetModifiers(TurnContext context)
		{
			GameItem gameItem = context.CurrentTurn.FetchGameItem(base.TargetContext.ItemId);
			GameItem gameItem2 = context.CurrentTurn.FetchGameItem(this._hostGamePieceId);
			if (gameItem != null)
			{
				context.RecalculateModifiers(gameItem);
			}
			if (gameItem2 != null)
			{
				context.RecalculateModifiers(gameItem2);
			}
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x00039BA0 File Offset: 0x00037DA0
		public sealed override void DeepClone(out GameItem gameItem)
		{
			VanitysAnointedActiveRitual vanitysAnointedActiveRitual = new VanitysAnointedActiveRitual();
			base.DeepCloneActiveRitualParts(vanitysAnointedActiveRitual);
			vanitysAnointedActiveRitual._globalModifierId = this._globalModifierId;
			vanitysAnointedActiveRitual._hostGamePieceId = this._hostGamePieceId;
			gameItem = vanitysAnointedActiveRitual;
		}

		// Token: 0x04000661 RID: 1633
		[JsonProperty]
		private Guid _globalModifierId;

		// Token: 0x04000662 RID: 1634
		[JsonProperty]
		private Identifier _hostGamePieceId;
	}
}
