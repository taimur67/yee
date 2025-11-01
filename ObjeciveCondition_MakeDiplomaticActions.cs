using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000540 RID: 1344
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ObjeciveCondition_MakeDiplomaticActions : ObjectiveCondition_EventFilter<TargetedByDiplomaticActionEvent>
	{
	}
}
