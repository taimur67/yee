using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using LoG.Simulation.Extensions;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020001FE RID: 510
	[Serializable]
	public abstract class GameEvent : IDeepClone<GameEvent>
	{
		// Token: 0x170001BE RID: 446
		// (get) Token: 0x060009E0 RID: 2528 RVA: 0x0002D5E2 File Offset: 0x0002B7E2
		[JsonIgnore]
		public IReadOnlyList<GameEvent> LocalChildEvents
		{
			get
			{
				return this.ChildEvents;
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x060009E1 RID: 2529 RVA: 0x0002D5EA File Offset: 0x0002B7EA
		protected virtual GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Private;
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x060009E2 RID: 2530 RVA: 0x0002D5ED File Offset: 0x0002B7ED
		protected virtual GameEventVisibilityStrippingRule GameEventVisibilityStrippingRule
		{
			get
			{
				return GameEventVisibilityStrippingRule.AllowParentChainToStripIfVisible;
			}
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x0002D5F0 File Offset: 0x0002B7F0
		public bool CanStrip(int playerId)
		{
			if (this.CanSeeEvent(playerId))
			{
				return false;
			}
			foreach (GameEvent gameEvent in this.EnumerateAllChildEvents())
			{
				if (gameEvent.CanSeeEvent(playerId) && gameEvent.GameEventVisibilityStrippingRule == GameEventVisibilityStrippingRule.ForceParentChainToRemainIfVisible)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x060009E4 RID: 2532 RVA: 0x0002D65C File Offset: 0x0002B85C
		[BindableValue("affected_name", BindingOption.IntPlayerId | BindingOption.BindDisplayName)]
		public int AffectedPlayerID
		{
			get
			{
				if (this.AffectedPlayerIds.Count <= 0)
				{
					return -1;
				}
				return this.AffectedPlayerIds[0];
			}
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x0002D67A File Offset: 0x0002B87A
		protected GameEvent(int triggeringPlayerID)
		{
			this.TriggeringPlayerID = triggeringPlayerID;
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x0002D6A6 File Offset: 0x0002B8A6
		[JsonConstructor]
		protected GameEvent()
		{
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x0002D6CB File Offset: 0x0002B8CB
		public void AddAffectedPlayerId(int playerId)
		{
			if (!this.AffectedPlayerIds.Contains(playerId))
			{
				this.AffectedPlayerIds.Add(playerId);
			}
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x0002D6E8 File Offset: 0x0002B8E8
		public void AddAffectedPlayerIds(IEnumerable<int> playerIds)
		{
			if (playerIds == null)
			{
				return;
			}
			foreach (int playerId in playerIds)
			{
				this.AddAffectedPlayerId(playerId);
			}
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x0002D734 File Offset: 0x0002B934
		public virtual bool IsAssociatedWith(int playerId)
		{
			return this.TriggeringPlayerID == playerId || this.AffectedPlayerIds.Contains(playerId);
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x0002D750 File Offset: 0x0002B950
		public bool CanSeeEvent(int playerId)
		{
			bool result;
			switch (this.GameEventVisibility)
			{
			case GameEventVisibility.Private:
				result = this.IsAssociatedWith(playerId);
				break;
			case GameEventVisibility.Public:
				result = true;
				break;
			case GameEventVisibility.Secret:
				result = (this.TriggeringPlayerID == playerId);
				break;
			default:
				result = false;
				break;
			}
			return result;
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x0002D795 File Offset: 0x0002B995
		public bool ShouldShowInTurnLog(int playerId)
		{
			return this.GetTurnLogEntryType(playerId) != TurnLogEntryType.None && this.CanSeeEvent(playerId);
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x0002D7A9 File Offset: 0x0002B9A9
		public virtual string GetDebugName(TurnContext context)
		{
			return base.GetType().Name;
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x0002D7B6 File Offset: 0x0002B9B6
		public virtual TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			return TurnLogEntryType.None;
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x0002D7B9 File Offset: 0x0002B9B9
		public virtual SequencePlaybackType GetSequencePlaybackType(int forPlayerID)
		{
			return SequencePlaybackType.TurnPlayback;
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x0002D7BC File Offset: 0x0002B9BC
		public IEnumerable<GameEvent> EnumerateSelfAndAllChildEvents()
		{
			yield return this;
			foreach (GameEvent gameEvent in this.EnumerateAllChildEvents())
			{
				yield return gameEvent;
			}
			IEnumerator<GameEvent> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x0002D7CC File Offset: 0x0002B9CC
		public IEnumerable<GameEvent> EnumerateAllChildEvents()
		{
			return this.ChildEvents.SelectMany((GameEvent t) => t.EnumerateSelfAndAllChildEvents());
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x0002D7F8 File Offset: 0x0002B9F8
		public T AddChildEvent<T>() where T : GameEvent, new()
		{
			return this.AddChildEvent<T>(Activator.CreateInstance<T>());
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x0002D808 File Offset: 0x0002BA08
		public T AddChildEvent<T>(T ev) where T : GameEvent
		{
			if (ev == null)
			{
				return default(T);
			}
			this.ChildEvents.Add(ev);
			return ev;
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x0002D839 File Offset: 0x0002BA39
		public GameEvent AddChildEvent(GameEvent ev)
		{
			this.AddChildEvent(IEnumerableExtensions.ToEnumerable<GameEvent>(ev));
			return ev;
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x0002D848 File Offset: 0x0002BA48
		public void AddChildEvent(params GameEvent[] events)
		{
			this.AddChildEvent(events.AsEnumerable<GameEvent>());
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x0002D856 File Offset: 0x0002BA56
		public void AddChildEvent(IEnumerable<GameEvent> events)
		{
			this.ChildEvents.AddRange(events.ExcludeNull<GameEvent>());
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x0002D869 File Offset: 0x0002BA69
		public IEnumerable<T> Enumerate<T>() where T : GameEvent
		{
			return this.EnumerateSelfAndAllChildEvents().OfType<T>();
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x0002D876 File Offset: 0x0002BA76
		public bool Contains<T>() where T : GameEvent
		{
			return IEnumerableExtensions.Any<T>(this.Enumerate<T>());
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x0002D883 File Offset: 0x0002BA83
		public bool Contains<T>(Func<T, bool> predicate) where T : GameEvent
		{
			return this.Enumerate<T>().Any(predicate);
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x0002D891 File Offset: 0x0002BA91
		protected T Get<T>() where T : GameEvent
		{
			return IEnumerableExtensions.FirstOrDefault<T>(this.Enumerate<T>());
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x0002D89E File Offset: 0x0002BA9E
		public bool TryGet<T>(out T var) where T : GameEvent
		{
			var = this.Get<T>();
			return var != null;
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x0002D8BA File Offset: 0x0002BABA
		public void RemoveHidden(int playerId)
		{
			this.ChildEvents.RemoveHiddenGameEvents(playerId);
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x0002D8C8 File Offset: 0x0002BAC8
		protected T DeepCloneGameEventParts<T>(T gameEvent) where T : GameEvent
		{
			gameEvent.TriggeringPlayerID = this.TriggeringPlayerID;
			gameEvent.AffectedPlayerIds = this.AffectedPlayerIds.DeepClone();
			gameEvent.ChildEvents = this.ChildEvents.DeepClone<GameEvent>();
			gameEvent.EventID = this.EventID;
			return gameEvent;
		}

		// Token: 0x060009FD RID: 2557
		public abstract void DeepClone(out GameEvent clone);

		// Token: 0x040004B8 RID: 1208
		[BindableValue("source_name", BindingOption.IntPlayerId | BindingOption.BindDisplayName)]
		[JsonProperty]
		[DefaultValue(-1)]
		public int TriggeringPlayerID = -1;

		// Token: 0x040004B9 RID: 1209
		[BindableValue("affected_names", BindingOption.IntPlayerId)]
		[JsonProperty]
		public List<int> AffectedPlayerIds = new List<int>();

		// Token: 0x040004BA RID: 1210
		[JsonProperty]
		private List<GameEvent> ChildEvents = new List<GameEvent>();

		// Token: 0x040004BB RID: 1211
		[JsonProperty]
		public int EventID;
	}
}
