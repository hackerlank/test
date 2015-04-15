namespace msg
{
    using ProtoBuf;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="MSG_Ret_Move_SC")]
    public class MSG_Ret_Move_SC : IExtensible
    {
        private ulong _charid;
        private readonly List<MoveData> _movedata = new List<MoveData>();
        private uint _speed;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue((float) 0f), ProtoMember(1, IsRequired=false, Name="charid", DataFormat=DataFormat.TwosComplement)]
        public ulong charid
        {
            get
            {
                return this._charid;
            }
            set
            {
                this._charid = value;
            }
        }

        [ProtoMember(2, Name="movedata", DataFormat=DataFormat.Default)]
        public List<MoveData> movedata
        {
            get
            {
                return this._movedata;
            }
        }

        [ProtoMember(3, IsRequired=false, Name="speed", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
        public uint speed
        {
            get
            {
                return this._speed;
            }
            set
            {
                this._speed = value;
            }
        }
    }
}

