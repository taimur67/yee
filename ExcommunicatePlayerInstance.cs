using System;

namespace LoG
{
	// Token: 0x0200071A RID: 1818
	public class ExcommunicatePlayerInstance : EdictCandidateEffectModuleInstance
	{
		// Token: 0x060022A2 RID: 8866 RVA: 0x000787D0 File Offset: 0x000769D0
		public override void DeepClone(out TurnModuleInstance clone)
		{
			ExcommunicatePlayerInstance excommunicatePlayerInstance = new ExcommunicatePlayerInstance();
			base.DeepCloneEdictCandidateEffectModuleInstanceParts(excommunicatePlayerInstance);
			clone = excommunicatePlayerInstance;
		}
	}
}
