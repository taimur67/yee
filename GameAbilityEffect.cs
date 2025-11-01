using System;

namespace LoG
{
	// Token: 0x0200035F RID: 863
	public abstract class GameAbilityEffect : AbilityEffect
	{
		// Token: 0x06001067 RID: 4199 RVA: 0x00040800 File Offset: 0x0003EA00
		public virtual void Process(TurnProcessContext context, PlayerState player)
		{
		}

		// Token: 0x06001068 RID: 4200
		public abstract override void DeepClone(out AbilityEffect clone);
	}
}
