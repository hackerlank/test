namespace msg
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="MSG_Ret_MapScreenRefreshNpc_SC")]
    public class MSG_Ret_MapScreenRefreshNpc_SC : IExtensible
    {
        private MapNpcData _data;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue((string) null), ProtoMember(1, IsRequired=false, Name="data", DataFormat=DataFormat.Default)]
        public MapNpcData data
        {
            get
            {
                return this._data;
            }
            set
            {
                this._data = value;
            }
        }
    }
}

