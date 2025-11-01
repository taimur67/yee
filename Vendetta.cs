using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000525 RID: 1317
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class Vendetta : IIdentifiable, IEquatable<IIdentifiable>, IDeepClone<Vendetta>
	{
		// Token: 0x170003AA RID: 938
		// (get) Token: 0x060019A0 RID: 6560 RVA: 0x0005A035 File Offset: 0x00058235
		// (set) Token: 0x060019A1 RID: 6561 RVA: 0x0005A03D File Offset: 0x0005823D
		[JsonIgnore]
		public Identifier Id
		{
			get
			{
				return this._id;
			}
			set
			{
				this._id = value;
			}
		}

		// Token: 0x060019A2 RID: 6562 RVA: 0x0005A046 File Offset: 0x00058246
		public Vendetta()
		{
		}

		// Token: 0x060019A3 RID: 6563 RVA: 0x0005A050 File Offset: 0x00058250
		public Vendetta(Identifier id, int actorId, int targetId, Rank targetRank, VendettaObjective objective = null, int turnTotal = 3, int prestigeWager = 6, StatModifier additionalPrestigeReward = null)
		{
			this.Id = id;
			this.Objective = (objective ?? new NoVendettaObjective());
			this.ActorId = actorId;
			this.TargetId = targetId;
			this.TurnTotal = turnTotal;
			this.TurnRemaining = turnTotal + 1;
			this.PrestigeWager = prestigeWager;
			this.PrestigeReward = GrievanceProcessor.CalculateVendettaPrestigeReward(prestigeWager, turnTotal, objective, additionalPrestigeReward, targetRank);
		}

		// Token: 0x060019A4 RID: 6564 RVA: 0x0005A0BD File Offset: 0x000582BD
		public void OnTurnEnd()
		{
			this.TurnRemaining--;
		}

		// Token: 0x060019A5 RID: 6565 RVA: 0x0005A0CD File Offset: 0x000582CD
		public bool IsComplete(TurnState turn)
		{
			return this.Objective.IsCompleted(turn) || this.TurnRemaining <= 0;
		}

		// Token: 0x060019A6 RID: 6566 RVA: 0x0005A0EC File Offset: 0x000582EC
		public void PrestigeCompletePayment(TurnState turn)
		{
			if (this.Objective.IsCompleted(turn))
			{
				turn.FindPlayerState(this.ActorId, null).GivePrestige(this.PrestigeReward);
				turn.FindPlayerState(this.TargetId, null).RemovePrestige(this.PrestigeWager);
				return;
			}
			if (this.TurnRemaining <= 0)
			{
				turn.FindPlayerState(this.TargetId, null).GivePrestige(this.PrestigeWager);
			}
		}

		// Token: 0x060019A7 RID: 6567 RVA: 0x0005A15E File Offset: 0x0005835E
		public override int GetHashCode()
		{
			return (int)this._id;
		}

		// Token: 0x060019A8 RID: 6568 RVA: 0x0005A166 File Offset: 0x00058366
		public bool Equals(IIdentifiable other)
		{
			return other != null && (this == other || this.Id == other.Id);
		}

		// Token: 0x060019A9 RID: 6569 RVA: 0x0005A184 File Offset: 0x00058384
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (this == obj)
			{
				return true;
			}
			IIdentifiable identifiable = obj as IIdentifiable;
			return identifiable != null && this.Equals(identifiable);
		}

		// Token: 0x060019AA RID: 6570 RVA: 0x0005A1B0 File Offset: 0x000583B0
		public void DeepClone(out Vendetta clone)
		{
			clone = new Vendetta
			{
				_id = this._id,
				ActorId = this.ActorId,
				TargetId = this.TargetId,
				TurnTotal = this.TurnTotal.DeepClone<ModifiableValue>(),
				TurnRemaining = this.TurnRemaining,
				PrestigeWager = this.PrestigeWager,
				PrestigeReward = this.PrestigeReward.DeepClone<ModifiableValue>(),
				Objective = this.Objective.DeepClone<VendettaObjective>(),
				TurnStarted = this.TurnStarted
			};
		}

		// Token: 0x04000BAE RID: 2990
		[JsonProperty]
		private Identifier _id;

		// Token: 0x04000BAF RID: 2991
		public const int DefaultVendettaTurnLength = 3;

		// Token: 0x04000BB0 RID: 2992
		public const int DefaultVendettaCost = 6;

		// Token: 0x04000BB1 RID: 2993
		[JsonProperty]
		public int ActorId;

		// Token: 0x04000BB2 RID: 2994
		[JsonProperty]
		public int TargetId;

		// Token: 0x04000BB3 RID: 2995
		[JsonProperty]
		public ModifiableValue TurnTotal;

		// Token: 0x04000BB4 RID: 2996
		[JsonProperty]
		public int TurnRemaining;

		// Token: 0x04000BB5 RID: 2997
		[JsonProperty]
		public int PrestigeWager;

		// Token: 0x04000BB6 RID: 2998
		[JsonProperty]
		public ModifiableValue PrestigeReward;

		// Token: 0x04000BB7 RID: 2999
		[JsonProperty]
		public VendettaObjective Objective;

		// Token: 0x04000BB8 RID: 3000
		[JsonProperty]
		public int TurnStarted;
	}
}
