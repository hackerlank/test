#ifndef _UTIL_H_
#define _UTIL_H_
#include "cocos2d.h"

#include <vector>
#include <string>
#include <cmath>
//algorithmͷ�ļ�����make_heap,pop_heap�ȶѲ�����android��by hubin.
#include <algorithm>
//����sleep������ͷ�ļ�����Ȼandroid�汾�Ų�����Ӧ�ĺ�����iphone����Ĭ���ҵ�ƻ��ϵͳ���ͬ������ by hubin
#ifdef CC_PLATFORM_WIN32

// typedef struct
// {
//     int volatile value;
// } pthread_mutex_t;

#include "pthread.h"

#else

#include <unistd.h>
#include <pthread.h>
typedef signed char byte;
typedef bool boolean;

#endif

typedef std::string String;

class ByteBuffer; 


using namespace std;

#define  Float_MAX_VALUE  3.4028235E+38F
#define  Float_MIN_VALUE  1.4E-45F;

// Helpers to safely delete objects and arrays
#define SQ_SAFE_DELETE(x)       {if(x){ delete x; x = 0; }}
#define SQ_SAFE_DELETE_ARRAY(x) {if(x){ delete[] x; x = 0; }}
#define SQ_SAFE_DELETE_VEC(x) {for(int i = 0; i < x->size();i++){delete (x+i);}delete[] x;}
/**
 * @Fields ERROR_INS : �������
 */
static const double ERROR_INS = 0.005;
extern unsigned char* readFile(const char *filename);
extern string byteToHexStr(unsigned char *byte_arr, int arr_len);
extern double distance(double x1, double y1, double x2, double y2);
extern unsigned long msNextPOT(unsigned long x);

#define SAFE_DELETE_ELEMENT( ptr ) if (ptr != NULL) {delete ptr; ptr = NULL;}
#define SAFE_DELETE_ARRAY( ptr )if (ptr != NULL){delete[] ptr;ptr = NULL;}

template<typename _RandomAccessIterator>
inline void safe_delete_vector(_RandomAccessIterator __first, _RandomAccessIterator __last)
{

	for (_RandomAccessIterator it = __first;it!= __last ; ++it) {
		if( (*it)!=NULL){
			delete *it;
			*it = NULL;
		}
	}
}	


//CString StringConvertToUnicodeBytesCodes(const wchar_t* lpszNeedConvert);

template <class Class, typename TT>
inline bool instanceof(TT const &object)
{
    return dynamic_cast<Class const *>(&object);
	//return false;
}

const char* fullpathFromRelatePath(const char* relatePath);


/**
 return : �Ƿ�浵�ɹ�
 **/
bool writeSaveData(const char* relatepath,char* buf,int len);
/**
 ��ȡ�浵
 */
ByteBuffer* readSaveData(const char* relatepath);

//��ȡassets������Դ,isfullPath�Ƿ���ȫĿ¼,�������ԴĿ¼�е���Դ����false��������Ǿ���true
ByteBuffer* getFileData(const char* pszfullFilepath, bool isfullPath);

class MyLock{
	pthread_mutex_t* mutex_t;
public:
	MyLock(pthread_mutex_t* _mutex_t);
	~MyLock();
	
};



String createRandString(int len,boolean filter);



#endif
