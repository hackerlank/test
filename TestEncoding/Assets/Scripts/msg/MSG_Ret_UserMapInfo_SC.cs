namespace msg
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="MSG_Ret_UserMapInfo_SC")]
    public class MSG_Ret_UserMapInfo_SC : IExtensible
    {
        private string _filename = string.Empty;
        private uint _mapid;
        private string _mapname = string.Empty;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(""), ProtoMember(3, IsRequired=false, Name="filename", DataFormat=DataFormat.Default)]
        public string filename
        {
            get
            {
                return this._filename;
            }
            set
            {
                this._filename = value;
            }
        }

        [ProtoMember(1, IsRequired=false, Name="mapid", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
        public uint mapid
        {
            get
            {
                return this._mapid;
            }
            set
            {
                this._mapid = value;
            }
        }

        [DefaultValue(""), ProtoMember(2, IsRequired=false, Name="mapname", DataFormat=DataFormat.Default)]
        public string mapname
        {
            get
            {
                return this._mapname;
            }
            set
            {
                this._mapname = value;
            }
        }
    }
}

