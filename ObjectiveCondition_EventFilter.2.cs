using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using LoG.Simulation.Extensions;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000585 RID: 1413
	[Serializable]
	public abstract class ObjectiveCondition_EventFilter<T> : ObjectiveCondition_EventFilter where T : GameEvent
	{
		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06001AE3 RID: 6883 RVA: 0x0005DD42 File Offset: 0x0005BF42
		// (set) Token: 0x06001AE4 RID: 6884 RVA: 0x0005DD4A File Offset: 0x0005BF4A
		[JsonProperty]
		public bool AllowVassalContributions { get; set; }

		// Token: 0x06001AE5 RID: 6885 RVA: 0x0005DD53 File Offset: 0x0005BF53
		protected sealed override int CalculateProgressIncrement(TurnContext context, PlayerState owner, IEnumerable<GameEvent> events)
		{
			return this.CalculateProgressIncrement(context, owner, events.OfType<T>());
		}

		// Token: 0x06001AE6 RID: 6886 RVA: 0x0005DD64 File Offset: 0x0005BF64
		protected virtual int CalculateProgressIncrement(TurnContext context, PlayerState owner, IEnumerable<T> events)
		{
			TurnState currentTurn = context.CurrentTurn;
			int num = 0;
			foreach (T t in events)
			{
				PlayerState owner2 = owner;
				PlayerState target;
				base.GetActors(currentTurn, t, ref owner2, out target);
				int num2;
				if (this.Filter(context, t, owner2, target, out num2))
				{
					num += num2;
					this.OnCommitted(owner2, target, t);
				}
			}
			return num;
		}

		// Token: 0x06001AE7 RID: 6887 RVA: 0x0005DDE4 File Offset: 0x0005BFE4
		protected virtual bool Filter(TurnContext context, T @event, PlayerState owner, [MaybeNull] PlayerState target)
		{
			if (owner != null && @event.TriggeringPlayerID != owner.Id)
			{
				if (!this.AllowVassalContributions)
				{
					return false;
				}
				int num;
				if (!context.Diplomacy.IsBloodLordOfAny(owner.Id, out num))
				{
					return false;
				}
				if (@event.TriggeringPlayerID != num)
				{
					return false;
				}
			}
			if (target != null)
			{
				if (!@event.AffectedPlayerIds.Contains(target.Id))
				{
					if (!this.AllowVassalContributions)
					{
						return false;
					}
					int item;
					if (!context.Diplomacy.IsBloodLordOfAny(target.Id, out item))
					{
						return false;
					}
					if (!@event.AffectedPlayerIds.Contains(item))
					{
						return false;
					}
				}
				if (this.UniquePlayers && @event.AffectedPlayerIds.All(new Func<int, bool>(base.SeenPlayers.IsSet)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001AE8 RID: 6888 RVA: 0x0005DEC2 File Offset: 0x0005C0C2
		protected virtual bool Filter(TurnContext context, T @event, PlayerState owner, [MaybeNull] PlayerState target, out int strength)
		{
			if (!this.Filter(context, @event, owner, target))
			{
				strength = 0;
				return false;
			}
			strength = 1;
			return true;
		}

		// Token: 0x06001AE9 RID: 6889 RVA: 0x0005DEDC File Offset: 0x0005C0DC
		public override void Reset(int count)
		{
			base.Reset(count);
			base.SeenPlayers = default(BitMask);
		}

		// Token: 0x06001AEA RID: 6890 RVA: 0x0005DEFF File Offset: 0x0005C0FF
		protected virtual void OnCommitted(PlayerState owner, PlayerState target, T @event)
		{
			if (!this.UniquePlayers)
			{
				return;
			}
			this.UpdateSeenPlayers(@event);
		}

		// Token: 0x06001AEB RID: 6891 RVA: 0x0005DF14 File Offset: 0x0005C114
		private void UpdateSeenPlayers(T @event)
		{
			int index;
			if (@event.AffectedPlayerIds.TryFirst(out index, (int t) => !base.SeenPlayers.IsSet(t)))
			{
				base.SeenPlayers.Set(index);
			}
		}
	}
}
