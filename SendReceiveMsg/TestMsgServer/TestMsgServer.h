
// TestMsgServer.h : PROJECT_NAME Ӧ�ó������ͷ�ļ�
//

#pragma once

#ifndef __AFXWIN_H__
	#error "�ڰ������ļ�֮ǰ������stdafx.h�������� PCH �ļ�"
#endif

#include "resource.h"		// ������


// CTestMsgServerApp:
// �йش����ʵ�֣������ TestMsgServer.cpp
//

class CTestMsgServerApp : public CWinApp
{
public:
	CTestMsgServerApp();

// ��д
public:
	virtual BOOL InitInstance();

// ʵ��

	DECLARE_MESSAGE_MAP()
};

extern CTestMsgServerApp theApp;