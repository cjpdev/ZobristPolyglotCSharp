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
            string booknameA = "komodo";

            if(TestCase(filename, booknameA))
            {
                TestCaseHashMove(booknameA);
            }
           
            filename = "..\\..\\..\\..\\..\\book\\codekiddy.bin";
            string booknameB = "codekiddy";

            if (TestCase(filename, booknameB))
            {
                TestCaseHashMove(booknameB);
            }

            TestCaseMerge(booknameA, booknameB);

        }

        static bool TestCase(string filename, string bookname)
        {
            Console.Write("\n**** TEST CASE START **** ");
            Console.WriteLine("Load an opening book");

            // Open a book.

            System.Console.WriteLine("Try to loaded opening book {0}", filename);
            OpeningBooks.Book book = OpeningBooks.LoadBook(bookname, filename);

            if (book != null)
            {
                System.Console.WriteLine("**** TEST CASE END: PASSED ****");
                return true;
            }
            
            System.Console.WriteLine("Could not load opening book {0}", filename);
            System.Console.WriteLine("**** TEST CASE END: FAILED ****");

            return false ;
        }


        static bool TestCaseHashMove(string bookname)
        {
            System.Console.Write("**** TEST CASE START **** ");
            System.Console.WriteLine("Find first move for white. Every opening book should have at least this.");

            OpeningBooks.Book book = OpeningBooks.GetBook(bookname);

            if (book != null)
            {
                // Get next move from current board loyout, which in this case is      
                // the first open move. So should be in every openng book.
                string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
                System.UInt64 knownHash = 5060803636482931868;
                System.UInt64 hash = ZobristHash.GetHash(fen);


                System.Console.WriteLine("TEST pass: {0}, Hash {1} : FEN:{0}", (hash == knownHash), hash, fen);

                OpeningBooks.Move move = OpeningBooks.GetMoveBest(book, hash);
                List<OpeningBooks.Move> moves = OpeningBooks.GetMoveList(book, hash);

                if (move != null)
                {
                    System.Console.WriteLine("Found first move for white {0}", move.ToString());
                    System.Console.WriteLine("**** TEST CASE END: PASSED ****");
                    return true;
                }
            }
            System.Console.WriteLine("**** TEST CASE END: FAILED ****");
            return false;
        }


        static bool TestCaseMerge(string booknameA, string booknameB)
        {
            System.Console.WriteLine("**** TEST CASE START **** ");
            System.Console.WriteLine("Merging book {0} with book {1}", booknameA, booknameB);

            OpeningBooks.Book komodoBook = OpeningBooks.GetBook(booknameA);
            OpeningBooks.Book codeKiddyBook = OpeningBooks.GetBook(booknameB);

            if (komodoBook != null && codeKiddyBook != null)
            {
                System.Console.WriteLine("Start merge.....\n");

                if (komodoBook.Merge(codeKiddyBook, true))
                {
                    System.Console.WriteLine("Entries merge from {0}\n", booknameB);
                    System.Console.WriteLine("\n**** TEST CASE END: PASSED ****\n");

                    if (TestCaseHashMove(booknameA) == true)
                    {

                        return true;
                    }

                }
                else
                {
                    System.Console.WriteLine("There was nothing to merge form {0}\n", booknameB);
                    System.Console.WriteLine("\n**** TEST CASE END: FAILED ****");
                   
                }
            }
            return false;
        }
    }
}
