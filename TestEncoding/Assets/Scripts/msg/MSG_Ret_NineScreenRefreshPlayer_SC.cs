namespace msg
{
    using ProtoBuf;
    using System;
    using System.Collections.Generic;

    [Serializable, ProtoContract(Name="MSG_Ret_NineScreenRefreshPlayer_SC")]
    public class MSG_Ret_NineScreenRefreshPlayer_SC : IExtensible
    {
        private readonly List<MapUserData> _data = new List<MapUserData>();
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name="data", DataFormat=DataFormat.Default)]
        public List<MapUserData> data
        {
            get
            {
                return this._data;
            }
        }
    }
}

