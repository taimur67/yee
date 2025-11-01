using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000333 RID: 819
	public class CombatEffect_BlockStratagems : CombatAbilityEffect
	{
		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000FBF RID: 4031 RVA: 0x0003E615 File Offset: 0x0003C815
		[JsonIgnore]
		public override bool CanBeCancelled
		{
			get
			{
				return !this.ApplyToSelf;
			}
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x0003E620 File Offset: 0x0003C820
		protected override GameEvent OnBlockStratagems(Ability source, CombatAbilityContext context, BattleEvent battleEvent)
		{
			foreach (Identifier id in (this.ApplyToSelf ? context.Actor : context.Opponent).Slots)
			{
				Stratagem stratagem = context.Turn.FetchGameItem(id) as Stratagem;
				if (stratagem != null)
				{
					stratagem.Status = GameItemStatus.Banished;
				}
			}
			return null;
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x0003E6A0 File Offset: 0x0003C8A0
		public override void DeepClone(out AbilityEffect clone)
		{
			CombatEffect_BlockStratagems combatEffect_BlockStratagems = new CombatEffect_BlockStratagems
			{
				ApplyToSelf = this.ApplyToSelf
			};
			base.DeepCloneCombatAbilityEffectParts(combatEffect_BlockStratagems);
			clone = combatEffect_BlockStratagems;
		}

		// Token: 0x04000758 RID: 1880
		public bool ApplyToSelf;
	}
}
