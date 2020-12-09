
/*************************************************************************/
/* Copyright (c) 2020 Chay Palton                                        */
/*                                                                       */
/* Permission is hereby granted, free of charge, to any person obtaining */
/* a copy of this software and associated documentation files (the       */
/* "Software"), to deal in the Software without restriction, including   */
/* without limitation the rights to use, copy, modify, merge, publish,   */
/* distribute, sublicense, and/or sell copies of the Software, and to    */
/* permit persons to whom the Software is furnished to do so, subject to */
/* the following conditions:                                             */
/*                                                                       */
/* The above copyright notice and this permission notice shall be        */
/* included in all copies or substantial portions of the Software.       */
/*                                                                       */
/* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,       */
/* EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF    */
/* MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.*/
/* IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY  */
/* CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,  */
/* TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE     */
/* SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                */
/*************************************************************************/

using System;
using System.Collections.Generic;
using Chess.Utils;


namespace ZobristPolyglotCSharp
{
    class Program
    {
        static int errorCount = 0;

        static void Main(string[] args)
        {
            bool runtest = false;

            Console.WriteLine("Chay's chess.\n");

            if (runtest)
                TestCases();
            else
                BookTester();
        }

        static void BookTester()
        {
            // Move tester

            string filename = "..\\..\\..\\..\\..\\book\\komodo.bin";
            string bookname = "komodo";

            OpeningBooks.Book book = OpeningBooks.LoadBook(bookname, filename);
            MoveChecker("rnbqkbnr/pppppppp/8/8/3P4/8/PPP1PPPP/RNBQKBNR b KQkq d3 0 1", bookname);
        }

        static void TestCases()
        {
      
          
            System.Console.WriteLine("Test Hash");
            // Get next move from current board loyout, which in this case is      
            // the first open move. So should be in every openng book.
            string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            System.UInt64 knownHash = 5060803636482931868;
            System.UInt64 hash = ZobristHash.GetHash(fen);
            System.Console.WriteLine("TEST pass: {0}, Hash {1} : FEN:{0}", (hash == knownHash), hash, fen);
            System.Console.WriteLine("\n\n");
            if (hash != knownHash) errorCount++;

            string filename = "..\\..\\..\\..\\..\\book\\komodo.bin";
            string booknameA = "komodo";

            if(TestCaseLoadBook(filename, booknameA))
            {
                TestCaseHashMove(booknameA);
            }

            filename = "..\\..\\..\\..\\..\\book\\codekiddy.bin";
            string booknameB = "codekiddy";

            if (TestCaseLoadBook(filename, booknameB))
            {
                TestCaseHashMove(booknameB);
            }


            TestCaseMerge(booknameA, booknameB);

            if (errorCount > 0)
            {
                System.Console.WriteLine("*******************************");
                System.Console.WriteLine("**    {0} TESTS FAILED     ****", errorCount);
                System.Console.WriteLine("*******************************");
            }
        }

        static bool TestCaseLoadBook(string filename, string bookname)
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
            
            errorCount++;

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

            errorCount++;

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

            errorCount++;

            return false;
        }


        static bool MoveChecker(string fen, string bookname = null)
        {
            OpeningBooks.Book book = null;
           System.Console.Write("**** MOVE TESTER  **** ");
            System.Console.WriteLine("FE:{0}", fen);

           
            book = OpeningBooks.GetBook(bookname);
            

            System.Console.WriteLine("Test Hash");
            ZobristHash.Result result = ZobristHash.GetHashResult(fen);
            if (result != null)
            {
                System.Console.WriteLine(result);
            } else
            {
                System.Console.WriteLine("HASH ERROR..");
            }

      
            if (book != null && result != null)
            {
                OpeningBooks.Move move = OpeningBooks.GetMoveBest(book, result.hash);
                List<OpeningBooks.Move> moves = OpeningBooks.GetMoveList(book, result.hash);

                if (move != null)
                {
                    System.Console.WriteLine("Found {0} moves", move.ToString());

                    foreach(OpeningBooks.Move m in moves)
                    {
                        System.Console.WriteLine(m.strmove);
                    }
                } else
                {
                    System.Console.WriteLine("No next move for this layout found in book...");
                }
            }

            if (book == null)
            {
                System.Console.WriteLine("No opening book to test move with.");
            }


            return false;
        }
    }
}


