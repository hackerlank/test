
// TestMsgClient.h : PROJECT_NAME Ӧ�ó������ͷ�ļ�
//

#pragma once

#ifndef __AFXWIN_H__
	#error "�ڰ������ļ�֮ǰ������stdafx.h�������� PCH �ļ�"
#endif

#include "resource.h"		// ������


// CTestMsgClientApp:
// �йش����ʵ�֣������ TestMsgClient.cpp
//

class CTestMsgClientApp : public CWinApp
{
public:
	CTestMsgClientApp();

// ��д
public:
	virtual BOOL InitInstance();

// ʵ��

	DECLARE_MESSAGE_MAP()
};

extern CTestMsgClientApp theApp;