using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200024B RID: 587
	public abstract class ItemsAcquiredEvent : GameEvent
	{
		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000B7B RID: 2939
		[JsonIgnore]
		public abstract IEnumerable<Identifier> AcquiredItems { get; }

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000B7C RID: 2940 RVA: 0x0002FE92 File Offset: 0x0002E092
		[JsonIgnore]
		public int Owner
		{
			get
			{
				return this.TriggeringPlayerID;
			}
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x0002FE9A File Offset: 0x0002E09A
		protected ItemsAcquiredEvent()
		{
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x0002FEA2 File Offset: 0x0002E0A2
		protected ItemsAcquiredEvent(int owningPlayer) : base(owningPlayer)
		{
		}
	}
}
