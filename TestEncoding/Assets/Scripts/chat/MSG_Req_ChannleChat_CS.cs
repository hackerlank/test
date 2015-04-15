namespace chat
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="MSG_Req_ChannleChat_CS")]
    public class MSG_Req_ChannleChat_CS : IExtensible
    {
        private string _src_name = string.Empty;
        private string _str_chat = string.Empty;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(""), ProtoMember(2, IsRequired=false, Name="src_name", DataFormat=DataFormat.Default)]
        public string src_name
        {
            get
            {
                return this._src_name;
            }
            set
            {
                this._src_name = value;
            }
        }

        [DefaultValue(""), ProtoMember(1, IsRequired=false, Name="str_chat", DataFormat=DataFormat.Default)]
        public string str_chat
        {
            get
            {
                return this._str_chat;
            }
            set
            {
                this._str_chat = value;
            }
        }
    }
}

