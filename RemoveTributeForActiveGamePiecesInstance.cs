using System;

namespace LoG
{
	// Token: 0x0200073B RID: 1851
	public class RemoveTributeForActiveGamePiecesInstance : EdictEffectModuleInstance
	{
		// Token: 0x060022E4 RID: 8932 RVA: 0x000792C4 File Offset: 0x000774C4
		public override void DeepClone(out TurnModuleInstance clone)
		{
			RemoveTributeForActiveGamePiecesInstance removeTributeForActiveGamePiecesInstance = new RemoveTributeForActiveGamePiecesInstance();
			base.DeepCloneEdictEffectModuleInstanceParts(removeTributeForActiveGamePiecesInstance);
			clone = removeTributeForActiveGamePiecesInstance;
		}
	}
}
