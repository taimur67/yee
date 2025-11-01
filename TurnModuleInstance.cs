using System;
using System.ComponentModel;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000535 RID: 1333
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public abstract class TurnModuleInstance : IDeepClone<TurnModuleInstance>
	{
		// Token: 0x060019DD RID: 6621 RVA: 0x0005A83D File Offset: 0x00058A3D
		[JsonConstructor]
		protected TurnModuleInstance()
		{
		}

		// Token: 0x060019DE RID: 6622 RVA: 0x0005A853 File Offset: 0x00058A53
		protected TurnModuleInstance(ConfigRef staticDataId)
		{
			this.StaticDataId = staticDataId;
		}

		// Token: 0x060019DF RID: 6623 RVA: 0x0005A870 File Offset: 0x00058A70
		protected void DeepCloneTurnModuleInstanceParts(TurnModuleInstance turnModuleInstance)
		{
			turnModuleInstance.Id = this.Id;
			turnModuleInstance.StaticDataId = this.StaticDataId.DeepClone();
			turnModuleInstance.CreatedTurn = this.CreatedTurn;
			turnModuleInstance.Lifetime = this.Lifetime;
		}

		// Token: 0x060019E0 RID: 6624
		public abstract void DeepClone(out TurnModuleInstance clone);

		// Token: 0x04000BCA RID: 3018
		public const int NoLifetime = -1;

		// Token: 0x04000BCB RID: 3019
		[JsonProperty]
		[DefaultValue(TurnModuleInstanceId.Invalid)]
		public TurnModuleInstanceId Id = TurnModuleInstanceId.Invalid;

		// Token: 0x04000BCC RID: 3020
		[JsonProperty]
		public ConfigRef StaticDataId;

		// Token: 0x04000BCD RID: 3021
		[JsonProperty]
		public int CreatedTurn;

		// Token: 0x04000BCE RID: 3022
		[JsonProperty]
		[DefaultValue(-1)]
		public int Lifetime = -1;
	}
}
