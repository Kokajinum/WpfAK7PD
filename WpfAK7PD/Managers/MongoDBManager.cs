using BCrypt.Net;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfAK7PD.Models;

namespace WpfAK7PD.Managers
{
    public class MongoDBManager
    {
        public MongoClient MongoClient { get; set; }

        public IMongoDatabase CurrentDatabase { get; set; }

        public IMongoCollection<User> UserCollection { get; set; }

        public IMongoCollection<Book> BookCollection { get; set; }

        public IMongoCollection<UserBook> UserBookCollection { get; set; }

        public MongoDBManager(string connectionString)
        {
            try
            {
                MongoClient = new MongoClient(connectionString);
                CurrentDatabase = MongoClient.GetDatabase("DB-Knihovna");
                UserCollection = CurrentDatabase.GetCollection<User>("User");
                BookCollection = CurrentDatabase.GetCollection<Book>("Book");
                UserBookCollection = CurrentDatabase.GetCollection<UserBook>("UserBook");

                //List<string> databases = MongoClient.ListDatabaseNames().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CreateOrdinaryUserAsync(User user)
        {
            try
            {
                await UserCollection.InsertOneAsync(user);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        public async Task<User> LoginUserAsync(string login, string password)
        {
            try
            {
                User user = await UserCollection.Find(x => x.Login == login).FirstOrDefaultAsync();
                if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    return user;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<List<UserBook>> GetUserBooks(User user)
        {
            List<UserBook> userBooks = new List<UserBook>();
            try
            {
                userBooks = await UserBookCollection.Find(x => x.UserId == user.Id.ToString()).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
            return userBooks;
        }

        public async Task CreateUserBook(UserBook userBook)
        {
            try
            {
                await UserBookCollection.InsertOneAsync(userBook);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteUserBook(UserBook userBook)
        {
            try
            {
                await UserBookCollection.DeleteOneAsync(x => x.Id == userBook.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Book>> GetBooks()
        {
            BookCollection = null;
            List<Book> books = new List<Book>();
            try
            {
                books = await BookCollection.Find(new BsonDocument()).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
            return books;
        }

        public async Task CreateBook(Book book)
        {
            try
            {
                await BookCollection.InsertOneAsync(book);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteBook(Book book)
        {
            try
            {
                await BookCollection.DeleteOneAsync(x => x.Id == book.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateBook(Book book)
        {
            try
            {
                var filter = Builders<Book>.Filter.Eq("_id", book.Id);
                await BookCollection.ReplaceOneAsync(filter, book);
            }
            catch (Exception)
            {
                throw;
            }
        }

        
    }
}
