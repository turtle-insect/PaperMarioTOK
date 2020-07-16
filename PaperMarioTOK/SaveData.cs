using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace PaperMarioTOK
{
	class SaveData
	{
		private static SaveData mThis;
		public JObject Root { get; private set; }
		private string mFilename;

		public static SaveData Instance()
		{
			if (mThis == null) mThis = new SaveData();
			return mThis;
		}

		private SaveData() { }

		public bool Open(string filename, bool force)
		{
			if (!System.IO.File.Exists(filename)) return false;

			string all = System.IO.File.ReadAllText(filename);
			string json = all.Substring(0, all.Length - 8);
			if (!force)
			{
				string crc = all.Substring(all.Length - 8);
				Crc32 crc32 = new Crc32();
				byte[] buf = System.Text.Encoding.UTF8.GetBytes(json);
				if (Convert.ToUInt32(crc, 16) != crc32.Calc(ref buf, 0, buf.Length)) return false;
			}
			Root = JObject.Parse(json);
			mFilename = filename;
			Backup();
			return true;
		}

		public void Save()
		{
			Crc32 crc32 = new Crc32();
			string json = Root.ToString();
			json = json.Replace("\r\n", "\n");
			byte[] buf = System.Text.Encoding.UTF8.GetBytes(json);
			uint crc = crc32.Calc(ref buf, 0, buf.Length);
			System.IO.File.WriteAllText(mFilename, json + Convert.ToString(crc, 16));
		}

		private void Backup()
		{
			DateTime now = DateTime.Now;
			String path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
			path = System.IO.Path.Combine(path, "backup");
			if (!System.IO.Directory.Exists(path))
			{
				System.IO.Directory.CreateDirectory(path);
			}
			path = System.IO.Path.Combine(path,
				String.Format("{0:0000}-{1:00}-{2:00} {3:00}-{4:00}", now.Year, now.Month, now.Day, now.Hour, now.Minute));
			System.IO.File.Copy(mFilename, path, true);
		}
	}
}
