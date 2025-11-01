using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200059A RID: 1434
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ObjectiveCondition_PlayGrandEvents : ObjectiveCondition_EventFilter<GrandEventPlayed>
	{
	}
}
