using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace Bussiness_social_media.MVVM.Model.Repository
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
        private List<Post> posts;
        private string xmlFilePath;

        private static Random random = new Random();

        public PostRepository(string xmlFilePath)
        {
            this.xmlFilePath = xmlFilePath;
            posts = new List<Post>();
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
                posts = new List<Post>();
                if (File.Exists(xmlFilePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Post>), new XmlRootAttribute("ArrayOfPost"));

                    using (FileStream fileStream = new FileStream(xmlFilePath, FileMode.Open))
                    using (XmlReader reader = XmlReader.Create(fileStream))
                    {
                        posts = (List<Post>)serializer.Deserialize(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading posts from XML: {ex.Message}");
            }
        }

        private void SavePostsToXml()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Post>), new XmlRootAttribute("ArrayOfPost"));

                using (FileStream fileStream = new FileStream(xmlFilePath, FileMode.Create))
                {
                    serializer.Serialize(fileStream, posts);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving posts to XML: {ex.Message}");
            }
        }

        public List<Post> GetAllPosts()
        {
            return posts;
        }

        public Post GetPostById(int id)
        {
            return posts.FirstOrDefault(p => p.Id == id);
        }

        public int AddPost(DateTime creationDate, string imagePath, string caption)
        {
            int newID = GetNextId();
            Post post = new Post(newID, creationDate, imagePath, caption);
            posts.Add(post);
            SavePostsToXml();
            return newID;
        }

        public void UpdatePost(Post post)
        {
            var existingPost = posts.FirstOrDefault(p => p.Id == post.Id);
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
            var postToRemove = posts.FirstOrDefault(p => p.Id == id);
            if (postToRemove != null)
            {
                posts.Remove(postToRemove);
                SavePostsToXml();
            }
        }

        private int GetNextId()
        {
            return posts.Count > 0 ? posts.Max(p => p.Id) + 1 : 1;
        }

        public void ForcePostSavingToXml()
        {
            SavePostsToXml();
        }
    }
}
