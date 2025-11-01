using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000238 RID: 568
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class RedeployedEvent : TeleportedEvent
	{
		// Token: 0x06000B1B RID: 2843 RVA: 0x0002F579 File Offset: 0x0002D779
		[JsonConstructor]
		protected RedeployedEvent()
		{
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x0002F581 File Offset: 0x0002D781
		public RedeployedEvent(int playerId, Identifier legionId, HexCoord from, HexCoord to) : base(playerId, legionId, from, to)
		{
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x0002F590 File Offset: 0x0002D790
		public override void DeepClone(out GameEvent clone)
		{
			RedeployedEvent redeployedEvent = new RedeployedEvent
			{
				Redeployer = this.Redeployer
			};
			base.DeepCloneMoveStepEventParts(redeployedEvent);
			clone = redeployedEvent;
		}

		// Token: 0x0400050D RID: 1293
		[JsonProperty]
		public Identifier Redeployer;
	}
}
