using System;
using System.Collections.Generic;
using Core.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000429 RID: 1065
	public interface IUnlockProvider
	{
		// Token: 0x170002FF RID: 767
		// (get) Token: 0x060014C0 RID: 5312
		IEnumerable<ConfigRef<AbilityStaticData>> Unlocks { get; }
	}
}
