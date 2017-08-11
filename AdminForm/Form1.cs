using AdminForm.Helpers;
using AdminForm.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdminForm
{
    public partial class Form1 : Form
    {
        MySiteContent context = new MySiteContent();

        public Form1()
        {
            InitializeComponent();
            LoadListBooks();
            LoadCategories();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            Book book = new Book()
            {
                Id = Guid.NewGuid(),
                Title = txtTitle.Text,
                Publisher = txtPublisher.Text,
                Author = txtAuthor.Text,
                UrlDownload = txtLinkDownload.Text,
                UrlImage = txtLinkImage.Text,
                UrlRead = txtReadOnline.Text,
                UrlThumbnail = txtLinkThumbnail.Text,
                Category = int.Parse(cboCategory.ValueMember)
            };
            context.Books.Add(book);
            context.SaveChanges();
            LoadListBooks();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void LoadListBooks()
        {
            DataTable dtbBooks = new DataTable();
            
            //dtbBooks.Columns.Add(new DataColumn() { Caption = "Title", ColumnName = "Title" });
            //dtbBooks.Columns.Add(new DataColumn() { Caption = "Author", ColumnName = "Author" });
            //dtbBooks.Columns.Add(new DataColumn() { Caption = "Publisher", ColumnName = "Publisher" });
            //dtbBooks.Columns.Add(new DataColumn() { Caption = "Description", ColumnName = "Description" });
            
            //dtbBooks = context.Books.ToList().ToDataTable();
            //gridEbooks.DataSource = context.Books.Select(o=> new {
            //    Title = o.Title,
            //    Author = o.Author,
            //    Publisher = o.Publisher,
            //    Description = o.Description }).ToList();
        }

        private void LoadCategories()
        {
            treeCategories.Nodes.Clear();
            var rootNode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            var listRoot = context.Categories.Where(c => c.ParentId == rootNode).ToList();
            foreach(var cat in listRoot)
            {
                TreeNode node = new TreeNode() { Text = cat.Name, Name=cat.Id.ToString() };
                treeCategories.Nodes.Add(node);
            }
            foreach (TreeNode node in treeCategories.Nodes)
            {
                Guid nodeId = Guid.Parse(node.Name.ToString());
                List<Category> categories = context.Categories.Where(c => c.ParentId == nodeId).ToList();
                if (categories.Count > 0)
                {
                    AddChildNodes(categories, node);
                }
            }
            
        }

        private void AddChildNodes(List<Category> nodes, TreeNode node)
        {
            foreach (Category cat in nodes)
            {
                TreeNode childNode = new TreeNode() { Text = cat.Name, Name = cat.Id.ToString() };
                node.Nodes.Add(childNode);
                var childNodes = context.Categories.Where(c => c.ParentId == cat.Id).ToList();
                if (childNodes.Count > 0)
                {
                    AddChildNodes(childNodes, childNode);
                }
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            GoogleDriverConsole gdc = new GoogleDriverConsole();
            gdc.UploadFile();
            txtLinkDownload.Text = gdc.FileId;
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            GoogleDriverConsole gdc = new GoogleDriverConsole();
            gdc.DownloadFile(txtLinkDownload.Text);
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            var parentKey = context.Categories.Where(c => c.Name == txtParentCategory.Text).FirstOrDefault().Id;
            Guid parentId = !string.IsNullOrEmpty(txtParentCategory.Text) ? Guid.Parse(treeCategories.Nodes.Find(parentKey.ToString(), true).FirstOrDefault().Name) : Guid.Parse("00000000-0000-0000-0000-000000000000");
            Category cat = new Category();
            cat.Id = Guid.NewGuid();
            cat.Name = txtCategoryName.Text;
            cat.ParentId = parentId;
            cat.Description = txtCategoryDesc.Text;
            context.Categories.Add(cat);
            context.SaveChanges();
            LoadCategories();
        }

        private void treeCategories_Click(object sender, EventArgs e)
        {
            //txtParentCategory.Text = treeCategories.SelectedNode.Text;
        }

        private void treeCategories_AfterSelect(object sender, TreeViewEventArgs e)
        {
            txtParentCategory.Text = treeCategories.SelectedNode.Text;
        }
    }
}
