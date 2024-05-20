using Bussiness_social_media.MVVM.Model.Repository;
using Bussiness_social_media.Services;

namespace Bussiness_social_media.Services
{
    public interface IPostService
    {
        List<Post> GetAllPosts();
        Post GetPostById(int id);
        int AddPost(DateTime creationDate, string imagePath, string caption);
        void UpdatePost(int id, DateTime newCreationDate, string newImagePath, string newCaption);
        void DeletePost(int id);
        void LinkCommentIdToPost(int postId, int commentId);
    }
    public class PostService : IPostService
    {
        private IPostRepository postRepository;

        public PostService(IPostRepository postRepository)
        {
            this.postRepository = postRepository;
        }

        public List<Post> GetAllPosts()
        {
            return postRepository.GetAllPosts();
        }

        public Post GetPostById(int id)
        {
            Post post = postRepository.GetPostById(id);
            return postRepository.GetPostById(id);
        }

        public int AddPost(DateTime creationDate, string imagePath, string caption)
        {
            return postRepository.AddPost(creationDate, imagePath, caption);
        }

        public void UpdatePost(int id, DateTime newCreationDate, string newImagePath, string newCaption)
        {
            Post postToUpdate = GetPostById(id);
            postRepository.UpdatePost(postToUpdate);
        }

        public void DeletePost(int id)
        {
            postRepository.DeletePost(id);
        }

        public void LinkCommentIdToPost(int postId, int commentId)
        {
            Post postToCommentOn = GetPostById(postId);
            postToCommentOn.AddComment(commentId);
            postRepository.ForcePostSavingToXml();
        }
    }
}
