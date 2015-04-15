namespace magic
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="MSG_Ret_StartMagicAttack_SC")]
    public class MSG_Ret_StartMagicAttack_SC : IExtensible
    {
        private ulong _att_tempid;
        private uint _desx;
        private uint _desy;
        private uint _dir;
        private uint _skillid;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue((float) 0f), ProtoMember(1, IsRequired=false, Name="att_tempid", DataFormat=DataFormat.TwosComplement)]
        public ulong att_tempid
        {
            get
            {
                return this._att_tempid;
            }
            set
            {
                this._att_tempid = value;
            }
        }

        [ProtoMember(2, IsRequired=false, Name="desx", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
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

        [ProtoMember(3, IsRequired=false, Name="desy", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
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

        [DefaultValue((long) 0L), ProtoMember(4, IsRequired=false, Name="dir", DataFormat=DataFormat.TwosComplement)]
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

        [DefaultValue((long) 0L), ProtoMember(5, IsRequired=false, Name="skillid", DataFormat=DataFormat.TwosComplement)]
        public uint skillid
        {
            get
            {
                return this._skillid;
            }
            set
            {
                this._skillid = value;
            }
        }
    }
}

