namespace magic
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="MSG_Req_MagicAttack_CS")]
    public class MSG_Req_MagicAttack_CS : IExtensible
    {
        private uint _desx;
        private uint _desy;
        private uint _dir;
        private uint _magictype;
        private ulong _targetid;
        private uint _targettype;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue((long) 0L), ProtoMember(4, IsRequired=false, Name="desx", DataFormat=DataFormat.TwosComplement)]
        public uint desx
        {
            get
            {
                return this._desx;
            }
            set
            {
                this._desx = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(5, IsRequired=false, Name="desy", DataFormat=DataFormat.TwosComplement)]
        public uint desy
        {
            get
            {
                return this._desy;
            }
            set
            {
                this._desy = value;
            }
        }

        [ProtoMember(6, IsRequired=false, Name="dir", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
        public uint dir
        {
            get
            {
                return this._dir;
            }
            set
            {
                this._dir = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(3, IsRequired=false, Name="magictype", DataFormat=DataFormat.TwosComplement)]
        public uint magictype
        {
            get
            {
                return this._magictype;
            }
            set
            {
                this._magictype = value;
            }
        }

        [DefaultValue((float) 0f), ProtoMember(1, IsRequired=false, Name="targetid", DataFormat=DataFormat.TwosComplement)]
        public ulong targetid
        {
            get
            {
                return this._targetid;
            }
            set
            {
                this._targetid = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(2, IsRequired=false, Name="targettype", DataFormat=DataFormat.TwosComplement)]
        public uint targettype
        {
            get
            {
                return this._targettype;
            }
            set
            {
                this._targettype = value;
            }
        }
    }
}

