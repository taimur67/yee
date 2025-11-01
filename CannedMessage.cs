using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000307 RID: 775
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class CannedMessage : Message
	{
		// Token: 0x06000F24 RID: 3876 RVA: 0x0003C1D5 File Offset: 0x0003A3D5
		[JsonConstructor]
		public CannedMessage()
		{
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x0003C1E8 File Offset: 0x0003A3E8
		public CannedMessage(TurnContext context, PlayerState sender, PlayerState receiver) : base(context, sender, receiver)
		{
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x0003C1FE File Offset: 0x0003A3FE
		public override string GetContent()
		{
			return this.LocalizedStringId;
		}

		// Token: 0x06000F27 RID: 3879 RVA: 0x0003C208 File Offset: 0x0003A408
		public override string SetContent(string stringId)
		{
			this.LocalizedStringId = stringId;
			return stringId;
		}

		// Token: 0x06000F28 RID: 3880 RVA: 0x0003C220 File Offset: 0x0003A420
		public override void DeepClone(out Message message)
		{
			CannedMessage cannedMessage;
			this.DeepClone(out cannedMessage);
			message = cannedMessage;
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x0003C238 File Offset: 0x0003A438
		private void DeepClone(out CannedMessage clone)
		{
			clone = new CannedMessage
			{
				LocalizedStringId = this.LocalizedStringId.DeepClone()
			};
			base.DeepCloneMessageParts(clone);
		}

		// Token: 0x040006EC RID: 1772
		[JsonProperty]
		public string LocalizedStringId = "Chronicles.Message.Test";
	}
}
