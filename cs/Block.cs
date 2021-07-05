using System;

namespace asciiLasers {
    public enum DirMask {
        Up    = 0b0001,
        Right = 0b0010,
        Down  = 0b0100,
        Left  = 0b1000
    }
    
    public class Block {
        public int     OutputDirs; // This is a mask (see DirMask)
        public Board   Board;
        public Block[] Outputs;

        
        private readonly Func<Board, int[], int> _calc;

        private readonly char  _symbol;
        private readonly int   _maxQueueLen; // Maximal length of the queue
        private readonly int[] _queue;

        private int _queueLen; // How long the queue is right now
        private int _output;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="symbol">The ascii symbol that represents this block</param>
        /// <param name="maxQueueLen">Number of inputs this Block takes</param>
        /// <param name="calc">This block's logic implementation</param>
        public Block(char symbol, int maxQueueLen, Func<Board, int[], int> calc) {
            _symbol      = symbol;
            _calc        = calc;
            _maxQueueLen = maxQueueLen;
            _queue       = new int[3];

            OutputDirs = 0;
            Outputs    = new Block[4];
        }

        public override string ToString() {
            return $"{_symbol} -> ({Outputs[0]._symbol}, {Outputs[1]._symbol}, {Outputs[2]._symbol}, {Outputs[3]._symbol})\n";
        }

        /// <summary>
        /// Calculates the output value from queue. Doesn't push output to other blocks.
        /// </summary>
        public void Eval() {
            if (_queueLen == 0) return; // Dont evaluated if we don't have any inputs
            _queueLen = 0;
            _output   = _calc(Board, _queue);
            Board.BlockEvaluated();
        }

        /// <summary>
        /// Pushes the calculated output into output blocks.
        /// </summary>
        public void PushValue() {
            if (_output == -1) return; // don't send output if we have dont have one
            for (int i = 0; i < 4; i++) // for each direction
                if ((OutputDirs & (1 << i)) != 0) // if that direction is enabled
                    Outputs[i].AddToQueue(_output); // push value there
        }

        /// <summary>
        /// Adds laser to the queue.
        /// </summary>
        /// <param name="l">Laser value</param>
        public void AddToQueue(int l) {
            if (_queueLen >= _maxQueueLen) return; // Don't register inputs if queue is already full
            _queue[_queueLen] = l;
            _queueLen++;
        }
    }
}
