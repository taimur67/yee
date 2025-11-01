using System;
using System.Text;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003C9 RID: 969
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public abstract class PraetorCombatMoveEffectData : IStaticData
	{
		// Token: 0x170002CD RID: 717
		// (get) Token: 0x060012F9 RID: 4857 RVA: 0x00048514 File Offset: 0x00046714
		[JsonIgnore]
		public string TypeName
		{
			get
			{
				return StringExtensions.RemoveStart(base.GetType().Name, "PraetorCombatMoveEffectData_");
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x060012FA RID: 4858 RVA: 0x0004852B File Offset: 0x0004672B
		[JsonIgnore]
		public virtual string DebugDescription
		{
			get
			{
				return this.TypeName + " " + this.RestrictionsToString();
			}
		}

		// Token: 0x060012FB RID: 4859 RVA: 0x00048544 File Offset: 0x00046744
		public virtual string RestrictionsToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.RestrictionsToString(stringBuilder);
			string text = stringBuilder.ToString();
			if (text.Length > 0)
			{
				text = "\n" + text;
			}
			return text;
		}

		// Token: 0x060012FC RID: 4860 RVA: 0x0004857B File Offset: 0x0004677B
		protected virtual void RestrictionsToString(StringBuilder builder)
		{
			if (!this.StyleRestriction.IsEmpty())
			{
				builder.AppendLine("Against: " + this.StyleRestriction.Id);
			}
		}

		// Token: 0x060012FD RID: 4861 RVA: 0x000485A6 File Offset: 0x000467A6
		public virtual bool PreDamage(GameEvent ev, TurnProcessContext context, DuelProcessContext duel, DuelParticipantInstance source)
		{
			return false;
		}

		// Token: 0x060012FE RID: 4862 RVA: 0x000485A9 File Offset: 0x000467A9
		public virtual bool PostDamage(GameEvent ev, TurnProcessContext context, DuelProcessContext duel, DuelParticipantInstance source)
		{
			return false;
		}

		// Token: 0x060012FF RID: 4863 RVA: 0x000485AC File Offset: 0x000467AC
		public virtual bool ModifyPower(TurnProcessContext context, DuelProcessContext duel, DuelParticipantInstance source, ref int workingPower)
		{
			return false;
		}

		// Token: 0x06001300 RID: 4864 RVA: 0x000485B0 File Offset: 0x000467B0
		public virtual bool CheckRestrictions(DuelProcessContext duel, DuelParticipantInstance source)
		{
			if (!this.StyleRestriction.IsEmpty())
			{
				DuelParticipantInstance duelParticipantInstance;
				if (!duel.TryGetOther(source, out duelParticipantInstance))
				{
					return false;
				}
				ConfigRef styleRestriction = this.StyleRestriction;
				PraetorCombatMoveStaticData combatMoveData = duelParticipantInstance.CombatMoveData;
				if (!styleRestriction.Equals((combatMoveData != null) ? combatMoveData.TechniqueType : null))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040008CE RID: 2254
		[JsonProperty]
		public ConfigRef<PraetorCombatMoveStyle> StyleRestriction;
	}
}
