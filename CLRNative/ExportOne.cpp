#include "ExportOne.h"

#include <string>

class clrn::ExportOneImp {
	std::string n;
public:
	ExportOneImp() :n("12321321") {

	}
	~ExportOneImp() {

	}
};

using namespace clrn;

ExportOne::ExportOne():a(123),imp(new ExportOneImp()) {}

ExportOne::~ExportOne() {

}

int ExportOne::AddA() {
	return this->a++;
}