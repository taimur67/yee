using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000299 RID: 665
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EntityTag_TributeSiphon : EntityTag
	{
		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000CF3 RID: 3315 RVA: 0x0003450C File Offset: 0x0003270C
		[JsonIgnore]
		public int PlayerId
		{
			get
			{
				PlayerContext playerContext = base.Source as PlayerContext;
				if (playerContext == null)
				{
					return int.MinValue;
				}
				return playerContext.PlayerId;
			}
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x00034534 File Offset: 0x00032734
		public override void DeepClone(out EntityTag clone)
		{
			EntityTag_TributeSiphon entityTag_TributeSiphon = new EntityTag_TributeSiphon();
			base.DeepCloneEntityTagParts(entityTag_TributeSiphon);
			clone = entityTag_TributeSiphon;
		}
	}
}
