using System;
using System.Linq;
using Game.Simulation.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002DB RID: 731
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class InfernalJuggernautActiveRitual : ActiveRitual
	{
		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000E45 RID: 3653 RVA: 0x000388C1 File Offset: 0x00036AC1
		// (set) Token: 0x06000E46 RID: 3654 RVA: 0x000388C9 File Offset: 0x00036AC9
		[JsonProperty]
		public GamePieceModifierStaticData BuffModifier { get; set; }

		// Token: 0x06000E47 RID: 3655 RVA: 0x000388D4 File Offset: 0x00036AD4
		public override Result StartRitual(TurnProcessContext context, PlayerState player, RitualCastEvent ritualCastEvent)
		{
			GameDatabase database = context.Database;
			InfernalJuggernautRitualData data = database.Fetch<InfernalJuggernautRitualData>(base.StaticDataId);
			TurnState currentTurn = context.CurrentTurn;
			GamePiece gamePiece = currentTurn.FetchGameItem<GamePiece>(base.TargetContext.ItemId);
			if (gamePiece == null)
			{
				return Result.Failure;
			}
			Ability ability = context.GetAllAbilitiesFor(gamePiece).FirstOrDefault((Ability a) => a.SourceId == data.ExhaustionDebuff.Id);
			if (ability != null)
			{
				currentTurn.PopGlobalModifier(ability.ModifierGroupId);
				ritualCastEvent.AddChildEvent<AbilitySetOnGameItemEvent>(new AbilitySetOnGameItemEvent(player.Id, gamePiece, data.ExhaustionDebuff, false));
				this.RecalculateTargetModifiers(context);
			}
			bool rollWithAdvantage = player.HasTag<EntityTag_CheatLuckyRitualEffectRolls>();
			this.BuffModifier = new GamePieceModifierStaticData().SetValue(GamePieceStat.Melee, (float)currentTurn.GetRandomRoll(data.MinRandDamage, data.MaxRandDamage, rollWithAdvantage), ModifierTarget.ValueOffset).SetValue(GamePieceStat.Ranged, (float)currentTurn.GetRandomRoll(data.MinRandDamage, data.MaxRandDamage, rollWithAdvantage), ModifierTarget.ValueOffset).SetValue(GamePieceStat.Infernal, (float)currentTurn.GetRandomRoll(data.MinRandDamage, data.MaxRandDamage, rollWithAdvantage), ModifierTarget.ValueOffset).SetValue(GamePieceStat.Movement, (float)data.MovementBuff, ModifierTarget.ValueOffset);
			GameItemTargetGroup modifiableTargetGroup = new GameItemTargetGroup(new GamePieceModifier(this.BuffModifier)
			{
				Source = new RitualContext(player.Id, player.ArchfiendId, base.StaticDataId)
			}, new Identifier[]
			{
				base.TargetContext.ItemId
			});
			this._globalModifierId = currentTurn.PushGlobalModifier(modifiableTargetGroup);
			foreach (ModifierStaticDataExtensions.GamePieceStatModification gamePieceStatModification in this.BuffModifier.EffectiveModifications(gamePiece))
			{
				ritualCastEvent.AddChildEvent<ModifyGamePieceEvent>(new ModifyGamePieceEvent(player.Id, gamePiece, gamePieceStatModification.Stat, gamePieceStatModification.AttemptedOffset, gamePieceStatModification.EffectiveOffset, false));
			}
			this.RecalculateTargetModifiers(context);
			return base.StartRitual(context, player, ritualCastEvent);
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x00038ADC File Offset: 0x00036CDC
		public override Result EndRitual(TurnProcessContext context, PlayerState player, ItemBanishedEvent banishedEvent)
		{
			TurnState currentTurn = context.CurrentTurn;
			InfernalJuggernautRitualData infernalJuggernautRitualData = context.Database.Fetch<InfernalJuggernautRitualData>(base.StaticDataId);
			GamePiece gamePiece = context.CurrentTurn.FetchGameItem<GamePiece>(base.TargetContext.ItemId);
			banishedEvent.AddChildEvent<AbilitySetOnGameItemEvent>(new AbilitySetOnGameItemEvent(player.Id, gamePiece, infernalJuggernautRitualData.ExhaustionDebuff, true));
			if (gamePiece != null)
			{
				foreach (ModifierStaticDataExtensions.GamePieceStatModification gamePieceStatModification in this.BuffModifier.EffectiveModifications(gamePiece))
				{
					banishedEvent.AddChildEvent<ModifyGamePieceEvent>(new ModifyGamePieceEvent(player.Id, gamePiece, gamePieceStatModification.Stat, gamePieceStatModification.AttemptedOffset, gamePieceStatModification.EffectiveOffset, true));
				}
			}
			currentTurn.PopGlobalModifier(this._globalModifierId);
			ItemAbilityStaticData itemAbilityStaticData = context.Database.Fetch<ItemAbilityStaticData>(infernalJuggernautRitualData.ExhaustionDebuff.Id);
			Ability ability = new Ability(itemAbilityStaticData)
			{
				Name = infernalJuggernautRitualData.ExhaustionDebuff.Id,
				SourceId = infernalJuggernautRitualData.ExhaustionDebuff.Id
			};
			GameItemTargetGroup gameItemTargetGroup = new GameItemTargetGroup();
			gameItemTargetGroup.Targets.Add(base.TargetContext.ItemId);
			gameItemTargetGroup.Abilities.Add(ability);
			foreach (ModifierStaticData modifierStaticData in itemAbilityStaticData.GetModifiers())
			{
				GamePieceModifierStaticData gamePieceModifierStaticData = modifierStaticData as GamePieceModifierStaticData;
				if (gamePieceModifierStaticData != null)
				{
					gameItemTargetGroup.Modifiers.Add(new GamePieceModifier(gamePieceModifierStaticData)
					{
						Source = new RitualContext(player.Id, player.ArchfiendId, infernalJuggernautRitualData.Id)
					});
				}
			}
			ability.ModifierGroupId = currentTurn.PushGlobalModifier(gameItemTargetGroup);
			this.RecalculateTargetModifiers(context);
			return base.EndRitual(context, player, banishedEvent);
		}

		// Token: 0x06000E49 RID: 3657 RVA: 0x00038CAC File Offset: 0x00036EAC
		private void RecalculateTargetModifiers(TurnProcessContext context)
		{
			PlayerState player = context.CurrentTurn.FindControllingPlayer(base.TargetContext.ItemId);
			context.RecalculateAllModifiersFor(player);
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x00038CD8 File Offset: 0x00036ED8
		public sealed override void DeepClone(out GameItem gameItem)
		{
			InfernalJuggernautActiveRitual infernalJuggernautActiveRitual = new InfernalJuggernautActiveRitual();
			base.DeepCloneActiveRitualParts(infernalJuggernautActiveRitual);
			infernalJuggernautActiveRitual._globalModifierId = this._globalModifierId;
			infernalJuggernautActiveRitual.BuffModifier = this.BuffModifier.DeepClone(CloneFunction.FastClone);
			gameItem = infernalJuggernautActiveRitual;
		}

		// Token: 0x0400064F RID: 1615
		[JsonProperty]
		private Guid _globalModifierId;
	}
}
