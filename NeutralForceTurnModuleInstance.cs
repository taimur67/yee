using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000402 RID: 1026
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class NeutralForceTurnModuleInstance : TurnModuleInstance
	{
		// Token: 0x06001480 RID: 5248 RVA: 0x0004E6C8 File Offset: 0x0004C8C8
		protected void DeepCloneNeutralForceTurnModuleInstanceParts(NeutralForceTurnModuleInstance neutralForceTurnModuleInstance)
		{
			neutralForceTurnModuleInstance.EventEffectId = this.EventEffectId.DeepClone();
			neutralForceTurnModuleInstance.GamePieceId = this.GamePieceId;
			neutralForceTurnModuleInstance.CurrentTarget = this.CurrentTarget;
			neutralForceTurnModuleInstance.MinTurnDuration = this.MinTurnDuration;
			neutralForceTurnModuleInstance.TurnDurationLimit = this.TurnDurationLimit;
			base.DeepCloneTurnModuleInstanceParts(neutralForceTurnModuleInstance);
		}

		// Token: 0x06001481 RID: 5249 RVA: 0x0004E720 File Offset: 0x0004C920
		public override void DeepClone(out TurnModuleInstance clone)
		{
			NeutralForceTurnModuleInstance neutralForceTurnModuleInstance = new NeutralForceTurnModuleInstance();
			this.DeepCloneNeutralForceTurnModuleInstanceParts(neutralForceTurnModuleInstance);
			clone = neutralForceTurnModuleInstance;
		}

		// Token: 0x0400090A RID: 2314
		[JsonProperty]
		public string EventEffectId;

		// Token: 0x0400090B RID: 2315
		[JsonProperty]
		public Identifier GamePieceId;

		// Token: 0x0400090C RID: 2316
		[JsonProperty]
		[DefaultValue(Identifier.Invalid)]
		public Identifier CurrentTarget = Identifier.Invalid;

		// Token: 0x0400090D RID: 2317
		[JsonProperty]
		public int MinTurnDuration;

		// Token: 0x0400090E RID: 2318
		[JsonProperty]
		public int TurnDurationLimit;
	}
}
