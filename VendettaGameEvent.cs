using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000219 RID: 537
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public abstract class VendettaGameEvent : GameEvent, IGrievanceAccessor, ISelectionAccessor, ICancellableGameEvent
	{
		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000A77 RID: 2679 RVA: 0x0002E4EA File Offset: 0x0002C6EA
		// (set) Token: 0x06000A78 RID: 2680 RVA: 0x0002E4F2 File Offset: 0x0002C6F2
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public GrievanceContext GrievanceResponse { get; set; }

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000A79 RID: 2681 RVA: 0x0002E4FB File Offset: 0x0002C6FB
		// (set) Token: 0x06000A7A RID: 2682 RVA: 0x0002E503 File Offset: 0x0002C703
		[JsonProperty]
		public bool Cancelled { get; set; }

		// Token: 0x06000A7B RID: 2683 RVA: 0x0002E50C File Offset: 0x0002C70C
		protected VendettaGameEvent()
		{
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x0002E514 File Offset: 0x0002C714
		protected VendettaGameEvent(int triggeringPlayerId, VendettaObjective objective, int prestigeWager, int turns) : base(triggeringPlayerId)
		{
			this.Objective = objective;
			this.PrestigeWager = prestigeWager;
			this.Turns = turns;
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x0002E534 File Offset: 0x0002C734
		protected void DeepCloneVendettaGameEventParts(VendettaGameEvent vendettaGameEvent)
		{
			vendettaGameEvent.Objective = this.Objective.DeepClone<VendettaObjective>();
			vendettaGameEvent.PrestigeWager = this.PrestigeWager;
			vendettaGameEvent.Turns = this.Turns;
			vendettaGameEvent.GrievanceResponse = this.GrievanceResponse.DeepClone<GrievanceContext>();
			vendettaGameEvent.Cancelled = this.Cancelled;
			base.DeepCloneGameEventParts<VendettaGameEvent>(vendettaGameEvent);
		}

		// Token: 0x06000A7E RID: 2686
		public abstract override void DeepClone(out GameEvent clone);

		// Token: 0x040004DB RID: 1243
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public VendettaObjective Objective;

		// Token: 0x040004DC RID: 1244
		[BindableValue("prestige", BindingOption.None)]
		[JsonProperty]
		public int PrestigeWager;

		// Token: 0x040004DD RID: 1245
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public int Turns;
	}
}
