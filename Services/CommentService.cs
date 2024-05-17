using bussiness_social_media.MVVM.Model.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bussiness_social_media.Services
{
    public interface ICommentService
    {
        List<Comment> GetAllComments();
        Comment GetCommentById(int id);
        int AddComment(string username, string content, DateTime dateOfCreation);
        void UpdateComment(int id, string username, string content, DateTime dateOfCreation);
        void DeleteComment(int id);
    }
    public class CommentService : ICommentService
    {
        private ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public int AddComment(string username, string content, DateTime dateOfCreation)
        {
            return _commentRepository.AddComment(username, content, dateOfCreation);
        }

        public void DeleteComment(int id)
        {
            _commentRepository.DeleteComment(id);
        }

        public List<Comment> GetAllComments()
        {
            return _commentRepository.GetAllComments();
        }

        public Comment GetCommentById(int id)
        {
            return _commentRepository.GetCommentById(id);
        }

        public void UpdateComment(int id, string username, string content, DateTime dateOfCreation)
        {
            _commentRepository.UpdateComment(id, username, content, dateOfCreation, DateTime.Now);
        }
    }
}
