using NLog;
using BlogsConsole.Models;
using System;
using System.Linq;
using NLog.Fluent;
using System.Collections.Generic;

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
                    Console.WriteLine("4)Display Posts");
                    Console.WriteLine("Enter to quit");
                    choice = Console.ReadLine();
                    logger.Info("User choice: {choice}", choice);
                    if (choice == "1")
                    {
                        var db = new BloggingContext();
                        var query = db.Blogs.OrderBy(b => b.Name);
                        Console.WriteLine($"{query.Count()} blogs returned");
                        foreach (var item in query)
                        {
                            Console.WriteLine(item.Name);
                        }
                    }
                    else if (choice == "2")
                    {
                        Console.Write("Enter a name for a new Blog: ");
                        var name = Console.ReadLine();
                        if (name.Length == 0)
                        {
                            logger.Error("Blog name cannot be null");
                        }
                        else
                        {
                            var blog = new Blog { Name = name };

                            var db = new BloggingContext();
                            db.AddBlog(blog);
                            logger.Info("Blog added - {name}", name);
                        }
                    }
                    else if (choice == "3")
                    {
                        var db = new BloggingContext();
                        Console.Write("Please choose a Blog you would like to post to: \n");
                        var query = db.Blogs.OrderBy(b => b.BlogId);
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.BlogId}) {item.Name}");
                        }
                        if (int.TryParse(Console.ReadLine(), out int BlogId))
                        {
                            if (db.Blogs.Any(b => b.BlogId == BlogId))
                            {
                                Post post = new Post();
                                post.BlogId = BlogId;
                                Console.WriteLine("Post title: ");
                                post.Title = Console.ReadLine();
                                logger.Info("Post added - {title}", post.Title);
                                if (post.Title.Length == 0)
                                {
                                    logger.Error("Post title cannot be null");
                                }
                                else
                                {
                                    Console.WriteLine("Post Content: ");
                                    post.Content = Console.ReadLine();
                                    db.AddPost(post);
                                    logger.Info("Post added - {content}", post.Content);
                                }
                            }
                            else
                            {
                                logger.Error("There are no Blogs with that ID");
                            }
                        }
                        else
                        {
                            logger.Error("Invalid Blog Id");
                        }
                    }
                    else if (choice == "4")
                    {
                        var db = new BloggingContext();
                        var query = db.Blogs.OrderBy(b => b.BlogId);
                        Console.WriteLine("Please enter the Blog you would like to see the posts for: ");
                        Console.WriteLine("0) Posts from all blogs");
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.BlogId}) {item.Name}");
                        }
                        if (int.TryParse(Console.ReadLine(), out int BlogId))
                        {
                            if (BlogId != 0 && db.Blogs.Count(b => b.BlogId == BlogId) == 0)
                            {
                                logger.Error("No Blogs were saved with that id");
                            }
                            else
                            {
                                var post = db.Posts.OrderBy(p => p.Title);
                                if(BlogId == 0)
                                {
                                    post = db.Posts.OrderBy(p => p.Title);
                                }
                                else
                                {
                                    post = db.Posts.Where(p => p.BlogId == BlogId).OrderBy(p => p.Title);
                                }
                                Console.WriteLine($"{post.Count()} Posts returned");
                                foreach(var item in post)
                                {
                                    Console.WriteLine($"Blog: {item.Blog.Name}");
                                    Console.WriteLine($"Title: {item.Title}");
                                    Console.WriteLine($"Content: {item.Content}\n");
                                }
                            }
                        }
                        else
                        {
                            logger.Error("Blog Id does not exist");
                        }
                    }
                } while (choice == "1" || choice == "2" || choice == "3"|| choice == "4");
                logger.Info("Program ended");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
    }
}
