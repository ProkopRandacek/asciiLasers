using namespace std;

class Object {
	private:
		const static char _symbol = ':'; // char representation of the Object

	protected:
		const static int maxQueueLen = 2;
		int queue[maxQueueLen]; // input queue
		int queueLen; // how many lasers are there in the queue
		int outputDirs; // what directions are output

		Object* outputs[4]; // outputs

		void outputValue(int l); // pushes value to all outputs
	public:

		Object(int outputDirs = 0b0000); 

		void addToQueue(int l); // pushes value to this object's value
		void setOutput(int dir, Object* output); // Sets a output object. Only when constructing the graph

		void eval(); // evaluate the Object and push the output to output objects
		virtual int calc() const; // calculates ouput value.
		virtual char symbol() const { return _symbol; } // return what symbol the Object represents
};

class Increment : public Object {
	using Object::Object;
	private:
		const static char _symbol = 'i';
	protected:
		const static int maxQueueLen = 1; // increment takes only 1 input
	public:
		int calc() const;
		char symbol() const { return _symbol; }
};

