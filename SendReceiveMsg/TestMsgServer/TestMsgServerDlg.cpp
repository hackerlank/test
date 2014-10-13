
// TestMsgServerDlg.cpp : ʵ���ļ�
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
// ����Ӧ�ó��򡰹��ڡ��˵���� CAboutDlg �Ի���

class CAboutDlg : public CDialogEx
{
public:
	CAboutDlg();

// �Ի�������
	enum { IDD = IDD_ABOUTBOX };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV ֧��

// ʵ��
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


// CTestMsgServerDlg �Ի���


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

		string rtMsg = avar("��ӭ�����ͻ���%d\n", ++i);
		send(sClient, rtMsg.c_str(), strlen(rtMsg.c_str()), 0);  

		//һֱ������Ϣ
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

		string rtMsg = avar("��ӭ�����ͻ���%d\n", ++i);
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
		//һֱ������Ϣ
		for (int i=0; i<m_vecClientSockets.size(); ++i)
		{
			SOCKET sClient = m_vecClientSockets[i];
			char buffer[256] = "\0";  
			int  nRecv = 0;  

			nRecv = recv(sClient, buffer, 256, 0);  

			if (nRecv > 0)  
			{  
				//////////////////////////////////////////////////////////////////////////
				//ԭ�����ظ��ͻ���
				send(sClient, buffer, nRecv, 0); 

				buffer[nRecv] = '\0';  
				cout << "\n===========�����ͻ��ˣ� " << i+1 << "����Ϣ=============" << endl;
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


// CTestMsgServerDlg ��Ϣ�������

BOOL CTestMsgServerDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// ��������...���˵�����ӵ�ϵͳ�˵��С�

	// IDM_ABOUTBOX ������ϵͳ���Χ�ڡ�
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

	// ���ô˶Ի����ͼ�ꡣ��Ӧ�ó��������ڲ��ǶԻ���ʱ����ܽ��Զ�
	//  ִ�д˲���
	SetIcon(m_hIcon, TRUE);			// ���ô�ͼ��
	SetIcon(m_hIcon, FALSE);		// ����Сͼ��

	// TODO: �ڴ���Ӷ���ĳ�ʼ������
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



	return TRUE;  // ���ǽ��������õ��ؼ������򷵻� TRUE
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

// �����Ի��������С����ť������Ҫ����Ĵ���
//  �����Ƹ�ͼ�ꡣ����ʹ���ĵ�/��ͼģ�͵� MFC Ӧ�ó���
//  �⽫�ɿ���Զ���ɡ�

void CTestMsgServerDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // ���ڻ��Ƶ��豸������

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// ʹͼ���ڹ����������о���
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// ����ͼ��
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialogEx::OnPaint();
	}
}

//���û��϶���С������ʱϵͳ���ô˺���ȡ�ù��
//��ʾ��
HCURSOR CTestMsgServerDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}



void CTestMsgServerDlg::OnBnClickedButton1()
{
	// TODO: �ڴ���ӿؼ�֪ͨ����������
	for (int i=0; i<m_vecClientSockets.size(); ++i)
	{
		SOCKET sClient = m_vecClientSockets[i];
		static int j = 0;
		string rtMsg = avar("��ӭ�����ͻ���%d\n", ++j);
		//send(sClient, rtMsg.c_str(), strlen(rtMsg.c_str()), 0); 
	}
}
