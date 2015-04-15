namespace msg
{
    using ProtoBuf;
    using System;
    using System.Collections.Generic;

    [Serializable, ProtoContract(Name="MSG_Ret_MapScreenBatchRefreshNpc_SC")]
    public class MSG_Ret_MapScreenBatchRefreshNpc_SC : IExtensible
    {
        private readonly List<MapNpcData> _data = new List<MapNpcData>();
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name="data", DataFormat=DataFormat.Default)]
        public List<MapNpcData> data
        {
            get
            {
                return this._data;
            }
        }
    }
}

