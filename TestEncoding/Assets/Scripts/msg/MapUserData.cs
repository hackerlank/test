namespace msg
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="MapUserData")]
    public class MapUserData : IExtensible
    {
        private ulong _charid;
        private CharacterMapData _mapdata;
        private CharacterMapShow _mapshow;
        private string _name = string.Empty;
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

        [DefaultValue((string) null), ProtoMember(4, IsRequired=false, Name="mapdata", DataFormat=DataFormat.Default)]
        public CharacterMapData mapdata
        {
            get
            {
                return this._mapdata;
            }
            set
            {
                this._mapdata = value;
            }
        }

        [DefaultValue((string) null), ProtoMember(3, IsRequired=false, Name="mapshow", DataFormat=DataFormat.Default)]
        public CharacterMapShow mapshow
        {
            get
            {
                return this._mapshow;
            }
            set
            {
                this._mapshow = value;
            }
        }

        [DefaultValue(""), ProtoMember(2, IsRequired=false, Name="name", DataFormat=DataFormat.Default)]
        public string name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }
    }
}

