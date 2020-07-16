using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Newtonsoft.Json.Linq;

namespace PaperMarioTOK
{
	class ViewModel
	{
		public uint Coin
		{
			get
			{
				JObject root = SaveData.Instance().Root;
				if (root == null) return 0;
				return (uint)root["Pouch"]["coin"];
			}
			set
			{
				JObject root = SaveData.Instance().Root;
				if (root == null) return;
				root["Pouch"]["coin"] = value;
			}
		}

		public uint HP
		{
			get
			{
				JObject root = SaveData.Instance().Root;
				if (root == null) return 0;
				return (uint)root["Pouch"]["hp"];
			}
			set
			{
				JObject root = SaveData.Instance().Root;
				if (root == null) return;
				root["Pouch"]["hp"] = value;
			}
		}

		public uint HPMax
		{
			get
			{
				JObject root = SaveData.Instance().Root;
				if (root == null) return 0;
				return (uint)root["Pouch"]["hp_max"];
			}
			set
			{
				JObject root = SaveData.Instance().Root;
				if (root == null) return;
				root["Pouch"]["hp_max"] = value;
			}
		}

		public System.Windows.Media.ImageSource Thumbnail
		{
			get
			{
				JObject root = SaveData.Instance().Root;
				if (root == null) return null;
				byte[] buf = Convert.FromBase64String((string)root["Header"]["thumbnail"]["base64"]);
				JpegBitmapDecoder decoder = new JpegBitmapDecoder(new System.IO.MemoryStream(buf), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
				return decoder.Frames[0];
			}
		}
	}
}
