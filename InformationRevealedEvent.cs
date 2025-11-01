using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000281 RID: 641
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class InformationRevealedEvent : GameEvent
	{
		// Token: 0x06000C7D RID: 3197 RVA: 0x0003175C File Offset: 0x0002F95C
		[JsonConstructor]
		protected InformationRevealedEvent()
		{
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x0003176F File Offset: 0x0002F96F
		public InformationRevealedEvent(int triggeringPlayerId, int affectedPlayerId) : base(triggeringPlayerId)
		{
			base.AddAffectedPlayerId(affectedPlayerId);
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x0003178C File Offset: 0x0002F98C
		public override void DeepClone(out GameEvent clone)
		{
			InformationRevealedEvent informationRevealedEvent = new InformationRevealedEvent
			{
				Revealed = this.Revealed.DeepClone<InformationContext>()
			};
			base.DeepCloneGameEventParts<InformationRevealedEvent>(informationRevealedEvent);
			clone = informationRevealedEvent;
		}

		// Token: 0x04000576 RID: 1398
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public InformationContext Revealed = new InformationContext();
	}
}
