using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020001CD RID: 461
	[Serializable]
	public class BattlePhaseResult : IModifiable, IDeepClone<BattlePhaseResult>
	{
		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x0600089A RID: 2202 RVA: 0x0002991E File Offset: 0x00027B1E
		[JsonIgnore]
		public int StatDifference
		{
			get
			{
				return Math.Abs(this.AttackerPower - this.DefenderPower);
			}
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x0002993C File Offset: 0x00027B3C
		[JsonConstructor]
		public BattlePhaseResult()
		{
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x000299AC File Offset: 0x00027BAC
		public BattlePhaseResult(BattlePhase phase, int damage, Identifier winningLegionId, Identifier losingLegionId)
		{
			this.BattlePhase = phase;
			this.HPDamage = damage;
			this.WinningLegionId = winningLegionId;
			this.LosingLegionId = losingLegionId;
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x00029A3C File Offset: 0x00027C3C
		public void ClearModifiers()
		{
			this.ClearStatModifiers();
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x00029A44 File Offset: 0x00027C44
		public void DeepClone(out BattlePhaseResult clone)
		{
			clone = new BattlePhaseResult
			{
				Unreached = this.Unreached,
				BattlePhase = this.BattlePhase,
				HPDamage = this.HPDamage.DeepClone<ModifiableValue>(),
				CounterDamage = this.CounterDamage.DeepClone<ModifiableValue>(),
				AttackerPower = this.AttackerPower.DeepClone<ModifiableValue>(),
				DefenderPower = this.DefenderPower.DeepClone<ModifiableValue>(),
				WinningLegionId = this.WinningLegionId,
				LosingLegionId = this.LosingLegionId,
				AttackerItems = this.AttackerItems.DeepClone(),
				DefenderItems = this.DefenderItems.DeepClone(),
				PermanentDamage = this.PermanentDamage,
				Fatal = this.Fatal
			};
		}

		// Token: 0x04000436 RID: 1078
		[JsonProperty]
		public bool Unreached;

		// Token: 0x04000437 RID: 1079
		[JsonProperty]
		[DefaultValue(BattlePhase.Undefined)]
		public BattlePhase BattlePhase = BattlePhase.Undefined;

		// Token: 0x04000438 RID: 1080
		[JsonProperty]
		[DefaultValue(0)]
		public ModifiableValue HPDamage = 0;

		// Token: 0x04000439 RID: 1081
		[JsonProperty]
		[DefaultValue(0)]
		public ModifiableValue CounterDamage = 0;

		// Token: 0x0400043A RID: 1082
		[JsonProperty]
		[DefaultValue(0)]
		public ModifiableValue AttackerPower = 0;

		// Token: 0x0400043B RID: 1083
		[JsonProperty]
		[DefaultValue(0)]
		public ModifiableValue DefenderPower = 0;

		// Token: 0x0400043C RID: 1084
		[JsonProperty]
		[DefaultValue(Identifier.Invalid)]
		public Identifier WinningLegionId = Identifier.Invalid;

		// Token: 0x0400043D RID: 1085
		[JsonProperty]
		[DefaultValue(Identifier.Invalid)]
		public Identifier LosingLegionId = Identifier.Invalid;

		// Token: 0x0400043E RID: 1086
		[JsonProperty]
		public List<Identifier> AttackerItems = new List<Identifier>();

		// Token: 0x0400043F RID: 1087
		[JsonProperty]
		public List<Identifier> DefenderItems = new List<Identifier>();

		// Token: 0x04000440 RID: 1088
		[JsonProperty]
		public bool PermanentDamage;

		// Token: 0x04000441 RID: 1089
		[JsonProperty]
		public bool Fatal;
	}
}
