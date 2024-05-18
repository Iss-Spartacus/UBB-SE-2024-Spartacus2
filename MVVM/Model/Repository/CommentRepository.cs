using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace Bussiness_social_media.MVVM.Model.Repository
{
    public interface ICommentRepository
    {
        List<Comment> GetAllComments();
        Comment GetCommentById(int id);
        int AddComment(string username, string content, DateTime dateOfCreation);
        void UpdateComment(int id, string username, string content, DateTime dateOfCreation, DateTime dateOfUpdate);
        void DeleteComment(int id);
    }

    internal class CommentRepository : ICommentRepository
    {
        private List<Comment> comments;
        private string xmlFilePath;

        public CommentRepository(string xmlFilePath)
        {
            this.xmlFilePath = xmlFilePath;
            comments = new List<Comment>();
            LoadCommentsFromXml();
        }

        ~CommentRepository()
        {
            SaveCommentsToXml();
        }

        private void LoadCommentsFromXml()
        {
            try
            {
                comments = new List<Comment>();
                if (File.Exists(xmlFilePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Comment>), new XmlRootAttribute("ArrayOfComments"));

                    using (FileStream fileStream = new FileStream(xmlFilePath, FileMode.Open))
                    using (XmlReader reader = XmlReader.Create(fileStream))
                    {
                        comments = (List<Comment>)serializer.Deserialize(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading comments from XML: {ex.Message}");
            }
        }

        private void SaveCommentsToXml()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Comment>), new XmlRootAttribute("ArrayOfComments"));

                using (FileStream fileStream = new FileStream(xmlFilePath, FileMode.Create))
                {
                    serializer.Serialize(fileStream, comments);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving comments to XML: {ex.Message}");
            }
        }

        private int GetNextId()
        {
            return comments.Count > 0 ? comments.Max(comment => comment.Id) + 1 : 1;
        }

        public int AddComment(string username, string content, DateTime dateOfCreation)
        {
            int newId = GetNextId();
            Comment comment = new Comment(newId, username, content, dateOfCreation);
            comments.Add(comment);
            SaveCommentsToXml();
            return newId;
        }

        public void DeleteComment(int id)
        {
            var commentToRemove = comments.FirstOrDefault(comment => comment.Id == id);
            if (commentToRemove != null)
            {
                comments.Remove(commentToRemove);
                SaveCommentsToXml();
            }
        }

        public List<Comment> GetAllComments()
        {
            return comments;
        }

        public Comment GetCommentById(int id)
        {
            return comments.FirstOrDefault(comment => comment.Id == id);
        }

        public void UpdateComment(int id, string username, string content, DateTime dateOfCreation, DateTime dateOfUpdate)
        {
            Comment existingComment = GetCommentById(id);
            if (existingComment != null)
            {
                existingComment.Username = username;
                existingComment.Content = content;
                SaveCommentsToXml();
            }
        }
    }
}
