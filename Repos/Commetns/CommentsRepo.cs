using AutoMapper;
using cattoapi.ClientModles;
using cattoapi.CustomResponse;
using cattoapi.DTOS;
using cattoapi.Interfaces.Comments;
using cattoapi.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Collections.Generic;

namespace cattoapi.Repos.Commetns
{
    public class CommentsRepo : ICommentsRepo
    {
        private readonly CattoDbContext _context;
        private readonly IMapper _mapper;

        public CommentsRepo(CattoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public CustomResponse<IEnumerable<CommentDTO>> GetPostComments(GetFromPostModel getComments)
        {
            int postId = getComments.postId;
            int take = getComments.limitGetModel.take;
            int skip = getComments.limitGetModel.skip;

            if (postId < 0)
                return new CustomResponse<IEnumerable<CommentDTO>>(400, "ID must be positive");

            if (take == 0)
                return new CustomResponse<IEnumerable<CommentDTO>>(404, "Not found");

            if (take < 0 || skip < 0)
                return new CustomResponse<IEnumerable<CommentDTO>>(400, "Take and skip must be positive values");

            List<Comment> comments = _context.Comments.Where(com => com.PostId == postId).Take(take).Skip(skip).ToList();

            if (comments.Count() == 0)
                return new CustomResponse<IEnumerable<CommentDTO>>(404, "Not found");

            List<CommentDTO> commentsDTO = _mapper.Map<List<CommentDTO>>(comments);


            for (int i = 0; i < commentsDTO.Count; i++)
            {
                commentsDTO[i].Profile = _mapper.Map<ProfileDTO>(_context.Accounts.SingleOrDefault(acc => acc.AccountId == comments[i].AccountId));
            }



            return new CustomResponse<IEnumerable<CommentDTO>>(200, "Comments retreived", commentsDTO);



        }
        public CustomResponse<IEnumerable<CommentDTO>> GetUserComments(int accountId, TakeSkipModel takeSkipModel)
        {

            int take = takeSkipModel.take;
            int skip = takeSkipModel.skip;

            if (accountId < 0)
                return new CustomResponse<IEnumerable<CommentDTO>>(400, "ID must be positive");

            if (take == 0)
                return new CustomResponse<IEnumerable<CommentDTO>>(404, "Not found");

            if (take < 0 || skip < 0)
                return new CustomResponse<IEnumerable<CommentDTO>>(400, "Take and skip must be positive values");

            IEnumerable<Comment> comments = _context.Comments.Where(com => com.AccountId == accountId).Take(take).Skip(skip);

            if (comments.Count() == 0)
                return new CustomResponse<IEnumerable<CommentDTO>>(404, "Not found");

            IEnumerable<CommentDTO> commentsDTO = _mapper.Map<IEnumerable<CommentDTO>>(comments);
            return new CustomResponse<IEnumerable<CommentDTO>>(200, "Comments retreived", commentsDTO);
        }
        public CustomResponse<CommentDTO> PostComment(int accountId, PostCommentModel postCommentModel)
        {
            int postId = postCommentModel.postId;
            string commentText = postCommentModel.commentText.Trim();

            if (commentText.Length == 0)
                return new CustomResponse<CommentDTO>(400, "Comment text can not be empty");


            if (_context.Posts.SingleOrDefault(post => post.PostId == postId) == null)
                return new CustomResponse<CommentDTO>(404, "Post does not exist");


            Comment comment = new Comment();
            comment.AccountId = accountId;
            comment.PostId = postCommentModel.postId;
            comment.CommentText = postCommentModel.commentText;

            try
            {
                _context.Comments.Add(comment);
                _context.SaveChanges();
                return new CustomResponse<CommentDTO>(200, "Comment posted sucessfully", _mapper.Map<CommentDTO>(comment));
            }
            catch
            {
                return new CustomResponse<CommentDTO>(500, "Something went wrong. try again later");
            }

        }

        public CustomResponse<CommentDTO> EditComment(int accountId, EditCommentModel editCommentModel)
        {
            int commentId = editCommentModel.commentId;
            string editedText = editCommentModel.commentText.Trim();

            if (editedText.Length == 0)
                return new CustomResponse<CommentDTO>(400, "edited comment text can not be empty");


            Comment comment = _context.Comments.SingleOrDefault(com => com.CommentId == commentId);

            if (comment == null)
                return new CustomResponse<CommentDTO>(404, "Comment not found");

            if (comment.AccountId != accountId)
                return new CustomResponse<CommentDTO>(401, "Unauthorized action. don't play with what is not yours");

            comment.CommentText = editedText;

            try
            {
                _context.SaveChanges();
                return new CustomResponse<CommentDTO>(200, "Comment edited sucessfully", _mapper.Map<CommentDTO>(comment));
            }
            catch
            {
                return new CustomResponse<CommentDTO>(500, "Something went wrong. try again later");
            }


        }

        public CustomResponse<bool> DeleteComment(int accountId, int commentId, string role)
        {
            Comment comment = _context.Comments.SingleOrDefault(com => com.CommentId == commentId);

            if (comment == null)
                return new CustomResponse<bool>(404, "Comment not found");


            if (role != "admin")
            {
                if (comment.AccountId != accountId)
                    return new CustomResponse<bool>(401, "Unauthorized action. don't play with what is not yours");
            }

            try
            {
                _context.Comments.Remove(comment);
                _context.SaveChanges();
                return new CustomResponse<bool>(200, "Comment deleted sucessfully");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Something went wrong. try again later");
            }

        }


    }
}
