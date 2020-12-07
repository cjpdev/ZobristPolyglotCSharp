using System;
using System.Collections.Generic;
using PolyglotCSharp;
using ZobristCSharp;

namespace ZobristPolyglotCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Chay's chess.\n");
            string filename = "..\\..\\..\\..\\..\\book\\komodo.bin";
            string bookname = "komodo";

            TestCase(filename, bookname);
           

            filename = "..\\..\\..\\..\\..\\book\\codekiddy.bin";
            bookname = "codekiddy";

            TestCase(filename, bookname);
        }

        static bool TestCase(string filename, string bookname)
        {
            Console.WriteLine("\n**** Test: Load a opening book, and look for first move in the book. ****");

            // Open a book.

            System.Console.WriteLine("Try to loaded opening book {0}", filename);
            OpeningBooks.Book bookCodeKomodo = OpeningBooks.LoadBook(bookname, filename);


            if (bookCodeKomodo == null)
            {
                System.Console.WriteLine("Could not load opening book {0}", filename);
            }
            else
            {
                System.Console.WriteLine("Book {0} opened", filename);

                // Get next move from current board loyout, which in this case is      
                // the first open move. So should be in every openng book.
                string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
                System.UInt64 knownHash = 5060803636482931868;
                System.UInt64 hash = ZobristHash.GetHash(fen);

                
                System.Console.WriteLine("TEST pass: {0}, Hash {1} : FEN:{0}", (hash == knownHash), hash, fen);

                OpeningBooks.Move move = OpeningBooks.GetMoveBest(bookCodeKomodo, hash);
                List<OpeningBooks.Move> moves = OpeningBooks.GetMoveList(bookCodeKomodo, hash);

                if (move != null)
                {
                    System.Console.WriteLine("Found first move for white {0}", move.ToString());
                    System.Console.WriteLine("*** TEST Passed ***\n");
                    return true;
                }
            }
            System.Console.WriteLine("*** TEST Failed ***\n");
            return false;
        }
    }
}
