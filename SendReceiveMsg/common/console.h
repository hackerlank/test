#pragma once

#include <io.h>
#include <fcntl.h>
#include <stdio.h>
#include <iostream>
#include <Windows.h>
#include "singleton.h"

#define ConsoleOut console::Manager::GetInstance()->Output
#define ConsoleOutEx console::Manager::GetInstance()->OutputEx

const char* avar(const char* pszFmt, ...);

namespace console
{
	enum {
		CONSOLE_COLOR_COLOR0 = 0,
		CONSOLE_COLOR_COLOR1 = 1,
		CONSOLE_COLOR_COLOR2 = 2,
		CONSOLE_COLOR_COLOR3 = 3,
		CONSOLE_COLOR_COLOR4 = 4,
		CONSOLE_COLOR_COLOR5 = 5,
		CONSOLE_COLOR_COLOR6 = 6,
		CONSOLE_COLOR_DEFAULT = 7,
		CONSOLE_COLOR_GRAY = 8,
		CONSOLE_COLOR_COLOR9 = 9,
		CONSOLE_COLOR_GREEN = 10,
		CONSOLE_COLOR_CYAN = 11,
		CONSOLE_COLOR_RED = 12,
		CONSOLE_COLOR_PURPLE = 13,
		CONSOLE_COLOR_YELLOW = 14,
		CONSOLE_COLOR_WHITE = 15,
	};

	class Win32Con
	{
		HANDLE m_hConsole;
	public:
		Win32Con();
		~Win32Con();

		void Create(const char* szTitle);
		void Destroy();
		void Output(DWORD dwColor, const char* szText);

	private:
		void Attach(int conWidth, int conHeight);
	};


	class FileLog
	{
		char* m_pPathName;
	public:
		FileLog();
		~FileLog();

		bool Create(const char* szFileName);
		void Destroy();
		void Output(const void* szString);
	};

	class Manager
	{
		DECLARE_SINGLETON(Manager);

		Win32Con * m_pWin32Con;
		FileLog * m_pFileLog;

	public:
		Manager();
		~Manager();

		void Create(const char* szLogFile, bool bWin32Con);
		void Destroy();

		void Output(const void* szFormatString, ...);
		void OutputEx(DWORD dwColor, const void* szFormatString, ...);
	};


}