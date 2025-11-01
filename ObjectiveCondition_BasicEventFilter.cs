using System;
using System.Linq;

namespace LoG
{
	// Token: 0x02000584 RID: 1412
	[Serializable]
	public abstract class ObjectiveCondition_BasicEventFilter<T> : IncrementingObjectiveCondition where T : GameEvent
	{
		// Token: 0x06001AE0 RID: 6880 RVA: 0x0005DCEC File Offset: 0x0005BEEC
		protected override int CalculateProgressIncrement(TurnContext context, PlayerState owner)
		{
			return context.CurrentTurn.GameEvents.OfType<T>().Count((T t) => this.Filter(context, owner, t));
		}

		// Token: 0x06001AE1 RID: 6881
		protected abstract bool Filter(TurnContext context, PlayerState owner, T @event);
	}
}
