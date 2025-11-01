using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002C1 RID: 705
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class GamePiece : GameItem, IDeepClone<GamePiece>
	{
		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000D89 RID: 3465 RVA: 0x000356A7 File Offset: 0x000338A7
		public override GameItemCategory Category
		{
			get
			{
				return GameItemCategory.GamePiece;
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000D8A RID: 3466 RVA: 0x000356AA File Offset: 0x000338AA
		public override bool RecordAsDeadEntityOnBanish
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000D8B RID: 3467 RVA: 0x000356AD File Offset: 0x000338AD
		[JsonIgnore]
		public int RemainingSlots
		{
			get
			{
				return this.MaxSlots - this.Slots.Count;
			}
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x000356C8 File Offset: 0x000338C8
		public override void ConfigureFrom(IdentifiableStaticData data)
		{
			base.ConfigureFrom(data);
			GamePieceStaticData gamePieceStaticData = data as GamePieceStaticData;
			if (gamePieceStaticData != null)
			{
				this.CanBeConverted = gamePieceStaticData.CanBeConverted;
				this.SubCategory = gamePieceStaticData.SubCategory;
				this.Faction = gamePieceStaticData.Faction;
				this.SupportRange.SetBase((float)gamePieceStaticData.SupportRange);
				this.GroundMoveDistance.SetBase((float)gamePieceStaticData.Movement);
				this.PassivePrestige.SetBase((float)gamePieceStaticData.Prestige);
				this.CombatStats.Ranged.SetBase((float)gamePieceStaticData.Ranged);
				this.CombatStats.Melee.SetBase((float)gamePieceStaticData.Melee);
				this.CombatStats.Infernal.SetBase((float)gamePieceStaticData.Infernal);
				if (this.Level <= gamePieceStaticData.Level)
				{
					this.SetLevel(gamePieceStaticData.Level);
				}
				this.TotalHP.SetBase((float)gamePieceStaticData.HP);
				this.HP = gamePieceStaticData.HP;
			}
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x000357C0 File Offset: 0x000339C0
		public int GetEffectiveHP()
		{
			return this.HP + this.TemporaryHP;
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x000357CF File Offset: 0x000339CF
		public override void SetLevel(int level)
		{
			base.SetLevel(level);
			this.MaxSlots.SetBase((float)this.LookupMaxSlots());
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x000357EA File Offset: 0x000339EA
		public bool ShouldTeleport()
		{
			return this.CanTeleport && this.TeleportDistance > this.GroundMoveDistance;
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x00035814 File Offset: 0x00033A14
		public static float CalcCombatAdvantageAtPosition(TurnContext context, GamePiece attacker, GamePiece target)
		{
			bool flag;
			bool flag2;
			return GamePiece.CalcCombatAdvantageAtPosition(context, attacker, target, out flag, out flag2);
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x00035830 File Offset: 0x00033A30
		public static float CalcCombatAdvantageAtPosition(TurnContext context, GamePiece attacker, GamePiece target, out bool attackerWillDie, out bool defenderWillDie)
		{
			float result;
			using (SimProfilerBlock.ProfilerBlock(""))
			{
				CombatStats supportStats = attacker.SupportStats.DeepClone<CombatStats>();
				context.RecalculateSupportModifiers(attacker, target.Location);
				float num = GamePiece.CalcCombatAdvantage(context, attacker, target, out attackerWillDie, out defenderWillDie);
				attacker.SupportStats = supportStats;
				result = num;
			}
			return result;
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x00035894 File Offset: 0x00033A94
		public static float CalcCombatAdvantage(TurnContext context, GamePiece attacker, GamePiece target)
		{
			bool flag;
			bool flag2;
			return GamePiece.CalcCombatAdvantage(context, attacker, target, out flag, out flag2);
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x000358B0 File Offset: 0x00033AB0
		public static float CalcCombatAdvantage(TurnContext context, GamePiece attacker, GamePiece target, out bool attackerWillDie, out bool defenderWillDie)
		{
			List<CombatAbilityEffect> list = new List<CombatAbilityEffect>();
			foreach (Ability ability in context.GetAllAbilitiesFor(attacker))
			{
				foreach (AbilityEffect abilityEffect in ability.Effects)
				{
					CombatAbilityEffect combatAbilityEffect = abilityEffect as CombatAbilityEffect;
					if (combatAbilityEffect != null && combatAbilityEffect.IsActiveForContext(BattleRole.Attacker, target))
					{
						list.Add(combatAbilityEffect);
					}
				}
			}
			List<CombatAbilityEffect> list2 = new List<CombatAbilityEffect>();
			foreach (Ability ability2 in context.GetAllAbilitiesFor(target))
			{
				foreach (AbilityEffect abilityEffect2 in ability2.Effects)
				{
					CombatAbilityEffect combatAbilityEffect2 = abilityEffect2 as CombatAbilityEffect;
					if (combatAbilityEffect2 != null && combatAbilityEffect2.IsActiveForContext(BattleRole.Defender, attacker))
					{
						list2.Add(combatAbilityEffect2);
					}
				}
			}
			List<BattlePhaseModification> list3 = new List<BattlePhaseModification>();
			foreach (CombatAbilityEffect combatAbilityEffect3 in list)
			{
				CombatEffect_DeprioritisePhase combatEffect_DeprioritisePhase = combatAbilityEffect3 as CombatEffect_DeprioritisePhase;
				if (combatEffect_DeprioritisePhase == null)
				{
					CombatEffect_PrioritisePhase combatEffect_PrioritisePhase = combatAbilityEffect3 as CombatEffect_PrioritisePhase;
					if (combatEffect_PrioritisePhase == null)
					{
						CombatEffect_AddPhase combatEffect_AddPhase = combatAbilityEffect3 as CombatEffect_AddPhase;
						if (combatEffect_AddPhase == null)
						{
							StratagemTactic stratagemTactic = combatAbilityEffect3 as StratagemTactic;
							if (stratagemTactic != null)
							{
								foreach (IStaticData staticData in stratagemTactic.Modifiers)
								{
									BattleModifierStaticData battleModifierStaticData = staticData as BattleModifierStaticData;
									if (battleModifierStaticData != null)
									{
										if (battleModifierStaticData.PrioritisePhase)
										{
											list3.Add(new BattlePhaseModification(null, battleModifierStaticData.PhaseToPrioritise, BattlePhaseModificationType.First, attacker, target, BattleRole.Attacker));
										}
										if (battleModifierStaticData.SkipPhase)
										{
											list3.Add(new BattlePhaseModification(null, battleModifierStaticData.PhaseToSkip, BattlePhaseModificationType.Skip, attacker, target, BattleRole.Attacker));
										}
									}
								}
							}
						}
						else
						{
							list3.Add(new BattlePhaseModification(null, combatEffect_AddPhase.Phase, BattlePhaseModificationType.Twice, attacker, target, BattleRole.Attacker));
						}
					}
					else
					{
						list3.Add(new BattlePhaseModification(null, combatEffect_PrioritisePhase.Phase, BattlePhaseModificationType.First, attacker, target, BattleRole.Attacker));
					}
				}
				else
				{
					list3.Add(new BattlePhaseModification(null, combatEffect_DeprioritisePhase.Phase, BattlePhaseModificationType.Last, attacker, target, BattleRole.Attacker));
				}
			}
			foreach (CombatAbilityEffect combatAbilityEffect4 in list2)
			{
				CombatEffect_DeprioritisePhase combatEffect_DeprioritisePhase2 = combatAbilityEffect4 as CombatEffect_DeprioritisePhase;
				if (combatEffect_DeprioritisePhase2 == null)
				{
					CombatEffect_PrioritisePhase combatEffect_PrioritisePhase2 = combatAbilityEffect4 as CombatEffect_PrioritisePhase;
					if (combatEffect_PrioritisePhase2 == null)
					{
						CombatEffect_AddPhase combatEffect_AddPhase2 = combatAbilityEffect4 as CombatEffect_AddPhase;
						if (combatEffect_AddPhase2 == null)
						{
							StratagemTactic stratagemTactic2 = combatAbilityEffect4 as StratagemTactic;
							if (stratagemTactic2 != null)
							{
								foreach (IStaticData staticData2 in stratagemTactic2.Modifiers)
								{
									BattleModifierStaticData battleModifierStaticData2 = staticData2 as BattleModifierStaticData;
									if (battleModifierStaticData2 != null)
									{
										if (battleModifierStaticData2.PrioritisePhase)
										{
											list3.Add(new BattlePhaseModification(null, battleModifierStaticData2.PhaseToPrioritise, BattlePhaseModificationType.First, target, attacker, BattleRole.Defender));
										}
										if (battleModifierStaticData2.SkipPhase)
										{
											list3.Add(new BattlePhaseModification(null, battleModifierStaticData2.PhaseToSkip, BattlePhaseModificationType.Skip, target, attacker, BattleRole.Defender));
										}
									}
								}
							}
						}
						else
						{
							list3.Add(new BattlePhaseModification(null, combatEffect_AddPhase2.Phase, BattlePhaseModificationType.Twice, target, attacker, BattleRole.Defender));
						}
					}
					else
					{
						list3.Add(new BattlePhaseModification(null, combatEffect_PrioritisePhase2.Phase, BattlePhaseModificationType.First, target, attacker, BattleRole.Defender));
					}
				}
				else
				{
					list3.Add(new BattlePhaseModification(null, combatEffect_DeprioritisePhase2.Phase, BattlePhaseModificationType.Last, target, attacker, BattleRole.Defender));
				}
			}
			ValueTuple<float, float> valueTuple = GamePiece.CalcPredictedDamageRatios(BattleProcessor.GetOrderedPhases(GamePiece.DefaultBattlePhases, list3, null), attacker, target, list, list2);
			float item = valueTuple.Item1;
			float item2 = valueTuple.Item2;
			attackerWillDie = (item2 >= 1f);
			defenderWillDie = (item >= 1f);
			return 0.5f + (item - item2) / 2f;
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x00035D40 File Offset: 0x00033F40
		private static ValueTuple<float, float> CalcPredictedDamageRatios(List<BattlePhase> phases, GamePiece actor, GamePiece target, List<CombatAbilityEffect> actorEffects, List<CombatAbilityEffect> targetEffects)
		{
			int num = 0;
			int num2 = 0;
			foreach (BattlePhase phase in phases)
			{
				GamePiece.PredictDamageFromPhase(actor, target, phase, actorEffects, targetEffects, ref num, ref num2);
			}
			float item = Math.Clamp((float)num2 / (float)target.HP, 0f, 1f);
			float item2 = Math.Clamp((float)num / (float)actor.HP, 0f, 1f);
			return new ValueTuple<float, float>(item, item2);
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x00035DD8 File Offset: 0x00033FD8
		private static void PredictDamageFromPhase(GamePiece actor, GamePiece target, BattlePhase phase, List<CombatAbilityEffect> actorEffects, List<CombatAbilityEffect> targetEffects, ref int attackerDamageTaken, ref int targetDamageTaken)
		{
			if (attackerDamageTaken >= actor.HP || targetDamageTaken >= target.HP)
			{
				return;
			}
			int num = actor.GetTotalStat(phase.GetCombatStat()) - target.GetTotalStat(phase.GetCombatStat());
			if (num <= 0)
			{
				if (num < 0)
				{
					if (actorEffects.OfType<CombatEffect_NegateDamage>().Any((CombatEffect_NegateDamage negate) => negate.Phase == phase))
					{
						return;
					}
					attackerDamageTaken += Math.Abs(num);
				}
				return;
			}
			if (targetEffects.OfType<CombatEffect_NegateDamage>().Any((CombatEffect_NegateDamage negate) => negate.Phase == phase))
			{
				return;
			}
			targetDamageTaken += num;
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x00035E80 File Offset: 0x00034080
		public bool IsHexWithinSupportRange(TurnContext context, HexCoord positionToSupport)
		{
			return IEnumerableExtensions.Contains<HexCoord>(context.CurrentTurn.HexBoard.EnumerateRangeNormalized(this.Location, this.SupportRange), positionToSupport);
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x00035EAC File Offset: 0x000340AC
		public bool CanBeSupportedBy(TurnState turn, GamePiece target)
		{
			if (target == null)
			{
				return false;
			}
			if (!target.IsActive)
			{
				return false;
			}
			if (target.Id == base.Id)
			{
				return false;
			}
			if (this.Faction != target.Faction)
			{
				return false;
			}
			if (this.ControllingPlayerId == -1)
			{
				if (target.SubCategory.IsFixture())
				{
					return false;
				}
				if (this.SubCategory.IsFixture())
				{
					return false;
				}
			}
			if (this.ControllingPlayerId == target.ControllingPlayerId)
			{
				return true;
			}
			DiplomaticPairStatus diplomaticStatus = turn.CurrentDiplomaticTurn.GetDiplomaticStatus(this.ControllingPlayerId, target.ControllingPlayerId);
			return diplomaticStatus != null && diplomaticStatus.DiplomaticState.AllowSupport;
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x00035F47 File Offset: 0x00034147
		public HealGamePieceEvent Heal(int amount)
		{
			return this.Heal(new HealGamePieceContext(this.ControllingPlayerId, amount));
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x00035F5C File Offset: 0x0003415C
		public HealGamePieceEvent Heal(HealGamePieceContext context)
		{
			this.HP += context.HealingAmount;
			if (this.HP > this.TotalHP)
			{
				this.HP = this.TotalHP;
			}
			return new HealGamePieceEvent(context.PlayerSource, this, context.HealingAmount);
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000D9A RID: 3482 RVA: 0x00035FBC File Offset: 0x000341BC
		public bool CanMove
		{
			get
			{
				return this.GroundMoveDistance > 0;
			}
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x00035FCC File Offset: 0x000341CC
		public bool IsAlive()
		{
			return this.HP > 0 && this.IsActive;
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x00035FE0 File Offset: 0x000341E0
		public int TotalMelee()
		{
			return Math.Clamp(this.CombatStats.Melee + this.SupportStats.Melee - this.StatDecreases.Melee, this.CombatStats.Melee.LowerBound, this.CombatStats.Melee.UpperBound);
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x00036044 File Offset: 0x00034244
		public int TotalRanged()
		{
			return Math.Clamp(this.CombatStats.Ranged + this.SupportStats.Ranged - this.StatDecreases.Ranged, this.CombatStats.Ranged.LowerBound, this.CombatStats.Ranged.UpperBound);
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x000360A8 File Offset: 0x000342A8
		public int TotalInfernal()
		{
			return Math.Clamp(this.CombatStats.Infernal + this.SupportStats.Infernal - this.StatDecreases.Infernal, this.CombatStats.Infernal.LowerBound, this.CombatStats.Infernal.UpperBound);
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x0003610C File Offset: 0x0003430C
		public SlotType GetProvidedSlotType()
		{
			return this.SubCategory.GetProvidedSlotType();
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x00036119 File Offset: 0x00034319
		public bool IsStronghold()
		{
			return this.SubCategory == GamePieceCategory.Stronghold;
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x00036124 File Offset: 0x00034324
		public bool IsPandaemonium()
		{
			return this.SubCategory == GamePieceCategory.Pandaemonium;
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x0003612F File Offset: 0x0003432F
		public bool IsFixture()
		{
			return this.SubCategory.IsFixture();
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x0003613C File Offset: 0x0003433C
		public bool IsLegionOrTitan()
		{
			return this.SubCategory.IsLegion();
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x00036149 File Offset: 0x00034349
		public bool IsCapturable()
		{
			return this.IsFixture() && !base.HasTag<EntityTag_DestructibleFixture>();
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x0003615E File Offset: 0x0003435E
		public bool IsDestructible()
		{
			return !this.IsFixture() || base.HasTag<EntityTag_DestructibleFixture>();
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x00036170 File Offset: 0x00034370
		public bool IsCurrentlyCapturable()
		{
			return this.IsCapturable() && this.CanBeCaptured;
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x00036187 File Offset: 0x00034387
		public bool IsOwnedBy(int playerId)
		{
			return this.ControllingPlayerId == playerId;
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x00036194 File Offset: 0x00034394
		public bool IsOwned()
		{
			int controllingPlayerId = this.ControllingPlayerId;
			return controllingPlayerId != int.MinValue && controllingPlayerId != -1;
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x000361BF File Offset: 0x000343BF
		public override ItemBanishedEvent OnBanished(TurnProcessContext context, PlayerState itemOwner, int instigatorId = -2147483648)
		{
			ItemBanishedEvent result = base.OnBanished(context, itemOwner, instigatorId);
			this.DefeatedLocation = this.Location;
			this.Location = HexCoord.Invalid;
			this.HP = 0;
			this.DeathTurn = context.CurrentTurn.TurnValue;
			return result;
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x000361F9 File Offset: 0x000343F9
		public bool IsHostile(TurnState turn, GamePiece other)
		{
			return this.IsHostile(turn, other.ControllingPlayerId);
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x00036208 File Offset: 0x00034408
		public bool IsHostile(TurnState turn, int otherPlayerId)
		{
			return turn.CombatAuthorizedBetween(this.ControllingPlayerId, otherPlayerId);
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x00036217 File Offset: 0x00034417
		public bool IsNeutral(TurnState turn, GamePiece other)
		{
			return this.IsNeutral(turn, other.ControllingPlayerId);
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x00036226 File Offset: 0x00034426
		public bool IsNeutral(TurnState turn, int otherPlayerID)
		{
			return turn.CurrentDiplomaticTurn.GetDiplomaticStatus(this.ControllingPlayerId, otherPlayerID).DiplomaticState is NeutralState;
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x00036247 File Offset: 0x00034447
		public bool CanBeHealedBy(TurnState turn, GamePiece other)
		{
			return this.CanBeHealedBy(turn, other.ControllingPlayerId);
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x00036256 File Offset: 0x00034456
		public bool CanBeHealedBy(TurnState turn, int providingPlayerId)
		{
			return turn.CurrentDiplomaticTurn.GetDiplomaticStatus(this.ControllingPlayerId, providingPlayerId).DiplomaticState.AllowNearbyHealingProvidedBy(providingPlayerId);
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x00036275 File Offset: 0x00034475
		public int GetPrestigeGeneration()
		{
			if (!base.HasTag<EntityTag_CannotGeneratePrestige>())
			{
				return this.PassivePrestige;
			}
			return 0;
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x0003628C File Offset: 0x0003448C
		public HexCoord LastKnownPosition()
		{
			if (!this.IsAlive())
			{
				return this.DefeatedLocation;
			}
			return this.Location;
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x000362A3 File Offset: 0x000344A3
		public bool TryAttachItem(Identifier gameItemId)
		{
			if (this.Slots.Count >= this.MaxSlots)
			{
				return false;
			}
			this.Slots.Add(gameItemId);
			return true;
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x000362CC File Offset: 0x000344CC
		public bool TryRemoveItem(Identifier gameItemId)
		{
			return this.Slots.Remove(gameItemId);
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x000362DA File Offset: 0x000344DA
		public void LevelUp(bool resetExperience, int newLevel)
		{
			if (resetExperience)
			{
				this.LevelExperience.SetBase(0f);
			}
			this.SetLevel(newLevel);
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x000362F8 File Offset: 0x000344F8
		public sealed override void DeepClone(out GameItem clone)
		{
			GamePiece gamePiece;
			this.DeepClone(out gamePiece);
			clone = gamePiece;
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x00036310 File Offset: 0x00034510
		public void DeepClone(out GamePiece clone)
		{
			clone = new GamePiece();
			base.DeepCloneGameItemParts(clone);
			clone.SubCategory = this.SubCategory;
			clone.Faction = this.Faction;
			clone.HP = this.HP;
			clone.TemporaryHP = this.TemporaryHP;
			clone.ControllingPlayerId = this.ControllingPlayerId;
			clone.DeathTurn = this.DeathTurn;
			clone.SpawnTurn = this.SpawnTurn;
			clone.LastMoveTurn = this.LastMoveTurn;
			clone.LastBattleTurn = this.LastBattleTurn;
			clone.Location = this.Location;
			clone.DefeatedLocation = this.DefeatedLocation;
			clone.LastDefeatedBy = this.LastDefeatedBy;
			clone.LastFriendlyCanton = this.LastFriendlyCanton;
			clone.LastConvertedTurn = this.LastConvertedTurn;
			clone.TotalHP = this.TotalHP.DeepClone<ModifiableValue>();
			clone.HealingBonus = this.HealingBonus.DeepClone<ModifiableValue>();
			clone.CombatRewardMultiplier = this.CombatRewardMultiplier.DeepClone<ModifiableValue>();
			clone.CombatExperienceReward = this.CombatExperienceReward.DeepClone<ModifiableValue>();
			clone.LevelExperience = this.LevelExperience.DeepClone<ModifiableValue>();
			clone.SupportRange = this.SupportRange.DeepClone<ModifiableValue>();
			clone.MaxSlots = this.MaxSlots.DeepClone<ModifiableValue>();
			clone.SupportStrength = this.SupportStrength.DeepClone<ModifiableValue>();
			clone.PassivePrestige = this.PassivePrestige.DeepClone<ModifiableValue>();
			clone.GroundMoveDistance = this.GroundMoveDistance.DeepClone<ModifiableValue>();
			clone.TeleportDistance = this.TeleportDistance.DeepClone<ModifiableValue>();
			clone.CombatStats = this.CombatStats.DeepClone<CombatStats>();
			clone.SupportStats = this.SupportStats.DeepClone<CombatStats>();
			clone.StatDecreases = this.StatDecreases.DeepClone<CombatStats>();
			clone.CanTeleport = this.CanTeleport.DeepClone<ModifiableBool>();
			clone.CanBeCaptured = this.CanBeCaptured.DeepClone<ModifiableBool>();
			clone.CanMoveThroughEnemyTerritory = this.CanMoveThroughEnemyTerritory.DeepClone<ModifiableBool>();
			clone.CanInitiateCombat = this.CanInitiateCombat.DeepClone<ModifiableBool>();
			clone.CanCaptureCantons = this.CanCaptureCantons.DeepClone<ModifiableBool>();
			clone.CanUseCombatAbilities = this.CanUseCombatAbilities.DeepClone<ModifiableBool>();
			clone.CanBeAffectedByRituals = this.CanBeAffectedByRituals.DeepClone<ModifiableBool>();
			clone.CanBeConverted = this.CanBeConverted;
			clone.Slots = this.Slots.DeepClone();
			clone.LearntManuscriptsCount = this.LearntManuscriptsCount;
		}

		// Token: 0x040005F8 RID: 1528
		[JsonProperty]
		public GamePieceCategory SubCategory;

		// Token: 0x040005F9 RID: 1529
		[JsonProperty]
		[DefaultValue(GamePieceFaction.Hellish)]
		public GamePieceFaction Faction;

		// Token: 0x040005FA RID: 1530
		[JsonProperty]
		[DefaultValue(1)]
		public int HP = 1;

		// Token: 0x040005FB RID: 1531
		[JsonProperty]
		public int TemporaryHP;

		// Token: 0x040005FC RID: 1532
		[JsonProperty]
		public int ControllingPlayerId;

		// Token: 0x040005FD RID: 1533
		[JsonProperty]
		[DefaultValue(-1)]
		public int SpawnTurn = -1;

		// Token: 0x040005FE RID: 1534
		[JsonProperty]
		[DefaultValue(-1)]
		public int DeathTurn = -1;

		// Token: 0x040005FF RID: 1535
		[JsonProperty]
		[DefaultValue(-1)]
		public int LastMoveTurn = -1;

		// Token: 0x04000600 RID: 1536
		[JsonProperty]
		[DefaultValue(-1)]
		public int LastBattleTurn = -1;

		// Token: 0x04000601 RID: 1537
		[JsonProperty]
		[DefaultValue(Identifier.Invalid)]
		public Identifier LastDefeatedBy = Identifier.Invalid;

		// Token: 0x04000602 RID: 1538
		[JsonProperty]
		[DefaultValue(-1)]
		public int LastConvertedTurn = -1;

		// Token: 0x04000603 RID: 1539
		[JsonProperty]
		public HexCoord Location;

		// Token: 0x04000604 RID: 1540
		[JsonProperty]
		public HexCoord DefeatedLocation;

		// Token: 0x04000605 RID: 1541
		[JsonProperty]
		[DefaultValue(2147483647)]
		public HexCoord LastFriendlyCanton = HexCoord.Invalid;

		// Token: 0x04000606 RID: 1542
		[JsonProperty]
		[DefaultValue(1)]
		public ModifiableValue TotalHP = new ModifiableValue(1f, 1, int.MaxValue, RoundingMode.RoundDown);

		// Token: 0x04000607 RID: 1543
		[JsonProperty]
		[DefaultValue(0)]
		public ModifiableValue HealingBonus = 0;

		// Token: 0x04000608 RID: 1544
		[JsonProperty]
		[DefaultValue(1)]
		public ModifiableValue CombatRewardMultiplier = 1;

		// Token: 0x04000609 RID: 1545
		[JsonProperty]
		[DefaultValue(1)]
		public ModifiableValue CombatExperienceReward = 1;

		// Token: 0x0400060A RID: 1546
		[JsonProperty]
		[DefaultValue(0)]
		public ModifiableValue LevelExperience = 0;

		// Token: 0x0400060B RID: 1547
		[JsonProperty]
		[DefaultValue(0)]
		public ModifiableValue SupportRange = 0;

		// Token: 0x0400060C RID: 1548
		[JsonProperty]
		[DefaultValue(4)]
		public ModifiableValue MaxSlots = 4;

		// Token: 0x0400060D RID: 1549
		[JsonProperty]
		[DefaultValue(50)]
		public ModifiableValue SupportStrength = 50;

		// Token: 0x0400060E RID: 1550
		[JsonProperty]
		[DefaultValue(0)]
		public ModifiableValue PassivePrestige = 0;

		// Token: 0x0400060F RID: 1551
		[JsonProperty]
		[DefaultValue(0)]
		public ModifiableValue GroundMoveDistance = 0;

		// Token: 0x04000610 RID: 1552
		[JsonProperty]
		[DefaultValue(0)]
		public ModifiableValue TeleportDistance = 0;

		// Token: 0x04000611 RID: 1553
		[JsonProperty]
		public CombatStats CombatStats = new CombatStats();

		// Token: 0x04000612 RID: 1554
		[JsonProperty]
		public CombatStats SupportStats = new CombatStats();

		// Token: 0x04000613 RID: 1555
		[JsonProperty]
		public CombatStats StatDecreases = new CombatStats();

		// Token: 0x04000614 RID: 1556
		[JsonProperty]
		[DefaultValue(1)]
		public ModifiableBool CanTeleport = new ModifiableBool(false, LogicOperation.Or);

		// Token: 0x04000615 RID: 1557
		[JsonProperty]
		[DefaultValue(2)]
		public ModifiableBool CanBeCaptured = new ModifiableBool(true, LogicOperation.And);

		// Token: 0x04000616 RID: 1558
		[JsonProperty]
		[DefaultValue(1)]
		public ModifiableBool CanMoveThroughEnemyTerritory = new ModifiableBool(false, LogicOperation.Or);

		// Token: 0x04000617 RID: 1559
		[JsonProperty]
		[DefaultValue(2)]
		public ModifiableBool CanInitiateCombat = new ModifiableBool(true, LogicOperation.And);

		// Token: 0x04000618 RID: 1560
		[JsonProperty]
		[DefaultValue(2)]
		public ModifiableBool CanBeAffectedByRituals = new ModifiableBool(true, LogicOperation.And);

		// Token: 0x04000619 RID: 1561
		[JsonProperty]
		[DefaultValue(2)]
		public ModifiableBool CanCaptureCantons = new ModifiableBool(true, LogicOperation.And);

		// Token: 0x0400061A RID: 1562
		[JsonProperty]
		[DefaultValue(2)]
		public ModifiableBool CanUseCombatAbilities = new ModifiableBool(true, LogicOperation.And);

		// Token: 0x0400061B RID: 1563
		[JsonProperty]
		public bool CanBeConverted;

		// Token: 0x0400061C RID: 1564
		[JsonProperty]
		public List<Identifier> Slots = new List<Identifier>();

		// Token: 0x0400061D RID: 1565
		[JsonProperty]
		[DefaultValue(0)]
		public int LearntManuscriptsCount;

		// Token: 0x0400061E RID: 1566
		private static readonly BattlePhase[] DefaultBattlePhases = new BattlePhase[]
		{
			BattlePhase.Ranged,
			BattlePhase.Melee,
			BattlePhase.Infernal
		};
	}
}
