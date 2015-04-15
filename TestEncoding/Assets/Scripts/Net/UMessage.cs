
public class StringLengthFlag
{
    
}

public class ArraySizeFlag
{
    
}

namespace Net
{
    using GBKEncoding;
    using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
    using ProtoBuf;
    using System;
    using System.IO;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text;
    using UnityEngine;

    public class UMessage
    {
        public int BodyLength;
        public byte[] Buffer;
        private static byte[] data = new byte[] { 0x3f, 0x79, 0xd5, 0xe2, 0x4a, 140, 0xb6, 0xc1, 0xaf, 0x31, 0x5e, 0xc7, 0xeb, 0x9d, 110, 0xcb };
        private static DataBuffer dataBuffer = new DataBuffer(0x10000);
        private static DESCryptoServiceProvider des;
        public static CEncrypt DESEncryptHandle = new CEncrypt(true);
        private static byte[] desKey;
        public const int HEAD_SIZE = 4;
        private int index;
        public bool IsPack;
        public int Length;
        public byte MsgCmd;
        public ushort MsgId;
        public byte MsgParam;
        private static uint RC5_32_MASK = uint.MaxValue;
        private static uint RC5_32_P = 0xb7e15163;
        private static uint RC5_32_Q = 0x9e3779b9;
        private static uint rc5A = 0;
        private static uint rc5B = 0;
        private static int rc5Index = 0;
        private static byte[] rC5Input = new byte[8];
        private static uint[] rC5KeyData = null;
        private static uint[] rc5L = new uint[13];
        private static int rc5m = 0;
        private static byte[] rC5output = new byte[0x10];
        private static uint[] rc5R = new uint[13];
        private static int rc5Tmp = 0;
        private static string recKey = "2123544143372513";
        private static int rounds = 12;

        public UMessage()
        {
            this.Reset();
            initDes();
        }

        public static void c2l(byte[] c, ref uint l, ref uint index)
        {
            l = c[index];
            index++;
            l |= (uint)c[index] << 8;
            index++;
            l |= (uint)c[index] << 0x10;
            index++;
            l |= (uint)c[index] << 0x18;
            index++;
        }

        public static void c2ln(byte[] c, ref uint l1, ref uint l2, ref uint index, uint n)
        {
            l1 = 0;
            l2 = 0;
            switch (n)
            {
                case 1:
                    index--;
                    l1 = c[index];
                    break;

                case 2:
                    index--;
                    l1 = (uint) (c[index] << 8);
                    break;

                case 3:
                    l1 = (uint) (c[index] << 0x10);
                    break;

                case 4:
                    index--;
                    l1 = (uint) (c[index] << 0x18);
                    break;

                case 5:
                    index--;
                    l2 = c[index];
                    break;

                case 6:
                    index--;
                    l2 = (uint) (c[index] << 8);
                    break;

                case 7:
                    index--;
                    l2 = (uint) (c[index] << 0x10);
                    break;

                case 8:
                    index--;
                    l2 = (uint) (c[index] << 0x18);
                    break;
            }
        }

        public void Clear()
        {
            this.index = 0;
            this.Length = 0;
            this.BodyLength = 0;
            this.Buffer = null;
        }

        private static void compress(UMessage data, UMessage target)
        {
            dataBuffer.Length = 0;
            using (MemoryStream stream = new MemoryStream(dataBuffer.Buffer))
            {
                using (DeflaterOutputStream stream2 = new DeflaterOutputStream(stream))
                {
                    stream2.Write(data.Buffer, 4, data.BodyLength);
                    stream2.Finish();
                    target.BodyLength = (int) stream.Position;
                    target.Length = target.BodyLength + 4;
                    target.index = target.BodyLength;
                    for (int i = 0; i < target.BodyLength; i++)
                    {
                        target.Buffer[i + 4] = dataBuffer.Buffer[i];
                    }
                }
            }
        }

        public void Compress()
        {
            if (this.BodyLength > 0x20)
            {
                compress(this, this);
                this.WriteUInt16(this.Buffer, 0, (ushort) this.BodyLength);
                this.WriteByte(this.Buffer, 3, 0x40);
            }
        }

        public void CompressAndEncrypt()
        {
            this.Compress();
            this.FillZero();
            this.DESEncrypt();
        }

        public void CompressAndEncryptRC5()
        {
            this.Compress();
            this.FillZero();
            this.RC5Encrypt();
        }

        public UMessage copyNew()
        {
            UMessage message = new UMessage {
                index = this.index,
                Length = this.Length,
                BodyLength = this.BodyLength,
                MsgId = this.MsgId,
                IsPack = this.IsPack
            };
            Array.Copy(this.Buffer, message.Buffer, this.Length);
            return message;
        }

        private static void deCompress(UMessage data, UMessage target)
        {
            using (MemoryStream stream = new MemoryStream(data.Buffer, 4, data.BodyLength))
            {
                stream.Seek(0L, SeekOrigin.Begin);
                using (InflaterInputStream stream2 = new InflaterInputStream(stream))
                {
                    dataBuffer.Length = stream2.Read(dataBuffer.Buffer, 0, dataBuffer.Buffer.Length);
                }
            }
            for (int i = 0; i < dataBuffer.Length; i++)
            {
                target.Buffer[i + 4] = dataBuffer.Buffer[i];
            }
            target.BodyLength = dataBuffer.Length;
            target.Length = target.BodyLength + 4;
            target.index = 0;
        }

        public void Decompress()
        {
            if (this.IsCompress())
            {
                this.ReadLength();
                deCompress(this, this);
                this.WriteUInt16(this.Buffer, 0, (ushort) this.BodyLength);
                this.WriteByte(this.Buffer, 3, 0);
            }
        }

        public void DESDencryptStep1()
        {
            DESEncryptHandle.encdec_des(this.Buffer, 0, 8, false);
            this.ReadLength();
        }

        public void DESDencryptStep2()
        {
            DESEncryptHandle.encdec_des(this.Buffer, 8, this.Length - 8, false);
        }

        public void DESEncrypt()
        {
            DESEncryptHandle.encdec_des(this.Buffer, 0, this.Length, true);
        }

        public void FillZero()
        {
            int num = 8 - (this.Length % 8);
            if (num != 0)
            {
                for (int i = 0; i < num; i++)
                {
                    this.Buffer[4 + this.index] = 0;
                    this.index++;
                }
                this.updateLengthHandle();
            }
        }

        public static void generateSubKey()
        {
            if (rC5KeyData == null)
            {
                uint num6;
                uint num7;
                int num14;
                rC5KeyData = new uint[0x1a];
                uint[] numArray = new uint[0x40];
                uint l = 0;
                uint num4 = 0;
                uint[] numArray2 = new uint[0x22];
                int index = 0;
                int num15 = 0x10;
                uint num16 = 0;
                int num8 = 0;
                while (num8 <= (num15 - 8))
                {
                    c2l(data, ref l, ref num16);
                    numArray[index++] = l;
                    c2l(data, ref l, ref num16);
                    numArray[index++] = l;
                    num8 += 8;
                }
                if ((num15 - num8) != 0)
                {
                    num7 = (uint) (num15 & 7);
                    c2ln(data, ref l, ref num4, ref num16, num7);
                    numArray[index] = l;
                    numArray[index + 1] = num4;
                }
                int num11 = (num15 + 3) / 4;
                int num12 = (rounds + 1) * 2;
                numArray2[0] = RC5_32_P;
                num8 = 1;
                while (num8 < num12)
                {
                    numArray2[num8] = (numArray2[num8 - 1] + RC5_32_Q) & RC5_32_MASK;
                    num8++;
                }
                index = (num12 <= num11) ? num11 : num12;
                index *= 3;
                int num13 = num14 = 0;
                uint num5 = num6 = 0;
                for (num8 = 0; num8 < index; num8++)
                {
                    num7 = ((numArray2[num13] + num5) + num6) & RC5_32_MASK;
                    num5 = numArray2[num13] = ROTATE_l32(num7, 3);
                    int y = (int) (num5 + num6);
                    num7 = ((numArray[num14] + num5) + num6) & RC5_32_MASK;
                    num6 = numArray[num14] = ROTATE_l32(num7, y);
                    if (++num13 >= num12)
                    {
                        num13 = 0;
                    }
                    if (++num14 >= num11)
                    {
                        num14 = 0;
                    }
                }
                for (int i = 0; i < 0x1a; i++)
                {
                    rC5KeyData[i] = numArray2[i];
                }
                Debug.Log("generateSubKey completed!");
            }
        }

        public static ushort GetMsgId(stNullUserCmd st)
        {
            return (ushort) ((st.byCmd * 0x100) + st.byParam);
        }

        public static ushort GetMsgId(byte cmdId, byte cmdParam)
        {
            return (ushort) ((cmdId * 0x100) + cmdParam);
        }

        private static void initDes()
        {
            if (des == null)
            {
                des = new DESCryptoServiceProvider();
                byte[] buffer = desKey = new byte[] { 0x61, 0x61, 0x61, 0x61, 0x61, 0x61, 0x61, 0x61 };
                des.Key = buffer;
                des.IV = buffer;
            }
        }

        protected bool IsCompress()
        {
            uint num = (uint) (((this.Buffer[0] + (this.Buffer[1] << 8)) + (this.Buffer[2] << 0x10)) + (this.Buffer[3] << 0x18));
            uint num2 = 0x40000000;
            return (num2 == (num2 & num));
        }

        public static byte[] rC5Decrypt(byte[] input)
        {
            rc5Tmp = 0;
            while (rc5Tmp < 13)
            {
                rc5L[rc5Tmp] = rc5R[rc5Tmp] = 0;
                rc5Tmp++;
            }
            rc5Tmp = 0;
            while (rc5Tmp < 4)
            {
                rc5L[12] += (uint) ((input[rc5Tmp] & 0xff) << (8 * rc5Tmp));
                rc5R[12] += (uint) ((input[rc5Tmp + 4] & 0xff) << (8 * rc5Tmp));
                rc5Tmp++;
            }
            rc5Tmp = 12;
            while (rc5Tmp > 0)
            {
                rc5R[rc5Tmp - 1] = rotR(rc5R[rc5Tmp] - rC5KeyData[(2 * rc5Tmp) + 1], rc5L[rc5Tmp], 0x20) ^ rc5L[rc5Tmp];
                rc5L[rc5Tmp - 1] = rotR(rc5L[rc5Tmp] - rC5KeyData[2 * rc5Tmp], rc5R[rc5Tmp - 1], 0x20) ^ rc5R[rc5Tmp - 1];
                rc5Tmp--;
            }
            uint num = 0;
            uint num2 = 0;
            num = rc5L[0] - rC5KeyData[0];
            num2 = rc5R[0] - rC5KeyData[1];
            rc5Tmp = 0;
            while (rc5Tmp < 4)
            {
                rC5output[rc5Tmp] = (byte) ((num >> (8 * rc5Tmp)) & 0xff);
                rc5Tmp++;
            }
            rc5Tmp = 0;
            while (rc5Tmp < 4)
            {
                rC5output[rc5Tmp + 4] = (byte) ((num2 >> (8 * rc5Tmp)) & 0xff);
                rc5Tmp++;
            }
            return rC5output;
        }

        public void RC5Decrypt()
        {
            generateSubKey();
            rc5m = this.Length % 8;
            if (rc5m != 0)
            {
                this.Length += 8 - rc5m;
            }
            rc5Tmp = 0;
            rc5Index = 0;
            while (rc5Index < this.Length)
            {
                rC5Input[rc5Tmp] = this.Buffer[rc5Index];
                rc5Tmp++;
                if (rc5Tmp == 8)
                {
                    rC5Decrypt(rC5Input);
                    rc5Index -= 7;
                    rc5Tmp = 0;
                    while (rc5Tmp < 8)
                    {
                        this.Buffer[rc5Index] = rC5output[rc5Tmp];
                        rc5Index++;
                        rc5Tmp++;
                    }
                    rc5Index--;
                    rc5Tmp = 0;
                }
                rc5Index++;
            }
        }

        private byte[] rC5Encrypt(byte[] input)
        {
            rc5Tmp = 0;
            while (rc5Tmp < 13)
            {
                rc5L[rc5Tmp] = rc5R[rc5Tmp] = 0;
                rc5Tmp++;
            }
            rc5A = 0;
            rc5B = 0;
            rc5Tmp = 0;
            while (rc5Tmp < 4)
            {
                rc5A += (uint) ((input[rc5Tmp] & 0xff) << (8 * rc5Tmp));
                rc5B += (uint) ((input[rc5Tmp + 4] & 0xff) << (8 * rc5Tmp));
                rc5Tmp++;
            }
            rc5L[0] = rc5A + rC5KeyData[0];
            rc5R[0] = rc5B + rC5KeyData[1];
            rc5Tmp = 1;
            while (rc5Tmp <= 12)
            {
                rc5L[rc5Tmp] = rotL(rc5L[rc5Tmp - 1] ^ rc5R[rc5Tmp - 1], rc5R[rc5Tmp - 1], 0x20) + rC5KeyData[2 * rc5Tmp];
                rc5R[rc5Tmp] = rotL(rc5R[rc5Tmp - 1] ^ rc5L[rc5Tmp], rc5L[rc5Tmp], 0x20) + rC5KeyData[(2 * rc5Tmp) + 1];
                rc5Tmp++;
            }
            rc5Tmp = 0;
            while (rc5Tmp < 4)
            {
                rC5output[rc5Tmp] = (byte) ((rc5L[12] >> (8 * rc5Tmp)) & 0xff);
                rc5Tmp++;
            }
            rc5Tmp = 0;
            while (rc5Tmp < 4)
            {
                rC5output[rc5Tmp + 4] = (byte) ((rc5R[12] >> (8 * rc5Tmp)) & 0xff);
                rc5Tmp++;
            }
            return rC5output;
        }

        public void RC5Encrypt()
        {
            generateSubKey();
            rc5m = this.Length % 8;
            if (rc5m != 0)
            {
                this.Length += 8 - rc5m;
            }
            rc5Tmp = 0;
            rc5Index = 0;
            while (rc5Index < this.Length)
            {
                rC5Input[rc5Tmp] = this.Buffer[rc5Index];
                rc5Tmp++;
                if (rc5Tmp == 8)
                {
                    this.rC5Encrypt(rC5Input);
                    rc5Index -= 7;
                    rc5Tmp = 0;
                    while (rc5Tmp < 8)
                    {
                        this.Buffer[rc5Index] = rC5output[rc5Tmp];
                        rc5Index++;
                        rc5Tmp++;
                    }
                    rc5Index--;
                    rc5Tmp = 0;
                }
                rc5Index++;
            }
        }

        public bool ReadBool()
        {
            byte num = this.Buffer[4 + this.index];
            this.index++;
            if (num == 0)
            {
                return false;
            }
            return true;
        }

        public byte ReadByte()
        {
            byte num = this.Buffer[4 + this.index];
            this.index++;
            return num;
        }

        public byte[] ReadBytes()
        {
            int num = ReadUInt16(this.Buffer, 4 + this.index);
            byte[] buffer = new byte[num];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = this.Buffer[((4 + this.index) + 2) + i];
            }
            this.index += num + 2;
            return buffer;
        }

        public byte[] ReadBytes(int length)
        {
            byte[] buffer = new byte[length];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = this.Buffer[(4 + this.index) + i];
            }
            this.index += length;
            return buffer;
        }

        public float ReadFloat()
        {
            float num = BitConverter.ToSingle(this.Buffer, 4 + this.index);
            this.index += 4;
            return num;
        }

        public void ReadHead()
        {
            this.MsgId = ReadUInt16(this.Buffer, 4);
            this.MsgCmd = this.Buffer[4];
            this.MsgParam = this.Buffer[5];
            this.index += 2;
            this.ReadTimeStamp();
            this.index += 4;
        }

        public short ReadInt16()
        {
            short num = (short) (this.Buffer[4 + this.index] + (this.Buffer[(4 + this.index) + 1] << 8));
            this.index += 2;
            return num;
        }

        public int ReadInt32()
        {
            int num = ((this.Buffer[4 + this.index] + (this.Buffer[(4 + this.index) + 1] << 8)) + (this.Buffer[(4 + this.index) + 2] << 0x10)) + (this.Buffer[(4 + this.index) + 3] << 0x18);
            this.index += 4;
            return num;
        }

        public void ReadLength()
        {
            this.BodyLength = this.Buffer[0] + (this.Buffer[1] << 8);
            this.Length = this.BodyLength + 4;
            this.index = 0;
            this.IsPack = true;
        }

        public T ReadProto<T>()
        {
            using (MemoryStream stream = new MemoryStream(this.ReadBytes()))
            {
                return Serializer.Deserialize<T>(stream);
            }
        }

        public string ReadString()
        {
            int iLength = ReadUInt16(this.Buffer, 4 + this.index);
            string str = GBKEncoder.Read(this.Buffer, (4 + this.index) + 2, iLength);
            this.index += iLength + 2;
            return str;
        }

        public string ReadString(int length)
        {
            string str = GBKEncoder.Read(this.Buffer, 4 + this.index, length);
            this.index += length;
            return str;
        }

        public byte[] ReadStruct(int len)
        {
            byte[] buffer = new byte[len];
            for (int i = 0; i < len; i++)
            {
                buffer[i] = this.Buffer[4 + i];
            }
            this.index = len;
            return buffer;
        }

        public System.Type ReadStruct(System.Type type)
        {
            return type;
        }

        public void ReadStruct<T>(T t)
        {
            int length = 0;
            int result = 0;
            bool flag = false;
            System.Type type = typeof(T);
            if ((this.MsgCmd > 0) && (this.MsgParam > 0))
            {
                foreach (FieldInfo info in type.GetFields())
                {
                    if (((info.Name != "byCmd") && (info.Name != "byParam")) && (info.Name != "dwTimestamp"))
                    {
                        if (info.FieldType == typeof(ushort))
                        {
                            ushort num4 = this.ReadUInt16();
                            info.SetValue(t, num4);
                        }
                        else if (info.FieldType == typeof(int))
                        {
                            int num5 = this.ReadInt32();
                            info.SetValue(t, num5);
                        }
                        else if (info.FieldType == typeof(uint))
                        {
                            uint num6 = this.ReadUInt32();
                            info.SetValue(t, num6);
                        }
                        else if (info.FieldType == typeof(long))
                        {
                            ulong num7 = this.ReadUInt64();
                            info.SetValue(t, num7);
                        }
                        else if (info.FieldType == typeof(ulong))
                        {
                            ulong num8 = this.ReadUInt64();
                            info.SetValue(t, num8);
                        }
                        else if (info.FieldType == typeof(byte))
                        {
                            byte num9 = this.ReadByte();
                            info.SetValue(t, num9);
                        }
                        else if (info.FieldType == typeof(char))
                        {
                            byte num10 = this.ReadByte();
                            info.SetValue(t, num10);
                        }
                        else if (info.FieldType == typeof(string))
                        {
                            string str = this.ReadString(length);
                            info.SetValue(t, str);
                        }
                        else if (info.FieldType == typeof(StringLengthFlag))
                        {
                            string name = info.Name;
                            string s = name.Replace("L", "0");
                            length = 0;
                            if (!int.TryParse(s, out length))
                            {
                                Debug.LogError("解析StringLengthFlag异常, filedName: " + name);
                            }
                        }
                        else if (info.FieldType == typeof(ArraySizeFlag))
                        {
                            string str4 = info.Name;
                            string str5 = str4.Replace("L", "0");
                            result = 0;
                            flag = false;
                            if (!int.TryParse(str5, out result))
                            {
                                flag = true;
                            }
                            else
                            {
                                Debug.LogError("解析ArraySizeFlag异常, filedName: " + str4);
                            }
                        }
                        else
                        {
                            Debug.LogError(string.Concat(new object[] { "有未处理的类型 type: ", info.FieldType, " filedName: ", info.Name }));
                        }
                    }
                }
            }
        }

        public uint ReadTimeStamp()
        {
            return 0;
        }

        public ushort ReadUInt16()
        {
            ushort num = ReadUInt16(this.Buffer, 4 + this.index);
            this.index += 2;
            return num;
        }

        public static ushort ReadUInt16(byte[] buffer, int index)
        {
            return (ushort) (buffer[index] + (buffer[index + 1] << 8));
        }

        public uint ReadUInt32()
        {
            uint num = (uint) (((this.Buffer[4 + this.index] + (this.Buffer[(4 + this.index) + 1] << 8)) + (this.Buffer[(4 + this.index) + 2] << 0x10)) + (this.Buffer[(4 + this.index) + 3] << 0x18));
            this.index += 4;
            return num;
        }

        public ulong ReadUInt64()
        {
            uint num = this.ReadUInt32();
            uint num2 = this.ReadUInt32();
            return (num + (num2 << 0x20));
        }

        public void Reset()
        {
            this.index = 0;
            this.Length = 0;
            this.BodyLength = 0;
            this.Buffer = new byte[0x10000];
            this.IsPack = false;
        }

        public void ResetIndex()
        {
            this.index = 0;
        }

        public static uint ROTATE_l32(uint x, int y)
        {
            return ((x << (y & 0xff)) | (x >> (0x20 - (y & 0xff))));
        }

        public static uint ROTATE_r32(uint x, uint y)
        {
            return ((x >> (int)(y & 0xff)) | (x << (int)(0x20 - (y & 0xff))));
        }

        public static uint rotL(uint x, uint y, int w)
        {
            return ((x << (int)(y & 0xff)) | (x >> (w - ((int) ((ulong) (y & 0xff))))));
        }

        public static uint rotR(uint x, uint y, int w)
        {
            return ((x >> (int)(y & 0xff)) | (x << (w - ((int) ((ulong) (y & 0xff))))));
        }

        private void updateLengthHandle()
        {
            this.BodyLength = this.index;
            this.Length = 4 + this.index;
            this.WriteUInt16(this.Buffer, 0, (ushort) this.BodyLength);
        }

        public void WriteByte(byte b)
        {
            this.Buffer[4 + this.index] = b;
            this.index++;
            this.updateLengthHandle();
        }

        public void WriteByte(byte[] buffer, int index, byte b)
        {
            buffer[index] = b;
        }

        public void WriteBytes(byte[] bytes)
        {
            this.WriteUInt16(this.Buffer, 4 + this.index, (ushort) bytes.Length);
            for (int i = 0; i < bytes.Length; i++)
            {
                this.Buffer[((4 + this.index) + 2) + i] = bytes[i];
            }
            this.index += bytes.Length + 2;
            this.updateLengthHandle();
        }

        public void WriteBytes(byte[] bytes, int nSize)
        {
            for (int i = 0; i < nSize; i++)
            {
                this.Buffer[(4 + this.index) + i] = bytes[i];
            }
            this.index += nSize;
            this.updateLengthHandle();
        }

        public void WriteFloat(float f)
        {
            byte[] bytes = BitConverter.GetBytes(f);
            this.Buffer[4 + this.index] = bytes[0];
            this.index++;
            this.Buffer[4 + this.index] = bytes[1];
            this.index++;
            this.Buffer[4 + this.index] = bytes[2];
            this.index++;
            this.Buffer[4 + this.index] = bytes[3];
            this.index++;
            this.updateLengthHandle();
        }

        public void WriteHead(ushort msgId)
        {
            this.MsgId = msgId;
            this.WriteUInt16(this.MsgId);
            this.WriteTimeStamp(0);
            this.index += 4;
            this.updateLengthHandle();
        }

        public void WriteHead(byte cmd, byte para)
        {
            this.MsgCmd = cmd;
            this.MsgParam = para;
            this.Buffer[4] = this.MsgCmd;
            this.Buffer[5] = this.MsgParam;
            this.WriteTimeStamp(0);
            this.index += 6;
            this.updateLengthHandle();
        }

        public void WriteInt32(int b)
        {
            this.Buffer[4 + this.index] = (byte) ((b << 0x18) >> 0x18);
            this.index++;
            this.Buffer[4 + this.index] = (byte) ((b << 0x10) >> 0x18);
            this.index++;
            this.Buffer[4 + this.index] = (byte) ((b << 8) >> 0x18);
            this.index++;
            this.Buffer[4 + this.index] = (byte) (b >> 0x18);
            this.index++;
            this.updateLengthHandle();
        }

        public void WriteProto<T>(T t)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Serializer.Serialize<T>(stream, t);
                this.WriteBytes(stream.ToArray());
            }
        }

        public void WriteString(string s)
        {
            byte[] buffer = GBKEncoder.ToBytes(s);
            this.WriteUInt16(this.Buffer, 4 + this.index, (ushort) buffer.Length);
            for (int i = 0; i < buffer.Length; i++)
            {
                this.Buffer[((4 + this.index) + 2) + i] = buffer[i];
            }
            this.index += buffer.Length + 2;
            this.updateLengthHandle();
        }

        public void WriteString(string s, int nSize)
        {
            byte[] buffer = GBKEncoder.ToBytes(s);
            for (int i = 0; i < buffer.Length; i++)
            {
                this.Buffer[(4 + this.index) + i] = buffer[i];
            }
            for (int j = buffer.Length; j < nSize; j++)
            {
                this.Buffer[(4 + this.index) + j] = 0;
            }
            this.index += nSize;
            this.updateLengthHandle();
        }

        public bool WriteStruct<T>(T t)
        {
            int nSize = 0;
            int result = 0;
            bool flag = false;
            System.Type type = typeof(T);
            byte cmd = 0;
            byte para = 0;
            FieldInfo field = type.GetField("byCmd");
            if (field != null)
            {
                cmd = Convert.ToByte(field.GetValue(t));
            }
            else
            {
                Debug.LogError("解析byCmd出错");
            }
            FieldInfo info2 = type.GetField("byParam");
            if (info2 != null)
            {
                para = Convert.ToByte(info2.GetValue(t));
            }
            else
            {
                Debug.LogError("解析byParam出错");
            }
            if ((cmd <= 0) || (para <= 0))
            {
                return false;
            }
            this.WriteHead(cmd, para);
            foreach (FieldInfo info3 in type.GetFields())
            {
                if (((info3.Name != "byCmd") && (info3.Name != "byParam")) && (info3.Name != "dwTimestamp"))
                {
                    if (info3.FieldType == typeof(ushort))
                    {
                        ushort b = Convert.ToUInt16(info3.GetValue(t));
                        this.WriteUInt16(b);
                    }
                    else if (info3.FieldType == typeof(int))
                    {
                        int num7 = Convert.ToInt32(info3.GetValue(t));
                        this.WriteInt32(num7);
                    }
                    else if (info3.FieldType == typeof(uint))
                    {
                        uint num8 = Convert.ToUInt32(info3.GetValue(t));
                        this.WriteUInt32(num8);
                    }
                    else if (info3.FieldType == typeof(long))
                    {
                        long num9 = Convert.ToInt64(info3.GetValue(t));
                        this.WriteUInt64((ulong) num9);
                    }
                    else if (info3.FieldType == typeof(ulong))
                    {
                        ulong num10 = Convert.ToUInt64(info3.GetValue(t));
                        this.WriteUInt64(num10);
                    }
                    else if (info3.FieldType == typeof(byte))
                    {
                        byte num11 = Convert.ToByte(info3.GetValue(t));
                        this.WriteByte(num11);
                    }
                    else if (info3.FieldType == typeof(char))
                    {
                        char ch = Convert.ToChar(info3.GetValue(t));
                        this.WriteByte((byte) ch);
                    }
                    else if (info3.FieldType == typeof(string))
                    {
                        string s = Convert.ToString(info3.GetValue(t));
                        this.WriteString(s, nSize);
                    }
                    else if (info3.FieldType == typeof(StringLengthFlag))
                    {
                        string name = info3.Name;
                        string str3 = name.Replace("L", "0");
                        nSize = 0;
                        if (!int.TryParse(str3, out nSize))
                        {
                            Debug.LogError("解析StringLengthFlag异常, filedName: " + name);
                        }
                    }
                    else if (info3.FieldType == typeof(ArraySizeFlag))
                    {
                        string str4 = info3.Name;
                        string str5 = str4.Replace("L", "0");
                        result = 0;
                        flag = false;
                        if (!int.TryParse(str5, out result))
                        {
                            flag = true;
                        }
                        else
                        {
                            Debug.LogError("解析ArraySizeFlag异常, filedName: " + str4);
                        }
                    }
                    else
                    {
                        Debug.LogError(string.Concat(new object[] { "有未处理的类型 type: ", info3.FieldType, " filedName: ", info3.Name }));
                    }
                }
            }
            return true;
        }
        public void WriteStruct(byte cmd, byte para, byte[] bytes)
        {
            this.MsgCmd = cmd;
            this.MsgParam = para;
            for (int i = 0; i < bytes.Length; i++)
            {
                this.WriteByte(bytes[i]);
            }
        }

        public void WriteTimeStamp(uint b)
        {
            this.Buffer[6] = (byte) ((b << 0x18) >> 0x18);
            this.Buffer[7] = (byte) ((b << 0x10) >> 0x18);
            this.Buffer[8] = (byte) ((b << 8) >> 0x18);
            this.Buffer[9] = (byte) (b >> 0x18);
        }

        public void WriteUInt16(ushort b)
        {
            this.WriteUInt16(this.Buffer, 4 + this.index, b);
            this.index += 2;
            this.updateLengthHandle();
        }

        public void WriteUInt16(byte[] buffer, int index, ushort b)
        {
            buffer[index] = (byte) ((b << 0x18) >> 0x18);
            buffer[index + 1] = (byte) ((b << 0x10) >> 0x18);
        }

        public void WriteUInt32(uint b)
        {
            this.Buffer[4 + this.index] = (byte) ((b << 0x18) >> 0x18);
            this.index++;
            this.Buffer[4 + this.index] = (byte) ((b << 0x10) >> 0x18);
            this.index++;
            this.Buffer[4 + this.index] = (byte) ((b << 8) >> 0x18);
            this.index++;
            this.Buffer[4 + this.index] = (byte) (b >> 0x18);
            this.index++;
            this.updateLengthHandle();
        }

        public void WriteUInt64(ulong b)
        {
            this.Buffer[4 + this.index] = (byte) ((b << 0x38) >> 0x38);
            this.index++;
            this.Buffer[4 + this.index] = (byte) ((b << 0x30) >> 0x38);
            this.index++;
            this.Buffer[4 + this.index] = (byte) ((b << 40) >> 0x38);
            this.index++;
            this.Buffer[4 + this.index] = (byte) ((b << 0x20) >> 0x38);
            this.index++;
            this.Buffer[4 + this.index] = (byte) ((b << 0x18) >> 0x38);
            this.index++;
            this.Buffer[4 + this.index] = (byte) ((b << 0x10) >> 0x38);
            this.index++;
            this.Buffer[4 + this.index] = (byte) ((b << 8) >> 0x38);
            this.index++;
            this.Buffer[4 + this.index] = (byte) (b >> 0x38);
            this.index++;
            this.updateLengthHandle();
        }

        public void WriteUTFString(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            this.WriteUInt16(this.Buffer, 4 + this.index, (ushort) bytes.Length);
            for (int i = 0; i < bytes.Length; i++)
            {
                this.Buffer[((4 + this.index) + 2) + i] = bytes[i];
            }
            this.index += bytes.Length + 2;
            this.updateLengthHandle();
        }

        public void WriteUTFString(string s, int nSize)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            for (int i = 0; i < bytes.Length; i++)
            {
                this.Buffer[(4 + this.index) + i] = bytes[i];
            }
            for (int j = bytes.Length; j < nSize; j++)
            {
                this.Buffer[(4 + this.index) + j] = 0;
            }
            this.index += nSize;
            this.updateLengthHandle();
        }

        public static void ZipDecompress(byte[] data, MemoryStream target)
        {
            using (MemoryStream stream = new MemoryStream(data, 0, data.Length))
            {
                stream.Seek(0L, SeekOrigin.Begin);
                using (InflaterInputStream stream2 = new InflaterInputStream(stream))
                {
                    for (int i = stream2.ReadByte(); i > -1; i = stream2.ReadByte())
                    {
                        target.WriteByte((byte) i);
                    }
                }
            }
        }

        private class DataBuffer
        {
            public byte[] Buffer;
            public int Length;

            public DataBuffer(int size)
            {
                this.Buffer = new byte[size];
            }
        }
    }
}

