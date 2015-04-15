namespace magic
{
    using ProtoBuf;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="MSG_Ret_MagicAttack_SC")]
    public class MSG_Ret_MagicAttack_SC : IExtensible
    {
        private ulong _att_tempid;
        private ulong _def_tempid;
        private uint _desx;
        private uint _desy;
        private uint _dir;
        private readonly List<PKResult> _pklist = new List<PKResult>();
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

        [ProtoMember(2, IsRequired=false, Name="def_tempid", DataFormat=DataFormat.TwosComplement), DefaultValue((float) 0f)]
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

        [ProtoMember(3, IsRequired=false, Name="desx", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
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

        [DefaultValue((long) 0L), ProtoMember(4, IsRequired=false, Name="desy", DataFormat=DataFormat.TwosComplement)]
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

        [ProtoMember(5, IsRequired=false, Name="dir", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
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

        [ProtoMember(7, Name="pklist", DataFormat=DataFormat.Default)]
        public List<PKResult> pklist
        {
            get
            {
                return this._pklist;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(6, IsRequired=false, Name="skillid", DataFormat=DataFormat.TwosComplement)]
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

