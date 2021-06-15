using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TongXinBack.Model;
using MongoDB.Driver;
using TongXinBack.Config;
using TongXinBack.DTO;
using Microsoft.AspNetCore.Mvc;
using ReturnParam;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using IdentityCode;
using System.Net.Mail;
using StackExchange.Redis;
using MongoDB.Bson;

namespace TongXinBack.Service
{
    public class UserServiceImpl : UserService
    {
        private readonly IMongoCollection<User> _users;
        private readonly PostService _postService;
        private readonly FollowService _followService;
        private readonly CommentService _commentService;
        private readonly IdentityCodeService _identityCodeService;
        private readonly LikeService _likeService;
        //public UserServiceImpl()
        //{
        //    var client = new MongoClient("mongodb://81.68.78.139:17017");
        //    var database = client.GetDatabase("TongXin");

        //    _users = database.GetCollection<User>("Users");

        //}
        public UserServiceImpl(UserAdminDatabaseSettings settings,PostService postService, FollowService followService, CommentService commentService,IdentityCodeServiceImpl identityCodeService,LikeService likeService)
        {
            _postService = postService;
            _followService = followService;
            _likeService = likeService;
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _commentService = commentService;
            _users = database.GetCollection<User>("Users");
            _identityCodeService = identityCodeService;
        }   

        public Result login(UserLoginParam userLoginParam)
        {

            User tempUser= Get(userLoginParam.username);
            if (tempUser == null)
            {
                return  Result.error("该用户还未注册");
            }
            else
            {
                if (tempUser.password == userLoginParam.password)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                        new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddMinutes(30)).ToUnixTimeSeconds()}"),
                        new Claim(ClaimTypes.Name, userLoginParam.username)
                    };
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTSettings.SecurityKey));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            issuer: JWTSettings.Domain,
                            audience: JWTSettings.Domain,
                            claims: claims,
                            expires: DateTime.Now.AddMinutes(30),
                            signingCredentials: creds);

                    UserToken userToken = new UserToken();
                    userToken.token = new JwtSecurityTokenHandler().WriteToken(token);
                    userToken.username = userLoginParam.username;
                        return Result.success(userToken);
                    }
                else
                {
                    return Result.error("密码错误");
                }
                
            }
            
        }

        
        public Result register(UserRegisterParam newUser)
        {
            //验证code
            if( _identityCodeService.verifyCode(newUser.username, newUser.code))
            {
                if (Get(newUser.username) == null)
                {
                    User user = new User();
                    user.regtime = DateTime.Now;
                    user.nickname = newUser.nickname;
                    user.password = newUser.password;
                    user.stu_id = newUser.stuid;
                    user.username = newUser.username;
                    user.followed = 0;
                    user.following = 0;
                    //ToDo 设置默认头像
                    user.avatar = "/avatar/default.png";
                    _users.InsertOne(user);
                    //创建用户空帖子仓库
                     _postService.CreateNullList(newUser.username);
                    //创建跟随者followed仓库
                    _postService.CreateUserFollowedNullList(newUser.username);
                    //创建评论库
                    _commentService.CreateUserCommentNullList(newUser.username);
                    
                    _followService.CreateNullList(newUser.username);

                    //TODO 创建 like库
                    _likeService.createNewForUser(newUser.username);
                    
                    
                    return Result.success();
                }
                else
                {
                    return Result.error("00106","该用户已被注册");
                }
            }

            return Result.error("00105","验证码错误！");


        }

        public User Get(string username) =>
            _users.Find<User>(user => user.username == username).FirstOrDefault();

        public Result getByUsername(string username)
        {

            return Result.success( Get(username));
        }
        public  Result getAllUser()
        {
            return Result.success(myAllUser());
        }

        private List<User> myAllUser() => _users.Find<User>(user=>true).ToList();

        public Result updataAvatar(string username, string avatar)
        {
            Console.WriteLine(username + "," + avatar);
            try
            {
               _users.UpdateOne<User>(user => user.username == username, new BsonDocument("$set", new BsonDocument("avatar", avatar)));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return Result.error("更新头像失败");
            }
            Console.WriteLine("更新成功");
            return Result.success("更新成功");
        }

        public Result getAvatar(string username)
        {
            try
            {
                User user = _users.Find<User>(u => u.username == username).FirstOrDefault();
                String avatar = user.avatar;
                return Result.success(avatar);
            }catch(Exception e)
            {
                return Result.error("获取失败");
            }
        }
    }
}
