using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200051E RID: 1310
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PendingDiplomacy_DraconicRazzia : PendingDiplomacyState
	{
		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06001963 RID: 6499 RVA: 0x00059861 File Offset: 0x00057A61
		public override DiplomaticPendingValue DiplomaticPendingValue
		{
			get
			{
				return DiplomaticPendingValue.DraconicRazzia;
			}
		}

		// Token: 0x06001964 RID: 6500 RVA: 0x00059868 File Offset: 0x00057A68
		[JsonConstructor]
		public PendingDiplomacy_DraconicRazzia()
		{
		}

		// Token: 0x06001965 RID: 6501 RVA: 0x00059870 File Offset: 0x00057A70
		public PendingDiplomacy_DraconicRazzia(int playerId, int delay, int length) : base(playerId)
		{
			this._turnDelay = delay;
			this._length = length;
		}

		// Token: 0x06001966 RID: 6502 RVA: 0x00059888 File Offset: 0x00057A88
		protected override void Update(TurnProcessContext context, PlayerState actor, PlayerState target)
		{
			this._turnDelay--;
			if (this._turnDelay >= 0)
			{
				context.CurrentTurn.AddGameEvent<DraconicRazziaAnnouncementEvent>(new DraconicRazziaAnnouncementEvent(actor.Id, target.Id)
				{
					OrderType = OrderTypes.DraconicRazzia,
					Turn = context.CurrentTurn.TurnValue + this._turnDelay
				});
				return;
			}
			context.Diplomacy.GetDiplomaticStatus(actor, target).ChangeState<DiplomaticState_DraconicRazzia>(context, new DiplomaticState_DraconicRazzia(this.ActorId, this._length), false);
		}

		// Token: 0x06001967 RID: 6503 RVA: 0x00059910 File Offset: 0x00057B10
		public override void DeepClone(out DiplomaticState clone)
		{
			PendingDiplomacy_DraconicRazzia pendingDiplomacy_DraconicRazzia = new PendingDiplomacy_DraconicRazzia(this.ActorId, this._turnDelay, this._length);
			base.DeepClonePendingDiplomacyParts(pendingDiplomacy_DraconicRazzia);
			clone = pendingDiplomacy_DraconicRazzia;
		}

		// Token: 0x04000BA3 RID: 2979
		[JsonProperty]
		public int _turnDelay;

		// Token: 0x04000BA4 RID: 2980
		[JsonProperty]
		public int _length;
	}
}
