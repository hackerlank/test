namespace magic
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="MSG_Ret_SyncSkillStage_SC")]
    public class MSG_Ret_SyncSkillStage_SC : IExtensible
    {
        private ulong _att_tempid;
        private uint _att_type;
        private ulong _def_tempid;
        private uint _def_type;
        private uint _desx;
        private uint _desy;
        private uint _dir;
        private uint _skillstage;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired=false, Name="att_tempid", DataFormat=DataFormat.TwosComplement), DefaultValue((float) 0f)]
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

        [ProtoMember(2, IsRequired=false, Name="att_type", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
        public uint att_type
        {
            get
            {
                return this._att_type;
            }
            set
            {
                this._att_type = value;
            }
        }

        [ProtoMember(3, IsRequired=false, Name="def_tempid", DataFormat=DataFormat.TwosComplement), DefaultValue((float) 0f)]
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

        [DefaultValue((long) 0L), ProtoMember(4, IsRequired=false, Name="def_type", DataFormat=DataFormat.TwosComplement)]
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

        [ProtoMember(6, IsRequired=false, Name="desx", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
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

        [ProtoMember(7, IsRequired=false, Name="desy", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
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

        [DefaultValue((long) 0L), ProtoMember(8, IsRequired=false, Name="dir", DataFormat=DataFormat.TwosComplement)]
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

        [ProtoMember(5, IsRequired=false, Name="skillstage", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
        public uint skillstage
        {
            get
            {
                return this._skillstage;
            }
            set
            {
                this._skillstage = value;
            }
        }
    }
}

