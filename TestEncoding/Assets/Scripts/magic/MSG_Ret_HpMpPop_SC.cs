namespace magic
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="MSG_Ret_HpMpPop_SC")]
    public class MSG_Ret_HpMpPop_SC : IExtensible
    {
        private uint _entry_type;
        private uint _hp;
        private int _hp_change;
        private ulong _tempid;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(2, IsRequired=false, Name="entry_type", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
        public uint entry_type
        {
            get
            {
                return this._entry_type;
            }
            set
            {
                this._entry_type = value;
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

        [ProtoMember(4, IsRequired=false, Name="hp_change", DataFormat=DataFormat.TwosComplement), DefaultValue(0)]
        public int hp_change
        {
            get
            {
                return this._hp_change;
            }
            set
            {
                this._hp_change = value;
            }
        }

        [DefaultValue((float) 0f), ProtoMember(1, IsRequired=false, Name="tempid", DataFormat=DataFormat.TwosComplement)]
        public ulong tempid
        {
            get
            {
                return this._tempid;
            }
            set
            {
                this._tempid = value;
            }
        }
    }
}

