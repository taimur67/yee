using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000306 RID: 774
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class FreeMessage : Message
	{
		// Token: 0x06000F1F RID: 3871 RVA: 0x0003C165 File Offset: 0x0003A365
		public override string GetContent()
		{
			return this.Content;
		}

		// Token: 0x06000F20 RID: 3872 RVA: 0x0003C170 File Offset: 0x0003A370
		public override string SetContent(string content)
		{
			this.Content = content;
			return content;
		}

		// Token: 0x06000F21 RID: 3873 RVA: 0x0003C187 File Offset: 0x0003A387
		public override void DeepClone(out Message message)
		{
			message = new FreeMessage
			{
				Content = this.Content.DeepClone(),
				LocaleCode = this.LocaleCode.DeepClone()
			};
			base.DeepCloneMessageParts(message);
		}

		// Token: 0x06000F22 RID: 3874 RVA: 0x0003C1BA File Offset: 0x0003A3BA
		public FreeMessage()
		{
		}

		// Token: 0x06000F23 RID: 3875 RVA: 0x0003C1C2 File Offset: 0x0003A3C2
		public FreeMessage(TurnContext context, PlayerState sender, PlayerState receiver, string authorLocaleCode) : base(context, sender, receiver)
		{
			this.LocaleCode = authorLocaleCode;
		}

		// Token: 0x040006EA RID: 1770
		[JsonProperty]
		public string LocaleCode;

		// Token: 0x040006EB RID: 1771
		[JsonProperty]
		public string Content;
	}
}
