
// TestMsgClientDlg.h : ͷ�ļ�
//

#pragma once
#include "afxwin.h"


// CTestMsgClientDlg �Ի���
class CTestMsgClientDlg : public CDialogEx
{
// ����
public:
	CTestMsgClientDlg(CWnd* pParent = NULL);	// ��׼���캯��

// �Ի�������
	enum { IDD = IDD_TESTMSGCLIENT_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV ֧��

	int InitNetWork();
	int ReleaseNetWork();

	SOCKET m_sock;
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
	afx_msg void OnBnClickedOk();
	CString m_sendTxt;
	afx_msg void OnEnChangeEdit1();
	CEdit m_editCtrol;
	afx_msg void OnBnClickedButton2();
	afx_msg void OnBnClickedButton3();
};
