using System;

namespace LoG
{
	// Token: 0x02000591 RID: 1425
	[Serializable]
	public class ObjectiveCondition_InstigateArmistice : ObjectiveCondition_EventFilter<SendEmissaryResponseEvent>
	{
		// Token: 0x06001B08 RID: 6920 RVA: 0x0005E552 File Offset: 0x0005C752
		protected override bool Filter(TurnContext context, SendEmissaryResponseEvent emissaryResponse, PlayerState owner, PlayerState target)
		{
			return emissaryResponse.Response == YesNo.Yes && owner.Id == emissaryResponse.TriggeringPlayerID && target.Id == emissaryResponse.AffectedPlayerID && base.Filter(context, emissaryResponse, owner, target);
		}
	}
}
