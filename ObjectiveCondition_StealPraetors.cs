using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005A8 RID: 1448
	public class ObjectiveCondition_StealPraetors : ObjectiveCondition_Steal
	{
		// Token: 0x06001B39 RID: 6969 RVA: 0x0005EB23 File Offset: 0x0005CD23
		[JsonConstructor]
		public ObjectiveCondition_StealPraetors() : base(GameItemCategory.Praetor)
		{
		}
	}
}
