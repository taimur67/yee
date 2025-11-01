using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000397 RID: 919
	[Serializable]
	public abstract class StatModifierBase : IDeepClone<StatModifierBase>
	{
		// Token: 0x06001199 RID: 4505 RVA: 0x00043B5C File Offset: 0x00041D5C
		[JsonConstructor]
		public StatModifierBase()
		{
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x0600119A RID: 4506 RVA: 0x00043B64 File Offset: 0x00041D64
		public virtual int Priority
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x00043B67 File Offset: 0x00041D67
		public virtual void PreModification(ModifiableValue value)
		{
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x00043B69 File Offset: 0x00041D69
		public virtual void PostModification(ModifiableValue value, float workingValue)
		{
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x00043B6B File Offset: 0x00041D6B
		protected void DeepCloneModifierBaseParts(StatModifierBase clone)
		{
			clone.Provider = this.Provider.DeepClone<ModifierContext>();
		}

		// Token: 0x0600119E RID: 4510
		public abstract void DeepClone(out StatModifierBase clone);

		// Token: 0x04000809 RID: 2057
		public ModifierContext Provider;
	}
}
