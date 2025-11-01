using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000521 RID: 1313
	public class PendingDiplomacy_VileCalumny : PendingDiplomacy_Insult
	{
		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06001972 RID: 6514 RVA: 0x00059AD9 File Offset: 0x00057CD9
		public override DiplomaticPendingValue DiplomaticPendingValue
		{
			get
			{
				return DiplomaticPendingValue.VileCalumny;
			}
		}

		// Token: 0x06001973 RID: 6515 RVA: 0x00059AE0 File Offset: 0x00057CE0
		[JsonConstructor]
		public PendingDiplomacy_VileCalumny()
		{
		}

		// Token: 0x06001974 RID: 6516 RVA: 0x00059AE8 File Offset: 0x00057CE8
		public PendingDiplomacy_VileCalumny(int actorId) : base(actorId)
		{
		}

		// Token: 0x06001975 RID: 6517 RVA: 0x00059AF4 File Offset: 0x00057CF4
		protected override void Update(TurnProcessContext context, PlayerState actor, PlayerState target)
		{
			GameEvent gameEvent = context.CurrentTurn.AddGameEvent<VileCalumnySentEvent>(new VileCalumnySentEvent(this.SourceId, actor.Id, target.Id)
			{
				OrderType = OrderTypes.VileCalumny
			});
			VileCalumnyRequest decisionRequest = new VileCalumnyRequest(context.CurrentTurn)
			{
				RequestingPlayerId = this.SourceId,
				AffectedPlayerId = target.Id,
				ScapegoatId = actor.Id,
				PrestigeWager = this.Wager
			};
			gameEvent.AddChildEvent<DecisionAddedEvent>(context.CurrentTurn.AddDecisionToAskPlayer(target.Id, decisionRequest));
		}

		// Token: 0x06001976 RID: 6518 RVA: 0x00059B84 File Offset: 0x00057D84
		public override void DeepClone(out DiplomaticState clone)
		{
			PendingDiplomacy_VileCalumny pendingDiplomacy_VileCalumny = new PendingDiplomacy_VileCalumny();
			base.DeepClonePendingDiplomacyParts(pendingDiplomacy_VileCalumny);
			pendingDiplomacy_VileCalumny.SourceId = this.SourceId;
			clone = pendingDiplomacy_VileCalumny;
		}

		// Token: 0x04000BA8 RID: 2984
		[JsonProperty]
		public int SourceId;
	}
}
