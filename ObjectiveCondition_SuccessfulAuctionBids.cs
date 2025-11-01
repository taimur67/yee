using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005AA RID: 1450
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ObjectiveCondition_SuccessfulAuctionBids : ObjectiveCondition_EventFilter<BazaarBidEvent>
	{
	}
}
