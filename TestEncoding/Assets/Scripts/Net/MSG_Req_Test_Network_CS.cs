namespace Net
{
    using ProtoBuf;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="MSG_Req_Test_Network_CS")]
    public class MSG_Req_Test_Network_CS : IExtensible
    {
        private string _name = string.Empty;
        private uint _num;
        private readonly List<uint> _num2 = new List<uint>();
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(""), ProtoMember(2, IsRequired=false, Name="name", DataFormat=DataFormat.Default)]
        public string name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(1, IsRequired=false, Name="num", DataFormat=DataFormat.TwosComplement)]
        public uint num
        {
            get
            {
                return this._num;
            }
            set
            {
                this._num = value;
            }
        }

        [ProtoMember(3, Name="num2", DataFormat=DataFormat.TwosComplement)]
        public List<uint> num2
        {
            get
            {
                return this._num2;
            }
        }
    }
}

