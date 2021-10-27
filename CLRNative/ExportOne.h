#pragma once

namespace clrn {
	class ExportOneImp;

	class __declspec(dllexport) ExportOne {
		int a;
		ExportOneImp* imp;
	public:
		ExportOne();
		~ExportOne();
		int AddA();
	};
}