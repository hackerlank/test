namespace msg
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="CharacterBaseData")]
    public class CharacterBaseData : IExtensible
    {
        private ulong _exp;
        private uint _famelevel;
        private uint _gold;
        private uint _laststage;
        private uint _money;
        private ulong _nextexp;
        private uint _port;
        private uint _position;
        private uint _tilizhi;
        private uint _type;
        private uint _viplevel;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue((float) 0f), ProtoMember(2, IsRequired=false, Name="exp", DataFormat=DataFormat.TwosComplement)]
        public ulong exp
        {
            get
            {
                return this._exp;
            }
            set
            {
                this._exp = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(7, IsRequired=false, Name="famelevel", DataFormat=DataFormat.TwosComplement)]
        public uint famelevel
        {
            get
            {
                return this._famelevel;
            }
            set
            {
                this._famelevel = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(4, IsRequired=false, Name="gold", DataFormat=DataFormat.TwosComplement)]
        public uint gold
        {
            get
            {
                return this._gold;
            }
            set
            {
                this._gold = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(11, IsRequired=false, Name="laststage", DataFormat=DataFormat.TwosComplement)]
        public uint laststage
        {
            get
            {
                return this._laststage;
            }
            set
            {
                this._laststage = value;
            }
        }

        [ProtoMember(3, IsRequired=false, Name="money", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
        public uint money
        {
            get
            {
                return this._money;
            }
            set
            {
                this._money = value;
            }
        }

        [DefaultValue((float) 0f), ProtoMember(12, IsRequired=false, Name="nextexp", DataFormat=DataFormat.TwosComplement)]
        public ulong nextexp
        {
            get
            {
                return this._nextexp;
            }
            set
            {
                this._nextexp = value;
            }
        }

        [ProtoMember(10, IsRequired=false, Name="port", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
        public uint port
        {
            get
            {
                return this._port;
            }
            set
            {
                this._port = value;
            }
        }

        [ProtoMember(8, IsRequired=false, Name="position", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
        public uint position
        {
            get
            {
                return this._position;
            }
            set
            {
                this._position = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(5, IsRequired=false, Name="tilizhi", DataFormat=DataFormat.TwosComplement)]
        public uint tilizhi
        {
            get
            {
                return this._tilizhi;
            }
            set
            {
                this._tilizhi = value;
            }
        }

        [ProtoMember(6, IsRequired=false, Name="type", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
        public uint type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(9, IsRequired=false, Name="viplevel", DataFormat=DataFormat.TwosComplement)]
        public uint viplevel
        {
            get
            {
                return this._viplevel;
            }
            set
            {
                this._viplevel = value;
            }
        }
    }
}

