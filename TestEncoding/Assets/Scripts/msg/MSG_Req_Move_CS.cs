namespace msg
{
    using ProtoBuf;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="MSG_Req_Move_CS")]
    public class MSG_Req_Move_CS : IExtensible
    {
        private ulong _charid;
        private readonly List<MoveData> _movedata = new List<MoveData>();
        private uint _speed;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired=false, Name="charid", DataFormat=DataFormat.TwosComplement), DefaultValue((float) 0f)]
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

        [DefaultValue((long) 0L), ProtoMember(3, IsRequired=false, Name="speed", DataFormat=DataFormat.TwosComplement)]
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

