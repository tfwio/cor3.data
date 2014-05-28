/* oOo * 11/14/2007 : 10:22 PM */
using System;
using System.Xml.Serialization;

namespace System.IO
{
	public class SerializableClass<T>
		where T:SerializableClass<T>, new()
	{
		virtual public XmlSerializerNamespaces SerializableNamespaces
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		internal protected bool useNamespaces = false;
		virtual public bool UseNamespaces
		{
			get { return useNamespaces; }
		}
		
		[XmlIgnore] public string FileLoadedOrSaved = string.Empty;
		protected string fileFilter = ControlUtil.XmlFile;
		[XmlIgnore] virtual protected string FileFilter
		{
			get { return fileFilter; }
			set { fileFilter = value; }
		}

		virtual public void Save(T obj)
		{
			string fname = ControlUtil.FSave(FileFilter);
			if (fname==string.Empty) return;
			Save(fname,obj);
		}
		virtual public void Save(string fname,T obj)
		{
			obj.FileLoadedOrSaved = fname;
			
			Serial.SerializeXml(fname,typeof(T),obj);
		}
		virtual public Stream SaveStream()
		{
			return Serial.SerializeMemoryStream<T>(this);
		}

		/// <summary>Gets a</summary>
		static public T LoadBase64(string fname)
		{
			T obj;
			string dataStr = File.ReadAllText(fname);
			byte[] data = Convert.FromBase64String(dataStr);
			using (MemoryStream ms = new MemoryStream(data))
			{
				XmlSerializer xs = new XmlSerializer(typeof(T));
				obj = (T)xs.Deserialize(ms);
			}
			Array.Clear(data,0,data.Length);
			dataStr = null; data = null;
			return obj;
		}
		virtual public void SaveBase64()
		{
			string fname = ControlUtil.FSave(FileFilter);
			if (fname==string.Empty) return;
			SaveBase64(fname);
		}
		virtual public void SaveBase64(string fname)
		{
			byte[] data;
			using (MemoryStream ms = SaveStream() as MemoryStream)
				data = ms.ToArray();
			if (data.Length==0) return;
			string dataString = Convert.ToBase64String(data,Base64FormattingOptions.InsertLineBreaks);
			File.WriteAllText(fname,dataString);
			Array.Clear(data,0,data.Length);
			dataString = null; data = null;
		}

		static public T Load()
		{
			T nclass = new T();
			string filter = (nclass as SerializableClass<T>).FileFilter;
			nclass = null;
			string fname = ControlUtil.FGet(filter);
			if (fname==string.Empty) return null;
			return Load(fname);
		}
		static public T Load(string fname)
		{
			T obj = Serial.DeSerialize<T>(fname);
			obj.FileLoadedOrSaved = fname;
			return obj;
		}
		static public void Load(string fname, T obj)
		{
			obj = Serial.DeSerialize<T>(fname);
			obj.FileLoadedOrSaved = fname;
		}
	}
}
