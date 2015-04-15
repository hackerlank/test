namespace magic
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="SkillData")]
    public class SkillData : IExtensible
    {
        private ulong _lastusetime;
        private uint _level;
        private uint _playtime;
        private uint _qlevel;
        private uint _skillid;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(3, IsRequired=false, Name="lastusetime", DataFormat=DataFormat.TwosComplement), DefaultValue((float) 0f)]
        public ulong lastusetime
        {
            get
            {
                return this._lastusetime;
            }
            set
            {
                this._lastusetime = value;
            }
        }

        [ProtoMember(2, IsRequired=false, Name="level", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
        public uint level
        {
            get
            {
                return this._level;
            }
            set
            {
                this._level = value;
            }
        }

        [ProtoMember(4, IsRequired=false, Name="playtime", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
        public uint playtime
        {
            get
            {
                return this._playtime;
            }
            set
            {
                this._playtime = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(5, IsRequired=false, Name="qlevel", DataFormat=DataFormat.TwosComplement)]
        public uint qlevel
        {
            get
            {
                return this._qlevel;
            }
            set
            {
                this._qlevel = value;
            }
        }

        [ProtoMember(1, IsRequired=false, Name="skillid", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
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

