using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x02000356 RID: 854
	public class UniqueAbilityComparer : IEqualityComparer<Ability>
	{
		// Token: 0x06001047 RID: 4167 RVA: 0x00040400 File Offset: 0x0003E600
		public bool Equals(Ability x, Ability y)
		{
			return x.SourceId == y.SourceId && x.Unique && y.Unique;
		}

		// Token: 0x06001048 RID: 4168 RVA: 0x00040425 File Offset: 0x0003E625
		public int GetHashCode(Ability ability)
		{
			return ability.SourceId.GetHashCode() ^ ability.SourceId.GetHashCode();
		}
	}
}
