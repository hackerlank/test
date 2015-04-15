namespace msg
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="MSG_Ret_Common_Tips_SC")]
    public class MSG_Ret_Common_Tips_SC : IExtensible
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

