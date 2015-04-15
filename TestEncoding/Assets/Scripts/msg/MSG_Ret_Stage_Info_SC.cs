namespace msg
{
    using ProtoBuf;
    using System;
    using System.Collections.Generic;

    [Serializable, ProtoContract(Name="MSG_Ret_Stage_Info_SC")]
    public class MSG_Ret_Stage_Info_SC : IExtensible
    {
        private readonly List<StageInfo> _stages = new List<StageInfo>();
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name="stages", DataFormat=DataFormat.Default)]
        public List<StageInfo> stages
        {
            get
            {
                return this._stages;
            }
        }
    }
}

