using System;

namespace LoG
{
	// Token: 0x0200048B RID: 1163
	public class ValueConflict<T> : ActionConflict<ValueConflict<T>> where T : IComparable<T>
	{
		// Token: 0x1700031A RID: 794
		// (get) Token: 0x060015CA RID: 5578 RVA: 0x00051CEA File Offset: 0x0004FEEA
		// (set) Token: 0x060015CB RID: 5579 RVA: 0x00051CF2 File Offset: 0x0004FEF2
		public T Value { get; protected set; }

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x060015CC RID: 5580 RVA: 0x00051CFB File Offset: 0x0004FEFB
		// (set) Token: 0x060015CD RID: 5581 RVA: 0x00051D03 File Offset: 0x0004FF03
		public ConflictScopeFlags Scope { get; protected set; }

		// Token: 0x060015CE RID: 5582 RVA: 0x00051D0C File Offset: 0x0004FF0C
		public ValueConflict()
		{
		}

		// Token: 0x060015CF RID: 5583 RVA: 0x00051D14 File Offset: 0x0004FF14
		public ValueConflict(ConflictScopeFlags flags, T value)
		{
			this.Scope = flags;
			this.Value = value;
		}

		// Token: 0x060015D0 RID: 5584 RVA: 0x00051D2C File Offset: 0x0004FF2C
		protected override bool ConflictsWith(ValueConflict<T> other)
		{
			if ((this.Scope & other.Scope) != (ConflictScopeFlags)0)
			{
				T value = this.Value;
				return value.CompareTo(other.Value) == 0;
			}
			return false;
		}
	}
}
