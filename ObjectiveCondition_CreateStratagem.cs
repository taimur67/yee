using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200056B RID: 1387
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ObjectiveCondition_CreateStratagem : ObjectiveCondition_EventFilter<StratagemCreatedEvent>
	{
	}
}
