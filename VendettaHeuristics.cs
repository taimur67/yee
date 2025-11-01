using System;

namespace LoG
{
	// Token: 0x020000DA RID: 218
	public static class VendettaHeuristics
	{
		// Token: 0x060002F5 RID: 757 RVA: 0x0000C61F File Offset: 0x0000A81F
		public static VendettaHeuristics.ObjectiveType FromVendettaObjective(VendettaObjective objective)
		{
			if (objective is CaptureHexesVendettaObjective)
			{
				return VendettaHeuristics.ObjectiveType.CaptureCantons;
			}
			if (objective is CapturePoPsVendettaObjective)
			{
				return VendettaHeuristics.ObjectiveType.CapturePoPs;
			}
			return VendettaHeuristics.ObjectiveType.DestroyLegions;
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000C638 File Offset: 0x0000A838
		public static VendettaHeuristics.VendettaParameters FromVendettaContext(VendettaContext context)
		{
			return new VendettaHeuristics.VendettaParameters
			{
				ObjectiveType = VendettaHeuristics.FromVendettaObjective(context.Objective),
				ThresholdToReach = context.GetTotalTarget(),
				TurnLimit = context.TurnTotal,
				TypeRisk = -1f,
				PrestigeWager = -1
			};
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000C690 File Offset: 0x0000A890
		public static string GetIdentifier(this VendettaHeuristics.ObjectiveType type)
		{
			string result;
			switch (type)
			{
			case VendettaHeuristics.ObjectiveType.CaptureCantons:
				result = "Vendetta_CaptureCanton";
				break;
			case VendettaHeuristics.ObjectiveType.DestroyLegions:
				result = "Vendetta_DestroyLegion";
				break;
			case VendettaHeuristics.ObjectiveType.CapturePoPs:
				result = "Vendetta_CapturePoP";
				break;
			default:
				result = null;
				break;
			}
			return result;
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000C6CE File Offset: 0x0000A8CE
		public static float GetOverallRisk(int instigatorId, int defenderId, VendettaContext context)
		{
			return VendettaHeuristics.GetOverallRisk(instigatorId, defenderId, VendettaHeuristics.FromVendettaContext(context));
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000C6E0 File Offset: 0x0000A8E0
		public static float GetOverallRisk(int instigatorId, int defenderId, VendettaHeuristics.VendettaParameters parameters)
		{
			float typeRisk = parameters.TypeRisk;
			float amount = (parameters.ThresholdDifficulty + parameters.DurationDifficulty) / 2f;
			float result = 0f;
			ref result.LerpTo01(typeRisk);
			ref result.LerpTo01(amount);
			return result;
		}

		// Token: 0x020007BE RID: 1982
		public enum ObjectiveType
		{
			// Token: 0x040010D1 RID: 4305
			CaptureCantons = 1,
			// Token: 0x040010D2 RID: 4306
			DestroyLegions,
			// Token: 0x040010D3 RID: 4307
			CapturePoPs
		}

		// Token: 0x020007BF RID: 1983
		public struct VendettaParameters
		{
			// Token: 0x170004AA RID: 1194
			// (get) Token: 0x06002517 RID: 9495 RVA: 0x000803C4 File Offset: 0x0007E5C4
			public int ThresholdRank
			{
				get
				{
					int result;
					switch (this.ObjectiveType)
					{
					case VendettaHeuristics.ObjectiveType.CaptureCantons:
						result = this.ThresholdToReach;
						break;
					case VendettaHeuristics.ObjectiveType.DestroyLegions:
						result = 2 * this.ThresholdToReach;
						break;
					case VendettaHeuristics.ObjectiveType.CapturePoPs:
						result = 2 * this.ThresholdToReach;
						break;
					default:
						result = this.ThresholdToReach;
						break;
					}
					return result;
				}
			}

			// Token: 0x170004AB RID: 1195
			// (get) Token: 0x06002518 RID: 9496 RVA: 0x00080415 File Offset: 0x0007E615
			public float ThresholdDifficulty
			{
				get
				{
					return 1f - 1f / MathF.Pow((float)this.ThresholdRank + 0.5f, 0.5f);
				}
			}

			// Token: 0x170004AC RID: 1196
			// (get) Token: 0x06002519 RID: 9497 RVA: 0x0008043A File Offset: 0x0007E63A
			public float DurationDifficulty
			{
				get
				{
					return MathF.Max(0f, 1f / MathF.Pow((float)this.TurnLimit, 0.5f) - 0.04f * (float)this.TurnLimit);
				}
			}

			// Token: 0x0600251A RID: 9498 RVA: 0x0008046B File Offset: 0x0007E66B
			public VendettaParameters(VendettaHeuristics.ObjectiveType objectiveType, int turnLimit = -1, int thresholdToReach = -1, float typeRisk = -1f, float overallRisk = -1f, int prestigeWager = -1)
			{
				this.ObjectiveType = objectiveType;
				this.TurnLimit = turnLimit;
				this.ThresholdToReach = thresholdToReach;
				this.TypeRisk = typeRisk;
				this.OverallRisk = overallRisk;
				this.PrestigeWager = prestigeWager;
			}

			// Token: 0x040010D4 RID: 4308
			public int TurnLimit;

			// Token: 0x040010D5 RID: 4309
			public int ThresholdToReach;

			// Token: 0x040010D6 RID: 4310
			public VendettaHeuristics.ObjectiveType ObjectiveType;

			// Token: 0x040010D7 RID: 4311
			public float TypeRisk;

			// Token: 0x040010D8 RID: 4312
			public float OverallRisk;

			// Token: 0x040010D9 RID: 4313
			public int PrestigeWager;
		}
	}
}
