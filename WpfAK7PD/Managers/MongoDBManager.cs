using BCrypt.Net;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
            catch (Exception e)
            {

            }
        }

        public async Task<bool> CreateOrdinaryUserAsync(User user)
        {
            try
            {
                await UserCollection.InsertOneAsync(user);
                return true;
            }
            catch (Exception e)
            {

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
            catch (Exception e)
            {

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
            catch (Exception e)
            {

            }
            return userBooks;
        }

        public async Task<List<Book>> GetBooks()
        {
            List<Book> books = new List<Book>();
            try
            {
                books = await BookCollection.Find(new BsonDocument()).ToListAsync();
            }
            catch (Exception e)
            {

            }
            return books;
        }

        public async Task CreateBook(Book book)
        {
            try
            {
                await BookCollection.InsertOneAsync(book);
            }
            catch (Exception e)
            {

            }
        }
    }
}
