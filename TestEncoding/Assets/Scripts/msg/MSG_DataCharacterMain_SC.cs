namespace msg
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="MSG_DataCharacterMain_SC")]
    public class MSG_DataCharacterMain_SC : IExtensible
    {
        private CharacterMainData _data;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired=false, Name="data", DataFormat=DataFormat.Default), DefaultValue((string) null)]
        public CharacterMainData data
        {
            get
            {
                return this._data;
            }
            set
            {
                this._data = value;
            }
        }
    }
}

