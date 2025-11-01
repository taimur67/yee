using System;

namespace LoG
{
	// Token: 0x020000FE RID: 254
	public class ActionHellsMaw : ActionCastRitual<HellsMawRitualOrder>
	{
		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000440 RID: 1088 RVA: 0x00012C06 File Offset: 0x00010E06
		public override ActionID ID
		{
			get
			{
				return ActionID.Cast_Hells_Maw_Dark_Art;
			}
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00012C0A File Offset: 0x00010E0A
		protected override string GetRitualId()
		{
			return "beelzebub_hells_maw";
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00012C11 File Offset: 0x00010E11
		public static bool CanBeUsedByArchfiend(PlayerState wouldBeCaster)
		{
			return wouldBeCaster.ArchfiendId == "Beelzebub";
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00012C23 File Offset: 0x00010E23
		protected override PowerType GetPowerType()
		{
			return PowerType.Wrath;
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000444 RID: 1092 RVA: 0x00012C26 File Offset: 0x00010E26
		public override ActionOrderPriority Priority
		{
			get
			{
				return ActionOrderPriority.High;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000445 RID: 1093 RVA: 0x00012C29 File Offset: 0x00010E29
		protected override int CooldownDuration
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x00012C2C File Offset: 0x00010E2C
		public override void Prepare()
		{
			base.AddConstraint(new WPHellsMaw
			{
				InvertLogic = true
			});
			base.AddConstraint(new WPCanAttack());
			base.AddEffect(new WPTributeBoost());
			base.AddEffect(new WPHellsMaw());
			base.AddScalarCostModifier(-1f, PFCostModifier.Archfiend_Bonus);
			base.Prepare();
		}

		// Token: 0x04000244 RID: 580
		public const string RitualId = "beelzebub_hells_maw";

		// Token: 0x04000245 RID: 581
		private const string ArchfiendId = "Beelzebub";
	}
}
