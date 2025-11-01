using System;

namespace LoG
{
	// Token: 0x02000498 RID: 1176
	public class AbilityLockedProblem : Problem
	{
		// Token: 0x1700031C RID: 796
		// (get) Token: 0x060015F4 RID: 5620 RVA: 0x00052027 File Offset: 0x00050227
		public override string DebugString
		{
			get
			{
				return "Ability Locked";
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x060015F5 RID: 5621 RVA: 0x0005202E File Offset: 0x0005022E
		public override string LocKey
		{
			get
			{
				return this.LocKeyScope + ".AbilityLocked";
			}
		}

		// Token: 0x04000B06 RID: 2822
		public string AbilityId;
	}
}
