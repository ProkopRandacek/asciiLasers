using System;
using System.Linq;
using System.Threading;

namespace asciiLasers {
    internal static class Program {
        private static void Main(string[] argv) {
            string filename = argv.Length == 0 ? "./main.al" : argv[0];
            Console.Clear();
            
            Board board = BoardFactory.CreateBoardFromFile(filename);
            
            board.Init();
            board.InitRender();
            int tick = 0;
            while (!board.ShouldStop && (tick < 1_000_000)) {
                board.Render();
                board.Eval();
                tick++;
                Thread.Sleep(200);
            }
            board.Eval();
            board.Render();

            Console.WriteLine($"Exit code: {board.ExitCode}\nTicks: {tick}");
        }
    }
}
