namespace magic
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="MSG_Req_SyncSkillStage_CS")]
    public class MSG_Req_SyncSkillStage_CS : IExtensible
    {
        private uint _desx;
        private uint _desy;
        private uint _dir;
        private uint _skillstage;
        private ulong _targetid;
        private uint _targettype;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(4, IsRequired=false, Name="desx", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
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

        [ProtoMember(3, IsRequired=false, Name="skillstage", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
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

        [ProtoMember(1, IsRequired=false, Name="targetid", DataFormat=DataFormat.TwosComplement), DefaultValue((float) 0f)]
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

        [ProtoMember(2, IsRequired=false, Name="targettype", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
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

