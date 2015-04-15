namespace msg
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="MSG_Ret_Base_Item_SC")]
    public class MSG_Ret_Base_Item_SC : IExtensible
    {
        private uint _gold;
        private uint _money;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
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

        [ProtoMember(1, IsRequired=false, Name="money", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
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
    }
}

