using System;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000360 RID: 864
	public class GameEffect_EnsureMinimuimArchfiendStatvalue : GameAbilityEffect
	{
		// Token: 0x0600106A RID: 4202 RVA: 0x0004080C File Offset: 0x0003EA0C
		public override void Process(TurnProcessContext context, PlayerState player)
		{
			ModifiableValue modifiableValue;
			if (player.TryGet(this.Stat, out modifiableValue) && modifiableValue.BaseValue < this.Value)
			{
				modifiableValue.SetBase((float)this.Value);
			}
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x00040844 File Offset: 0x0003EA44
		public override void DeepClone(out AbilityEffect clone)
		{
			clone = new GameEffect_EnsureMinimuimArchfiendStatvalue
			{
				Stat = this.Stat,
				Value = this.Value
			};
			base.DeepCloneAbilityEffectParts(clone);
		}

		// Token: 0x040007AF RID: 1967
		[JsonProperty]
		public ArchfiendStat Stat;

		// Token: 0x040007B0 RID: 1968
		[JsonProperty]
		public int Value;
	}
}
