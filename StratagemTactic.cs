using System;
using System.Collections.Generic;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200047D RID: 1149
	[Serializable]
	public class StratagemTactic : CombatAbilityEffect, IDeepClone<StratagemTactic>
	{
		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06001548 RID: 5448 RVA: 0x00050552 File Offset: 0x0004E752
		[JsonIgnore]
		public override bool CanBeCancelled
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001549 RID: 5449 RVA: 0x00050558 File Offset: 0x0004E758
		protected override GameEvent OnStratagems(Ability source, CombatAbilityContext context, BattleEvent battleEvent, BattleContext battleContext)
		{
			TurnProcessContext turnContext = context.TurnContext;
			GamePiece actor = context.Actor;
			TurnState turn = context.Turn;
			GamePiece opponent = context.Opponent;
			PlayerState playerState = turn.FindControllingPlayer(this.ActorId);
			BattleStratagemEvent battleStratagemEvent = null;
			foreach (IStaticData staticData in this.Modifiers)
			{
				GamePieceModifierStaticData gamePieceModifierStaticData = staticData as GamePieceModifierStaticData;
				if (gamePieceModifierStaticData == null)
				{
					DecreaseAttributeTacticStaticData decreaseAttributeTacticStaticData = staticData as DecreaseAttributeTacticStaticData;
					if (decreaseAttributeTacticStaticData == null)
					{
						BattleModifierStaticData battleModifierStaticData = staticData as BattleModifierStaticData;
						if (battleModifierStaticData != null)
						{
							if (battleModifierStaticData.PrioritisePhase)
							{
								battleStratagemEvent = new BattleStratagemEvent(this.CurrentAbilityStage, source, context, base.TypeName + ".PrioritisePhase");
								battleStratagemEvent.BattlePhase = battleModifierStaticData.PhaseToPrioritise;
								battleContext.PhaseModifications.Add(new BattlePhaseModification(source, battleModifierStaticData.PhaseToPrioritise, BattlePhaseModificationType.First, context));
							}
							if (battleModifierStaticData.SkipPhase)
							{
								battleStratagemEvent = new BattleStratagemEvent(this.CurrentAbilityStage, source, context, base.TypeName + ".SkipPhase");
								battleStratagemEvent.BattlePhase = battleModifierStaticData.PhaseToSkip;
								battleContext.PhaseModifications.Add(new BattlePhaseModification(source, battleModifierStaticData.PhaseToSkip, BattlePhaseModificationType.Skip, context));
							}
						}
					}
					else
					{
						int num = -turn.GetRandomRoll(decreaseAttributeTacticStaticData.MinRoll, decreaseAttributeTacticStaticData.MaxRoll, playerState.HasTag<EntityTag_CheatLuckyStratagemEffectRolls>());
						GamePieceModifier gamePieceModifier = new GamePieceModifier(new GamePieceModifierStaticData().SetValue(decreaseAttributeTacticStaticData.Stat.ToGamePieceStat(), (float)num, ModifierTarget.ValueOffset));
						gamePieceModifier.Source = source;
						gamePieceModifier.ApplyTo(turnContext, opponent, true);
						battleStratagemEvent = new BattleStratagemEvent(this.CurrentAbilityStage, source, context, base.TypeName + ".DecreaseAttribute");
						battleStratagemEvent.PrimaryValueChange = num;
						battleStratagemEvent.MinValueChange = decreaseAttributeTacticStaticData.MinRoll;
						battleStratagemEvent.MaxValueChange = decreaseAttributeTacticStaticData.MaxRoll;
						battleStratagemEvent.AddAppliedModifier(opponent, gamePieceModifier);
					}
				}
				else
				{
					GamePieceModifier gamePieceModifier = new GamePieceModifier(gamePieceModifierStaticData);
					gamePieceModifier.Source = source;
					gamePieceModifier.ApplyTo(turnContext, actor);
					battleStratagemEvent = new BattleStratagemEvent(this.CurrentAbilityStage, source, context, base.TypeName + ".IncreaseAttribute");
					battleStratagemEvent.PrimaryValueChange = (int)gamePieceModifier.GetSingleValue();
					battleStratagemEvent.AddAppliedModifier(actor, gamePieceModifier);
				}
			}
			return battleStratagemEvent;
		}

		// Token: 0x0600154A RID: 5450 RVA: 0x000507C8 File Offset: 0x0004E9C8
		public override void DeepClone(out AbilityEffect clone)
		{
			StratagemTactic stratagemTactic;
			this.DeepClone(out stratagemTactic);
			clone = stratagemTactic;
		}

		// Token: 0x0600154B RID: 5451 RVA: 0x000507E0 File Offset: 0x0004E9E0
		public void DeepClone(out StratagemTactic clone)
		{
			StratagemTactic stratagemTactic = new StratagemTactic
			{
				StaticDataId = this.StaticDataId.DeepClone(),
				Modifiers = this.Modifiers.DeepClone(CloneFunction.FastClone)
			};
			base.DeepCloneCombatAbilityEffectParts(stratagemTactic);
			clone = stratagemTactic;
		}

		// Token: 0x04000ADB RID: 2779
		[JsonProperty]
		public string StaticDataId;

		// Token: 0x04000ADC RID: 2780
		[JsonProperty]
		public List<IStaticData> Modifiers = new List<IStaticData>();
	}
}
