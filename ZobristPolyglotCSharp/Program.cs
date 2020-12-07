using System;
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

            System.Console.WriteLine("Try load openbook {0}", filename);
            OpeningBooks.Book bookCodeKomodo = OpeningBooks.LoadBook(bookname, filename);


            if (bookCodeKomodo != null)
            {
                System.Console.WriteLine("Book {0} opened", filename);
            }
            else
            {
                System.Console.WriteLine("Could load openbook {0}", filename);
            }


        }
    }
}
