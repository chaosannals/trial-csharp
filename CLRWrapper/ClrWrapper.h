#pragma once

#include <ExportOne.h>

using namespace System;

namespace ClrW {
	public ref class ClrWrapper {
		clrn::ExportOne* pone;
	public:
		ClrWrapper() :pone(new clrn::ExportOne()) {}
		~ClrWrapper() {
			delete this->pone;
		}
		!ClrWrapper() {
			delete this->pone;
		}
		int AddA() {
			return this->pone->AddA();
		}
	};
}