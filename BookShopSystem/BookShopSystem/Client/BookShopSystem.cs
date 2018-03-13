using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopSystem.Data;
using System.Data.Entity;
using BookShopSystem.Migrations;

namespace BookShopSystem
{
    class BookShopSystem
    {
        static void Main(string[] args)
        {
            var context = new BookShopContext();
            Console.WriteLine("0: Show First Three Related Books.");
            Console.WriteLine("1: Books Titles By Age Restriction.");
            Console.WriteLine("2: Golden Books With Less Than 5000 Copies.");
            Console.WriteLine("3: Books By Price.");
            Console.WriteLine("4: Not Released Books.");
            Console.WriteLine("5: Book Titles By Category.");
            Console.WriteLine("6: Books Released Before Date.");
            Console.WriteLine("7: Authors Search.");
            Console.WriteLine("8: Books Search.");
            Console.WriteLine("9: Book Titles Search.");
            Console.WriteLine("10: Count Books.");
            Console.WriteLine("11: Total Book Copies.");
            Console.WriteLine("12: Find Profit.");
            Console.WriteLine("13: Most Recent Books.");
            Console.WriteLine("14: Increase Book Copies.");
            Console.WriteLine("15: Remove Books.");
            Console.WriteLine("16: Stored Procedure.");
            Console.WriteLine("17: Exit Program.");
            Console.Write("Enter Number: ");
            int num = int.Parse(Console.ReadLine());
            Console.Clear();
            while (num != 17)
            {
                switch (num)
                {
                    case 0:
                        ShowFirstThreeRelatedBooks(context);
                        break;
                    case 1:
                        BooksTitlesByAgeRestriction(context);
                        break;
                    case 2:
                        GoldenBooksWithLessThan5000Copies(context);
                        break;
                    case 3:
                        BooksByPrice(context);
                        break;
                    case 4:
                        NotReleasedBooks(context);
                        break;
                    case 5:
                        BookTitlesByCategory(context);
                        break;
                    case 6:
                        BooksReleasedBeforeDate(context);
                        break;
                    case 7:
                        AuthorsSearch(context);
                        break;
                    case 8:
                        BooksSearch(context);
                        break;
                    case 9:
                        BookTitlesSearch(context);
                        break;
                    case 10:
                        CountBooks(context);
                        break;
                    case 11:
                        TotalBookCopies(context);
                        break;
                    case 12:
                        FindProfit(context);
                        break;
                    case 13:
                        MostRecentBooks(context);
                        break;
                    case 14:
                        IncreaseBookCopies(context);
                        break;
                    case 15:
                        RemoveBooks(context);
                        break;
                    case 16:
                        StoredProcedure(context);
                        break;
                }
                Console.WriteLine("0: Show First Three Related Books.");
                Console.WriteLine("1: Books Titles By Age Restriction.");
                Console.WriteLine("2: Golden Books With Less Than 5000 Copies.");
                Console.WriteLine("3: Books By Price.");
                Console.WriteLine("4: Not Released Books.");
                Console.WriteLine("5: Book Titles By Category.");
                Console.WriteLine("6: Books Released Before Date.");
                Console.WriteLine("7: Authors Search.");
                Console.WriteLine("8: Books Search.");
                Console.WriteLine("9: Book Titles Search.");
                Console.WriteLine("10: Count Books.");
                Console.WriteLine("11: Total Book Copies.");
                Console.WriteLine("12: Find Profit.");
                Console.WriteLine("13: Most Recent Books.");
                Console.WriteLine("14: Increase Book Copies.");
                Console.WriteLine("15: Remove Books.");
                Console.WriteLine("16: Stored Procedure.");
                Console.WriteLine("17: Exit Program.");
                Console.Write("Enter Number: ");
                num = int.Parse(Console.ReadLine());
                Console.Clear();
            }
        }

        private static void StoredProcedure(BookShopContext context)
        {
            string[] names = Console.ReadLine().Split();

            int count = context.Database.SqlQuery<int>("EXEC dbo.usp_NumberOfBooksByAuthor {0},{1}", names[0], names[1]).First();

            Console.WriteLine($"{names[0]} {names[1]} has written {count} books");
        }

        private static void RemoveBooks(BookShopContext context)
        {
            var books = context.Books.Where(b => b.Copies < 4200);

            Console.WriteLine($"{books.Count()} books were deleted");

            foreach (var b in books)
            {
                context.Books.Remove(b);
            }

            context.SaveChanges();
        }

        private static void IncreaseBookCopies(BookShopContext context)
        {
            string input = "06-06-2013";
            DateTime date = DateTime.Parse(input);

            var books = context.Books.Where(b => b.ReleaseDate > date);
            int index = 0;

            foreach (var b in books)
            {
                b.Copies += 44;
                index++;
            }

            context.SaveChanges();

            Console.WriteLine(index * 44);
        }

        private static void MostRecentBooks(BookShopContext context)
        {
            var categories = context.Categories.Where(c => c.Books.Count > 35).OrderByDescending(c => c.Books.Count);

            foreach (var c in categories)
            {
                Console.WriteLine($"--{c.Name}: {c.Books.Count} books");

                var books = c.Books.OrderByDescending(b => b.ReleaseDate).ThenBy(b => b.Title).Take(3);

                foreach (var b in books)
                {
                    Console.WriteLine($"{b.Title} ({b.ReleaseDate.Value.Year})");
                }
            }
        }

        private static void FindProfit(BookShopContext context)
        {
            var categories = context.Categories.OrderByDescending(c => c.Books.Sum(b => b.Copies * b.Price)).ThenBy(c => c.Name);

            foreach (var c in categories)
            {
                Console.WriteLine($"{c.Name} - ${c.Books.Sum(b => b.Copies * b.Price):F2}");
            }
        }

        private static void TotalBookCopies(BookShopContext context)
        {
            var authors = context.Authors.OrderByDescending(a => a.Books.Sum(b => b.Copies)).Select(a => new { a.FirstName, a.LastName, a.Books });

            foreach (var a in authors)
            {
                Console.WriteLine($"{a.FirstName} {a.LastName} - ({a.Books.Sum(b => b.Copies)})");
            }
        }

        private static void CountBooks(BookShopContext context)
        {
            int input = int.Parse(Console.ReadLine());

            var books = context.Books.Where(b => b.Title.Length > input);

            Console.WriteLine(books.Count());
        }

        private static void BookTitlesSearch(BookShopContext context)
        {
            var input = Console.ReadLine().ToLower();

            var books = context.Books.ToList().Where(a => a.Author.LastName.ToLower().StartsWith(input)).OrderBy(b => b.Id);

            foreach (var b in books)
            {
                Console.WriteLine($"{b.Title} ({b.Author.FirstName} {b.Author.LastName})");
            }
        }

        private static void BooksSearch(BookShopContext context)
        {
            string input = Console.ReadLine().ToLower();

            var books = context.Books.Where(b => b.Title.ToLower().Contains(input));

            foreach (var b in books)
            {
                Console.WriteLine(b.Title);
            }
        }

        private static void AuthorsSearch(BookShopContext context)
        {
            string input = Console.ReadLine();

            var authors = context.Authors.Where(a => a.FirstName.EndsWith(input));

            foreach (var a in authors)
            {
                Console.WriteLine($"{a.FirstName} {a.LastName}");
            }
        }

        private static void BooksReleasedBeforeDate(BookShopContext context)
        {
            string input = Console.ReadLine();
            DateTime date = DateTime.Parse(input);

            var books = context.Books.Where(b => b.ReleaseDate < date);

            foreach (var book in books)
            {
                Console.WriteLine($"{book.Title} - {book.EditionType.ToString()} - {book.Price}");
            }
        }

        private static void BookTitlesByCategory(BookShopContext context)
        {
            List<string> categories = Console.ReadLine().ToLower().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            var books = context.Books.Where(b => b.Categories.Any(c => categories.Contains(c.Name.ToLower()))).OrderBy(b => b.Id);

            foreach (var b in books)
            {
                Console.WriteLine(b.Title);
            }
        }

        private static void NotReleasedBooks(BookShopContext context)
        {
            int year = int.Parse(Console.ReadLine());

            var books = context.Books.Where(b => b.ReleaseDate.Value.Year != year).OrderBy(b => b.Id);

            foreach (var b in books)
            {
                Console.WriteLine(b.Title);
            }
        }

        private static void BooksByPrice(BookShopContext context)
        {
            var books = context.Books.Where(b => b.Price < 5 || b.Price > 40).OrderBy(b => b.Id);

            foreach (var b in books)
            {
                Console.WriteLine($"{b.Title} - ${b.Price}");
            }
        }

        private static void GoldenBooksWithLessThan5000Copies(BookShopContext context)
        {
            var books = context.Books.Where(b => b.EditionType.ToString() == "Gold" && b.Copies < 5000).OrderBy(b => b.Id);

            foreach (var b in books)
            {
                Console.WriteLine(b.Title);
            }
        }

        private static void BooksTitlesByAgeRestriction(BookShopContext context)
        {
            string name = Console.ReadLine().ToLower();

            var books = context.Books.Where(b => b.AgeRestriction.ToString() == name);

            foreach (var b in books)
            {
                Console.WriteLine($"{b.Title}");
            }
        }

        private static void ShowFirstThreeRelatedBooks(BookShopContext context)
        {
            var books = context.Books.Take(3).ToList();

            books[0].RelatedBooks.Add(books[1]);
            books[1].RelatedBooks.Add(books[0]);
            books[0].RelatedBooks.Add(books[2]);
            books[2].RelatedBooks.Add(books[0]);

            context.SaveChanges();

            var booksFromQuery = context.Books.Take(3);

            foreach (var book in booksFromQuery)
            {
                Console.WriteLine($"--{book.Title}");
                foreach (var relatedBook in book.RelatedBooks)
                {
                    Console.WriteLine(relatedBook.Title);
                }
            }
        }
    }
}
