using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003AE RID: 942
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PraetorDuelPhaseEvent : GameEvent
	{
		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06001268 RID: 4712 RVA: 0x000467AB File Offset: 0x000449AB
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06001269 RID: 4713 RVA: 0x000467AE File Offset: 0x000449AE
		[JsonConstructor]
		public PraetorDuelPhaseEvent()
		{
		}

		// Token: 0x0600126A RID: 4714 RVA: 0x000467B6 File Offset: 0x000449B6
		public PraetorDuelPhaseEvent(PraetorDuelPhase phase)
		{
			this.Phase = phase;
		}

		// Token: 0x0600126B RID: 4715 RVA: 0x000467C5 File Offset: 0x000449C5
		public void OnStart(DuelProcessDamageTally tally)
		{
			this.DamageBefore = tally.DeepClone<DuelProcessDamageTally>();
		}

		// Token: 0x0600126C RID: 4716 RVA: 0x000467D3 File Offset: 0x000449D3
		public void OnEnd(DuelProcessDamageTally tally)
		{
			this.DamageAfter = tally.DeepClone<DuelProcessDamageTally>();
		}

		// Token: 0x0600126D RID: 4717 RVA: 0x000467E1 File Offset: 0x000449E1
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0} : {1}", base.GetDebugName(context), this.Phase);
		}

		// Token: 0x0600126E RID: 4718 RVA: 0x00046800 File Offset: 0x00044A00
		public override void DeepClone(out GameEvent clone)
		{
			PraetorDuelPhaseEvent praetorDuelPhaseEvent = new PraetorDuelPhaseEvent
			{
				Phase = this.Phase,
				DamageBefore = this.DamageBefore.DeepClone<DuelProcessDamageTally>(),
				DamageAfter = this.DamageAfter.DeepClone<DuelProcessDamageTally>()
			};
			base.DeepCloneGameEventParts<PraetorDuelPhaseEvent>(praetorDuelPhaseEvent);
			clone = praetorDuelPhaseEvent;
		}

		// Token: 0x04000893 RID: 2195
		[JsonProperty]
		public PraetorDuelPhase Phase;

		// Token: 0x04000894 RID: 2196
		[JsonProperty]
		public DuelProcessDamageTally DamageBefore;

		// Token: 0x04000895 RID: 2197
		[JsonProperty]
		public DuelProcessDamageTally DamageAfter;
	}
}
