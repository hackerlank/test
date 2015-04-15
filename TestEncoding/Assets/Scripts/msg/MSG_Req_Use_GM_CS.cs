namespace msg
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="MSG_Req_Use_GM_CS")]
    public class MSG_Req_Use_GM_CS : IExtensible
    {
        private string _strcontent = string.Empty;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired=false, Name="strcontent", DataFormat=DataFormat.Default), DefaultValue("")]
        public string strcontent
        {
            get
            {
                return this._strcontent;
            }
            set
            {
                this._strcontent = value;
            }
        }
    }
}

