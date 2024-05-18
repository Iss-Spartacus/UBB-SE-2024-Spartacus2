using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bussiness_social_media.MVVM.Model.Repository;
namespace Bussiness_social_media.Services
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
        private ICommentRepository commmentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            commmentRepository = commentRepository;
        }

        public int AddComment(string username, string content, DateTime dateOfCreation)
        {
            return commmentRepository.AddComment(username, content, dateOfCreation);
        }

        public void DeleteComment(int id)
        {
            commmentRepository.DeleteComment(id);
        }

        public List<Comment> GetAllComments()
        {
            return commmentRepository.GetAllComments();
        }

        public Comment GetCommentById(int id)
        {
            return commmentRepository.GetCommentById(id);
        }

        public void UpdateComment(int id, string username, string content, DateTime dateOfCreation)
        {
            commmentRepository.UpdateComment(id, username, content, dateOfCreation, DateTime.Now);
        }
    }
}
