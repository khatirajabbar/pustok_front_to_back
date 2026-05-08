using pustok_front_to_back.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace pustok_front_to_back.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(AppDbContext context, UserManager<User> userManager)
    {
        // Create admin user if it doesn't exist
        var adminEmail = "admin@pustok.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        
        if (adminUser == null)
        {
            adminUser = new User
            {
                FirstName = "Admin",
                LastName = "User",
                Email = adminEmail,
                UserName = adminEmail,
                IsEmailVerified = true,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            };

            var result = await userManager.CreateAsync(adminUser, "Admin@123");
            if (!result.Succeeded)
            {
                throw new Exception($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            // Add admin user to Admin role
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }

        // Check if database has any data already
        if (context.Categories.Any() || context.Authors.Any() || context.Products.Any())
        {
            return; // Database has been seeded
        }

        // Seed Categories
        var categories = new List<Category>
        {
            new Category 
            { 
                Name = "Fiction", 
                Description = "Fictional books and novels",
                CreatedAt = DateTime.Now,
                IsDeleted = false
            },
            new Category 
            { 
                Name = "Science & Technology", 
                Description = "Books about science and technology",
                CreatedAt = DateTime.Now,
                IsDeleted = false
            },
            new Category 
            { 
                Name = "Business & Money", 
                Description = "Business and financial books",
                CreatedAt = DateTime.Now,
                IsDeleted = false
            },
            new Category 
            { 
                Name = "Biography", 
                Description = "Biographical works",
                CreatedAt = DateTime.Now,
                IsDeleted = false
            },
            new Category 
            { 
                Name = "Self-Help", 
                Description = "Personal development books",
                CreatedAt = DateTime.Now,
                IsDeleted = false
            }
        };

        context.Categories.AddRange(categories);
        context.SaveChanges();

        // Seed Authors
        var authors = new List<Author>
        {
            new Author
            {
                Name = "J.K. Rowling",
                Email = "jk@example.com",
                Bio = "British author, best known for the Harry Potter series",
                ProfileImage = "/image/others/default-author.jpg",
                CreatedAt = DateTime.Now,
                IsDeleted = false
            },
            new Author
            {
                Name = "George R.R. Martin",
                Email = "george@example.com",
                Bio = "American novelist famous for A Song of Ice and Fire",
                ProfileImage = "/image/others/default-author.jpg",
                CreatedAt = DateTime.Now,
                IsDeleted = false
            },
            new Author
            {
                Name = "Stephen King",
                Email = "stephen@example.com",
                Bio = "Horror and thriller author with numerous bestsellers",
                ProfileImage = "/image/others/default-author.jpg",
                CreatedAt = DateTime.Now,
                IsDeleted = false
            },
            new Author
            {
                Name = "Haruki Murakami",
                Email = "haruki@example.com",
                Bio = "Japanese author known for surrealist novels",
                ProfileImage = "/image/others/default-author.jpg",
                CreatedAt = DateTime.Now,
                IsDeleted = false
            },
            new Author
            {
                Name = "Tony Robbins",
                Email = "tony@example.com",
                Bio = "Self-help author and motivational speaker",
                ProfileImage = "/image/others/default-author.jpg",
                CreatedAt = DateTime.Now,
                IsDeleted = false
            }
        };

        context.Authors.AddRange(authors);
        context.SaveChanges();

        // Seed Sliders
        var sliders = new List<Slider>
        {
            new Slider
            {
                Title = "Harry Potter Collection",
                Description = "Discover the magical world of Hogwarts",
                Image = "/image/bg-images/slider-1.jpg",
                ButtonText = "Shop Now",
                ButtonLink = "/Home/Shop",
                Order = 1,
                IsActive = true,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            },
            new Slider
            {
                Title = "Latest Science Books",
                Description = "Explore the mysteries of science and universe",
                Image = "/image/bg-images/slider-2.jpg",
                ButtonText = "Explore",
                ButtonLink = "/Home/Shop",
                Order = 2,
                IsActive = true,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            },
            new Slider
            {
                Title = "Business & Success",
                Description = "Learn secrets of successful entrepreneurs",
                Image = "/image/bg-images/slider-3.jpg",
                ButtonText = "Learn More",
                ButtonLink = "/Home/Shop",
                Order = 3,
                IsActive = true,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            }
        };

        context.Sliders.AddRange(sliders);
        context.SaveChanges();

        // Seed Products
        var products = new List<Product>
        {
            new Product
            {
                Title = "Harry Potter and the Philosopher's Stone",
                Description = "The first book in the Harry Potter series. Harry discovers he is a wizard and attends Hogwarts School of Witchcraft and Wizardry.",
                Price = 15.99m,
                SalePrice = 12.99m,
                Image = "/image/products/product-1.jpg",
                CategoryId = categories.First(c => c.Name == "Fiction").Id,
                AuthorId = authors.First(a => a.Name == "J.K. Rowling").Id,
                Sku = "HPP-001",
                Stock = 50,
                ViewCount = 156,
                IsFeatured = true,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            },
            new Product
            {
                Title = "A Game of Thrones",
                Description = "The first novel in A Song of Ice and Fire series. An epic fantasy novel set in the fictional continents of Westeros and Essos.",
                Price = 18.99m,
                SalePrice = 15.99m,
                Image = "/image/products/product-2.jpg",
                CategoryId = categories.First(c => c.Name == "Fiction").Id,
                AuthorId = authors.First(a => a.Name == "George R.R. Martin").Id,
                Sku = "GOT-001",
                Stock = 35,
                ViewCount = 243,
                IsFeatured = true,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            },
            new Product
            {
                Title = "The Shining",
                Description = "A horror novel about a family isolated in a hotel for the winter, where the father descends into madness.",
                Price = 14.99m,
                SalePrice = 11.99m,
                Image = "/image/products/product-3.jpg",
                CategoryId = categories.First(c => c.Name == "Fiction").Id,
                AuthorId = authors.First(a => a.Name == "Stephen King").Id,
                Sku = "TSH-001",
                Stock = 40,
                ViewCount = 128,
                IsFeatured = false,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            },
            new Product
            {
                Title = "Norwegian Wood",
                Description = "A novel about a lonely protagonist navigating love, loss, and friendship in 1960s Tokyo.",
                Price = 16.99m,
                SalePrice = 13.99m,
                Image = "/image/products/product-4.jpg",
                CategoryId = categories.First(c => c.Name == "Fiction").Id,
                AuthorId = authors.First(a => a.Name == "Haruki Murakami").Id,
                Sku = "NW-001",
                Stock = 45,
                ViewCount = 89,
                IsFeatured = false,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            },
            new Product
            {
                Title = "Awaken the Giant Within",
                Description = "A self-help book by Tony Robbins on personal power, productivity, and fulfilling your potential.",
                Price = 19.99m,
                SalePrice = 16.99m,
                Image = "/image/products/product-5.jpg",
                CategoryId = categories.First(c => c.Name == "Self-Help").Id,
                AuthorId = authors.First(a => a.Name == "Tony Robbins").Id,
                Sku = "AGW-001",
                Stock = 60,
                ViewCount = 312,
                IsFeatured = true,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            },
            new Product
            {
                Title = "Harry Potter and the Chamber of Secrets",
                Description = "The second book in the Harry Potter series. Harry returns to Hogwarts for his second year.",
                Price = 15.99m,
                SalePrice = 12.99m,
                Image = "/image/products/product-6.jpg",
                CategoryId = categories.First(c => c.Name == "Fiction").Id,
                AuthorId = authors.First(a => a.Name == "J.K. Rowling").Id,
                Sku = "HPP-002",
                Stock = 55,
                ViewCount = 198,
                IsFeatured = true,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            },
            new Product
            {
                Title = "A Clash of Kings",
                Description = "The second book in A Song of Ice and Fire series.",
                Price = 18.99m,
                SalePrice = 15.99m,
                Image = "/image/products/product-7.jpg",
                CategoryId = categories.First(c => c.Name == "Fiction").Id,
                AuthorId = authors.First(a => a.Name == "George R.R. Martin").Id,
                Sku = "GOT-002",
                Stock = 30,
                ViewCount = 167,
                IsFeatured = false,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            },
            new Product
            {
                Title = "It",
                Description = "A horror novel about a group of friends who confront an ancient evil entity in their hometown.",
                Price = 17.99m,
                SalePrice = 14.99m,
                Image = "/image/products/product-8.jpg",
                CategoryId = categories.First(c => c.Name == "Fiction").Id,
                AuthorId = authors.First(a => a.Name == "Stephen King").Id,
                Sku = "IT-001",
                Stock = 25,
                ViewCount = 234,
                IsFeatured = false,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            },
            new Product
            {
                Title = "Kafka on the Shore",
                Description = "A surreal novel about a young man on a journey of self-discovery.",
                Price = 16.99m,
                SalePrice = 13.99m,
                Image = "/image/products/product-9.jpg",
                CategoryId = categories.First(c => c.Name == "Fiction").Id,
                AuthorId = authors.First(a => a.Name == "Haruki Murakami").Id,
                Sku = "KOS-001",
                Stock = 38,
                ViewCount = 145,
                IsFeatured = false,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            },
            new Product
            {
                Title = "Unlimited Power",
                Description = "Personal development book about breaking through limitations and achieving excellence.",
                Price = 18.99m,
                SalePrice = 15.99m,
                Image = "/image/products/product-10.jpg",
                CategoryId = categories.First(c => c.Name == "Self-Help").Id,
                AuthorId = authors.First(a => a.Name == "Tony Robbins").Id,
                Sku = "UP-001",
                Stock = 70,
                ViewCount = 289,
                IsFeatured = true,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            }
        };

        context.Products.AddRange(products);
        context.SaveChanges();
    }
}