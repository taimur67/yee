using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000515 RID: 1301
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public abstract class PendingDiplomacyState : DiplomaticState
	{
		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06001932 RID: 6450
		[JsonIgnore]
		public abstract DiplomaticPendingValue DiplomaticPendingValue { get; }

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06001933 RID: 6451 RVA: 0x00059156 File Offset: 0x00057356
		[JsonIgnore]
		public override DiplomaticStateValue Type
		{
			get
			{
				return DiplomaticStateValue.PendingDiplomacy;
			}
		}

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06001934 RID: 6452 RVA: 0x00059159 File Offset: 0x00057359
		[JsonIgnore]
		public YesNo Response
		{
			get
			{
				return this._response;
			}
		}

		// Token: 0x06001935 RID: 6453 RVA: 0x00059161 File Offset: 0x00057361
		[JsonConstructor]
		public PendingDiplomacyState()
		{
		}

		// Token: 0x06001936 RID: 6454 RVA: 0x00059174 File Offset: 0x00057374
		public PendingDiplomacyState(int actorId)
		{
			this.ActorId = actorId;
		}

		// Token: 0x06001937 RID: 6455 RVA: 0x0005918E File Offset: 0x0005738E
		public void SetResponse(YesNo response)
		{
			this._response = response;
		}

		// Token: 0x06001938 RID: 6456 RVA: 0x00059198 File Offset: 0x00057398
		public override void Update(TurnProcessContext context, DiplomaticPairStatus relationship)
		{
			base.Update(context, relationship);
			PlayerState actor = context.CurrentTurn.FindPlayerState(this.ActorId, null);
			int playerId;
			if (!relationship.PlayerPair.GetOther(this.ActorId, out playerId))
			{
				return;
			}
			PlayerState target = context.CurrentTurn.FindPlayerState(playerId, null);
			this.Update(context, actor, target);
		}

		// Token: 0x06001939 RID: 6457 RVA: 0x000591F0 File Offset: 0x000573F0
		protected void DeepClonePendingDiplomacyParts(PendingDiplomacyState clone)
		{
			clone._response = this._response;
			clone.Wager = this.Wager;
			clone.ActorId = this.ActorId;
		}

		// Token: 0x0600193A RID: 6458 RVA: 0x00059216 File Offset: 0x00057416
		protected virtual void Update(TurnProcessContext context, PlayerState actor, PlayerState target)
		{
		}

		// Token: 0x04000B99 RID: 2969
		[JsonProperty]
		private YesNo _response;

		// Token: 0x04000B9A RID: 2970
		[JsonProperty]
		public int Wager;

		// Token: 0x04000B9B RID: 2971
		[JsonProperty]
		[DefaultValue(-2147483648)]
		public int ActorId = int.MinValue;
	}
}
