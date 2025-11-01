using System;

namespace LoG
{
	// Token: 0x02000595 RID: 1429
	[Serializable]
	public class ObjectiveCondition_LevelUpLegion : ObjectiveCondition_EventFilter<LegionLevelUpEvent>
	{
		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06001B13 RID: 6931 RVA: 0x0005E68E File Offset: 0x0005C88E
		protected override bool CanSupportTargets
		{
			get
			{
				return false;
			}
		}
	}
}
