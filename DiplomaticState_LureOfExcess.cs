using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000512 RID: 1298
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class DiplomaticState_LureOfExcess : ArmisticeState
	{
		// Token: 0x1700038B RID: 907
		// (get) Token: 0x0600191D RID: 6429 RVA: 0x0005909D File Offset: 0x0005729D
		[JsonIgnore]
		public override DiplomaticStateValue Type
		{
			get
			{
				return DiplomaticStateValue.LureOfExcess;
			}
		}

		// Token: 0x0600191E RID: 6430 RVA: 0x000590A1 File Offset: 0x000572A1
		public override bool AllowMovementIntoTerritory(DiplomaticTurnState diplomacy, int requestingPlayerId, int targetPlayer)
		{
			return requestingPlayerId == this.ActorId;
		}

		// Token: 0x0600191F RID: 6431 RVA: 0x000590AC File Offset: 0x000572AC
		public override bool AllowRedeployThroughFixtures(DiplomaticTurnState diplomacy, int requestingPlayerId, int targetPlayer)
		{
			return requestingPlayerId == this.ActorId;
		}

		// Token: 0x06001920 RID: 6432 RVA: 0x000590B7 File Offset: 0x000572B7
		public override bool AllowNearbyHealingProvidedBy(int providingPlayerId)
		{
			return providingPlayerId != this.ActorId;
		}

		// Token: 0x06001921 RID: 6433 RVA: 0x000590C5 File Offset: 0x000572C5
		[JsonConstructor]
		protected DiplomaticState_LureOfExcess()
		{
		}

		// Token: 0x06001922 RID: 6434 RVA: 0x000590D8 File Offset: 0x000572D8
		public DiplomaticState_LureOfExcess(int actorId, int length) : base(length)
		{
			this.ActorId = actorId;
		}

		// Token: 0x06001923 RID: 6435 RVA: 0x000590F3 File Offset: 0x000572F3
		protected DiplomaticState_LureOfExcess DeepCloneParts(DiplomaticState_LureOfExcess state)
		{
			base.DeepCloneParts(state);
			state.ActorId = this.ActorId;
			return state;
		}

		// Token: 0x06001924 RID: 6436 RVA: 0x0005910A File Offset: 0x0005730A
		public override void DeepClone(out DiplomaticState clone)
		{
			clone = this.DeepCloneParts(new DiplomaticState_LureOfExcess());
		}

		// Token: 0x04000B98 RID: 2968
		[JsonProperty]
		[DefaultValue(-2147483648)]
		public int ActorId = int.MinValue;
	}
}
