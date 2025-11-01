using System;

namespace LoG
{
	// Token: 0x0200070B RID: 1803
	public class DiscardAllEventsInstance : EdictCandidateEffectModuleInstance
	{
		// Token: 0x06002284 RID: 8836 RVA: 0x00078360 File Offset: 0x00076560
		public override void DeepClone(out TurnModuleInstance clone)
		{
			DiscardAllEventsInstance discardAllEventsInstance = new DiscardAllEventsInstance();
			base.DeepCloneEdictCandidateEffectModuleInstanceParts(discardAllEventsInstance);
			clone = discardAllEventsInstance;
		}
	}
}
