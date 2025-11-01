using System;

namespace LoG
{
	// Token: 0x02000744 RID: 1860
	public class SpawnLegionsInstance : EdictCandidateEffectModuleInstance
	{
		// Token: 0x060022F7 RID: 8951 RVA: 0x000794F4 File Offset: 0x000776F4
		public override void DeepClone(out TurnModuleInstance clone)
		{
			SpawnLegionsInstance spawnLegionsInstance = new SpawnLegionsInstance();
			base.DeepCloneEdictCandidateEffectModuleInstanceParts(spawnLegionsInstance);
			clone = spawnLegionsInstance;
		}
	}
}
