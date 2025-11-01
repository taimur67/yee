using System;
using System.Linq;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005FC RID: 1532
	public abstract class GrandEventActionProcessor<TOrder, TStaticData, TGameEvent> : ActionProcessor<TOrder, TStaticData>, IGrandEventActionProcessor where TOrder : PlayGrandEventOrder, new() where TStaticData : EventEffectStaticData where TGameEvent : GrandEventPlayed
	{
		// Token: 0x06001C9C RID: 7324 RVA: 0x00062ACA File Offset: 0x00060CCA
		public override Result Preview(ActionProcessContext context)
		{
			return this.Validate();
		}

		// Token: 0x06001C9D RID: 7325 RVA: 0x00062AD4 File Offset: 0x00060CD4
		public override Result Validate()
		{
			if (base._currentTurn.GetAllEventCardsHeldByPlayer(this._player.Id).All((GameItem x) => x.Id != base.request.EventCardId))
			{
				return Result.Failure;
			}
			if (!this._player.BlockEventCardUse)
			{
				return Result.Success;
			}
			return new GrandEventActionProcessor<TOrder, TStaticData, TGameEvent>.EventUseBlockedProblem(base.request.EventCardId);
		}

		// Token: 0x06001C9E RID: 7326 RVA: 0x00062B3D File Offset: 0x00060D3D
		public override Result IsAvailable()
		{
			if (!this._player.BlockEventCardUse)
			{
				return Result.Success;
			}
			return Result.Failure;
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x06001C9F RID: 7327 RVA: 0x00062B5C File Offset: 0x00060D5C
		// (set) Token: 0x06001CA0 RID: 7328 RVA: 0x00062B64 File Offset: 0x00060D64
		private protected TGameEvent GameEvent { protected get; private set; }

		// Token: 0x06001CA1 RID: 7329 RVA: 0x00062B70 File Offset: 0x00060D70
		public sealed override Result Process(ActionProcessContext context)
		{
			Problem problem = this.Validate() as Problem;
			if (problem != null)
			{
				return problem;
			}
			Problem problem2 = this.ProcessImmediate(context) as Problem;
			if (problem2 != null)
			{
				return problem2;
			}
			this.TurnProcessContext.EventsContext.AddGrandEventAction(new PlayerGrandEventAction
			{
				Order = base.request,
				Processor = this
			});
			return Result.Success;
		}

		// Token: 0x06001CA2 RID: 7330 RVA: 0x00062BD8 File Offset: 0x00060DD8
		public virtual Result ProcessImmediate(ActionProcessContext context)
		{
			return Result.Success;
		}

		// Token: 0x06001CA3 RID: 7331 RVA: 0x00062BE0 File Offset: 0x00060DE0
		public virtual Result ProcessDeferred(PlayGrandEventOrder order)
		{
			EventCard eventCard = base._currentTurn.FetchGameItem(base.request.EventCardId) as EventCard;
			if (eventCard == null)
			{
				return Result.Failure;
			}
			this.ProcessGameEvent();
			Problem problem = this.ProcessInternal(order) as Problem;
			if (problem == null)
			{
				if (this.GameEvent != null)
				{
					this.GameEvent.FoundTarget = true;
				}
				this.TurnProcessContext.BanishGameItemSilent(eventCard);
				return Result.Success;
			}
			if (problem is NoValidTargetsProblem)
			{
				if (this.GameEvent != null)
				{
					this.GameEvent.FoundTarget = false;
				}
				this.TurnProcessContext.BanishGameItemSilent(eventCard);
				return Result.Success;
			}
			return Result.Failure;
		}

		// Token: 0x06001CA4 RID: 7332
		protected abstract Result ProcessInternal(PlayGrandEventOrder order);

		// Token: 0x06001CA5 RID: 7333 RVA: 0x00062CA8 File Offset: 0x00060EA8
		protected virtual TGameEvent ProcessGameEvent()
		{
			TGameEvent tgameEvent = (TGameEvent)((object)Activator.CreateInstance(typeof(TGameEvent), new object[]
			{
				base.PlayerId,
				base.request.AbilityId
			}));
			this.GameEvent = tgameEvent;
			base._currentTurn.AddGameEvent<TGameEvent>(this.GameEvent);
			return tgameEvent;
		}

		// Token: 0x02000A48 RID: 2632
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class EventUseBlockedProblem : Problem
		{
			// Token: 0x06003052 RID: 12370 RVA: 0x0009901E File Offset: 0x0009721E
			[JsonConstructor]
			public EventUseBlockedProblem()
			{
			}

			// Token: 0x06003053 RID: 12371 RVA: 0x00099026 File Offset: 0x00097226
			public EventUseBlockedProblem(Identifier eventCardId)
			{
				this.EventCardId = eventCardId;
			}

			// Token: 0x170006F7 RID: 1783
			// (get) Token: 0x06003054 RID: 12372 RVA: 0x00099035 File Offset: 0x00097235
			public override string LocKey
			{
				get
				{
					return "Result.Event.DidNotCast.Malediction";
				}
			}

			// Token: 0x170006F8 RID: 1784
			// (get) Token: 0x06003055 RID: 12373 RVA: 0x0009903C File Offset: 0x0009723C
			public override string DebugString
			{
				get
				{
					return "Event failed because event use was blocked";
				}
			}

			// Token: 0x0400194D RID: 6477
			[BindableValue(null, BindingOption.None)]
			[JsonProperty]
			public Identifier EventCardId;
		}
	}
}
