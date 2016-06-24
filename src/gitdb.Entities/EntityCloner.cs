using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;

namespace gitdb.Entities
{
	public class EntityCloner
	{
		public EntityCloner ()
		{
		}

		public BaseEntity Clone(BaseEntity entity)
		{
			byte[] byteobj = ObjectToByteArray(entity);
			return (BaseEntity)ByteArrayToObject(byteobj);
		}

		// The following 2 functions are credited to:
		// http://ninocrudele.me/2015/09/24/c-tip-clone-a-net-object/

		public static byte[] ObjectToByteArray(object objectData)
		{
			if (objectData == null)
				return null;
			var binaryFormatter = new BinaryFormatter();
			var memoryStream = new MemoryStream();
			binaryFormatter.Serialize(memoryStream, objectData);
			return memoryStream.ToArray();
		}

		public static object ByteArrayToObject(byte[] arrayBytes)
		{
			if (arrayBytes == null) return Encoding.UTF8.GetBytes(string.Empty);
			var memoryStream = new MemoryStream();
			var binaryFormatter = new BinaryFormatter();
			memoryStream.Write(arrayBytes, 0, arrayBytes.Length);
			memoryStream.Seek(0, SeekOrigin.Begin);
			var obj = binaryFormatter.Deserialize(memoryStream);
			return obj;
		}
	}
}

