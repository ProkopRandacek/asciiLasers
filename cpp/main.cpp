#include <iostream>

#include "object.hpp"

using namespace std;

int main() {
	Increment *i = new Increment();
	i->setOutput(0, i);

	i->eval(); // wont do anything since there are no lasers on input
	i->eval(); // -//-
	i->addToQueue(6);
	i->addToQueue(2); // ignored - queue is full
	i->eval(); // evaluets
	i->addToQueue(6); // ignored - queue is filled by eval
	i->eval(); // evaluates
}

