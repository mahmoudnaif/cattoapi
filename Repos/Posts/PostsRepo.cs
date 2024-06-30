using AutoMapper;
using cattoapi.ClientModles;
using cattoapi.CustomResponse;
using cattoapi.DTOS;
using cattoapi.Interfaces.Posts;
using cattoapi.Models;
using cattoapi.utlities;
using Microsoft.Identity.Client;

namespace cattoapi.Repos.Posts
{
    public class PostsRepo : IPostsRepo
    {
        private readonly CattoDbContext _context;
        private readonly IMapper _mapper;

        public PostsRepo(CattoDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }  


       public CustomResponse<IEnumerable<PostDTO>> getUsersPosts(GetUsersPostsIdModel getUsersPostsModel)
        {
            //add limiter for the take (maximum 30) for preventing the client side from overlaoding the server
            string AccountIdOrUserame = getUsersPostsModel.AccountIdOrUserame;
            int take = getUsersPostsModel.limitGetModel.take;
            int skip = getUsersPostsModel.limitGetModel.skip;


            if (take == 0)
                return new CustomResponse<IEnumerable<PostDTO>>(404, "Not found");

            if (take < 0 || skip < 0)
                return new CustomResponse<IEnumerable<PostDTO>>(400, "Invalid numbers take and skip must be 0 or more");


            IEnumerable<Post> posts;
            if (!int.TryParse(AccountIdOrUserame, out int id))
            {
                Account account = _context.Accounts.SingleOrDefault(acc =>acc.UserName == AccountIdOrUserame);
                if(account== null)
                    return new CustomResponse<IEnumerable<PostDTO>>(404, "No posts found");


                posts = _context.Posts.Where(post => post.AccountId == account.AccountId).Skip(skip).Take(take);
            }
            else
            {
                posts = _context.Posts.Where(post => post.AccountId == id).Skip(skip).Take(take);
            }

           

            if (posts.Count() == 0)
                return new CustomResponse<IEnumerable<PostDTO>>(404, "No posts found");

            IEnumerable<PostDTO> postsDTO= _mapper.Map<IEnumerable<PostDTO>>(posts);

            return new CustomResponse<IEnumerable<PostDTO>>(200, "Retrieved successfully", postsDTO);

        }

        public async Task<CustomResponse<PostDTO>> PostaPost(int accountId, PostaPostModel postaPostModel)
        {
            if((postaPostModel.PostText == null || postaPostModel.PostText == "") && postaPostModel.PostPictrue == null)
                return new CustomResponse<PostDTO>(400, "Text and Picture can't be both empty");



            Post post = new Post();
            post.AccountId = accountId;
            post.PostText = postaPostModel.PostText;
            post.DateCreated = DateTime.Now;
            post.LikesCount = 0;
            post.CommentsCount = 0;
          
            try
            {
                if (postaPostModel.PostPictrue != null)
                {
                    bool isImage = Utlities.IsImage(postaPostModel.PostPictrue);
                    if (!isImage) {
                        return new CustomResponse<PostDTO>(400, "File sent was not an image or unsupported");
                    }
                }

                post.PostPictrue =  Utlities.ConvertToByteArray(postaPostModel.PostPictrue);
                _context.Posts.Add(post);
                _context.SaveChanges();
                return new CustomResponse<PostDTO>(201, "Post created sucessfully", _mapper.Map<PostDTO>(post));
            }
            catch
            {
                return new CustomResponse<PostDTO>(500, "Something went wrong");
            }

            
        }
        public async Task<CustomResponse<PostDTO>>EditPost(int accountId, EditPostModel editPostModel)
        {
            if ((editPostModel.data.PostText == null || editPostModel.data.PostText == "") && editPostModel.data.PostPictrue == null)
                return new CustomResponse<PostDTO>(400, "Text and Picture can't be both empty");


            Post post = _context.Posts.SingleOrDefault(p => p.PostId == editPostModel.postId);

            if (post == null)
                return new CustomResponse<PostDTO>(404, "Post not found");

            if(post.AccountId != accountId)
                return new CustomResponse<PostDTO>(401, "Unauthroized");

            post.PostText = editPostModel.data.PostText;
         
            try
            {
                if (editPostModel.data.PostPictrue != null)
                {
                    bool isImage = Utlities.IsImage(editPostModel.data.PostPictrue);
                    if (!isImage)
                    {
                        return new CustomResponse<PostDTO>(400, "File sent was not an image or unsupported");
                    }
                }

                post.PostPictrue =  Utlities.ConvertToByteArray(editPostModel.data.PostPictrue);
                _context.SaveChanges();
                return new CustomResponse<PostDTO>(200, "Post edited sucessfully", _mapper.Map<PostDTO>(post));
            }
            catch
            {
                return new CustomResponse<PostDTO>(500, "Something went wrong");
            }


        }
        public CustomResponse<bool> DeletePost(int accountId, int postId)
        {
            Post post = _context.Posts.SingleOrDefault(p => p.PostId == postId);

            if (post == null)
                return new CustomResponse<bool>(404, "Post not found");

            if (post.AccountId != accountId)
                return new CustomResponse<bool>(401, "Unauthroized");


            try
            {
               _context.Posts.Remove(post);
                _context.SaveChanges();
                return new CustomResponse<bool>(200, "Post deleted sucessfully");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Something went wrong");
            }
        }

        
    }
}
