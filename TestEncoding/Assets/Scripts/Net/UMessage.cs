using System;
using System.Reflection;

public class StringLengthFlag
{

}

public class ArraySizeFlag
{

}

namespace Net
{
    using System;
    using System.Text;
    using ICSharpCode.SharpZipLib.Zip;
    using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
    using System.IO;
    using System.Security.Cryptography;
    using UnityEngine;
    using ProtoBuf;
    using GBKEncoding;
    //using System.Runtime.Serialization.Formatters.Binary;

    /// <summary>
    /// 1-4 消息头(压缩，加密，长度); 5-6 消息号;7-10 时间戳
    /// </summary>
    public class UMessage
    {
		public const int HEAD_SIZE = 4;

        public byte[] Buffer;
        
        private int index;
        
        public int Length;
        public int BodyLength;
        public ushort MsgId;
        public byte MsgCmd;
        public byte MsgParam;

        public bool IsPack;
        //public static System.Text.Encoding gbk = System.Text.Encoding.GetEncoding("gbk");
        //public static System.Text.Encoding utf8 = System.Text.Encoding.GetEncoding("utf-8");

        /// <summary>
        /// GBK转UTF8
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        //public static string gbk_utf8(string text)
        //{
            //if (gbk == null)
            //    gbk = System.Text.Encoding.GetEncoding("gbk");
            //if (utf8 == null)
            //    utf8 = System.Text.Encoding.GetEncoding("utf-8");

            //byte[] gb;

            //gb = gbk.GetBytes(text);
            //gb = System.Text.Encoding.Convert(gbk, utf8, gb);

            //return gbk.GetString(gb);
           // return text;
        //}

        /// <summary>
        /// UTF8转GBK
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        //public static string utf8_gbk(string text)
        //{
            //if (gbk == null)
            //    gbk = System.Text.Encoding.GetEncoding("gbk");
            //if (utf8 == null)
            //    utf8 = System.Text.Encoding.GetEncoding("utf-8");

            //byte[] utf;

            //utf = utf8.GetBytes(text);
            //utf = System.Text.Encoding.Convert(utf8, gbk, utf);

            //return gbk.GetString(utf);
           // return text;
        //}

        ///// <summary>
        ///// 结构体序列化
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <param name="serializedStr"></param>
        ///// <returns></returns>
        //public static bool serializeObjToStr(System.Object obj, out string serializedStr)
        //{
        //    bool serializeOk = false;
        //    serializedStr = "";
        //    try 
        //    {
        //        MemoryStream memoryStream = new MemoryStream();
        //        BinaryFormatter binaryFormatter = new BinaryFormatter();
        //        binaryFormatter.Serialize(memoryStream, obj);
        //        serializedStr = System.Convert.ToBase64String(memoryStream.ToArray());

        //        serializeOk = true;
        //    }

        //    catch
        //    {
        //        serializeOk = false;
        //    }

        //    return serializeOk;
        //}

        ///// <summary>
        ///// 结构体反序列化
        ///// </summary>
        ///// <param name="serializedStr"></param>
        ///// <param name="deserializedObj"></param>
        ///// <returns></returns>
        //public static bool deserializeStrToObj(string serializedStr, out System.Object deserializedObj)
        //{
        //    bool deserializeOk = false;
        //    deserializedObj = null;

        //    try
        //    {
        //        byte[] restoreBytes = System.Convert.FromBase64String(serializedStr);
        //        MemoryStream restoreMemoryStream = new MemoryStream(restoreBytes);
        //        BinaryFormatter binaryFormatter = new BinaryFormatter();
        //        binaryFormatter.Deserialize(restoreMemoryStream);

        //        deserializeOk = true;
        //    }

        //    catch
        //    {
        //        deserializeOk = false;
        //    }

        //    return deserializeOk;
        //}



        //public bool deserializeStrToObj(string serializedStr, out object ) 
        public UMessage()
        {
            Reset();
            initDes();
        }

        #region MessageFunction

        public void ReadLength()
        {
            BodyLength = (int)(Buffer[0] + (Buffer[1] << 8));
            Length = BodyLength + HEAD_SIZE;
            index = 0;
            IsPack = true;
        }

        public void ReadHead()
        {
            MsgId = ReadUInt16(Buffer, HEAD_SIZE);
            MsgCmd = Buffer[HEAD_SIZE];
            MsgParam = Buffer[HEAD_SIZE + 1];
            index += 2;
            ReadTimeStamp();
            //时间戳
            index += 4;
        }

        public void WriteHead(byte cmd, byte para)
        {
            MsgCmd = cmd;
            MsgParam = para;
            Buffer[HEAD_SIZE] = MsgCmd;
            Buffer[HEAD_SIZE + 1] = MsgParam;
            WriteTimeStamp(0);
            index += 6;
            updateLengthHandle();
        }

        public void WriteHead(ushort msgId)
        {
            MsgId = (ushort)msgId;
            WriteUInt16(MsgId);
            WriteTimeStamp(0);
            index += 4;
            updateLengthHandle();
        }

        public void Reset()
        {
            index = 0;
            Length = 0;
            BodyLength = 0;
            Buffer = new byte[1024 * 64];
            IsPack = false;
        }

        public void Clear()
        {
            index = 0;
            Length = 0;
            BodyLength = 0;
            Buffer = null;
        }

        public UMessage copyNew()
        {
            UMessage newobj = new UMessage();
            newobj.index = this.index;
            newobj.Length = this.Length;
            newobj.BodyLength = this.BodyLength;
            newobj.MsgId = this.MsgId;
            newobj.IsPack = this.IsPack;
            Array.Copy(this.Buffer, newobj.Buffer, Length);
            return newobj;
        }

        public void ResetIndex()
        {
            index = 0;
        }

        //更新字节数组长度
        private void updateLengthHandle()
        {
            BodyLength = index;
            Length = HEAD_SIZE + index;
            WriteUInt16(Buffer, 0, (UInt16)BodyLength);
        }

        public UInt32 ReadTimeStamp()
        {
            return 0;
        }

        // 写时间戳
        public void WriteTimeStamp(UInt32 b)
        {
            Buffer[HEAD_SIZE + 2] = (byte)(b << 24 >> 24);
            Buffer[HEAD_SIZE + 3] = (byte)(b << 16 >> 24);
            Buffer[HEAD_SIZE + 4] = (byte)(b << 8 >> 24);
            Buffer[HEAD_SIZE + 5] = (byte)(b >> 24);
        }

        #endregion

        #region ReadFunction

        public byte ReadByte()
        {
            byte result = Buffer[HEAD_SIZE + index];
            index++;
            return result;
        }

        public bool ReadBool()
        {
            byte result = Buffer[HEAD_SIZE + index];
            index++;
            if (result == 0)
                return false;
            return true;
        }
        public UInt16 ReadUInt16()
        {
            UInt16 result = ReadUInt16(Buffer, HEAD_SIZE + index);
            index += 2;
            return result;
        }
        public UInt32 ReadUInt32()
        {
            UInt32 result = (UInt32)(Buffer[HEAD_SIZE + index] + (Buffer[HEAD_SIZE + index + 1] << 8) + (Buffer[HEAD_SIZE + index + 2] << 16) + (Buffer[HEAD_SIZE + index + 3] << 24));
            index += 4;
            return result;
        }

        public UInt64 ReadUInt64()
        {
            UInt32 low = ReadUInt32();
            UInt32 high = ReadUInt32();
            return low + ((UInt64)(high) << 32);
        }

        public static UInt16 ReadUInt16(byte[] buffer, int index)
        {
            return (UInt16)(buffer[index] + (buffer[index + 1] << 8));
        }

        public int ReadInt32()
        {
            int result = (int)(Buffer[HEAD_SIZE + index] + (Buffer[HEAD_SIZE + index + 1] << 8) + (Buffer[HEAD_SIZE + index + 2] << 16) + (Buffer[HEAD_SIZE + index + 3] << 24));
            index += 4;
            return result;
        }

        public Int16 ReadInt16()
        {
            Int16 result = (Int16)(Buffer[HEAD_SIZE + index] + (Buffer[HEAD_SIZE + index + 1] << 8));
            index += 2;
            return result;
        }

        public float ReadFloat()
        {
            float result = BitConverter.ToSingle(Buffer, HEAD_SIZE + index);
            index += 4;
            return result;
        }

        public string ReadString()
        {
            int length = (int)ReadUInt16(Buffer, HEAD_SIZE + index);
            //string result = Encoding.UTF8.GetString(Buffer, HEAD_SIZE + index + 2, length);
            string result = GBKEncoder.Read(Buffer, HEAD_SIZE + index + 2, length);
            index += length + 2;
            return result;
        }

        public string ReadString(int length)
        {
            //int strlen = 0;
            //for (int i = 0; i < length; i++)
            //{
            //    byte tmp = Buffer[HEAD_SIZE + index + i];

            //    if (tmp == 0)
            //    {
            //        break;
            //    }

            //    strlen += 1;
            //}

            //string result = Encoding.UTF8.GetString(Buffer, HEAD_SIZE + index, strlen);
            string result = GBKEncoder.Read(Buffer, HEAD_SIZE + index, length);
            index += length;
            return result;
        }

        public byte[] ReadBytes()
        {
            int length = (int)ReadUInt16(Buffer, HEAD_SIZE + index);
            byte[] ret = new byte[length];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = Buffer[HEAD_SIZE + index + 2 + i];
            }
            index += length + 2;
            return ret;
        }

        public Byte[] ReadBytes(int length)
        {
            Byte[] ret = new Byte[length];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = Buffer[HEAD_SIZE + index + i];
            }
            index += length;
            return ret;
        }

        public T ReadProto<T>()
        {
#if false
			T t;
			using(MemoryStream ms = new MemoryStream(ReadBytes()))
			{
				t = (T)protoSerializer.Deserialize(ms, null, typeof(T));
			}
			return t;
#else
			T t;
			using(MemoryStream ms = new MemoryStream(ReadBytes()))
			{
				t = Serializer.Deserialize<T>(ms);
			}
			return t;
#endif
        }

        #endregion

        #region WriteFunction
        public void WriteByte(byte b)
        {
            Buffer[HEAD_SIZE + index] = b;
            index += 1;
            updateLengthHandle();
        }

        public void WriteUInt16(UInt16 b)
        {
            WriteUInt16(Buffer, HEAD_SIZE + index, b);
            index += 2;
            updateLengthHandle();
        }

        public void WriteUInt32(UInt32 b)
        {
            Buffer[HEAD_SIZE + index] = (byte)(b << 24 >> 24);
            index++;
            Buffer[HEAD_SIZE + index] = (byte)(b << 16 >> 24);
            index++;
            Buffer[HEAD_SIZE + index] = (byte)(b << 8 >> 24);
            index++;
            Buffer[HEAD_SIZE + index] = (byte)(b >> 24);
            index++;
            updateLengthHandle();
        }

        public void WriteUInt64(UInt64 b)
        {
            Buffer[HEAD_SIZE + index] = (byte)(b << 56 >> 56);
            index++;
            Buffer[HEAD_SIZE + index] = (byte)(b << 48 >> 56);
            index++;
            Buffer[HEAD_SIZE + index] = (byte)(b << 40 >> 56);
            index++;
            Buffer[HEAD_SIZE + index] = (byte)(b << 32 >> 56);
            index++;
            Buffer[HEAD_SIZE + index] = (byte)(b << 24 >> 56);
            index++;
            Buffer[HEAD_SIZE + index] = (byte)(b << 16 >> 56);
            index++;
            Buffer[HEAD_SIZE + index] = (byte)(b << 8 >> 56);
            index++;
            Buffer[HEAD_SIZE + index] = (byte)(b >> 56);
            index++;
            updateLengthHandle();
        }

        public void WriteInt32(int b)
        {
            Buffer[HEAD_SIZE + index] = (byte)(b << 24 >> 24);
            index++;
            Buffer[HEAD_SIZE + index] = (byte)(b << 16 >> 24);
            index++;
            Buffer[HEAD_SIZE + index] = (byte)(b << 8 >> 24);
            index++;
            Buffer[HEAD_SIZE + index] = (byte)(b >> 24);
            index++;
            updateLengthHandle();
        }

        public void WriteUInt16(byte[] buffer, int index, UInt16 b)
        {
            buffer[index] = (byte)(b << 24 >> 24);
            buffer[index + 1] = (byte)(b << 16 >> 24);
        }

        public void WriteByte(byte[] buffer, int index, byte b)
        {
            buffer[index] = b;
        }

        public void WriteString(string s)
        {
            byte[] tmp = GBKEncoder.ToBytes(s);
            //byte[] tmp = gbk.GetBytes(s);
            //byte[] tmp = Encoding.UTF8.GetBytes(s);
            WriteUInt16(Buffer, HEAD_SIZE + index, (UInt16)tmp.Length);
            for (int i = 0; i < tmp.Length; i++)
            {
                Buffer[HEAD_SIZE + index + 2 + i] = tmp[i];
            }
            index += tmp.Length + 2;
            updateLengthHandle();
        }

        public void WriteString(string s, int nSize)
        {
            byte[] tmp = GBKEncoder.ToBytes(s);
            //byte[] tmp = gbk.GetBytes(s);
            //byte[] tmp = Encoding.UTF8.GetBytes(s);
            for (int i = 0; i < tmp.Length; i++)
            {
                Buffer[HEAD_SIZE + index + i] = tmp[i];
            }

            for (int j = tmp.Length; j < nSize; j++)
            {
                Buffer[HEAD_SIZE + index + j] = 0;
            }

            index += nSize;
            updateLengthHandle();
        }

        public void WriteUTFString(string s)
        {
            byte[] tmp = Encoding.UTF8.GetBytes(s);
            WriteUInt16(Buffer, HEAD_SIZE + index, (UInt16)tmp.Length);
            for (int i = 0; i < tmp.Length; i++)
            {
                Buffer[HEAD_SIZE + index + 2 + i] = tmp[i];
            }
            index += tmp.Length + 2;
            updateLengthHandle();
        }

        public void WriteUTFString(string s, int nSize)
        {
            byte[] tmp = Encoding.UTF8.GetBytes(s);
            for (int i = 0; i < tmp.Length; i++)
            {
                Buffer[HEAD_SIZE + index + i] = tmp[i];
            }

            for (int j = tmp.Length; j < nSize; j++)
            {
                Buffer[HEAD_SIZE + index + j] = 0;
            }

            index += nSize;
            updateLengthHandle();
        }

        public void WriteBytes(byte[] bytes)
        {
			WriteUInt16(Buffer, HEAD_SIZE + index, (UInt16)bytes.Length);
            for (int i = 0; i < bytes.Length; i++)
            {
                Buffer[HEAD_SIZE + index + 2 + i] = bytes[i];
            }
            index += bytes.Length + 2;
            updateLengthHandle();

			//USocket.PrintByteArr (Buffer, Length);
        }

        /// <summary>
        /// 写入字节流，不包含头
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="nSize"></param>
        public void WriteBytes(byte[] bytes, int nSize)
        {
            for (int i = 0; i < nSize; i++)
            {
                Buffer[HEAD_SIZE + index + i] = bytes[i];
            }

            index += nSize;
            updateLengthHandle();
        }
        public void WriteFloat(float f)
        {
            byte[] r = BitConverter.GetBytes(f);
            Buffer[HEAD_SIZE + index] = r[0];
            index++;
            Buffer[HEAD_SIZE + index] = r[1];
            index++;
            Buffer[HEAD_SIZE + index] = r[2];
            index++;
            Buffer[HEAD_SIZE + index] = r[3];
            index++;
            updateLengthHandle();
        }

        public void WriteProto<T>(T t)
        {
#if false
			using (MemoryStream ms = new MemoryStream())
            {
				protoSerializer.Serialize(ms, t);
                WriteBytes(ms.ToArray());
            }
#else
			using (MemoryStream ms = new MemoryStream())
			{
				Serializer.Serialize<T>(ms, t);
				WriteBytes(ms.ToArray());
			}
#endif
        }

        public void WriteStruct(byte cmd, byte para, byte[] bytes)
        {
            MsgCmd = cmd;
            MsgParam = para;

            for (int i = 0; i < bytes.Length; ++i)
            {
                WriteByte(bytes[i]);
            }
        }

        public byte[] ReadStruct(int len)
        {
            byte[] ret = new byte[len];

            for (int i = 0; i < len; i++)
            {
                ret[i] = Buffer[HEAD_SIZE + i];
            }
            index = len;

            return ret;
        }


        public void ReadStruct<T>(T t)
        {
            int stringlen = 0;
            int objCount = 0;
            bool objFlag = false;
            Type type = typeof(T);

            if ((MsgCmd > 0) && (MsgParam > 0))
            {
                FieldInfo[] fieldinfos = type.GetFields();
                foreach (FieldInfo filed in fieldinfos)
                {
                    //消息头已经处理了
                    if ((filed.Name == "byCmd") || (filed.Name == "byParam") || (filed.Name == "dwTimestamp"))
                    {

                    }
                    else if (filed.FieldType == typeof(System.UInt16))
                    {
                        UInt16 value = ReadUInt16();
                        filed.SetValue(t, value);
                    }
                    else if (filed.FieldType == typeof(System.Int32))
                    {
                        Int32 value = ReadInt32();
                        filed.SetValue(t, value);
                    }
                    else if (filed.FieldType == typeof(System.UInt32))
                    {
                        UInt32 value = ReadUInt32();
                        filed.SetValue(t, value);
                    }
                    else if (filed.FieldType == typeof(System.Int64))
                    {
                        UInt64 value = ReadUInt64();
                        filed.SetValue(t, value);
                    }
                    else if (filed.FieldType == typeof(System.UInt64))
                    {
                        UInt64 value = ReadUInt64();
                        filed.SetValue(t, value);
                    }
                    else if (filed.FieldType == typeof(System.Byte))
                    {
                        byte value = ReadByte();
                        filed.SetValue(t, value);
                    }
                    else if (filed.FieldType == typeof(System.Char))
                    {
                        byte value = ReadByte();
                        filed.SetValue(t, value);
                    }
                    else if (filed.FieldType == typeof(System.String))
                    {
                        string value = ReadString(stringlen);
                        filed.SetValue(t, value);
                    }
                    else if (filed.FieldType == typeof(StringLengthFlag))
                    {
                        string name = filed.Name;
                        string newname = name.Replace("L", "0");
                        stringlen = 0;
                        if (!int.TryParse(newname, out stringlen))
                            Debug.LogError("解析StringLengthFlag异常, filedName: " + name);
                    }
                    else if (filed.FieldType == typeof(ArraySizeFlag))
                    {
                        string name = filed.Name;
                        string newname = name.Replace("L", "0");
                        objCount = 0;
                        objFlag = false;
                        if (!int.TryParse(newname, out objCount))
                        {
                            objFlag = true;
                        }
                        else
                        {
                            Debug.LogError("解析ArraySizeFlag异常, filedName: " + name);
                        }
                    }
                    else// if (System.Type.GetTypeCode(filed.FieldType) == TypeCode.Object)
                    {
                        Debug.LogError("有未处理的类型 type: " + filed.FieldType + " filedName: " + filed.Name);
                    }
                }
            }

        }
        public bool WriteStruct<T>(T t)
        {
            int stringlen = 0;
            int objCount = 0;
            bool objFlag = false;
            Type type = typeof(T);

            //取消息头
            byte byCmd = 0;
            byte byParam = 0;
            FieldInfo byCmdField = type.GetField("byCmd");
            if (byCmdField != null)
            {
                object byCmdValue = byCmdField.GetValue(t);
                byCmd = Convert.ToByte(byCmdValue);
            }
            else
            {
                Debug.LogError("解析byCmd出错");
            }

            FieldInfo byParamField = type.GetField("byParam");
            if (byParamField != null)
            {
                object byParamValue = byParamField.GetValue(t);
                byParam = Convert.ToByte(byParamValue);
            }
            else
            {
                Debug.LogError("解析byParam出错");
            }

            if ((byCmd > 0) && (byParam > 0))
            {
                WriteHead(byCmd, byParam);
                FieldInfo[] fieldinfos = type.GetFields();
                foreach (FieldInfo filed in fieldinfos)
                {
                    //消息头单独处理
                    if ((filed.Name == "byCmd") || (filed.Name == "byParam") || (filed.Name == "dwTimestamp"))
                    {

                    }
                    else if (filed.FieldType == typeof(System.UInt16))
                    {
                        object value = filed.GetValue(t);
                        UInt16 convertedValue = Convert.ToUInt16(value);
                        WriteUInt16(convertedValue);
                    }
                    else if (filed.FieldType == typeof(System.Int32))
                    {
                        object value = filed.GetValue(t);
                        Int32 convertedValue = Convert.ToInt32(value);
                        WriteInt32(convertedValue);
                    }
                    else if (filed.FieldType == typeof(System.UInt32))
                    {
                        object value = filed.GetValue(t);
                        UInt32 convertedValue = Convert.ToUInt32(value);
                        WriteUInt32(convertedValue);
                    }
                    else if (filed.FieldType == typeof(System.Int64))
                    {
                        object value = filed.GetValue(t);
                        Int64 convertedValue = Convert.ToInt64(value);
                        WriteUInt64((UInt64)convertedValue);
                    }
                    else if (filed.FieldType == typeof(System.UInt64))
                    {
                        object value = filed.GetValue(t);
                        UInt64 convertedValue = Convert.ToUInt64(value);
                        WriteUInt64(convertedValue);
                    }
                    else if (filed.FieldType == typeof(System.Byte))
                    {
                        object value = filed.GetValue(t);
                        byte convertedValue = Convert.ToByte(value);
                        WriteByte(convertedValue);
                    }
                    else if (filed.FieldType == typeof(System.Char))
                    {
                        object value = filed.GetValue(t);
                        char convertedValue = Convert.ToChar(value);
                        WriteByte((byte)convertedValue);
                    }
                    else if (filed.FieldType == typeof(System.String))
                    {
                        object value = filed.GetValue(t);
                        string convertedValue = Convert.ToString(value);
                        WriteString(convertedValue, stringlen);
                    }
                    else if (filed.FieldType == typeof(StringLengthFlag))
                    {
                        string name = filed.Name;
                        string newname = name.Replace("L", "0");
                        stringlen = 0;
                        if (!int.TryParse(newname, out stringlen))
                            Debug.LogError("解析StringLengthFlag异常, filedName: " + name);
                    }
                    else if (filed.FieldType == typeof(ArraySizeFlag))
                    {
                        string name = filed.Name;
                        string newname = name.Replace("L", "0");
                        objCount = 0;
                        objFlag = false;
                        if (!int.TryParse(newname, out objCount))
                        {
                            objFlag = true;
                        }
                        else
                        {
                            Debug.LogError("解析ArraySizeFlag异常, filedName: " + name);
                        }
                    }
                    else// if (System.Type.GetTypeCode(filed.FieldType) == TypeCode.Object)
                    {
                        Debug.LogError("有未处理的类型 type: " + filed.FieldType + " filedName: " + filed.Name);
                    }
                }
                return true;
            }
            return false;
        }


        #endregion

        #region Compress
        public void Compress()
        {
            if (this.BodyLength > 32)
            {
                compress(this, this);
                WriteUInt16(Buffer, 0, (UInt16)BodyLength);
                WriteByte(Buffer, 3, (Byte)64);
            }
        }

        public void Decompress()
        {
            if (IsCompress())
            {
                ReadLength();
                deCompress(this, this);
                WriteUInt16(Buffer, 0, (UInt16)BodyLength);
                WriteByte(Buffer, 3, (Byte)0);
            }
        }

        public void FillZero()
        {
            // 补零
            int mod = 8 - Length % 8;
            if (0 != mod)
            {
                for (int i = 0; i < mod; i++)
                {
                    Buffer[HEAD_SIZE + index] = 0;
                    index++;
                }
                updateLengthHandle();
            }
        }
    
        protected bool IsCompress()
        {
            UInt32 result = (UInt32)(Buffer[0] + (Buffer[1] << 8) + (Buffer[2] << 16) + (Buffer[3] << 24));

            UInt32 PACKET_ZIP =	0x40000000;				/**< 数据包压缩标志 */

            if (PACKET_ZIP == (PACKET_ZIP & result))
            {
                return true;
            }
            return false;
        }



    private static DataBuffer dataBuffer = new DataBuffer(1024 * 64);
    private static void compress(UMessage data, UMessage target)
    {
        dataBuffer.Length = 0;
        using (MemoryStream ms = new MemoryStream(dataBuffer.Buffer))
        {
            using (DeflaterOutputStream zipStream = new DeflaterOutputStream(ms))
            {
                zipStream.Write(data.Buffer, HEAD_SIZE, data.BodyLength);
                zipStream.Finish();

                target.BodyLength = (int)ms.Position;
                target.Length = target.BodyLength + HEAD_SIZE;
                target.index = target.BodyLength;

                for (int i = 0; i < target.BodyLength; i++)
                {
                    target.Buffer[i + HEAD_SIZE] = dataBuffer.Buffer[i];
                }
            }
        }
    }
    private static void deCompress(UMessage data, UMessage target)
    {
        using (MemoryStream ms = new MemoryStream(data.Buffer, HEAD_SIZE, data.BodyLength))
        {
            ms.Seek(0, SeekOrigin.Begin);
            using (InflaterInputStream zipStream = new InflaterInputStream(ms))
            {
                dataBuffer.Length = zipStream.Read(dataBuffer.Buffer, 0, dataBuffer.Buffer.Length);
            }
        }
        for (int i = 0; i < dataBuffer.Length; i++)
        {
            target.Buffer[i + HEAD_SIZE] = dataBuffer.Buffer[i];
        }

        target.BodyLength = dataBuffer.Length;
        target.Length = target.BodyLength + HEAD_SIZE;

        target.index = 0;
    }
    public static void ZipDecompress(byte[] data, MemoryStream target)
    {
        using (MemoryStream ms = new MemoryStream(data, 0, data.Length))
        {
            ms.Seek(0, SeekOrigin.Begin);
            using (InflaterInputStream zipStream = new InflaterInputStream(ms))
            {
                int b = zipStream.ReadByte();
                while (b > -1)
                {
                    target.WriteByte((byte)b);
                    b = zipStream.ReadByte();
                }
            }
        }
    }
    private class DataBuffer
    {
        public DataBuffer(int size)
        {
            Buffer = new byte[size];
        }
        public byte[] Buffer;
        public int Length = 0;
    }
    #endregion

    #region DES
    private static byte[] desKey;
    private static DESCryptoServiceProvider des;
    private static void initDes()
    {
        if (des == null)
        {
            des = new DESCryptoServiceProvider();
            des.IV = des.Key = desKey = new byte[] { 97, 97, 97, 97, 97, 97, 97, 97};
        }
    }

    public static CEncrypt DESEncryptHandle = new CEncrypt(true); // 是否加密，true为加密，false为不加密

    public void CompressAndEncrypt()
    {
        Compress();
        FillZero();
        DESEncrypt();
    }

    public void CompressAndEncryptRC5()
    {
        Compress();
        FillZero();
        RC5Encrypt();
    }

    public void DESEncrypt()
    {
        DESEncryptHandle.encdec_des(Buffer,0,Length,true);
    }

    public void DESDencryptStep1()
    {
        DESEncryptHandle.encdec_des(Buffer, 0, 8, false);
        ReadLength();
    }

    public void DESDencryptStep2()
    {
        DESEncryptHandle.encdec_des(Buffer, 8, Length-8, false);
    }

    #endregion

        #region RC5
        private static string recKey = "2123544143372513";
        private static byte[] data = new byte[16] { 0x3f, 0x79, 0xd5, 0xe2, 0x4a, 0x8c, 0xb6, 0xc1, 0xaf, 0x31, 0x5e, 0xc7, 0xeb, 0x9d, 0x6e, 0xcb };
        private static int rounds = 12;
        private static uint RC5_32_P = 0xB7E15163;
        private static uint RC5_32_Q = 0x9E3779B9;
        private static uint RC5_32_MASK = 0xffffffff;
        //private static string recKey = Encoding.Default.GetString(recKeyData);
        private static uint[] rC5KeyData = null, rc5L = new uint[13], rc5R = new uint[13];
        private static byte[] rC5output = new byte[16], rC5Input = new byte[8];
        private static uint rc5A = 0, rc5B = 0;
        private static int rc5m = 0, rc5Index = 0, rc5Tmp = 0;
        //*****
        public static void generateSubKey()
        {
            if (rC5KeyData != null) return;
            rC5KeyData = new uint[26];    //t=2r+2,r=12,t=26
            uint p32 = 0xB7E15163;  //Odd((e-2)*2^w),w=32
            uint q32 = 0x9E3779B9;  //Odd((Φ-1)*2^w),w=32
            //int i, j;

            //rC5KeyData[0] = p32;
            //for (i = 1; i < 26; i++)
            //{
            //    rC5KeyData[i] = rC5KeyData[i - 1] + q32;
            //}
            //Encoding ascii = Encoding.ASCII;
            //Encoding unicode = Encoding.Unicode;
            ////byte[] asciiBytes = Encoding.Convert(unicode, ascii, unicode.GetBytes(recKey));
            //byte[] asciiBytes = recKeyData;
            //char[] asciiChars = new char[ascii.GetCharCount(asciiBytes, 0, asciiBytes.Length)];
            //ascii.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0);
            //uint[] L = new uint[4];    //c=b/u,u=w/8,w=32,b=16,c=4
            //for (i = 0; i < 16; i++)
            //{
            //    L[i / 4] += (uint)(asciiChars[i] & 0xFF) << (8 * (i % 4));
            //}

            //int k = 78; //3*MAX(t.c),t=26,c=4
            //uint A = 0;
            //uint B = 0;
            //for (i = 0, j = 0; k > 0; k--)
            //{
            //    A = rotL(rC5KeyData[i] + A + B, 3, 32);
            //    rC5KeyData[i] = A;
            //    B = rotL(L[j] + A + B, A + B, 32);
            //    L[j] = B;
            //    i = (i + 1) % 26;   //mod(t),t=26
            //    j = (j + 1) % 4;    //mod(c),c=4
            //}

            ///////////////////////////////////////////////////////////////////////////////
            //SetKey
            uint[] L = new uint[64];
            uint l = 0;
            uint ll = 0;
            uint A;
            uint B;
            uint[] S = new uint[34];
            uint k;
            int i;
            int j;
            int m;
            int c;
            int t;
            int ii;
            int jj;

            j = 0;
            //*****
            int len = 16;
            uint index = 0;
            //byte[] data = new byte[14];
            for (i = 0; i <= (len - 8); i += 8)
            {
                c2l(data, ref l, ref index);
                L[j++] = l;
                c2l(data, ref l, ref index);
                L[j++] = l;
            }
            ii = len - i;
            if (ii != 0)
            {
                k = (uint)len & 0x07;
                c2ln(data, ref l, ref ll, ref index, k);
                L[j + 0] = l;
                L[j + 1] = ll;
            }

            c = (len + 3) / 4;
            t = (rounds + 1) * 2;
            S[0] = RC5_32_P;
            for (i = 1; i < t; i++)
                S[i] = (S[i - 1] + RC5_32_Q) & RC5_32_MASK;

            j = (t > c) ? t : c;
            j *= 3;
            ii = jj = 0;
            A = B = 0;
            for (i = 0; i < j; i++)
            {
                k = (S[ii] + A + B) & RC5_32_MASK;
                A = S[ii] = ROTATE_l32(k, 3);
                m = (int)(A + B);
                k = (L[jj] + A + B) & RC5_32_MASK;
                B = L[jj] = ROTATE_l32(k, m);
                if (++ii >= t) ii = 0;
                if (++jj >= c) jj = 0;
            }

            for (int tmp = 0; tmp < 26; ++tmp)
            {
                rC5KeyData[tmp] = S[tmp];
            }

            Debug.Log("generateSubKey completed!");
        }


        /// <summary>
        /// c2l
        /// </summary>
        /// <param name="state"></param>
        /// <param name="l"></param>
        public static void c2l(byte[] c, ref uint l, ref uint index)
        {
            l = c[index];
            index++;
            l |= (uint)c[index] << 8;
            index++;
            l |= (uint)c[index] << 16;
            index++;
            l |= (uint)c[index] << 24;
            index++;
        }

        //*****这个函数没执行到，未做测试
        public static void c2ln(byte[] c, ref uint l1, ref uint l2, ref uint index, uint n)
        {
            l1 = 0;
            l2 = 0;
            
           switch (n)
           {
            case 8:
                {
                    index--;
                    l2 = (uint)c[index] << 24;
                    break;
                }
            case 7:
                {
                    index--;
                    l2 = (uint)c[index] << 16;
                    break;
                }
            case 6:
                {
                    index--;
                    l2 = (uint)c[index] << 8;
                    break;
                }
            case 5:
                {
                    index--;
                    l2 = (uint)c[index];
                    break;
                }

            case 4:
                {
                    index--;
                    l1 = (uint)c[index] << 24;
                    break;
                }
            case 3:
                {
                    l1 = (uint)c[index] << 16;
                    break;
                }
            case 2:
                {
                    index--;
                    l1 = (uint)c[index] << 8;
                    break;
                }
            case 1:
                {
                    index--;
                    l1 = (uint)c[index];
                    break;
                }

           }

        }

        public static uint ROTATE_l32(uint x, int y)
        {
            return ((x << (int)(y & 0xFF)) | (x >> (int)((32 - (y & 0xFF)))));
        }

        public static uint ROTATE_r32(uint x, uint y)
        {
            return ((x >> (int)(y & 0xFF)) | (x << (int)((32 - (y & 0xFF)))));
        }



        public static byte[] rC5Decrypt(byte[] input)
        {
            for (rc5Tmp = 0; rc5Tmp < 13; rc5Tmp++)
            {
                rc5L[rc5Tmp] = rc5R[rc5Tmp] = 0;
            }
            for (rc5Tmp = 0; rc5Tmp < 4; rc5Tmp++)
            {
                rc5L[12] += (uint)(input[rc5Tmp] & 0xFF) << (8 * rc5Tmp);
                rc5R[12] += (uint)(input[rc5Tmp + 4] & 0xFF) << (8 * rc5Tmp);
            }
            for (rc5Tmp = 12; rc5Tmp > 0; rc5Tmp--)
            {
                rc5R[rc5Tmp - 1] = (rotR((rc5R[rc5Tmp] - rC5KeyData[2 * rc5Tmp + 1]), rc5L[rc5Tmp], 32) ^ rc5L[rc5Tmp]);
                rc5L[rc5Tmp - 1] = (rotR((rc5L[rc5Tmp] - rC5KeyData[2 * rc5Tmp]), rc5R[rc5Tmp - 1], 32) ^ rc5R[rc5Tmp - 1]);
            }
            uint rc5A = 0;
            uint rc5B = 0;
            rc5A = rc5L[0] - rC5KeyData[0];
            rc5B = rc5R[0] - rC5KeyData[1];
            for (rc5Tmp = 0; rc5Tmp < 4; rc5Tmp++)
            {
                rC5output[rc5Tmp] = (byte)((rc5A >> (8 * rc5Tmp)) & 0xFF);
            }
            for (rc5Tmp = 0; rc5Tmp < 4; rc5Tmp++)
            {
                rC5output[rc5Tmp + 4] = (byte)((rc5B >> (8 * rc5Tmp)) & 0xFF);
            }
            return rC5output;
        }
        private byte[] rC5Encrypt(byte[] input)
        {
            for (rc5Tmp = 0; rc5Tmp < 13; rc5Tmp++)
            {
                rc5L[rc5Tmp] = rc5R[rc5Tmp] = 0;
            }
            rc5A = 0;
            rc5B = 0;
            for (rc5Tmp = 0; rc5Tmp < 4; rc5Tmp++)
            {
                rc5A += (uint)(input[rc5Tmp] & 0xFF) << (8 * rc5Tmp);
                rc5B += (uint)(input[rc5Tmp + 4] & 0xFF) << (8 * rc5Tmp);
            }
            rc5L[0] = (rc5A + rC5KeyData[0]);
            rc5R[0] = (rc5B + rC5KeyData[1]);
            for (rc5Tmp = 1; rc5Tmp <= 12; rc5Tmp++)
            {
                rc5L[rc5Tmp] = (rotL((rc5L[rc5Tmp - 1] ^ rc5R[rc5Tmp - 1]), rc5R[rc5Tmp - 1], 32) + rC5KeyData[2 * rc5Tmp]);
                rc5R[rc5Tmp] = (rotL((rc5R[rc5Tmp - 1] ^ rc5L[rc5Tmp]), rc5L[rc5Tmp], 32) + rC5KeyData[2 * rc5Tmp + 1]);
            }
            for (rc5Tmp = 0; rc5Tmp < 4; rc5Tmp++)
            {
                rC5output[rc5Tmp] = (byte)((rc5L[12] >> (8 * rc5Tmp)) & 0xFF);
            }
            for (rc5Tmp = 0; rc5Tmp < 4; rc5Tmp++)
            {
                rC5output[rc5Tmp + 4] = (byte)((rc5R[12] >> (8 * rc5Tmp)) & 0xFF);
            }
            return rC5output;
        }
        public static uint rotL(uint x, uint y, int w)
        {
            return ((x << (int)(y & 0xFF)) | (x >> (int)((w - (y & 0xFF)))));
        }
        public static uint rotR(uint x, uint y, int w)
        {
            return ((x >> (int)(y & 0xFF)) | (x << (int)((w - (y & 0xFF)))));
        }
        public void RC5Encrypt()
        {
            generateSubKey();
            //rc5m = (Length - HEAD_SIZE) % 8;
            rc5m = Length % 8;
            if (rc5m != 0) Length += 8 - rc5m;
            rc5Tmp = 0;
            //for (rc5Index = HEAD_SIZE; rc5Index < Length; rc5Index++)
            for (rc5Index = 0; rc5Index < Length; rc5Index++)
            {
                rC5Input[rc5Tmp] = Buffer[rc5Index];
                rc5Tmp++;
                if (rc5Tmp == 8)
                {
                    rC5Encrypt(rC5Input);
                    rc5Index = rc5Index - 7;
                    for (rc5Tmp = 0; rc5Tmp < 8; rc5Tmp++)
                    {
                        Buffer[rc5Index] = rC5output[rc5Tmp];
                        rc5Index++;
                    }
                    rc5Index--;
                    rc5Tmp = 0;
                }
            }
        }
        public void RC5Decrypt()
        {
            generateSubKey();
            //rc5m = (Length - HEAD_SIZE) % 8;
            rc5m = Length % 8;
            if (rc5m != 0) Length += 8 - rc5m;
            rc5Tmp = 0;
            //for (rc5Index = HEAD_SIZE; rc5Index < Length; rc5Index++)
            for (rc5Index = 0; rc5Index < Length; rc5Index++)
            {
                rC5Input[rc5Tmp] = Buffer[rc5Index];
                rc5Tmp++;
                if (rc5Tmp == 8)
                {
                    rC5Decrypt(rC5Input);
                    rc5Index = rc5Index - 7;
                    for (rc5Tmp = 0; rc5Tmp < 8; rc5Tmp++)
                    {
                        Buffer[rc5Index] = rC5output[rc5Tmp];
                        rc5Index++;
                    }
                    rc5Index--;
                    rc5Tmp = 0;
                }
            }
        }
        #endregion

        public static UInt16 GetMsgId(byte cmdId, byte cmdParam)
        {
            return (UInt16)(cmdId * 256 + cmdParam);
        }

        public static UInt16 GetMsgId(stNullUserCmd st)
        {
            return (UInt16)(st.byCmd * 256 + st.byParam);
        }
    }
}
