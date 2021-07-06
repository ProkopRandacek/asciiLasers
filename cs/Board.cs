using System;
using System.Collections.Generic;

namespace asciiLasers {
    public class Board {
        public Block[] Blocks;

        public readonly Block   Void;
        public readonly Block   Start;
        public readonly char[,] SymbolBoard;
        
        public int Width {
            get { return SymbolBoard.GetLength(0); }
        }

        public int Height {
            get { return SymbolBoard.GetLength(1); }
        }

        public  bool ShouldStop { get; private set; }
        public  int  ExitCode   { get; private set; }

        private bool _somethingEvaluated;

        private List<(int, int, int, int)> _tickedLineA = new();
        private List<(int, int, int, int)> _tickedLineB = new();

        private readonly ConsoleColor _defClr;

        private int  _currentLineWidth = 0;

        private bool _doOutput = false;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="blocks">All regular blocks</param>
        /// <param name="start">This block is the first one to be evaluated. It also evaluates only once</param>
        /// <param name="void">This block is supposed to eat all input and dont do anything</param>
        /// <param name="sb">This board's symbol board</param>
        public Board(Block[] blocks, Block start, Block @void, char[,] sb) {
            Blocks      = blocks;
            Start       = start;
            Void        = @void;
            SymbolBoard = sb;
            _defClr     = Console.BackgroundColor;
        }

        public override string ToString() {
            string str = "";
            str += $"(start: {Start}";
            foreach (Block block in Blocks)
                str += $"({block.Pos.Item2}, {block.Pos.Item2}): {block}";
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
            foreach (Block block in Blocks) block.Eval();
            foreach (Block block in Blocks) block.PushValue();
            if (!_somethingEvaluated) Terminate(0);
            _somethingEvaluated = false;
        }

        /// <summary>
        /// Terminates board execution. (The tick is finished)
        /// </summary>
        public void Terminate(int exitCode) {
            if (ShouldStop) return;
            ShouldStop = true;
            ExitCode   = exitCode;
        }

        /// <summary>
        /// Is called if a Block was evaluated.
        /// </summary>
        public void OnBlockEval() {
            _somethingEvaluated = true;
        }
        public void ValuePushed(Block from, Block to) {
            if (!_doOutput) return;
            _tickedLineA.Add((from.Pos.Item1, from.Pos.Item2, to.Pos.Item1, to.Pos.Item2));
        }

        public void InitRender() {
            if (!_doOutput) return;
            Console.Clear();
            for (int y = 0; y < Height; y++) {
                for (int x = 0; x < Width; x++)
                    Console.Write(SymbolBoard[x, y]);
                Console.WriteLine();
            }
        }

        public void Write(char c) {
            if (!_doOutput) return;
            Console.SetCursorPosition(_currentLineWidth, Height + 1);
            Console.Write(c);
            _currentLineWidth++;
        }

        public void WriteLine(int l) {
            if (!_doOutput) return;
            Console.SetCursorPosition(0, Height + 1);
            for (int i = 0; i < Width; i++) Console.Write(" ");
            Console.SetCursorPosition(0, Height + 1);
            Console.WriteLine(l);
        }
        public void Render() {
            if (!_doOutput) return;
            // TODO: this is very ugly
            foreach ((int x2, int y2, int x1, int y1) in _tickedLineB) {
                (int, int) off = (0, 0);
                if (x1 == x2) {
                    if      (y1 < y2) off = (0, 1);
                    else if (y1 > y2) off = (0, -1);
                } else if (y1 == y2) {
                    if      (x1 < x2) off = (1, 0);
                    else if (x1 > x2) off = (-1, 0);
                } else throw new ArgumentException("not a line");

                for ((int, int) pos = (x1, y1); !pos.Equals((x2, y2)); pos.Item1 += off.Item1, pos.Item2 += off.Item2) {
                    Console.SetCursorPosition(pos.Item1, pos.Item2);
                    Console.Write(SymbolBoard[pos.Item1, pos.Item2]);
                }
            }
            
            _tickedLineB.Clear();
            
            foreach ((int x2, int y2, int x1, int y1) in _tickedLineA) {
                (int, int) off = (0, 0);
                if (x1 == x2) {
                    if      (y1 < y2) off = (0, 1);
                    else if (y1 > y2) off = (0, -1);
                } else if (y1 == y2) {
                    if      (x1 < x2) off = (1, 0);
                    else if (x1 > x2) off = (-1, 0);
                } else throw new ArgumentException("not a line");

                Console.BackgroundColor = ConsoleColor.DarkRed;
                for ((int, int) pos = (x1, y1); !pos.Equals((x2, y2)); pos.Item1 += off.Item1, pos.Item2 += off.Item2) {
                    Console.SetCursorPosition(pos.Item1, pos.Item2);
                    Console.Write(SymbolBoard[pos.Item1, pos.Item2]);
                }
                Console.BackgroundColor = _defClr;
            }
            Console.SetCursorPosition(0, Height + 2);
            
            (_tickedLineA, _tickedLineB) = (_tickedLineB, _tickedLineA); // swap
        }
    }
}
