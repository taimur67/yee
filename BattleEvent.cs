using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020001EF RID: 495
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class BattleEvent : GameEvent
	{
		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x060009A5 RID: 2469 RVA: 0x0002D001 File Offset: 0x0002B201
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x0002D004 File Offset: 0x0002B204
		[JsonConstructor]
		private BattleEvent()
		{
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x0002D00C File Offset: 0x0002B20C
		public BattleEvent(BattleResult battleResult) : base(battleResult.AttackingPlayerId)
		{
			this.BattleResult = battleResult;
			base.AddAffectedPlayerId(battleResult.DefendingPlayerId);
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x0002D02D File Offset: 0x0002B22D
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("Battle between {0} and {1}. Outcome: {2}", this.BattleResult.Attacker_StartState.NameKey, this.BattleResult.Defender_StartState.NameKey, this.BattleResult.Outcome);
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x0002D06C File Offset: 0x0002B26C
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			TurnLogEntryType result;
			switch (this.BattleResult.Outcome)
			{
			case BattleOutcome.Victory_Attacker:
				result = TurnLogEntryType.BattleAttackerVictory;
				break;
			case BattleOutcome.Victory_Defender:
				result = TurnLogEntryType.BattleDefenderVictory;
				break;
			case BattleOutcome.Stalemate:
				result = TurnLogEntryType.BattleStalemate;
				break;
			case BattleOutcome.Both_Destroyed:
				result = TurnLogEntryType.BattleBothDestroyed;
				break;
			default:
				result = TurnLogEntryType.None;
				break;
			}
			return result;
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x0002D0B6 File Offset: 0x0002B2B6
		public bool IsVsPoP()
		{
			return this.BattleResult.Defender_StartState.IsFixture();
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x0002D0C8 File Offset: 0x0002B2C8
		public override void DeepClone(out GameEvent clone)
		{
			BattleEvent battleEvent = new BattleEvent
			{
				BattleResult = this.BattleResult.DeepClone<BattleResult>()
			};
			base.DeepCloneGameEventParts<BattleEvent>(battleEvent);
			clone = battleEvent;
		}

		// Token: 0x040004A3 RID: 1187
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public BattleResult BattleResult;
	}
}
