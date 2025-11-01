using System;

namespace LoG
{
	// Token: 0x02000191 RID: 401
	public class WPSpecificPopCaptured : WorldProperty
	{
		// Token: 0x06000768 RID: 1896 RVA: 0x000230BB File Offset: 0x000212BB
		public WPSpecificPopCaptured(Identifier popID)
		{
			this.PoPID = popID;
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x000230CA File Offset: 0x000212CA
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return false;
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x000230D0 File Offset: 0x000212D0
		public override WPProvidesEffect ProvidesEffect(WorldProperty precondition)
		{
			WPSpecificPopCaptured wpspecificPopCaptured = precondition as WPSpecificPopCaptured;
			if (wpspecificPopCaptured != null && wpspecificPopCaptured.PoPID == this.PoPID)
			{
				return WPProvidesEffect.Yes;
			}
			return WPProvidesEffect.No;
		}

		// Token: 0x04000362 RID: 866
		public Identifier PoPID;
	}
}
