using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using Presentation.Process.Common;
using Utilities.Common;
using System.Windows.Xps.Packaging;
using System.IO.Packaging;
using Telerik.Windows.Controls;

namespace PresentationWPF.CustomControl
{
    /// <summary>
    /// Interaction logic for WindowHelp.xaml
    /// </summary>
    public partial class WindowHelp : Window
    {
        private string filePath = "";
        private string title = "";
        //private Aspose.Words.Document aspDoc = null;
        public WindowHelp()
        {
            InitializeComponent();
            CommonFunction.setIcon(this);
        }

        public WindowHelp(string documentPath, string documentTitle)
            : this()
        {
            filePath = documentPath;
            title = LLanguage.SearchResourceByKey(documentTitle);
            LoadDocument();
            BuildTreeRoot();
            trvMenu.SelectionChanged += new SelectionChangedEventHandler(trvMenu_SelectionChanged);
        }

        void trvMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filePath = ((RadTreeViewItem)(((RadTreeView)sender).SelectedItem)).Uid;
            title = ((RadTreeViewItem)(((RadTreeView)sender).SelectedItem)).Header.ToString();
            LoadDocument();
        }

        public void LoadDocument()
        {
            this.Title = "mFinance - " + title;
            using (Stream stream = LFile.GetStreamDataFromFile(GetResourceUri(filePath)))
            {
                if (stream.IsNullOrEmpty())
                {
                    LMessage.ShowMessage("M.DungChung.ThongBao.TaiLieuDangDuocPhatTrien", LMessage.MessageBoxType.Information);
                    this.docViewer.Document = null;
                    return;
                }
                if (stream.Length == 0)
                {
                    LMessage.ShowMessage("M.DungChung.ThongBao.TaiLieuDangDuocPhatTrien", LMessage.MessageBoxType.Information);
                    this.docViewer.Document = null;
                    return;
                }
                else
                {
                    UpdatePdfViewer(stream);
                }
            }
            
        }

        public void LoadDocumentFirst()
        {
            this.Title = "mFinance - " + title;
            using (Stream stream = LFile.GetStreamDataFromFile(GetResourceUri(filePath)))
            {
                if (stream.IsNullOrEmpty())
                {
                    LMessage.ShowMessage("M.DungChung.ThongBao.TaiLieuDangDuocPhatTrien", LMessage.MessageBoxType.Information);
                    this.docViewer.Document = null;
                    return;
                }
                if (stream.Length == 0)
                {
                    LMessage.ShowMessage("M.DungChung.ThongBao.TaiLieuDangDuocPhatTrien", LMessage.MessageBoxType.Information);
                    this.docViewer.Document = null;
                    return;
                }
                else
                {
                    UpdatePdfViewer(stream);
                }
            }
        }

        private void UpdatePdfViewer(Stream stream)
        {
            //Aspose.Words.Rendering.XpsOptions xpsOptions = new Aspose.Words.Rendering.XpsOptions();
            //aspDoc.SaveToXps(0, aspDoc.PageCount, ClientInformation.HelpDir + @"\" + filePath + ".xps", xpsOptions);
            //aspDoc.SaveToXps(0, aspDoc.PageCount, outputStream, xpsOptions);
            //aspDoc.Save(ClientInformation.HelpDir + @"\" + filePath + ".xps",Aspose.Words.SaveFormat.Xps);
            //aspDoc.Save(outputStream,Aspose.Words.SaveFormat.Xaml);
            Package package = Package.Open(stream);

            //Create URI for Xps Package
            //Any Uri will actually be fine here. It acts as a place holder for the
            //Uri of the package inside of the PackageStore
            string inMemoryPackageName = string.Format("memorystream://{0}.xps", Guid.NewGuid());
            Uri packageUri = new Uri(inMemoryPackageName);

            //Add package to PackageStore
            PackageStore.AddPackage(packageUri, package);

            XpsDocument xpsDoc = new XpsDocument(package, CompressionOption.Maximum, inMemoryPackageName);
            FixedDocumentSequence fixedDocumentSequence = xpsDoc.GetFixedDocumentSequence();
            this.docViewer.Document = fixedDocumentSequence;
        }

        private static string GetResourceUri(string resource)
        {
            return ClientInformation.HelpDir + "\\" + resource + ".xps";
        }

        private void tbCurrentPage_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                if (e.Key == System.Windows.Input.Key.Enter)
                {
                    textBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                }
            }
        }

        private void BuildTreeRoot()
        {
            RadTreeViewItem treeviewChild = null;
            foreach (Presentation.Process.ZAMainAppServiceRef.ChucNangDto chucnang in ClientInformation.ListChucNang.Where(f => f.IDChucNangCha.IsNullOrEmpty()))
            {
                treeviewChild = new RadTreeViewItem();
                treeviewChild.Tag = chucnang.IDChucNang;
                treeviewChild.Header = LLanguage.SearchResourceByKey(chucnang.TieuDe);
                treeviewChild.Uid = chucnang.MenuHelp;
                trvMenu.Items.Add(treeviewChild);
                BuildTree(treeviewChild);
            }
        }

        private void BuildTree(RadTreeViewItem treeview)
        {
            RadTreeViewItem treeviewChild = null;
            foreach (Presentation.Process.ZAMainAppServiceRef.ChucNangDto chucnang in ClientInformation.ListChucNang.Where(f => f.IDChucNangCha.Equals(treeview.Tag)))
            {
                treeviewChild = new RadTreeViewItem();
                treeviewChild.Tag = chucnang.IDChucNang;
                treeviewChild.Header = LLanguage.SearchResourceByKey(chucnang.TieuDe);
                treeviewChild.Uid = chucnang.MenuHelp;
                treeview.Items.Add(treeviewChild);
                BuildTree(treeviewChild);
            }
        }
    }
}
