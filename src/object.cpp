#include <iostream>
#include "object.hpp"

Object::Object(int outputDirs) {
	this->outputDirs = outputDirs;
}

void Object::addToQueue(int l) {
	if (queueLen < maxQueueLen) { // Only register inputs if queue is not full
		queue[queueLen] = l;
		queueLen++;
	}
}

void Object::outputValue(int l) {
	if (outputDirs & 0b1000) { outputs[0]->addToQueue(l); }
	if (outputDirs & 0b0100) { outputs[1]->addToQueue(l); }
	if (outputDirs & 0b0010) { outputs[2]->addToQueue(l); }
	if (outputDirs & 0b0001) { outputs[3]->addToQueue(l); }
}

void Object::setOutput(int dir, Object* output) {
	outputs[dir] = output;
	if      (dir == 0) outputDirs |= 0b1000;
	else if (dir == 1) outputDirs |= 0b0100;
	else if (dir == 2) outputDirs |= 0b0010;
	else if (dir == 3) outputDirs |= 0b0001;
}

void Object::eval() {
	if (queueLen != 0) { // only if there are laser on input
		queueLen = 0;

		int output = this->calc();

		outputValue(output);
		cout << this->symbol() << " outputed " << output << endl;
	}
}

int Object::calc() const {
	return 0;
}

int Increment::calc() const {
	return queue[0] + 1;
}

