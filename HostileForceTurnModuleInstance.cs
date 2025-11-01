using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000403 RID: 1027
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class HostileForceTurnModuleInstance : NeutralForceTurnModuleInstance
	{
		// Token: 0x06001483 RID: 5251 RVA: 0x0004E74C File Offset: 0x0004C94C
		protected void DeepCloneHostileForceTurnModuleInstanceParts(HostileForceTurnModuleInstance hostileForceTurnModuleInstance)
		{
			hostileForceTurnModuleInstance.OrderedPlayerIdsToTarget = this.OrderedPlayerIdsToTarget.DeepClone();
			base.DeepCloneNeutralForceTurnModuleInstanceParts(hostileForceTurnModuleInstance);
		}

		// Token: 0x06001484 RID: 5252 RVA: 0x0004E768 File Offset: 0x0004C968
		public override void DeepClone(out TurnModuleInstance clone)
		{
			HostileForceTurnModuleInstance hostileForceTurnModuleInstance = new HostileForceTurnModuleInstance();
			this.DeepCloneHostileForceTurnModuleInstanceParts(hostileForceTurnModuleInstance);
			clone = hostileForceTurnModuleInstance;
		}

		// Token: 0x0400090F RID: 2319
		[JsonProperty]
		public List<int> OrderedPlayerIdsToTarget = new List<int>();
	}
}
