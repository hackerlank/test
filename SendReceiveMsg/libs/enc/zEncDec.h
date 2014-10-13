
#pragma once
#ifndef _INC_CSCOMMON_H_
#define _INC_CSCOMMON_H_

//#define _CRT_SECURE_CPP_OVERLOAD_STANDARD_NAMES 1

/**
 * \brief 定义空的指令
 *
 * 负责服务器内部交换使用，和客户端交互的指令需要另外定义
 * 
 */


/**
 * \brief 定义基本类型
 *
 * 
 */
//#define _MSGPARSE_

#define _X86
#define BGDWIN32
#define XMD_H
#define __LCC__
 
#include <windows.h>

#ifdef HAVE_STRINGS_H
#include <string>
#endif //HAVE_STRINGS_H


#define MINI_RC5


#include <miniCrypt.h>

#define __CAT(x)    #x

#define SAFE_DELETE(x) { if (NULL != x) { delete (x); (x) = NULL; } }
#define SAFE_DELETE_VEC(x) { if (NULL != x) { delete [] (x); (x) = NULL; } }

/**
 * \brief 双字节符号整数
 *
 */
typedef signed short SWORD;

/**
 * \brief 四字节符号整数
 *
 */
typedef long int SDWORD;

#ifdef _MSC_VER

typedef unsigned __int64 QWORD;
typedef signed __int64 SQWORD;

#else //_MSC_VER

/**
 * \brief 八字节无符号整数
 *
 */
typedef unsigned long long QWORD;

/**
 * \brief 八字节符号整数
 *
 */
typedef signed long long SQWORD;

#endif //_MSC_VER



#define ZEBRA_CLIENT_VERSION    20060807

// inline void mymemcpy(void* pDst, DWORD dwDstSize, void* pScr, DWORD dwCpSize )
// {
// 	if(dwCpSize>dwDstSize)
// 	{
// 		MessageBox(NULL,"内存操作将越界","错误",MB_ICONERROR);
// 	}
// 	memcpy_s(pDst,dwDstSize,pScr,dwCpSize);
// }
// 
// #define memcpy(d,s,size,dsize) mymemcpy((void*)(d),dsize,(void*)(s),size)

#ifndef HAVE_BZERO
#define bzero(p,s)      memset(p,0,s)
#define bcopy(s,d,ss,ds) memcpy(d,s,ss,ds)
#endif //HAVE_BZERO

class CEncrypt
{
public:
  CEncrypt();
  enum encMethod
  {
    ENCDEC_NONE,
    ENCDEC_DES,
    ENCDEC_RC5
  };
  void random_key_des(ZES_cblock *ret);
  void set_key_des(const_ZES_cblock *key);
  void set_key_rc5(const BYTE *data,int nLen,int rounds);
  int encdec(void *data,DWORD nLen,bool enc);

  void setEncMethod(encMethod method);
  encMethod getEncMethod() const;

public:
  void ZES_random_key(ZES_cblock *ret);
  void ZES_set_key(const_ZES_cblock *key,ZES_key_schedule *schedule);
  void ZES_encrypt1(ZES_LONG *data,ZES_key_schedule *ks,int enc);

  void RC5_32_set_key(RC5_32_KEY *key,int len,const BYTE *data,int rounds);
  void RC5_32_encrypt(RC5_32_INT *d,RC5_32_KEY *key);
  void RC5_32_decrypt(RC5_32_INT *d,RC5_32_KEY *key);

  int encdec_des(BYTE *data,DWORD nLen,bool enc);
  int encdec_rc5(BYTE *data,DWORD nLen,bool enc);

  ZES_key_schedule key_des;
  RC5_32_KEY key_rc5;
  bool haveKey_des;
  bool haveKey_rc5;

  encMethod method;

};



#endif //_INC_CSCOMMON_H_
