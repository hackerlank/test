
// TestMsgClientDlg.h : 头文件
//

#pragma once
#include "afxwin.h"


// CTestMsgClientDlg 对话框
class CTestMsgClientDlg : public CDialogEx
{
// 构造
public:
	CTestMsgClientDlg(CWnd* pParent = NULL);	// 标准构造函数

// 对话框数据
	enum { IDD = IDD_TESTMSGCLIENT_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV 支持

	int InitNetWork();
	int ReleaseNetWork();

	SOCKET m_sock;
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
	afx_msg void OnBnClickedOk();
	CString m_sendTxt;
	afx_msg void OnEnChangeEdit1();
	CEdit m_editCtrol;
	afx_msg void OnBnClickedButton2();
	afx_msg void OnBnClickedButton3();
};
