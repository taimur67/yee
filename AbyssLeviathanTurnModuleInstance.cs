using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200040A RID: 1034
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class AbyssLeviathanTurnModuleInstance : HostileForceTurnModuleInstance
	{
		// Token: 0x0600148D RID: 5261 RVA: 0x0004E7E8 File Offset: 0x0004C9E8
		public override void DeepClone(out TurnModuleInstance clone)
		{
			AbyssLeviathanTurnModuleInstance abyssLeviathanTurnModuleInstance = new AbyssLeviathanTurnModuleInstance();
			base.DeepCloneHostileForceTurnModuleInstanceParts(abyssLeviathanTurnModuleInstance);
			abyssLeviathanTurnModuleInstance.OrderedListOfPlayersToTarget = this.OrderedListOfPlayersToTarget.DeepClone();
			clone = abyssLeviathanTurnModuleInstance;
		}

		// Token: 0x04000918 RID: 2328
		[JsonProperty]
		public List<int> OrderedListOfPlayersToTarget = new List<int>();
	}
}
