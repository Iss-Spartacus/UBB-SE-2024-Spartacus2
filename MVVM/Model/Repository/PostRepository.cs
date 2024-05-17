using System.IO;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace bussiness_social_media.MVVM.Model.Repository
{
    public interface IPostRepository
    {
        List<Post> GetAllPosts();
        Post GetPostById(int id);
        int AddPost(DateTime creationDate, string imagePath, string caption);
        void UpdatePost(Post post);
        void DeletePost(int id);
        void ForcePostSavingToXml();
    }

    public class PostRepository : IPostRepository
    {
        private List<Post> _posts;
        private string _xmlFilePath;

        private static Random _random = new Random();


        public PostRepository(string xmlFilePath)
        {
            _xmlFilePath = xmlFilePath;
            _posts = new List<Post>();
            LoadPostsFromXml();
        }

        ~PostRepository()
        {
            SavePostsToXml();
        }

        private void LoadPostsFromXml()
        {
            try
            {
                if (File.Exists(_xmlFilePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Post), new XmlRootAttribute("Post"));

                    _posts = new List<Post>();

                    using (FileStream fileStream = new FileStream(_xmlFilePath, FileMode.Open))
                    {
                        using (XmlReader reader = XmlReader.Create(fileStream))
                        {
                            while (reader.ReadToFollowing("Post"))
                            {
                                Post post = (Post)serializer.Deserialize(reader);
                                _posts.Add(post);
                            }
                        }
                    }
                }
                else
                {
                    _posts = new List<Post>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something terrible, terrible has happened during the execution of the program. Show this to your local IT guy. PostRepository.LoadPostsFromXml():" + ex.Message);
            }
        }

        private void SavePostsToXml()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Post>), new XmlRootAttribute("ArrayOfPost"));

            using (FileStream fileStream = new FileStream(_xmlFilePath, FileMode.Create))
            {
                serializer.Serialize(fileStream, _posts);
            }
        }

        public List<Post> GetAllPosts()
        {
            return _posts;
        }

        public Post GetPostById(int id)
        {
            return _posts.FirstOrDefault(p => p.Id == id);
        }

        public int AddPost(DateTime creationDate, string imagePath, string caption)
        {
            int newID = _getNextId();
            Post post = new Post(newID, creationDate, imagePath, caption);
            _posts.Add(post);
            SavePostsToXml();
            return newID;
        }

        public void UpdatePost(Post post)
        {
            var existingPost = _posts.FirstOrDefault(p => p.Id == post.Id);
            if (existingPost != null)
            {
                existingPost.SetNumberOfLikes(post.NumberOfLikes);
                existingPost.SetCreationDate(post.CreationDate);
                existingPost.SetImagePath(post.ImagePath);
                existingPost.SetCaption(post.Caption);
                existingPost.SetComments(post.CommentIds);
                SavePostsToXml();
            }
        }

        public void DeletePost(int id)
        {
            var postToRemove = _posts.FirstOrDefault(p => p.Id == id);
            if (postToRemove != null)
            {
                _posts.Remove(postToRemove);
                SavePostsToXml();
            }
        }

        private int _getNextId()
        {
            return _posts.Count > 0 ? _posts.Max(p => p.Id) + 1 : 1;
        }

        public void ForcePostSavingToXml()
        {
            SavePostsToXml();
        }
    }
}