using System;
using System.Collections.Generic;
using System.ComponentModel;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000583 RID: 1411
	[Serializable]
	public abstract class ObjectiveCondition_EventFilter : IncrementingObjectiveCondition
	{
		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x06001AD5 RID: 6869 RVA: 0x0005DAB7 File Offset: 0x0005BCB7
		protected virtual bool CanSupportTargets
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06001AD6 RID: 6870 RVA: 0x0005DABA File Offset: 0x0005BCBA
		// (set) Token: 0x06001AD7 RID: 6871 RVA: 0x0005DAC2 File Offset: 0x0005BCC2
		[JsonProperty]
		public int? TargetingPlayer { get; set; }

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06001AD8 RID: 6872 RVA: 0x0005DACB File Offset: 0x0005BCCB
		// (set) Token: 0x06001AD9 RID: 6873 RVA: 0x0005DAD3 File Offset: 0x0005BCD3
		[JsonProperty]
		public BitMask SeenPlayers { get; set; }

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06001ADA RID: 6874 RVA: 0x0005DADC File Offset: 0x0005BCDC
		[JsonIgnore]
		public bool IsTargeted
		{
			get
			{
				return this.TargetRole == ObjectivePlayerRole.Specified && (this.TargetingPlayer != null || !this.TargetArchfiend.IsEmpty());
			}
		}

		// Token: 0x06001ADB RID: 6875 RVA: 0x0005DB14 File Offset: 0x0005BD14
		protected void GetActors(TurnState turn, GameEvent @event, ref PlayerState player, out PlayerState target)
		{
			target = null;
			if (this.TargetRole == ObjectivePlayerRole.Player)
			{
				target = player;
			}
			else if (this.TargetRole == ObjectivePlayerRole.Specified)
			{
				if (this.TargetingPlayer != null)
				{
					target = turn.FindPlayerState(this.TargetingPlayer.Value, null);
				}
				if (target == null && !this.TargetArchfiend.IsEmpty())
				{
					target = turn.FindPlayerState(this.TargetArchfiend.Id);
				}
			}
			if ((this.TargetRole == ObjectivePlayerRole.Anyone || target == null) && @event != null && @event.AffectedPlayerIds.Count > 0)
			{
				target = turn.FindPlayerState(@event.AffectedPlayerID, null);
			}
			if (this.InstigatorRole == ObjectivePlayerRole.Specified)
			{
				player = turn.FindPlayerState(this.InstigatorArchfiend.Id);
			}
			else if (this.InstigatorRole == ObjectivePlayerRole.Anyone)
			{
				if (@event != null)
				{
					player = turn.FindPlayerState(@event.TriggeringPlayerID, null);
				}
			}
			else if (this.InstigatorRole == ObjectivePlayerRole.Opposition)
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Warn("Instigator cannot be set to Opposition");
				}
			}
			if (this.TargetRole == ObjectivePlayerRole.Opposition && player != null && @event != null)
			{
				if (@event.TriggeringPlayerID == player.Id && @event.AffectedPlayerIds.Count > 0)
				{
					target = turn.FindPlayerState(@event.AffectedPlayerID, null);
					return;
				}
				if (@event.AffectedPlayerIds.Contains(player.Id) && @event.TriggeringPlayerID != -1)
				{
					target = turn.FindPlayerState(@event.TriggeringPlayerID, null);
				}
			}
		}

		// Token: 0x06001ADC RID: 6876 RVA: 0x0005DC80 File Offset: 0x0005BE80
		public sealed override Result CanBeCompleted(TurnContext context, PlayerState owner)
		{
			Problem problem = base.CanBeCompleted(context, owner) as Problem;
			if (problem != null)
			{
				return problem;
			}
			PlayerState playerState;
			this.GetActors(context.CurrentTurn, null, ref owner, out playerState);
			if (playerState != null && playerState.Eliminated)
			{
				return Result.Failure;
			}
			return Result.Success;
		}

		// Token: 0x06001ADD RID: 6877 RVA: 0x0005DCC7 File Offset: 0x0005BEC7
		protected sealed override int CalculateProgressIncrement(TurnContext context, PlayerState owner)
		{
			return this.CalculateProgressIncrement(context, owner, context.CurrentTurn.GetGameEvents());
		}

		// Token: 0x06001ADE RID: 6878
		protected abstract int CalculateProgressIncrement(TurnContext context, PlayerState owner, IEnumerable<GameEvent> gameEvents);

		// Token: 0x04000C37 RID: 3127
		[JsonProperty]
		public bool UniquePlayers;

		// Token: 0x04000C38 RID: 3128
		[JsonProperty]
		[DefaultValue(ObjectivePlayerRole.Player)]
		public ObjectivePlayerRole InstigatorRole;

		// Token: 0x04000C39 RID: 3129
		[JsonProperty]
		public ConfigRef<ArchFiendStaticData> InstigatorArchfiend;

		// Token: 0x04000C3A RID: 3130
		[JsonProperty]
		[DefaultValue(ObjectivePlayerRole.Specified)]
		public ObjectivePlayerRole TargetRole = ObjectivePlayerRole.Specified;

		// Token: 0x04000C3B RID: 3131
		[JsonProperty]
		public ConfigRef<ArchFiendStaticData> TargetArchfiend;
	}
}
