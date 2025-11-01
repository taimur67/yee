using System;
using System.Collections.Generic;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000425 RID: 1061
	[Serializable]
	public class BattleRulesStaticData : IdentifiableStaticData
	{
		// Token: 0x170002FE RID: 766
		// (get) Token: 0x060014BD RID: 5309 RVA: 0x0004F39F File Offset: 0x0004D59F
		[JsonIgnore]
		public IReadOnlyList<BattlePhase> DefaultPhaseOrder
		{
			get
			{
				return this._defaultPhaseOrder;
			}
		}

		// Token: 0x040009E7 RID: 2535
		[JsonProperty]
		public List<BattlePhase> _defaultPhaseOrder;
	}
}
