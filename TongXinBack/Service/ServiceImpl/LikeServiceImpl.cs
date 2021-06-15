using MongoDB.Bson;
using MongoDB.Driver;
using ReturnParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TongXinBack.Config;
using TongXinBack.dlls;
using TongXinBack.Model;

namespace TongXinBack.Service
{
    public class LikeServiceImpl : LikeService
    {
        private readonly IMongoCollection<UserLikes> _userLikes;
        private readonly IMongoCollection<UserPost> _userPosts;
        private readonly IMongoCollection<Post> _AllPost;
        public LikeServiceImpl(UserAdminDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _userLikes = database.GetCollection<UserLikes>("UserLikes");
            _userPosts = database.GetCollection<UserPost>("UserPosts");
            _AllPost = database.GetCollection<Post>("AllPostsCapped");
        }



        public void createNewForUser(string username)
        {
            UserLikes likes = new UserLikes();
            likes.username = username;
            likes.likesList = new List<ObjectId>();
            _userLikes.InsertOne(likes);

        }


        public async Task<Result> userLikeAsync(string username, string postId)
        {
            try
            {

                if (isUserLike(username, postId))
                {
                    return Result.error("该用户已收藏");
                }
                Task task1= _userLikes.UpdateOneAsync<UserLikes>(ul => ul.username == username, new BsonDocument("$push",  new BsonDocument("likesList", postId)));
                Task task2= _AllPost.UpdateOneAsync<Post>(ul => ul.Id == postId, new BsonDocument("$push", new BsonDocument("likesList", username)));
                Task task3= _userPosts.UpdateOneAsync<UserPost>(x => x.userPostList.Any<Post>(p => p.Id == postId), new BsonDocument("$push", new BsonDocument("userPostList.$.likesList", username)));
                await task1;
                await task2;
                await task3;
                return Result.success("添加收藏成功");
            } 
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return Result.error("失败");
            }
        }

        public async Task<Result> userUnLikeAsync(string username, string postId)
        {
            try
            {
                Task task1= _userLikes.UpdateOneAsync<UserLikes>(ul => ul.username == username, new BsonDocument("$pull", new BsonDocument("likesList", postId)));
                Task task2= _AllPost.UpdateOneAsync<Post>(ul => ul.Id == postId, new BsonDocument("$pull", new BsonDocument("likesList", username)));
                Task task3=_userPosts.UpdateOneAsync<UserPost>(x => x.userPostList.Any<Post>(p => p.Id == postId), new BsonDocument("$pull", new BsonDocument("userPostList.$.likesList", username)));
                await task1;
                await task2;
                await task3;
                return Result.success("取消收藏成功");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return Result.error("失败");
            }
        }

        public bool isUserLike(string username,string postId)
        {
            UserLikes uk = _userLikes.Find<UserLikes>(ul => ul.username == username).FirstOrDefault();
            if (uk == null)
            {
                return false;
            }
            else
            {
                if (uk.likesList.Any<ObjectId>(id => id.ToString() == postId))
                {
                    return true;
                }
                return false;
            }
        }

        public Result getUserLike(string username, int pageNum, int pageSize)
        {
            PageInfo<Post> page = new PageInfo<Post>();
            page.pageNum = pageNum;
            page.pageSize = pageSize;
            try
            {

                UserLikes uk = _userLikes.Find<UserLikes>(ul => ul.username == username).FirstOrDefault();
                //ul => ul.username == username , ul=> ul.likesList.Skip((pageNum-1)*pageSize).Take(pageSize)
                //UserLikes uk = _userLikes.Find(new BsonDocument { { "username", username }, new BsonDocument ("likesList",BsonDocument.Parse("{ $slice: ["+ (pageNum - 1) * pageSize +","+pageSize+"]}")) }).FirstOrDefault();
                List<Post> postlist = new List<Post>();
                List<ObjectId> temp;
                if (uk == null)
                {

                    return Result.error("没有该用户");
                }
                else
                {
                    int start = (pageNum - 1) * pageSize;
                    temp = uk.likesList.GetRange(start, Math.Min(pageSize, uk.likesList.Count() - start));

                    foreach (ObjectId id in temp)
                    {
                        postlist.Add(_AllPost.Find<Post>(p => p.Id == id.ToString()).FirstOrDefault());
                    }


                    page.total = uk.likesList.Count();
                    page.list = postlist;
                    page.size = page.list.Count();
                    return Result.success(page);
                }
            }
            catch (NullReferenceException e)
            {
                page.list = new List<Post>();
                return Result.success(page);
            }catch(Exception e)
            {
                return Result.error("查询失败");
            }
        }
    }
}
