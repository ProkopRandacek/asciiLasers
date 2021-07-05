using System.Collections.Generic;

namespace asciiLasers {
    public class Board {
        public readonly Dictionary<(int, int), Block> Blocks;

        public readonly Block Void;
        public readonly Block Start;

        public  bool ShouldStop { get; private set; }
        public  int  ExitCode   { get; private set; }
        
        private bool _somethingEvaluated;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="blocks">All regular blocks</param>
        /// <param name="start">This block is the first one to be evaluated. It also evaluates only once</param>
        /// <param name="void">This block is supposed to eat all input and dont do anything</param>
        public Board(Dictionary<(int, int), Block> blocks, Block start, Block @void) {
            Blocks = blocks;
            Start  = start;
            Void   = @void;
        }

        public override string ToString() {
            string str = "";
            str += $"(start: {Start}";
            foreach (((int x, int y), Block block) in Blocks)
                str += $"({x}, {y}): {block}";
            return str;
        }

        public void Init() {
            Start.AddToQueue(1); // Manually trigger start block
            Start.Eval();
            Start.PushValue();
        }

        /// <summary>
        /// Evaluates a single tick.
        /// </summary>
        public void Eval() {
            foreach ((_, Block block) in Blocks) block.Eval();
            foreach ((_, Block block) in Blocks) block.PushValue();
            if (!_somethingEvaluated)
                Terminate(0);
        }

        /// <summary>
        /// Terminates board execution. (The tick is finished)
        /// </summary>
        public void Terminate(int exitCode) {
            ShouldStop = true;
            ExitCode   = exitCode;
        }

        /// <summary>
        /// Is called if a Block was evaluated.
        /// </summary>
        public void BlockEvaluated() {
            _somethingEvaluated = true;
        }
    }
}
