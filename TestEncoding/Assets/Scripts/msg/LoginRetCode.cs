namespace msg
{
    using ProtoBuf;
    using System;

    [ProtoContract(Name="LoginRetCode")]
    public enum LoginRetCode
    {
        [ProtoEnum(Name="LOGIN_RETURN_ACCOUNTEXIST", Value=10)]
        LOGIN_RETURN_ACCOUNTEXIST = 10,
        [ProtoEnum(Name="LOGIN_RETURN_ACCOUNTSUCCESS", Value=11)]
        LOGIN_RETURN_ACCOUNTSUCCESS = 11,
        [ProtoEnum(Name="LOGIN_RETURN_BUSY", Value=0x20)]
        LOGIN_RETURN_BUSY = 0x20,
        [ProtoEnum(Name="LOGIN_RETURN_CHANGE_LOGIN", Value=0x17)]
        LOGIN_RETURN_CHANGE_LOGIN = 0x17,
        [ProtoEnum(Name="LOGIN_RETURN_CHANGEPASSWORD", Value=5)]
        LOGIN_RETURN_CHANGEPASSWORD = 5,
        [ProtoEnum(Name="LOGIN_RETURN_CHARNAME_INVALID", Value=0x2b)]
        LOGIN_RETURN_CHARNAME_INVALID = 0x2b,
        [ProtoEnum(Name="LOGIN_RETURN_CHARNAMEREPEAT", Value=12)]
        LOGIN_RETURN_CHARNAMEREPEAT = 12,
        [ProtoEnum(Name="LOGIN_RETURN_DB", Value=3)]
        LOGIN_RETURN_DB = 3,
        [ProtoEnum(Name="LOGIN_RETURN_FORBID", Value=0x21)]
        LOGIN_RETURN_FORBID = 0x21,
        [ProtoEnum(Name="LOGIN_RETURN_GATEWAYNOTAVAILABLE", Value=8)]
        LOGIN_RETURN_GATEWAYNOTAVAILABLE = 8,
        [ProtoEnum(Name="LOGIN_RETURN_IDINCLOSE", Value=7)]
        LOGIN_RETURN_IDINCLOSE = 7,
        [ProtoEnum(Name="LOGIN_RETURN_IDINUSE", Value=6)]
        LOGIN_RETURN_IDINUSE = 6,
        [ProtoEnum(Name="LOGIN_RETURN_IMG_LOCK", Value=0x1d)]
        LOGIN_RETURN_IMG_LOCK = 0x1d,
        [ProtoEnum(Name="LOGIN_RETURN_IMG_LOCK2", Value=0x22)]
        LOGIN_RETURN_IMG_LOCK2 = 0x22,
        [ProtoEnum(Name="LOGIN_RETURN_JPEG_PASSPORT", Value=0x11)]
        LOGIN_RETURN_JPEG_PASSPORT = 0x11,
        [ProtoEnum(Name="LOGIN_RETURN_LOCK", Value=0x12)]
        LOGIN_RETURN_LOCK = 0x12,
        [ProtoEnum(Name="LOGIN_RETURN_MAINTAIN", Value=0x23)]
        LOGIN_RETURN_MAINTAIN = 0x23,
        [ProtoEnum(Name="LOGIN_RETURN_MATRIX_DOWN", Value=0x1b)]
        LOGIN_RETURN_MATRIX_DOWN = 0x1b,
        [ProtoEnum(Name="LOGIN_RETURN_MATRIX_ERROR", Value=0x18)]
        LOGIN_RETURN_MATRIX_ERROR = 0x18,
        [ProtoEnum(Name="LOGIN_RETURN_MATRIX_LOCK", Value=0x1a)]
        LOGIN_RETURN_MATRIX_LOCK = 0x1a,
        [ProtoEnum(Name="LOGIN_RETURN_MATRIX_NEED", Value=0x19)]
        LOGIN_RETURN_MATRIX_NEED = 0x19,
        [ProtoEnum(Name="LOGIN_RETURN_NEWUSER_OLDZONE", Value=20)]
        LOGIN_RETURN_NEWUSER_OLDZONE = 20,
        [ProtoEnum(Name="LOGIN_RETURN_OLDUSER_NEWZONE", Value=0x1c)]
        LOGIN_RETURN_OLDUSER_NEWZONE = 0x1c,
        [ProtoEnum(Name="LOGIN_RETURN_PASSPOD_DOWN", Value=0x1f)]
        LOGIN_RETURN_PASSPOD_DOWN = 0x1f,
        [ProtoEnum(Name="LOGIN_RETURN_PASSPOD_PASSWORDERROR", Value=30)]
        LOGIN_RETURN_PASSPOD_PASSWORDERROR = 30,
        [ProtoEnum(Name="LOGIN_RETURN_PASSWORDERROR", Value=4)]
        LOGIN_RETURN_PASSWORDERROR = 4,
        [ProtoEnum(Name="LOGIN_RETURN_PAYFAILED", Value=0x10)]
        LOGIN_RETURN_PAYFAILED = 0x10,
        [ProtoEnum(Name="LOGIN_RETURN_SHOW_MSG", Value=0x29)]
        LOGIN_RETURN_SHOW_MSG = 0x29,
        [ProtoEnum(Name="LOGIN_RETURN_TDCODE_DOWN", Value=0x25)]
        LOGIN_RETURN_TDCODE_DOWN = 0x25,
        [ProtoEnum(Name="LOGIN_RETURN_TDCODE_GEN_ERROR", Value=0x24)]
        LOGIN_RETURN_TDCODE_GEN_ERROR = 0x24,
        [ProtoEnum(Name="LOGIN_RETURN_TIMEOUT", Value=15)]
        LOGIN_RETURN_TIMEOUT = 15,
        [ProtoEnum(Name="LOGIN_RETURN_TOKEN_ERROR", Value=0x26)]
        LOGIN_RETURN_TOKEN_ERROR = 0x26,
        [ProtoEnum(Name="LOGIN_RETURN_TOKEN_TIMEOUT", Value=40)]
        LOGIN_RETURN_TOKEN_TIMEOUT = 40,
        [ProtoEnum(Name="LOGIN_RETURN_TOKEN_TOO_QUICK", Value=0x27)]
        LOGIN_RETURN_TOKEN_TOO_QUICK = 0x27,
        [ProtoEnum(Name="LOGIN_RETURN_UNKNOWN", Value=0)]
        LOGIN_RETURN_UNKNOWN = 0,
        [ProtoEnum(Name="LOGIN_RETURN_USER_TOZONE", Value=0x16)]
        LOGIN_RETURN_USER_TOZONE = 0x16,
        [ProtoEnum(Name="LOGIN_RETURN_USERDATANOEXIST", Value=13)]
        LOGIN_RETURN_USERDATANOEXIST = 13,
        [ProtoEnum(Name="LOGIN_RETURN_USERINLOGIN", Value=0x2a)]
        LOGIN_RETURN_USERINLOGIN = 0x2a,
        [ProtoEnum(Name="LOGIN_RETURN_USERMAX", Value=9)]
        LOGIN_RETURN_USERMAX = 9,
        [ProtoEnum(Name="LOGIN_RETURN_USERNAMEREPEAT", Value=14)]
        LOGIN_RETURN_USERNAMEREPEAT = 14,
        [ProtoEnum(Name="LOGIN_RETURN_UUID", Value=2)]
        LOGIN_RETURN_UUID = 2,
        [ProtoEnum(Name="LOGIN_RETURN_UUID_ERROR", Value=0x15)]
        LOGIN_RETURN_UUID_ERROR = 0x15,
        [ProtoEnum(Name="LOGIN_RETURN_VERSIONERROR", Value=1)]
        LOGIN_RETURN_VERSIONERROR = 1,
        [ProtoEnum(Name="LOGIN_RETURN_WAITACTIVE", Value=0x13)]
        LOGIN_RETURN_WAITACTIVE = 0x13
    }
}

