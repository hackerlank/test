
// TestMsgClientDlg.cpp : 实现文件
//

#include "stdafx.h"
#include "TestMsgClient.h"
#include "TestMsgClientDlg.h"
#include "afxdialogex.h"

#include "command.h"

#include "console.h"
#include <iostream>
#include<iomanip>

// #include "json.h"
// #include "SocketClient.h"
// #include "message.h"
// #include "MessageQueue.h"
 #include "SocketManager.h"#include <string>


using namespace std;

#ifdef _DEBUG
#define new DEBUG_NEW
#endif



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


// CTestMsgClientDlg 对话框




CTestMsgClientDlg::CTestMsgClientDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CTestMsgClientDlg::IDD, pParent)
	, m_sendTxt(_T(""))
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
	m_sock = 0;
}

void CTestMsgClientDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Text(pDX, IDC_EDIT1, m_sendTxt);
	DDX_Control(pDX, IDC_EDIT1, m_editCtrol);
}

int CTestMsgClientDlg::InitNetWork()
{
	SocketManager::getInstance()->startSocket();

	return 0;

// 	WSADATA wsaData;  
// 	WORD sockVersion = MAKEWORD(2, 2);  
// 	SOCKET sock = 0;  
// 
// 	if (WSAStartup(sockVersion, &wsaData) != 0)  
// 	{  
// 		cout << "initlization failed!" << endl;  
// 		exit(0);  
// 	}  
// 
// 	sock = ::socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);  
// 
// 	if (sock == INVALID_SOCKET)  
// 	{  
// 		cout << "failed socket!" << endl;  
// 
// 		return 0;  
// 	}  
// 
// 	sockaddr_in sin;  
// 
// 	sin.sin_family = AF_INET;   
// 	sin.sin_port = htons(4567);  
// 	sin.sin_addr.S_un.S_addr = inet_addr("127.0.0.1");  
// 
// 	if (connect(sock, (sockaddr*)&sin, sizeof(sockaddr)) == -1)  
// 	{  
// 		cout << "connect failed!" << endl;  
// 
// 		return 0;  
// 	}  
// 
// 	char buffer[256] = "\0";  
// 	int  nRecv = 0;  
// 
// 	nRecv = recv(sock, buffer, 256, 0);  
// 
// 	if (nRecv > 0)  
// 	{  
// 		buffer[nRecv] = '\0';  
// 
// 		cout << "reveive data: " << buffer << endl;  
// 
// 		m_sock = sock;
// 	}  

 
}

int CTestMsgClientDlg::ReleaseNetWork()
{
	closesocket(m_sock);  

	WSACleanup(); 

	return 0;
}

BEGIN_MESSAGE_MAP(CTestMsgClientDlg, CDialogEx)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_BUTTON1, &CTestMsgClientDlg::OnBnClickedButton1)
	ON_BN_CLICKED(IDOK, &CTestMsgClientDlg::OnBnClickedOk)
	ON_EN_CHANGE(IDC_EDIT1, &CTestMsgClientDlg::OnEnChangeEdit1)
	ON_BN_CLICKED(IDC_BUTTON2, &CTestMsgClientDlg::OnBnClickedButton2)
	ON_BN_CLICKED(IDC_BUTTON3, &CTestMsgClientDlg::OnBnClickedButton3)
END_MESSAGE_MAP()


// CTestMsgClientDlg 消息处理程序

BOOL CTestMsgClientDlg::OnInitDialog()
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
	console::Manager::GetInstance()->Create("/log.htm", true);
	// 
	ConsoleOutEx(console::CONSOLE_COLOR_GREEN, "===============TestClient log!=============\n");

	//InitNetWork();

	return TRUE;  // 除非将焦点设置到控件，否则返回 TRUE
}

void CTestMsgClientDlg::OnSysCommand(UINT nID, LPARAM lParam)
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

void CTestMsgClientDlg::OnPaint()
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
HCURSOR CTestMsgClientDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}



void CTestMsgClientDlg::OnBnClickedButton1()
{
	//	static int i = 0;

	// 	GetDlgItemText(IDC_EDIT1,m_sendTxt);
	// 	string rtMsg = avar("第%d次%s\n", ++i, (LPCTSTR)m_sendTxt);
	// 	send(m_sock, rtMsg.c_str(), strlen(rtMsg.c_str()), 0); 

	// 	Json::FastWriter  writer;
	// 	Json::Value person;
	// 	person["username"]="wumingsheng";
	// 	person["password"]=12345678;
	// 	std::string  json_file=writer.write(person);//192.168.1.210   114.252.70.61  183.60.243.195
	// 	CCLog("%s",json_file.c_str());
	// 	SocketManager::getInstance()->sendMessage(json_file.c_str(), 101);

	//stUserRequestLoginCmd cmd;
	//strcpy(cmd.pstrName, "goodluck02@gmail.com");
	//	cmd.version = 1234;
	//  	cmd.version = 20141015;
	//SEND_USER_CMD(cmd);

	//	SocketManager::getInstance()->send((char*)(&cmd), 10);


	//连接登入服务器
	SocketManager::getInstance()->startSocket();

}


void CTestMsgClientDlg::OnBnClickedOk()
{
	// TODO: 在此添加控件通知处理程序代码
	CDialogEx::OnOK();
	ReleaseNetWork();

}


void CTestMsgClientDlg::OnEnChangeEdit1()
{
	// TODO:  如果该控件是 RICHEDIT 控件，它将不
	// 发送此通知，除非重写 CDialogEx::OnInitDialog()
	// 函数并调用 CRichEditCtrl().SetEventMask()，
	// 同时将 ENM_CHANGE 标志“或”运算到掩码中。

	// TODO:  在此添加控件通知处理程序代码
	//m_sendTxt = GetDlgItemText(IDC_EDIT1);
}


void CTestMsgClientDlg::OnBnClickedButton2()
{
	// TODO: 在此添加控件通知处理程序代码
	stServerReturnLoginSuccessCmd cmd;
	SEND_USER_CMD(cmd);
}


void CTestMsgClientDlg::OnBnClickedButton3()
{
	// TODO: 在此添加控件通知处理程序代码
	stUserRequestLoginCmd cmd;
	SEND_USER_CMD(cmd);
}
