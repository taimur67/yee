using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005A6 RID: 1446
	public class ObjectiveCondition_StealArtifacts : ObjectiveCondition_Steal
	{
		// Token: 0x06001B32 RID: 6962 RVA: 0x0005EA40 File Offset: 0x0005CC40
		[JsonConstructor]
		public ObjectiveCondition_StealArtifacts() : base(GameItemCategory.Artifact)
		{
		}
	}
}
