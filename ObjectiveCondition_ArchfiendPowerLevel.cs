using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000548 RID: 1352
	[Serializable]
	public class ObjectiveCondition_ArchfiendPowerLevel : ObjectiveCondition, IDynamicObjective
	{
		// Token: 0x06001A36 RID: 6710 RVA: 0x0005B6A3 File Offset: 0x000598A3
		[JsonConstructor]
		public ObjectiveCondition_ArchfiendPowerLevel()
		{
		}

		// Token: 0x06001A37 RID: 6711 RVA: 0x0005B6AB File Offset: 0x000598AB
		public ObjectiveCondition_ArchfiendPowerLevel(PowerType powerType)
		{
			this.ArchfiendPower = powerType;
		}

		// Token: 0x06001A38 RID: 6712 RVA: 0x0005B6BA File Offset: 0x000598BA
		protected override int CalculateTotalProgress(TurnContext context, PlayerState owner, bool isInitialProgress)
		{
			if (this.ArchfiendPower != PowerType.None)
			{
				return owner.PowersLevels[this.ArchfiendPower];
			}
			return owner.PowersLevels.GetHighestPower().Item2.CurrentLevel;
		}

		// Token: 0x06001A39 RID: 6713 RVA: 0x0005B6F8 File Offset: 0x000598F8
		public ObjectiveDifficulty CalculateDifficulty(TurnContext context, PlayerState player)
		{
			int value = player.PowersLevels[this.ArchfiendPower].CurrentLevel.Value;
			int num = this.Target - value;
			ObjectiveDifficulty result;
			if (num <= 3)
			{
				if (num > 1)
				{
					if (num > 2)
					{
						result = ObjectiveDifficulty.Moderate;
					}
					else
					{
						result = ObjectiveDifficulty.Easy;
					}
				}
				else
				{
					result = ObjectiveDifficulty.Trivial;
				}
			}
			else if (num > 4)
			{
				result = ObjectiveDifficulty.Hard;
			}
			else
			{
				result = ObjectiveDifficulty.Hard;
			}
			return result;
		}

		// Token: 0x06001A3A RID: 6714 RVA: 0x0005B751 File Offset: 0x00059951
		public override int GetHashCode()
		{
			return (int)(base.GetHashCode() * 19 + this.ArchfiendPower);
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06001A3B RID: 6715 RVA: 0x0005B763 File Offset: 0x00059963
		public override string Name
		{
			get
			{
				return string.Format("Raise {0} to {1}", this.ArchfiendPower.ToString(), this.Target);
			}
		}

		// Token: 0x04000BE3 RID: 3043
		[JsonProperty]
		[BindableValue(null, BindingOption.None)]
		public PowerType ArchfiendPower;
	}
}
