
// TestMsgServerDlg.h : ͷ�ļ�
//

#pragma once

#include <vector>

#include "pthread.h"

// CTestMsgServerDlg �Ի���
class CTestMsgServerDlg : public CDialogEx
{
// ����
public:
	CTestMsgServerDlg(CWnd* pParent = NULL);	// ��׼���캯��

// �Ի�������
	enum { IDD = IDD_TESTMSGSERVER_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV ֧��

	int InitNetWork();

	pthread_t pthread_t_listen;
	bool m_bThreadListenCreated;
	pthread_t pthread_t_receive;
	bool m_bThreadRecvCreated;

	static void* ThreadListenClient(void *p);
	static void* ThreadReceivetMsg(void *p);

//	std::vector<SOCKET> m_vecClientSockets;
// ʵ��
protected:
	HICON m_hIcon;

	// ���ɵ���Ϣӳ�亯��
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedButton1();
};
