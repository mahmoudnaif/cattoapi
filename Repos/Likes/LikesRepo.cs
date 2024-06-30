using AutoMapper;
using cattoapi.ClientModles;
using cattoapi.CustomResponse;
using cattoapi.DTOS;
using cattoapi.Interfaces.Likes;
using cattoapi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;

namespace cattoapi.Repos.Likes
{
    public class LikesRepo : ILikesRepo
    {
        private readonly CattoDbContext _context;
        private readonly IMapper _mapper;

        public LikesRepo(CattoDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }
        public CustomResponse<IEnumerable<ProfileDTO>> GetPostLikes(GetFromPostModel getFromPostModel)
        {
            int postId = getFromPostModel.postId;
            int take = getFromPostModel.limitGetModel.take;
            int skip = getFromPostModel.limitGetModel.skip;


            IEnumerable<Account> likeProfiles = _context.LikedPosts.Where(like => like.PostId == postId).Skip(skip).Take(take).Select(like => _context.Accounts.SingleOrDefault(acc => acc.AccountId == like.AccountId) );

            if (likeProfiles.Count() == 0)
                return new CustomResponse<IEnumerable<ProfileDTO>>(404, "NOT FOUND");
           
            IEnumerable<ProfileDTO> likeProfilesDTO = _mapper.Map<IEnumerable<ProfileDTO>>(likeProfiles);

            return new CustomResponse<IEnumerable<ProfileDTO>>(200, "Likes", likeProfilesDTO);

            
        }

        public CustomResponse<IEnumerable<PostDTO>> GetUserLikes(int accountId, TakeSkipModel takeSkipModel)
        {
            int take = takeSkipModel.take; 
            int skip = takeSkipModel.skip;

            IEnumerable<Post> likedPosts= _context.LikedPosts.Where(like => like.AccountId== accountId).Skip(skip).Take(take).Select(like => _context.Posts.SingleOrDefault(post => post.PostId == like.PostId));

            if (likedPosts.Count() == 0)
                return new CustomResponse<IEnumerable<PostDTO>>(404, "NOT FOUND");

            IEnumerable<PostDTO> likedPostsDTO = _mapper.Map<IEnumerable<PostDTO>>(likedPosts);

            return new CustomResponse<IEnumerable<PostDTO>>(200, "Likes", likedPostsDTO);


        }
        public CustomResponse<bool> AddLike(int accountId,int postId)
        {
            Post post= _context.Posts.SingleOrDefault(post => post.PostId == postId);

            if (post == null)
                return new CustomResponse<bool>(404, "post not found");

            bool exists = _context.LikedPosts.Any(like=>like.AccountId==accountId && like.PostId == postId);
            if (exists)
                return new CustomResponse<bool>(409, "Already liked");

            LikedPost likedPost = new LikedPost();
            likedPost.AccountId = accountId;
            likedPost.PostId = postId;
            
            try
            {
                _context.LikedPosts.Add(likedPost);
                post.LikesCount++;
                _context.SaveChanges();
                return new CustomResponse<bool>(201, "liked");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Something went wrong please try again later");
            }
           

        }

        public CustomResponse<bool> RemoveLike(int accountId, int postId)
        {
            LikedPost likedPost = _context.LikedPosts.SingleOrDefault(like => like.PostId == postId && like.AccountId == accountId);
            if (likedPost == null)
                return new CustomResponse<bool>(404, "like doesn't exist");
            try
            {
                _context.LikedPosts.Remove(likedPost);
                Post post = _context.Posts.SingleOrDefault(post => post.PostId == postId);
                post.LikesCount--;
                _context.SaveChanges();
                return new CustomResponse<bool>(200, "like reomved");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Something went wrong please try again later");
            }
        }
    }
}
