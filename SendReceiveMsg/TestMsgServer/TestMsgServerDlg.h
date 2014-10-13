
// TestMsgServerDlg.h : 头文件
//

#pragma once

#include <vector>

#include "pthread.h"

// CTestMsgServerDlg 对话框
class CTestMsgServerDlg : public CDialogEx
{
// 构造
public:
	CTestMsgServerDlg(CWnd* pParent = NULL);	// 标准构造函数

// 对话框数据
	enum { IDD = IDD_TESTMSGSERVER_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV 支持

	int InitNetWork();

	pthread_t pthread_t_listen;
	bool m_bThreadListenCreated;
	pthread_t pthread_t_receive;
	bool m_bThreadRecvCreated;

	static void* ThreadListenClient(void *p);
	static void* ThreadReceivetMsg(void *p);

//	std::vector<SOCKET> m_vecClientSockets;
// 实现
protected:
	HICON m_hIcon;

	// 生成的消息映射函数
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedButton1();
};
