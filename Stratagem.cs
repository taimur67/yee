using System;
using System.Collections.Generic;
using System.Linq;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002E6 RID: 742
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class Stratagem : GameItem
	{
		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000E7E RID: 3710 RVA: 0x00039C26 File Offset: 0x00037E26
		public override GameItemCategory Category
		{
			get
			{
				return GameItemCategory.Stratagem;
			}
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x00039C2C File Offset: 0x00037E2C
		public override void ConfigureFrom(IdentifiableStaticData data)
		{
			base.ConfigureFrom(data);
			StratagemStaticData stratagemStaticData = data as StratagemStaticData;
			if (stratagemStaticData != null)
			{
				this.AttachableTo = stratagemStaticData.SlotType;
			}
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x00039C56 File Offset: 0x00037E56
		public void SetMaxSlots(int slots)
		{
			this.MaxSlots = slots;
			if (slots < this.Tactics.Count)
			{
				this.Tactics.RemoveRange(slots, this.Tactics.Count - slots);
			}
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x00039C86 File Offset: 0x00037E86
		public void AddTactics(params StratagemTacticLevelStaticData[] data)
		{
			this.AddTactics(data.AsEnumerable<StratagemTacticLevelStaticData>());
		}

		// Token: 0x06000E82 RID: 3714 RVA: 0x00039C94 File Offset: 0x00037E94
		public void AddTactics(IEnumerable<StratagemTacticLevelStaticData> tactics)
		{
			foreach (StratagemTacticLevelStaticData data in tactics)
			{
				this.AddTactic(data);
			}
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x00039CDC File Offset: 0x00037EDC
		public void AddTactic(StratagemTacticLevelStaticData data)
		{
			this.AddTactic(new StratagemTactic
			{
				Modifiers = data.Components,
				StaticDataId = data.Id
			});
		}

		// Token: 0x06000E84 RID: 3716 RVA: 0x00039D10 File Offset: 0x00037F10
		public void AddTactic(StratagemTactic tactic)
		{
			if (this.Tactics.Count >= this.MaxSlots)
			{
				return;
			}
			this.Tactics.Add(tactic);
			Ability ability = new Ability();
			ability.AddEffect(tactic);
			ability.SourceId = tactic.StaticDataId;
			ability.Name = tactic.StaticDataId;
			base.AddAbility(ability);
		}

		// Token: 0x06000E85 RID: 3717 RVA: 0x00039D6C File Offset: 0x00037F6C
		public bool RemoveTactic(string staticDataId)
		{
			StratagemTactic tactic = this.Tactics.FirstOrDefault((StratagemTactic t) => t.StaticDataId == staticDataId);
			return this.RemoveTactic(tactic);
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x00039DA8 File Offset: 0x00037FA8
		public bool RemoveTactic(StratagemTactic tactic)
		{
			if (tactic == null)
			{
				return false;
			}
			Ability ability = base.Abilities.FirstOrDefault((Ability t) => t.SourceId == tactic.StaticDataId);
			if (ability != null)
			{
				base.RemoveAbility(ability);
			}
			return this.Tactics.Remove(tactic);
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x00039E00 File Offset: 0x00038000
		public sealed override void DeepClone(out GameItem gameItem)
		{
			Stratagem stratagem = new Stratagem();
			base.DeepCloneGameItemParts(stratagem);
			stratagem.Tactics = this.Tactics.DeepClone<StratagemTactic>();
			stratagem.MaxSlots = this.MaxSlots;
			gameItem = stratagem;
		}

		// Token: 0x04000665 RID: 1637
		[JsonProperty]
		public List<StratagemTactic> Tactics = new List<StratagemTactic>();

		// Token: 0x04000666 RID: 1638
		[JsonProperty]
		public int MaxSlots;
	}
}
