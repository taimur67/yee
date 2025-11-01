using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000523 RID: 1315
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class VassalisedState : DiplomaticState
	{
		// Token: 0x06001987 RID: 6535 RVA: 0x00059EB1 File Offset: 0x000580B1
		[JsonConstructor]
		public VassalisedState()
		{
		}

		// Token: 0x06001988 RID: 6536 RVA: 0x00059EB9 File Offset: 0x000580B9
		public VassalisedState(int vassalId, int bloodLordId)
		{
			this._bloodLordId = bloodLordId;
			this._vassalId = vassalId;
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06001989 RID: 6537 RVA: 0x00059ECF File Offset: 0x000580CF
		[JsonIgnore]
		public int BloodLordId
		{
			get
			{
				return this._bloodLordId;
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x0600198A RID: 6538 RVA: 0x00059ED7 File Offset: 0x000580D7
		[JsonIgnore]
		public int VassalId
		{
			get
			{
				return this._vassalId;
			}
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x0600198B RID: 6539 RVA: 0x00059EDF File Offset: 0x000580DF
		public override DiplomaticStateValue Type
		{
			get
			{
				return DiplomaticStateValue.Vassalised;
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x0600198C RID: 6540 RVA: 0x00059EE3 File Offset: 0x000580E3
		public override bool AllowAnyDiplomacy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600198D RID: 6541 RVA: 0x00059EE6 File Offset: 0x000580E6
		public override bool AllowMovementIntoTerritory(DiplomaticTurnState diplomacy, int requestingPlayer, int targetPlayer)
		{
			return this.GetRelevantBloodLordState(diplomacy, requestingPlayer, targetPlayer).AllowMovementIntoTerritory(diplomacy, this._bloodLordId, targetPlayer);
		}

		// Token: 0x0600198E RID: 6542 RVA: 0x00059EFE File Offset: 0x000580FE
		public override bool AllowCombat(DiplomaticTurnState diplomacy, int requestingPlayer, int targetPlayer)
		{
			return this.GetRelevantBloodLordState(diplomacy, requestingPlayer, targetPlayer).AllowCombat(diplomacy, this._bloodLordId, targetPlayer);
		}

		// Token: 0x0600198F RID: 6543 RVA: 0x00059F16 File Offset: 0x00058116
		public override bool AllowStrongholdCapture(DiplomaticTurnState diplomacy, int requestingPlayer, int targetPlayer)
		{
			return this.GetRelevantBloodLordState(diplomacy, requestingPlayer, targetPlayer).AllowStrongholdCapture(diplomacy, this._bloodLordId, targetPlayer);
		}

		// Token: 0x06001990 RID: 6544 RVA: 0x00059F2E File Offset: 0x0005812E
		public override CantonCaptureRule GetCantonCaptureRules(DiplomaticTurnState diplomacy, int requestingPlayer, int targetPlayer)
		{
			return this.GetRelevantBloodLordState(diplomacy, requestingPlayer, targetPlayer).GetCantonCaptureRules(diplomacy, this._bloodLordId, targetPlayer);
		}

		// Token: 0x06001991 RID: 6545 RVA: 0x00059F46 File Offset: 0x00058146
		public override CantonCaptureRestrictions GetCantonCaptureRestrictions(DiplomaticTurnState diplomacy, int requestingPlayer, int targetPlayer)
		{
			return this.GetRelevantBloodLordState(diplomacy, requestingPlayer, targetPlayer).GetCantonCaptureRestrictions(diplomacy, this._bloodLordId, targetPlayer);
		}

		// Token: 0x06001992 RID: 6546 RVA: 0x00059F60 File Offset: 0x00058160
		public DiplomaticState GetRelevantBloodLordState(DiplomaticTurnState diplomacy, int requestingPlayer, int targetPlayer)
		{
			int secondPlayerId = (requestingPlayer == this._vassalId) ? targetPlayer : requestingPlayer;
			return diplomacy.GetDiplomaticStatus(this._bloodLordId, secondPlayerId).DiplomaticState;
		}

		// Token: 0x06001993 RID: 6547 RVA: 0x00059F8D File Offset: 0x0005818D
		public override void OnStateEntered(TurnProcessContext context, DiplomaticPairStatus pair, DiplomaticState prevState, bool interrupt)
		{
			base.HandleDiplomacyCancellation(context, pair, prevState);
		}

		// Token: 0x06001994 RID: 6548 RVA: 0x00059F98 File Offset: 0x00058198
		public override void DeepClone(out DiplomaticState clone)
		{
			clone = new VassalisedState
			{
				_bloodLordId = this._bloodLordId,
				_vassalId = this._vassalId
			};
		}

		// Token: 0x04000BAB RID: 2987
		[JsonProperty]
		private int _bloodLordId;

		// Token: 0x04000BAC RID: 2988
		[JsonProperty]
		private int _vassalId;
	}
}
