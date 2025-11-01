using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020001EB RID: 491
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class BattlePhaseModificationEvent : BattleAbilityEvent
	{
		// Token: 0x06000992 RID: 2450 RVA: 0x0002CCC6 File Offset: 0x0002AEC6
		[JsonConstructor]
		private BattlePhaseModificationEvent()
		{
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x0002CCD0 File Offset: 0x0002AED0
		public BattlePhaseModificationEvent(BattlePhaseModification battlePhaseModification) : base(CombatAbilityStage.PreBattle, battlePhaseModification.Ability, battlePhaseModification.ControllingPlayerId, battlePhaseModification.OpponentId, battlePhaseModification.ActorId, null)
		{
			this.BattlePhase = battlePhaseModification.BattlePhase;
			this.BattlePhaseModificationType = battlePhaseModification.BattlePhaseModificationType;
			this.EffectId = this.GetEffectId();
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x0002CD24 File Offset: 0x0002AF24
		private string GetEffectId()
		{
			string result;
			switch (this.BattlePhaseModificationType)
			{
			case BattlePhaseModificationType.Twice:
				result = "PhaseTwice";
				break;
			case BattlePhaseModificationType.First:
				result = "PhaseFirst";
				break;
			case BattlePhaseModificationType.Last:
				result = "PhaseLast";
				break;
			case BattlePhaseModificationType.Skip:
				result = "PhaseSkip";
				break;
			default:
				result = string.Empty;
				break;
			}
			return result;
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x0002CD77 File Offset: 0x0002AF77
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0} changed phase {1} with {2}", this.AbilityContext.SourceId, this.BattlePhase, this.BattlePhaseModificationType);
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x0002CDA4 File Offset: 0x0002AFA4
		public override void DeepClone(out GameEvent clone)
		{
			BattlePhaseModificationEvent battlePhaseModificationEvent = new BattlePhaseModificationEvent
			{
				BattlePhase = this.BattlePhase,
				BattlePhaseModificationType = this.BattlePhaseModificationType
			};
			base.DeepCloneBattleAbilityEventParts(battlePhaseModificationEvent);
			clone = battlePhaseModificationEvent;
		}

		// Token: 0x04000498 RID: 1176
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public BattlePhase BattlePhase;

		// Token: 0x04000499 RID: 1177
		[JsonProperty]
		public BattlePhaseModificationType BattlePhaseModificationType;
	}
}
