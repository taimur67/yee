using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020001DD RID: 477
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public abstract class GameEntityFilter<T> : GameEntityFilter where T : GameEntity
	{
		// Token: 0x06000969 RID: 2409 RVA: 0x0002C848 File Offset: 0x0002AA48
		public override bool Filter(TurnContext context, GameEntity entity)
		{
			T entity2;
			return this.TryResolve(context, entity, out entity2) && this.Filter(context, entity2);
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0002C86B File Offset: 0x0002AA6B
		protected virtual bool TryResolve(TurnContext context, GameEntity entity, out T typedEntity)
		{
			typedEntity = (entity as T);
			return typedEntity != null;
		}

		// Token: 0x0600096B RID: 2411
		protected abstract bool Filter(TurnContext context, T entity);
	}
}
