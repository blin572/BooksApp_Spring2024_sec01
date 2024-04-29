using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using BooksApp_Spring2024_sec01.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace BooksApp_Spring2024_sec01.Data
{
    public class BooksDbContext: IdentityDbContext<IdentityUser>
    {
        public BooksDbContext(DbContextOptions<BooksDbContext> options) : base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }  //adds categories table to database
        public DbSet<Books> Books { get; set; } //adds books table to database
        public DbSet<Cart> Carts { get; set; } //adds cart table to database
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    CategoryID = 1,
                    Name = "Science Fiction",
                    Description = "This is the description for Science Fiction Category"
                },

                new Category
                {
                    CategoryID = 2,
                    Name = "Technology",
                    Description = "This is the description for Technology Category"
                },

                new Category
                {
                    CategoryID = 3,
                    Name = "History",
                    Description = "This is the description for History Category"
                }
                );
            modelBuilder.Entity<Books>().HasData(
                new Books
                {
                    BookID = 1,
                    Title = "Grate Expectations",
                    Author = "Charles Dickens",
                    Description = "13th century novel about educating an orphan nicknamed Pip",
                    Price = 19.99m,
                    ISBN = "82HB837HUN",
                    CategoryID = 3,
                    ImgUrl = ""
                },

                new Books
                {
                    BookID = 2,
                    Title = "The Cat in the Hat",
                    Author = "Dr. Seuss",
                    Description = "Children's novel about a talking cat",
                    Price = 11.99m,
                    ISBN = "82HB927DNS",
                    CategoryID = 1,
                    ImgUrl = ""
                },

                new Books
                {
                    BookID = 3,
                    Title = "Brown Bear, Brown Bear, What Do You See?",
                    Author = "Bill Martin",
                    Description = "Children's book about colors",
                    Price = 5.99m,
                    ISBN = "73BS82DSHN",
                    CategoryID = 2,
                    ImgUrl = ""
                }
                );
            

        }
    }
}
