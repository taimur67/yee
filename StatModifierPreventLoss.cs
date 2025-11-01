using System;

namespace LoG
{
	// Token: 0x02000398 RID: 920
	[Serializable]
	public class StatModifierPreventLoss : StatModifierBase
	{
		// Token: 0x17000293 RID: 659
		// (get) Token: 0x0600119F RID: 4511 RVA: 0x00043B7E File Offset: 0x00041D7E
		public override int Priority
		{
			get
			{
				return -50;
			}
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x00043B82 File Offset: 0x00041D82
		public override void PreModification(ModifiableValue value)
		{
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x00043B84 File Offset: 0x00041D84
		public override void DeepClone(out StatModifierBase clone)
		{
			clone = new StatModifierPreventLoss();
			base.DeepCloneModifierBaseParts(clone);
		}
	}
}
