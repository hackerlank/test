
// TestMsgServerDlg.cpp : 实现文件
//

#include "stdafx.h"
#include "TestMsgServer.h"
#include "TestMsgServerDlg.h"
#include "afxdialogex.h"

#include "../common/console.h"
#include <iostream>

using namespace std;

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

#pragma comment(lib, "WS2_32.lib")
// 用于应用程序“关于”菜单项的 CAboutDlg 对话框

class CAboutDlg : public CDialogEx
{
public:
	CAboutDlg();

// 对话框数据
	enum { IDD = IDD_ABOUTBOX };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持

// 实现
protected:
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialogEx(CAboutDlg::IDD)
{
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialogEx)
END_MESSAGE_MAP()


// CTestMsgServerDlg 对话框




CTestMsgServerDlg::CTestMsgServerDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CTestMsgServerDlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CTestMsgServerDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}


int CTestMsgServerDlg::InitNetWork()
{
	WSADATA wsaData;  
	WORD sockVersion = MAKEWORD(2, 2);  
	SOCKET sListen = 0;  
	sockaddr_in sin  = {0};  
	sockaddr_in remoteAddr = {0};  
	char szText[] = "TCP Server Demo";  
	int nAddrLen = 0;  

	nAddrLen = sizeof(sockaddr_in);  
	//fill sin  
	sin.sin_port = htons(4567);  
	sin.sin_family = AF_INET;  
	sin.sin_addr.S_un.S_addr = INADDR_ANY;  

	//init wsa  
	if (WSAStartup(sockVersion, &wsaData) != 0)  
	{  
		cout << "initlization failed!" << endl;  

		exit(0);  
	}  

	sListen = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);  

	if (bind(sListen, (LPSOCKADDR)&sin, sizeof(sin)) == SOCKET_ERROR)  
	{  
		cout << "bind failed!" << endl;  

		return 0;  
	}  

	if (listen(sListen, 2) == SOCKET_ERROR)  
	{  
		cout << "listen failed!" << endl;  

		return 0;  
	}  

	SOCKET sClient = INADDR_ANY;  

	while (true)  
	{  
		sClient = accept(sListen, (SOCKADDR*)&remoteAddr, &nAddrLen);  

		if (sClient == INVALID_SOCKET)  
		{  
			cout << "accept failed!" << endl;  

			continue;  
		}  

		send(sClient, szText, strlen(szText), 0);  

		closesocket(sClient);   

	}  

	closesocket(sListen);  

	WSACleanup();  
}

BEGIN_MESSAGE_MAP(CTestMsgServerDlg, CDialogEx)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
END_MESSAGE_MAP()


// CTestMsgServerDlg 消息处理程序

BOOL CTestMsgServerDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// 将“关于...”菜单项添加到系统菜单中。

	// IDM_ABOUTBOX 必须在系统命令范围内。
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != NULL)
	{
		BOOL bNameValid;
		CString strAboutMenu;
		bNameValid = strAboutMenu.LoadString(IDS_ABOUTBOX);
		ASSERT(bNameValid);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}

	// 设置此对话框的图标。当应用程序主窗口不是对话框时，框架将自动
	//  执行此操作
	SetIcon(m_hIcon, TRUE);			// 设置大图标
	SetIcon(m_hIcon, FALSE);		// 设置小图标

	// TODO: 在此添加额外的初始化代码
	console::Manager::InitInstance();
	console::Manager::GetInstance()->Create("/serverlog.htm", true);
	// 
	ConsoleOutEx(console::CONSOLE_COLOR_RED, "===============TestServer log!=============\n");

	InitNetWork();

	return TRUE;  // 除非将焦点设置到控件，否则返回 TRUE
}

void CTestMsgServerDlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialogEx::OnSysCommand(nID, lParam);
	}
}

// 如果向对话框添加最小化按钮，则需要下面的代码
//  来绘制该图标。对于使用文档/视图模型的 MFC 应用程序，
//  这将由框架自动完成。

void CTestMsgServerDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // 用于绘制的设备上下文

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// 使图标在工作区矩形中居中
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// 绘制图标
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialogEx::OnPaint();
	}
}

//当用户拖动最小化窗口时系统调用此函数取得光标
//显示。
HCURSOR CTestMsgServerDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

