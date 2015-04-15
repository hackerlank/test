namespace msg
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="stageDropInfo")]
    public class stageDropInfo : IExtensible
    {
        private uint _itemid;
        private uint _itemnum;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired=false, Name="itemid", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
        public uint itemid
        {
            get
            {
                return this._itemid;
            }
            set
            {
                this._itemid = value;
            }
        }

        [ProtoMember(2, IsRequired=false, Name="itemnum", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
        public uint itemnum
        {
            get
            {
                return this._itemnum;
            }
            set
            {
                this._itemnum = value;
            }
        }
    }
}

