using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020001E8 RID: 488
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class BattlePhaseEvent : GameEvent
	{
		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000986 RID: 2438 RVA: 0x0002CB49 File Offset: 0x0002AD49
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x0002CB4C File Offset: 0x0002AD4C
		[JsonConstructor]
		private BattlePhaseEvent()
		{
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x0002CB54 File Offset: 0x0002AD54
		public BattlePhaseEvent(BattlePhaseResult battlePhaseResult, int attackerId, int defenderId) : base(attackerId)
		{
			this.PhaseResult = battlePhaseResult;
			base.AddAffectedPlayerId(defenderId);
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x0002CB6B File Offset: 0x0002AD6B
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0} Phase was {1} vs {2}", this.PhaseResult.BattlePhase, this.PhaseResult.AttackerPower, this.PhaseResult.DefenderPower);
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x0002CBA0 File Offset: 0x0002ADA0
		public override void DeepClone(out GameEvent clone)
		{
			BattlePhaseEvent battlePhaseEvent = new BattlePhaseEvent
			{
				PhaseResult = this.PhaseResult
			};
			base.DeepCloneGameEventParts<BattlePhaseEvent>(battlePhaseEvent);
			clone = battlePhaseEvent;
		}

		// Token: 0x04000491 RID: 1169
		[JsonProperty]
		public BattlePhaseResult PhaseResult;
	}
}
