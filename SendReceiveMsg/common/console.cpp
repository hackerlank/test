#include "stdafx.h"
#include "console.h"

#define _OUTUT_CONSOLE

// #ifdef _DEBUG
// #define new DEBUG_NEW
// #endif

const char* avar(const char* pszFmt, ...)
{
	__declspec( thread ) static char szBuffer[1024];
	va_list ap;
	va_start(ap, pszFmt);

	_vsnprintf_s(szBuffer, 1024, pszFmt, ap);
	szBuffer[1023] = 0;

	va_end ( ap );
	return szBuffer;
}

namespace console
{
// 	const DWORD COLORSET[] = 
// 	{
// 		0xffffffff,
// 		0xffffffff,
// 		0xff444444,
// 		0xff00ff00,
// 		0xffff0000,
// 		0xffff8080,
// 		0xffffff00,
// 		0xffffffff,
// 	};

	Win32Con::Win32Con()
	{

	}

	Win32Con::~Win32Con()
	{

	}

	void Win32Con::Create(const char * szTile)
	{
		AllocConsole();
		SetConsoleTitle(szTile);

		m_hConsole = GetStdHandle(STD_OUTPUT_HANDLE);

		Attach(400, 200);
	}

	void Win32Con::Destroy()
	{
		FreeConsole();
	}

	void Win32Con::Attach(int conWidth, int conHeight)
	{
		HANDLE hStd;
		int fd;
		FILE* file;

		//重定向
		hStd = GetStdHandle(STD_INPUT_HANDLE);
		fd = _open_osfhandle(reinterpret_cast<intptr_t>(hStd), _O_TEXT);//文本模式
		file = _fdopen(fd, "r");
		setvbuf(file, NULL, _IONBF, 0); //无h冲
		*stdin = * file;

		//重定向
		hStd = GetStdHandle(STD_OUTPUT_HANDLE);
		COORD size;
		size.X = conWidth;
		size.Y = conHeight;
		SetConsoleScreenBufferSize(hStd, size);
		fd = _open_osfhandle(reinterpret_cast<intptr_t>(hStd), _O_TEXT);//文本模式
		file = _fdopen(fd, "w");
		setvbuf(file, NULL, _IONBF, 0); //无h冲
		*stdout = * file;

		//重定向
		hStd = GetStdHandle(STD_ERROR_HANDLE);
		fd = _open_osfhandle(reinterpret_cast<intptr_t>(hStd), _O_TEXT);//文本模式
		file = _fdopen(fd, "w");
		setvbuf(file, NULL, _IONBF, 0); //无h冲
		*stderr = * file;
	}

	void Win32Con::Output(DWORD dwColor, const char* szConsoleText)
	{
#ifndef _OUTUT_CONSOLE
		return;
#endif
		SetConsoleTextAttribute(m_hConsole, (WORD)dwColor);
		std::cout << szConsoleText;
		SetConsoleTextAttribute(m_hConsole, 7);
	}

	FileLog::FileLog(void)
	{

	}

	FileLog::~FileLog(void)
	{

	}

	bool FileLog::Create(const char* szFileName)
	{
		int iLen = GetCurrentDirectory(0, NULL);

		m_pPathName = new char [lstrlen(szFileName) + iLen + 1];

		GetCurrentDirectory(iLen, m_pPathName);

		lstrcat(m_pPathName, szFileName);

		char szTextBuffer[128];
		FILE* fp(NULL);
		errno_t err = fopen_s(&fp, m_pPathName, "wb");
		if (fp)
		{
			fprintf(fp, "---start---\n");
			_strdate_s(szTextBuffer, 128);
			fprintf(fp, "--%s--", szTextBuffer);
			_strdate_s(szTextBuffer, 128);
			fprintf(fp, "%s\n", szTextBuffer);
			fclose(fp);
			return true;
		}

		return false;
	}

	void FileLog::Destroy()
	{
		FILE* fp(NULL);
		fopen_s(&fp, m_pPathName, "ab");
		if (fp)
		{
			fprintf(fp, "---End---");
			fclose(fp);
		}

		if (m_pPathName != NULL)
		{
			delete m_pPathName;
			m_pPathName = NULL;
		}
	}

	void FileLog::Output(const void* szString)
	{
#ifndef _OUTUT_CONSOLE
		return;
#endif
		FILE* fp(NULL);
		errno_t err = fopen_s(&fp, m_pPathName, "ab");
		if (fp)
		{
			fprintf(fp, "%s", szString);

			fclose(fp);
		}
	}

	IMPLEMENT_SINGLETON(Manager);


	Manager::Manager()
	{

	}

	Manager::~Manager()
	{

	}

	void Manager::Create(const char* szLogFile, bool bWin32Con)
	{
		if (bWin32Con)
		{
			m_pWin32Con = new Win32Con;
 			m_pWin32Con->Create("console");

			m_pFileLog = new FileLog;
			m_pFileLog->Create(szLogFile);
		}
	}


	void Manager::Destroy()
	{
		if (m_pFileLog)
			m_pFileLog->Destroy();

		if (m_pWin32Con)
			m_pWin32Con->Destroy();
	}

	void Manager::Output(const void* szFormatString, ...)
	{
#ifndef _OUTUT_CONSOLE
		return;
#endif

		if (m_pFileLog == NULL && m_pWin32Con == NULL)
			return;

		static char szTextBuffer[2048];
		va_list vapt;
		va_start(vapt, szFormatString);
		_vsnprintf_s((char*)szTextBuffer, 2047, 2047, (const char*)szFormatString, vapt);
		va_end(vapt);

		time_t ti = time(NULL);
		tm* t = localtime(&ti);
		char szTime[MAX_PATH] = {0};
		strftime(szTime, MAX_PATH, "[%y-%m-%d %H:%M:%S] ", t);

		if (m_pWin32Con)
		{
			m_pWin32Con->Output(CONSOLE_COLOR_DEFAULT, szTime);
			m_pWin32Con->Output(CONSOLE_COLOR_DEFAULT, szTextBuffer);
			m_pWin32Con->Output(CONSOLE_COLOR_DEFAULT, "\n");
		}

		if (m_pFileLog)
		{
			m_pFileLog->Output(szTime);
			m_pFileLog->Output(szFormatString);
			m_pFileLog->Output("\n");
		}

	}

	void Manager::OutputEx(DWORD dwColor, const void* szFormatString, ...)
	{
#ifndef _OUTUT_CONSOLE
		return;
#endif

		if (m_pFileLog == NULL && m_pWin32Con == NULL)
			return;

		static char szTextBuffer[2048];
		va_list vapt;
		va_start(vapt, szFormatString);
		_vsnprintf_s((char*)szTextBuffer, 2047, 2047, (const char*)szFormatString, vapt);
		va_end(vapt);


		time_t ti = time(NULL);
		tm* t = localtime(&ti);
		char szTime[MAX_PATH] = {0};
		strftime(szTime, MAX_PATH, "[%y-%m-%d %H:%M:%S] ", t);

		if (m_pWin32Con)
		{
			m_pWin32Con->Output(dwColor, "\n");
			m_pWin32Con->Output(dwColor, szTime);
			m_pWin32Con->Output(dwColor, szTextBuffer);
			m_pWin32Con->Output(dwColor, "\n");
		}

		if (m_pFileLog)
		{
			m_pWin32Con->Output(dwColor, "\n");
			m_pFileLog->Output(szTime);
			m_pFileLog->Output(szTextBuffer);
			m_pFileLog->Output("\n");
		}
	}
}