using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000543 RID: 1347
	[Serializable]
	public abstract class ObjectiveCondition
	{
		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06001A17 RID: 6679 RVA: 0x0005B245 File Offset: 0x00059445
		[JsonIgnore]
		public virtual string Name
		{
			get
			{
				return string.Format("{0}: {1}", base.GetType().Name, this.Target);
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06001A18 RID: 6680 RVA: 0x0005B267 File Offset: 0x00059467
		[JsonIgnore]
		public virtual string LocalizationKey
		{
			get
			{
				string[] array = base.GetType().Name.Split("_", StringSplitOptions.None);
				return array[array.Length - 1];
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06001A19 RID: 6681 RVA: 0x0005B285 File Offset: 0x00059485
		[JsonIgnore]
		public bool IsComplete
		{
			get
			{
				return this.Count >= this.Target;
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06001A1A RID: 6682 RVA: 0x0005B298 File Offset: 0x00059498
		[JsonIgnore]
		public float Progress
		{
			get
			{
				if ((float)this.Target != 0f)
				{
					return (float)this.Count / (float)this.Target;
				}
				return 1f;
			}
		}

		// Token: 0x06001A1B RID: 6683 RVA: 0x0005B2C0 File Offset: 0x000594C0
		public static bool AnyActiveRitualsMatch(IEnumerable<ActiveRitual> activeRituals, IEnumerable<ConfigRef<RitualStaticData>> ritualConfigs)
		{
			return activeRituals.Any((ActiveRitual activeRitual) => ritualConfigs.Any((ConfigRef<RitualStaticData> config) => config.Id == activeRitual.StaticDataId));
		}

		// Token: 0x06001A1C RID: 6684 RVA: 0x0005B2EC File Offset: 0x000594EC
		public void SetStartingProgress(TurnContext context, PlayerState owner)
		{
			if (this.ResetEachTurn)
			{
				this.Count = 0;
				return;
			}
			this.Count = this.CalculateTotalProgress(context, owner, true);
			if (!this.AllowOverflow)
			{
				this.Count = Math.Min(this.Count, this.Target);
			}
		}

		// Token: 0x06001A1D RID: 6685 RVA: 0x0005B32C File Offset: 0x0005952C
		public virtual void Update(TurnContext context, PlayerState owner)
		{
			if (this.CompleteForever && this.IsComplete)
			{
				return;
			}
			int num = this.CalculateTotalProgress(context, owner, false);
			int num2 = num - this.Count;
			this.Count = num;
			if (this.Count < this.Target)
			{
				if (num2 <= 0 && this.ResetOnNoProgress)
				{
					this.Count = 0;
				}
				if (this.ResetEachTurn)
				{
					this.Count = 0;
				}
			}
			if (!this.AllowOverflow)
			{
				this.Count = Math.Min(this.Count, this.Target);
			}
		}

		// Token: 0x06001A1E RID: 6686 RVA: 0x0005B3B3 File Offset: 0x000595B3
		public virtual Result CanBeCompleted(TurnContext context, PlayerState owner)
		{
			return Result.Success;
		}

		// Token: 0x06001A1F RID: 6687 RVA: 0x0005B3BC File Offset: 0x000595BC
		public virtual Result IsValidFor(TurnContext context, PlayerState owner)
		{
			int count = this.Count;
			this.Update(context, owner);
			bool isComplete = this.IsComplete;
			this.Reset(count);
			if (isComplete)
			{
				return Result.Failure;
			}
			return Result.Success;
		}

		// Token: 0x06001A20 RID: 6688 RVA: 0x0005B3F2 File Offset: 0x000595F2
		public virtual void Reset(int count)
		{
			this.Count = count;
		}

		// Token: 0x06001A21 RID: 6689 RVA: 0x0005B3FB File Offset: 0x000595FB
		public int CalculateProgress(TurnContext context, PlayerState owner)
		{
			return this.CalculateTotalProgress(context, owner, false);
		}

		// Token: 0x06001A22 RID: 6690
		protected abstract int CalculateTotalProgress(TurnContext context, PlayerState owner, bool isInitialProgress);

		// Token: 0x06001A23 RID: 6691 RVA: 0x0005B408 File Offset: 0x00059608
		public virtual string FormatDebugStr()
		{
			return string.Format(" {0} {1}/{2} ({3}%)", new object[]
			{
				this.Name,
				this.Count,
				this.Target,
				(int)(this.Progress * 100f)
			});
		}

		// Token: 0x06001A24 RID: 6692 RVA: 0x0005B45F File Offset: 0x0005965F
		public override int GetHashCode()
		{
			return base.GetType().GetHashCode() * 19 + this.Target;
		}

		// Token: 0x06001A25 RID: 6693 RVA: 0x0005B476 File Offset: 0x00059676
		public virtual void Start(TurnState turn, PlayerState player)
		{
		}

		// Token: 0x06001A26 RID: 6694 RVA: 0x0005B478 File Offset: 0x00059678
		protected List<PlayerState> GetPotentialCandidates(TurnContext context, PlayerState owner, ObjectivePlayerRole role, ConfigRef<ArchFiendStaticData> specificArchfiend, bool negate)
		{
			List<PlayerState> list = new List<PlayerState>();
			if (negate)
			{
				list.AddRange(context.CurrentTurn.PlayerStates);
			}
			if (role == ObjectivePlayerRole.Player)
			{
				if (negate)
				{
					list.Remove(owner);
				}
				else
				{
					list.Add(owner);
				}
			}
			else if (role == ObjectivePlayerRole.Anyone)
			{
				if (negate)
				{
					SimLogger logger = SimLogger.Logger;
					if (logger != null)
					{
						logger.Error("Bad configuration trying to negate anyone results in no one being a valid target");
					}
					list.Clear();
				}
				else
				{
					list.AddRange(context.CurrentTurn.PlayerStates);
				}
			}
			else if (role == ObjectivePlayerRole.Specified)
			{
				PlayerState playerState = (specificArchfiend != null) ? context.CurrentTurn.FindPlayerState(specificArchfiend.Id) : null;
				if (playerState != null)
				{
					if (negate)
					{
						list.Remove(playerState);
					}
					else
					{
						list.Add(playerState);
					}
				}
				else
				{
					SimLogger logger2 = SimLogger.Logger;
					if (logger2 != null)
					{
						logger2.Error(string.Format("Player state not found for {0}", specificArchfiend));
					}
				}
			}
			return list;
		}

		// Token: 0x04000BDB RID: 3035
		[BindableValue("objective_count", BindingOption.None)]
		[JsonProperty]
		public int Count;

		// Token: 0x04000BDC RID: 3036
		[BindableValue("objective_target", BindingOption.None)]
		[JsonProperty]
		[DefaultValue(1)]
		public int Target = 1;

		// Token: 0x04000BDD RID: 3037
		[JsonProperty]
		[DefaultValue(true)]
		public bool CompleteForever = true;

		// Token: 0x04000BDE RID: 3038
		[JsonProperty]
		public bool ResetOnNoProgress;

		// Token: 0x04000BDF RID: 3039
		[JsonProperty]
		public bool ResetEachTurn;

		// Token: 0x04000BE0 RID: 3040
		[JsonProperty]
		public bool AllowOverflow;
	}
}
