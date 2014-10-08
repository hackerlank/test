
// TestMsgClientDlg.cpp : ʵ���ļ�
//

#include "stdafx.h"
#include "TestMsgClient.h"
#include "TestMsgClientDlg.h"
#include "afxdialogex.h"

#include "../common/console.h"
#include <iostream>

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


// CTestMsgClientDlg �Ի���




CTestMsgClientDlg::CTestMsgClientDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CTestMsgClientDlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CTestMsgClientDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

int CTestMsgClientDlg::InitNetWork()
{
	WSADATA wsaData;  
	WORD sockVersion = MAKEWORD(2, 2);  
	SOCKET sock = 0;  

	if (WSAStartup(sockVersion, &wsaData) != 0)  
	{  
		cout << "initlization failed!" << endl;  
		exit(0);  
	}  

	sock = ::socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);  

	if (sock == INVALID_SOCKET)  
	{  
		cout << "failed socket!" << endl;  

		return 0;  
	}  

	sockaddr_in sin;  

	sin.sin_family = AF_INET;   
	sin.sin_port = htons(4567);  
	sin.sin_addr.S_un.S_addr = inet_addr("127.0.0.1");  

	if (connect(sock, (sockaddr*)&sin, sizeof(sockaddr)) == -1)  
	{  
		cout << "connect failed!" << endl;  

		return 0;  
	}  

	char buffer[256] = "\0";  
	int  nRecv = 0;  

	nRecv = recv(sock, buffer, 256, 0);  

	if (nRecv > 0)  
	{  
		buffer[nRecv] = '\0';  

		cout << "reveive data: " << buffer << endl;  
	}  

	closesocket(sock);  

	WSACleanup();  
}

BEGIN_MESSAGE_MAP(CTestMsgClientDlg, CDialogEx)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
END_MESSAGE_MAP()


// CTestMsgClientDlg ��Ϣ�������

BOOL CTestMsgClientDlg::OnInitDialog()
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
	console::Manager::GetInstance()->Create("/log.htm", true);
	// 
	ConsoleOutEx(console::CONSOLE_COLOR_GREEN, "===============TestClient log!=============\n");

	InitNetWork();

	return TRUE;  // ���ǽ��������õ��ؼ������򷵻� TRUE
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

// �����Ի��������С����ť������Ҫ����Ĵ���
//  �����Ƹ�ͼ�ꡣ����ʹ���ĵ�/��ͼģ�͵� MFC Ӧ�ó���
//  �⽫�ɿ���Զ���ɡ�

void CTestMsgClientDlg::OnPaint()
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
HCURSOR CTestMsgClientDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

