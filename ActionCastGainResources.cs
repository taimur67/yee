using System;

namespace LoG
{
	// Token: 0x020000F4 RID: 244
	public class ActionCastGainResources : ActionCastRitual<GainResourcesRitualOrder>
	{
		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060003DE RID: 990 RVA: 0x000117D6 File Offset: 0x0000F9D6
		public override ActionID ID
		{
			get
			{
				return ActionID.Cast_Produce_Prestige;
			}
		}

		// Token: 0x060003DF RID: 991 RVA: 0x000117DA File Offset: 0x0000F9DA
		protected override string GetRitualId()
		{
			return "burnt_offerings";
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x000117E1 File Offset: 0x0000F9E1
		protected override PowerType GetPowerType()
		{
			return PowerType.Charisma;
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x000117E4 File Offset: 0x0000F9E4
		public override ActionOrderPriority Priority
		{
			get
			{
				return ActionOrderPriority.Low_AlwaysLast;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060003E2 RID: 994 RVA: 0x000117E7 File Offset: 0x0000F9E7
		protected override int CooldownDuration
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x000117EA File Offset: 0x0000F9EA
		public override void Prepare()
		{
			base.AddEffect(WPTribute.PrestigeOnly(10));
			base.AddEffect(new WPPrestigeProduction(5));
			base.AddScalarCostReduction(1f, PFCostModifier.Heuristic_Bonus);
			base.Prepare();
		}

		// Token: 0x0400022C RID: 556
		public const string RitualId = "burnt_offerings";
	}
}
