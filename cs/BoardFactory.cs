using System;
using System.Collections.Generic;
using System.IO;

namespace asciiLasers {
    public static class BoardFactory {
        private const string Blocks  = "idmnasl*#}$&";
        private const string Consts  = "0123456789ABCDEF";
        private const string Mirrors = "><^v";

        public static Board CreateBoardFromFile(string filename) {
            char[,] symbolBoard = FileToSymbolBoard(filename);

            Dictionary<(int, int), Block> graph;

            Block start;
            Block @void = new((char)0, 0, (-1, -1), null);
            (graph, start) = SymbolBoardToGraph(symbolBoard);

            Board board = new(graph, start, @void, symbolBoard);

            // Create link from blocks to the board
            foreach ((_, Block block) in board.Blocks) {
                for (int i = 0; i < block.Outputs.Length; i++)
                    block.Outputs[i] ??= board.Void;
                block.Board = board;
            }

            start.Board = board;
            @void.Board = board;
            for (int i = 0; i < start.Outputs.Length; i++)
                start.Outputs[i] ??= board.Void;

            return board;
        }
        
        private static char[,] FileToSymbolBoard(string filename) {
            string data   = "";
            int    width  = 0;
            int    height = 0;
            using (StreamReader sr = new(filename)) {
                int currentWidth = 0;
                while (sr.Peek() >= 0) {
                    char symbol = (char)sr.Read();
                    data += symbol;
                    if (symbol == '\n') {
                        height++;
                        width = Math.Max(width, currentWidth);
                        currentWidth  = 0;
                    } else
                        currentWidth++;
                }
            }

            char[,] symbolBoard = new char[width, height];
            for (int i = 0; i < (width * height); i++)
                symbolBoard[i % width, i / width] = ' ';

            int x = 0;
            int y = 0;
            foreach (char c in data)
                if (c == '\n') {
                    x = 0;
                    y++;
                } else {
                    symbolBoard[x, y] = c;
                    x++;
                }

            return symbolBoard;
        }

        private static (Dictionary<(int, int), Block>, Block) SymbolBoardToGraph(char[,] sb) {
            Dictionary<(int, int), Block> blocks = new();

            Block      start    = null;
            (int, int) startPos = (0, 0);

            // First iteration creates blocks without links between
            for (int y = 0; y < sb.GetLength(1); y++) {
                for (int x = 0; x < sb.GetLength(0); x++) {
                    char symbol = sb[x, y];
                    if (symbol == '{') {
                        start    = CreateBlock(sb, x, y);
                        startPos = (x, y);
                    } else if (IsBlock(symbol)) blocks[(x, y)]  = CreateBlock(sb, x, y);
                    else if (IsMirror(x, y, sb)) blocks[(x, y)] = CreateMirror(sb, x, y);
                    else if (IsConst(symbol)) blocks[(x, y)]    = CreateCons(sb, x, y);
                }
            }

            if (start == null) throw new Exception("No start found");

            // Link Blocks together
            start.Outputs = FindOutputBlocks(blocks, startPos.Item1, startPos.Item2, sb.GetLength(0), sb.GetLength(1));
            foreach (((int x, int y), Block block) in blocks)
                block.Outputs = FindOutputBlocks(blocks, x, y, sb.GetLength(0), sb.GetLength(1));
            return (blocks, start);
        }

        private static Block CreateBlock(char[,] sb, int x, int y) {
            char symbol = sb[x, y];
            Block block = symbol switch {
                'i' => new Block(symbol, 1, (x, y), (_, queue) => queue[0] + 1),
                'd' => new Block(symbol, 1, (x, y), (_, queue) => queue[0] - 1),
                'm' => new Block(symbol, 2, (x, y), (_, queue) => queue[0] * queue[1]),
                'n' => new Block(symbol, 2, (x, y), (_, queue) => queue[0] / queue[1]),
                'a' => new Block(symbol, 2, (x, y), (_, queue) => queue[0] + queue[1]),
                's' => new Block(symbol, 2, (x, y), (_, queue) => queue[0] - queue[1]),
                'l' => new Block(symbol, 2, (x, y), (_, queue) => queue[0] % queue[1]),
                '{' => new Block(symbol, 1, (x, y), (_, queue) => queue[0]),
                '*' => new Block(symbol, 1, (x, y), (_, queue) => queue[0]),
                '#' => new Block(symbol, 1, (x, y), (_, _) => -1),
                '}' => new Block(symbol, 1, (x, y), (board, queue) => {
                    if (queue.Length != 0) // only terminate if laser actually arrived
                        board.Terminate(queue[0]);
                    return -1;
                }),
                '$' => new Block('$', 1, (x, y), (_, queue) => {
                    //Console.WriteLine(queue[0]);
                    return -1;
                }),
                '&' => new Block('$', 1, (x, y), (_, queue) => {
                    //Console.Write((char) queue[0]);
                    return -1;
                }),
                _ => throw new Exception($"'{symbol}' is not supported")
            };

            block.OutputDirs = FindOutputs(sb, x, y);

            return block;
        }

        private static Block CreateMirror(char[,] sb, int x, int y) {
            char  symbol = sb[x, y];
            Block block = new(symbol, 1, (x, y), (_, queue) => queue[0]) {
                OutputDirs = symbol switch {
                    '>' => (int) DirMask.Right,
                    '<' => (int) DirMask.Left,
                    '^' => (int) DirMask.Up,
                    'v' => (int) DirMask.Down,
                    _   => throw new ArgumentOutOfRangeException($"'{symbol}' is not supported")
                }
            };

            return block;
        }

        private static Block CreateCons(char[,] sb, int x, int y) {
            char  symbol = sb[x, y];
            Block block = symbol switch {
                '0' => new Block(symbol, 1, (x, y), (_, _) => 0),
                '1' => new Block(symbol, 1, (x, y), (_, _) => 1),
                '2' => new Block(symbol, 1, (x, y), (_, _) => 2),
                '3' => new Block(symbol, 1, (x, y), (_, _) => 3),
                '4' => new Block(symbol, 1, (x, y), (_, _) => 4),
                '5' => new Block(symbol, 1, (x, y), (_, _) => 5),
                '6' => new Block(symbol, 1, (x, y), (_, _) => 6),
                '7' => new Block(symbol, 1, (x, y), (_, _) => 7),
                '8' => new Block(symbol, 1, (x, y), (_, _) => 8),
                '9' => new Block(symbol, 1, (x, y), (_, _) => 9),
                'A' => new Block(symbol, 1, (x, y), (_, _) => 10),
                'B' => new Block(symbol, 1, (x, y), (_, _) => 11),
                'C' => new Block(symbol, 1, (x, y), (_, _) => 12),
                'D' => new Block(symbol, 1, (x, y), (_, _) => 13),
                'E' => new Block(symbol, 1, (x, y), (_, _) => 14),
                'F' => new Block(symbol, 1, (x, y), (_, _) => 15),
                _   => throw new ArgumentOutOfRangeException($"'{symbol}' is not supported")
            };
            block.OutputDirs = FindOutputs(sb, x, y);
            return block;
        }

        private static int FindOutputs(char[,] sb, int x, int y) {
            int mask = 0;
            if (((x - 1) >= 0             ) && (sb[x - 1, y] == '<')) mask |= (int) DirMask.Left;
            if (((x + 1) < sb.GetLength(0)) && (sb[x + 1, y] == '>')) mask |= (int) DirMask.Right;
            if (((y - 1) >= 0             ) && (sb[x, y - 1] == '^')) mask |= (int) DirMask.Up;
            if (((y + 1) < sb.GetLength(1)) && (sb[x, y + 1] == 'v')) mask |= (int) DirMask.Down;
            return mask;
        }

        private static Block[] FindOutputBlocks(IReadOnlyDictionary<(int, int), Block> blocks, int bx, int by, int width, int height) {
            // TODO: use not stupid approach
            Block[] outputs = { null, null, null, null };
            
            for (int x = bx - 1; x >= 0; x--) { // Left
                if (!blocks.ContainsKey((x, by))) continue;
                outputs[3] = blocks[(x, by)];
                break;
            }
            for (int x = bx + 1; x < width; x++) { // Right
                if (!blocks.ContainsKey((x, by))) continue;
                outputs[1] = blocks[(x, by)];
                break;
            }
            for (int y = by + 1; y < height; y++) { // Down
                if (!blocks.ContainsKey((bx, y))) continue;
                outputs[2] = blocks[(bx, y)];
                break;
            }
            for (int y = by - 1; y >= 0; y--) { // Up
                if (!blocks.ContainsKey((bx, y))) continue;
                outputs[0] = blocks[(bx, y)];
                break;
            }

            return outputs;
        }

        private static bool IsBlock(char c) => Blocks.Contains(c);
        
        private static bool IsConst(char c) => Consts.Contains(c);

        private static bool IsBlockOrConstOrStart(char c) => Blocks.Contains(c) || Consts.Contains(c) || c == '{';

        private static bool IsMirror(int x, int y, char[,] sb) {
            char c = sb[x, y];
            if (!Mirrors.Contains(c)) return false;
            switch (c) { // check if it is not *output* mirror
                case 'v' when IsBlockOrConstOrStart(SafeSymbolGet(sb, x, y - 1)):
                case '^' when IsBlockOrConstOrStart(SafeSymbolGet(sb, x, y + 1)):
                case '>' when IsBlockOrConstOrStart(SafeSymbolGet(sb, x - 1, y)):
                case '<' when IsBlockOrConstOrStart(SafeSymbolGet(sb, x + 1, y)):
                    return false;
                default:
                    return true;
            }
        }

        private static char SafeSymbolGet(char[,] sb, int x, int y) {
            int width = sb.GetLength(0);
            int height = sb.GetLength(1);
            if ((x >= width) || (x < 0)) return ' ';
            if ((y >= height) || (y < 0)) return ' ';
            return sb[x, y];
        }
    }
}
