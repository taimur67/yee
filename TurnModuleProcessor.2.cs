using System;

namespace LoG
{
	// Token: 0x02000538 RID: 1336
	public class TurnModuleProcessor<TModuleInstance> : TurnModuleProcessor where TModuleInstance : TurnModuleInstance
	{
		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x060019EE RID: 6638 RVA: 0x0005AA98 File Offset: 0x00058C98
		protected TModuleInstance Instance
		{
			get
			{
				TModuleInstance result;
				if ((result = this._typedInstance) == null)
				{
					result = (this._typedInstance = (TModuleInstance)((object)this._instance));
				}
				return result;
			}
		}

		// Token: 0x060019EF RID: 6639 RVA: 0x0005AAC8 File Offset: 0x00058CC8
		public void RemoveSelf()
		{
			base._currentTurn.RemoveActiveTurnModule(this.TurnProcessContext, this.Instance);
		}

		// Token: 0x04000BD2 RID: 3026
		private TModuleInstance _typedInstance;
	}
}
