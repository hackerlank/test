namespace magic
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="PKResult")]
    public class PKResult : IExtensible
    {
        private int _changehp;
        private ulong _def_tempid;
        private uint _def_type;
        private uint _hp;
        private uint _retcode;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(4, IsRequired=false, Name="changehp", DataFormat=DataFormat.TwosComplement), DefaultValue(0)]
        public int changehp
        {
            get
            {
                return this._changehp;
            }
            set
            {
                this._changehp = value;
            }
        }

        [ProtoMember(1, IsRequired=false, Name="def_tempid", DataFormat=DataFormat.TwosComplement), DefaultValue((float) 0f)]
        public ulong def_tempid
        {
            get
            {
                return this._def_tempid;
            }
            set
            {
                this._def_tempid = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(2, IsRequired=false, Name="def_type", DataFormat=DataFormat.TwosComplement)]
        public uint def_type
        {
            get
            {
                return this._def_type;
            }
            set
            {
                this._def_type = value;
            }
        }

        [ProtoMember(3, IsRequired=false, Name="hp", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
        public uint hp
        {
            get
            {
                return this._hp;
            }
            set
            {
                this._hp = value;
            }
        }

        [ProtoMember(5, IsRequired=false, Name="retcode", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
        public uint retcode
        {
            get
            {
                return this._retcode;
            }
            set
            {
                this._retcode = value;
            }
        }
    }
}

