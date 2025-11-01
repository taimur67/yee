using System;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000367 RID: 871
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class GamePieceSupportModifier : Modifier<GamePiece>
	{
		// Token: 0x06001097 RID: 4247 RVA: 0x000416E4 File Offset: 0x0003F8E4
		public override void ApplyTo(TurnContext context, GamePiece item)
		{
			base.ApplyStatModifier(item.SupportStats.Ranged, this.Ranged, ModifierTarget.ValueOffset);
			base.ApplyStatModifier(item.SupportStats.Melee, this.Melee, ModifierTarget.ValueOffset);
			base.ApplyStatModifier(item.SupportStats.Infernal, this.Infernal, ModifierTarget.ValueOffset);
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x0004173C File Offset: 0x0003F93C
		public override void InstallInto(GamePiece item, TurnState turn, bool baseAdjust = false)
		{
			base.InstallStatModifier(item.SupportStats.Ranged, this.Ranged, ModifierTarget.ValueOffset, baseAdjust);
			base.InstallStatModifier(item.SupportStats.Melee, this.Melee, ModifierTarget.ValueOffset, baseAdjust);
			base.InstallStatModifier(item.SupportStats.Infernal, this.Infernal, ModifierTarget.ValueOffset, baseAdjust);
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x00041798 File Offset: 0x0003F998
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (FieldInfo fieldInfo in base.GetType().GetFields())
			{
				if (!(fieldInfo.FieldType != typeof(int)))
				{
					int num = (int)fieldInfo.GetValue(this);
					if (num != 0)
					{
						stringBuilder.Append(string.Format("{0}: {1} ", fieldInfo.Name, num));
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040007B8 RID: 1976
		[JsonProperty]
		public int Ranged;

		// Token: 0x040007B9 RID: 1977
		[JsonProperty]
		public int Melee;

		// Token: 0x040007BA RID: 1978
		[JsonProperty]
		public int Infernal;
	}
}
