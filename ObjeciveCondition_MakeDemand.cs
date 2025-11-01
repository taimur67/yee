using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200053F RID: 1343
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ObjeciveCondition_MakeDemand : ObjectiveCondition_EventFilter<MakeDemandEvent>
	{
	}
}
