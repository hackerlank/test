
// TestMsgServerDlg.cpp : 实现文件
//

#include "stdafx.h"
#include "TestMsgServer.h"
#include "TestMsgServerDlg.h"
#include "afxdialogex.h"

#include "../common/console.h"
#include <iostream>
#include <string>
#include<iomanip>
#include <vector>



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


std::vector<SOCKET> m_vecClientSockets;


CTestMsgServerDlg::CTestMsgServerDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CTestMsgServerDlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);

	m_bThreadListenCreated = false;
	m_bThreadRecvCreated = false;
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
	sin.sin_port = htons(45678);  
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
		static int i = 0;

		string rtMsg = avar("欢迎您，客户端%d\n", ++i);
		send(sClient, rtMsg.c_str(), strlen(rtMsg.c_str()), 0);  

		//一直接收消息
		while (true)
		{
			char buffer[256] = "\0";  
			int  nRecv = 0;  

			nRecv = recv(sClient, buffer, 256, 0);  

			if (nRecv > 0)  
			{  
				buffer[nRecv] = '\0';  

				cout << "reveive size: " << nRecv << endl; 
				cout << "reveive data: " << buffer << endl; 
				for (int i=0; i<nRecv; ++i)
				{
					cout << setfill('0') << setw(3) << (int)(buffer[i]) << " ";
					if ((i+1)%20 == 0)
						cout << endl;
				}
				//cout << "reveive data: " << buffer << endl; 
			}

		}

		closesocket(sClient);   

	}  

	closesocket(sListen);  

	WSACleanup();  
}

void* CTestMsgServerDlg::ThreadListenClient(void *p)
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
	sin.sin_port = htons(1234);  
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
		static int i = 0;

		string rtMsg = avar("欢迎您，客户端%d\n", ++i);
		//send(sClient, rtMsg.c_str(), strlen(rtMsg.c_str()), 0);  

		m_vecClientSockets.push_back(sClient);


		//closesocket(sClient);   

	}  

	closesocket(sListen);  

	WSACleanup(); 
}

void* CTestMsgServerDlg::ThreadReceivetMsg(void *p)
{
	while (true)
	{
		//一直接收消息
		for (int i=0; i<m_vecClientSockets.size(); ++i)
		{
			SOCKET sClient = m_vecClientSockets[i];
			char buffer[256] = "\0";  
			int  nRecv = 0;  

			nRecv = recv(sClient, buffer, 256, 0);  

			if (nRecv > 0)  
			{  
				//////////////////////////////////////////////////////////////////////////
				//原样发回给客户端
				send(sClient, buffer, nRecv, 0); 

				buffer[nRecv] = '\0';  
				cout << "\n===========来至客户端： " << i+1 << "的消息=============" << endl;
				cout << "reveive size: " << nRecv << endl; 
				cout << "reveive data: " << buffer << endl; 
				for (int i=0; i<nRecv; ++i)
				{
					cout << setfill('0') << setw(3) << (int)(buffer[i]) << " ";
					if ((i+1)%20 == 0)
						cout << endl;
				}
				//cout << "reveive data: " << buffer << endl; 
			}

		}
	}


	return NULL;
}

BEGIN_MESSAGE_MAP(CTestMsgServerDlg, CDialogEx)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_BUTTON1, &CTestMsgServerDlg::OnBnClickedButton1)
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



	if( !m_bThreadListenCreated ){

		if(pthread_create( &pthread_t_listen, NULL,ThreadListenClient, this)!=0)
			return false;
		m_bThreadListenCreated = true;
	}

	if( !m_bThreadRecvCreated ){

		if(pthread_create( &pthread_t_receive, NULL,ThreadReceivetMsg, this)!=0)
			return false;
		m_bThreadRecvCreated = true;
	}



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



void CTestMsgServerDlg::OnBnClickedButton1()
{
	// TODO: 在此添加控件通知处理程序代码
	for (int i=0; i<m_vecClientSockets.size(); ++i)
	{
		SOCKET sClient = m_vecClientSockets[i];
		static int j = 0;
		string rtMsg = avar("欢迎您，客户端%d\n", ++j);
		//send(sClient, rtMsg.c_str(), strlen(rtMsg.c_str()), 0); 
	}
}
