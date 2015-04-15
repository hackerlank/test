namespace msg
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="MSG_Ret_Finish_Stage_SC")]
    public class MSG_Ret_Finish_Stage_SC : IExtensible
    {
        private CharacterBaseData _basedata;
        private uint _exp;
        private uint _gold;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue((string) null), ProtoMember(3, IsRequired=false, Name="basedata", DataFormat=DataFormat.Default)]
        public CharacterBaseData basedata
        {
            get
            {
                return this._basedata;
            }
            set
            {
                this._basedata = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(1, IsRequired=false, Name="exp", DataFormat=DataFormat.TwosComplement)]
        public uint exp
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

        [DefaultValue((long) 0L), ProtoMember(2, IsRequired=false, Name="gold", DataFormat=DataFormat.TwosComplement)]
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
    }
}

