using System;
using Core.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000473 RID: 1139
	public class StratagemStaticData : IdentifiableStaticData
	{
		// Token: 0x17000306 RID: 774
		// (get) Token: 0x0600153A RID: 5434 RVA: 0x00050277 File Offset: 0x0004E477
		public SlotType SlotType
		{
			get
			{
				return SlotType.Legion | SlotType.Fixture;
			}
		}
	}
}
