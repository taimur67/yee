using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000441 RID: 1089
	public class ModifyArchfiendTurnModuleInstance : TurnModuleInstance
	{
		// Token: 0x060014E3 RID: 5347 RVA: 0x0004F70C File Offset: 0x0004D90C
		public override void DeepClone(out TurnModuleInstance clone)
		{
			ModifyArchfiendTurnModuleInstance modifyArchfiendTurnModuleInstance = new ModifyArchfiendTurnModuleInstance
			{
				TriggeringPlayerId = this.TriggeringPlayerId,
				AffectedPlayerIds = this.AffectedPlayerIds.DeepClone<int>(),
				Modifier = this.Modifier.DeepClone(CloneFunction.FastClone),
				EventEffectId = this.EventEffectId.DeepClone(),
				GlobalModifierId = this.GlobalModifierId
			};
			base.DeepCloneTurnModuleInstanceParts(modifyArchfiendTurnModuleInstance);
			clone = modifyArchfiendTurnModuleInstance;
		}

		// Token: 0x04000A3D RID: 2621
		[JsonProperty]
		public int TriggeringPlayerId;

		// Token: 0x04000A3E RID: 2622
		[JsonProperty]
		public int[] AffectedPlayerIds;

		// Token: 0x04000A3F RID: 2623
		[JsonProperty]
		public IModifier Modifier;

		// Token: 0x04000A40 RID: 2624
		[JsonProperty]
		public string EventEffectId;

		// Token: 0x04000A41 RID: 2625
		[JsonProperty]
		public Guid GlobalModifierId;
	}
}
