using NLog;
using BlogsConsole.Models;
using System;
using System.Linq;

namespace Blogs_Console
{
    class MainClass
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            logger.Info("Program started");
            string choice = "";
            try
            {
                do
                {
                    Console.WriteLine("1)Display all Blogs");
                    Console.WriteLine("2)Add a Blog");
                    Console.WriteLine("3)Create a Post");
                    Console.WriteLine("Enter to quit");
                    choice = Console.ReadLine();
                    logger.Info("User choice: {choice}", choice);
                    if (choice == "1")
                    {
                        var db = new BloggingContext();
                        var query = db.Blogs.OrderBy(b => b.Name);

                        Console.WriteLine("All blogs in the database:");
                        foreach (var item in query)
                        {
                            Console.WriteLine(item.Name);
                        }
                    }
                    else if (choice == "2")
                    {
                        Console.Write("Enter a name for a new Blog: ");
                        var name = Console.ReadLine();

                        var blog = new Blog { Name = name };

                        var db = new BloggingContext();
                        db.AddBlog(blog);
                        logger.Info("Blog added - {name}", name);
                    }
                    else if (choice == "3")
                    {
                        var db = new BloggingContext();
                        Console.WriteLine("Please choose a Blog you would like to post to: ");
                        var name = Console.ReadLine();
                        var blog = db.Blogs.FirstOrDefault(b => b.Name.Contains(name));
                        Console.WriteLine("What is the title of the Post: ");
                        var title = Console.ReadLine();
                        Console.WriteLine("What is the content of the Post: ");
                        var content = Console.ReadLine();
                        var post = new Post { Title = title, Content = content, BlogId = blog.BlogId };
                        db.AddPost(post);
                        logger.Info("Post added - {title}", title);
                        logger.Info("Post added - {content}", content);

                    }
                } while (choice == "1" || choice == "2" || choice == "3");
                logger.Info("Program ended");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
    }
}
