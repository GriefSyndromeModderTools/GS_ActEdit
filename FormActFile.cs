using GS_ActEdit.Format;
using GS_ActEdit.Format.Serialization;
using GS_ActEdit.UI;
using GS_ActEdit.UI.TreeNodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_ActEdit
{
    partial class FormActFile : Form
    {
        public ActObject file;
        public string filename;
        public FileNode root;
        private Act2DMapLayoutObject current_layout;

        public FormActFile()
        {
            InitializeComponent();
        }
        public void SetFileNameToCaption(string value)
        {
            this.filename = value;
            this.Text = "Act File Editor - " + Path.GetFileName(value);
        }
        public void SetNodeSelected(TreeNode tn)
        {
            this.TreeViewMain.SelectedNode = tn;
            tn.EnsureVisible();
        }
        private void UpdateObjectList(Act2DMapLayoutObject layout)
        {
            this.current_layout = layout;
            this.ListViewLayout.Items.Clear();
            if (layout == null)
            {
                return;
            }
            foreach (Act2DMapLayoutObject.Element current in layout.elements)
            {
                this.ListViewLayout.Items.Add(new ListViewItemElement(current, this));
            }
        }
        public void AddElementToList()
        {
            if (this.current_layout != null)
            {
                Act2DMapLayoutObject.Element element = new Act2DMapLayoutObject.Element();
                this.current_layout.elements.Add(element);
                this.ListViewLayout.Items.Add(new ListViewItemElement(element, this));
            }
        }
        private void UpdateTreeViewMain()
        {
            this.TreeViewMain.Nodes.Clear();
            this.root = new FileNode(this.file, this);
            this.TreeViewMain.Nodes.Add(this.root);
            this.TreeViewMain.SelectedNode = this.root;
        }
        private void UpdateTreeViewSelect(TreeNode tn)
        {
            if (tn != null && tn is AbstractNode)
            {
                AbstractNode abstractNode = (AbstractNode)tn;
                this.PropertyGridMain.SelectedObject = abstractNode.GetPropertyObject();
                this.PropertyGridMain.Tag = abstractNode;
            }
            else
            {
                this.PropertyGridMain.SelectedObject = null;
                this.PropertyGridMain.Tag = null;
            }
            if (tn != null && tn is MapLayoutNode)
            {
                this.UpdateObjectList(((MapLayoutNode)tn).GetLayout());
                return;
            }
            this.UpdateObjectList(null);
        }
        private void TreeViewMain_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.UpdateTreeViewSelect(e.Node);
        }
        private void PropertyGridMain_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (this.PropertyGridMain.Tag != null && this.PropertyGridMain.Tag is AbstractNode)
            {
                ((AbstractNode)this.PropertyGridMain.Tag).UpdateObject();
            }
        }
        private void TreeViewMain_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            this.TreeViewMain.SelectedNode = e.Node;
        }
        private T GetCurrentNodeAs<T>() where T : TreeNode
        {
            if (this.TreeViewMain.SelectedNode != null && this.TreeViewMain.SelectedNode is T)
            {
                return (T)((object)this.TreeViewMain.SelectedNode);
            }
            return default(T);
        }
        private void Menu_File_Save_Click(object sender, EventArgs e)
        {
            if (!this.file.SaveActToFile(this.filename))
            {
                MessageBox.Show("Can not save to act file: " + this.SaveDialogFile.FileName);
            }
        }
        private void Menu_File_SaveAs_Click(object sender, EventArgs e)
        {
            if (this.SaveDialogFile.ShowDialog() == DialogResult.OK)
            {
                if (!this.file.SaveActToFile(this.SaveDialogFile.FileName))
                {
                    MessageBox.Show("Can not save to act file: " + this.SaveDialogFile.FileName);
                    return;
                }
                this.SetFileNameToCaption(this.SaveDialogFile.FileName);
            }
        }
        private void Menu_File_Refresh_Click(object sender, EventArgs e)
        {
            if (this.GetCurrentNodeAs<FileNode>() != null)
            {
                this.GetCurrentNodeAs<FileNode>().UpdateObject();
                this.UpdateResList();
            }
        }
        private void Menu_File_Layer_NewLayer_Click(object sender, EventArgs e)
        {
            if (this.root != null)
            {
                this.root.AddLayer();
            }
        }
        private void Menu_File_Resources_LinkToMcdFile_Click(object sender, EventArgs e)
        {
            if (this.FolderBrowserRoot.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            string selectedPath = this.FolderBrowserRoot.SelectedPath;
            this.file.LinkRootFolder(selectedPath);
            this.UpdateTreeViewMain();
            this.UpdateResList();
        }
        private void Menu_Code_EditCode_Click(object sender, EventArgs e)
        {
            if (this.GetCurrentNodeAs<CodeNode>() != null)
            {
                this.GetCurrentNodeAs<CodeNode>().EditCode();
            }
        }
        private void FormActFile_Shown(object sender, EventArgs e)
        {
            string text;
            if (Environment.GetCommandLineArgs().Length == 2)
            {
                text = Environment.GetCommandLineArgs()[1];
            }
            else
            {
                if (this.OpenDialogFile.ShowDialog() != DialogResult.OK)
                {
                    base.Close();
                    return;
                }
                text = this.OpenDialogFile.FileName;
            }
            ActObject actObject = ActFile.ReadActFromFile(text);
            if (actObject == null)
            {
                MessageBox.Show("Can not open act file: " + text);
                base.Close();
                return;
            }
            this.SetFileNameToCaption(text);
            this.file = actObject;
            this.UpdateTreeViewMain();
        }
        private void ListViewLayout_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ListViewLayout.SelectedItems.Count == 0)
            {
                return;
            }
            ListViewItemElement listViewItemElement = this.ListViewLayout.SelectedItems.OfType<ListViewItemElement>().First<ListViewItemElement>();
            this.PropertyGridMain.SelectedObject = listViewItemElement.GetPropertyObject();
            this.PropertyGridMain.Tag = null;
            this.PictureResourcePreview.Image = this.file.CreateBitmapForResource(listViewItemElement.GetElement().resourceID);
        }
        private void UpdateResList()
        {
            this.ListViewResources.Items.Clear();
            if (this.file.mcd == null)
            {
                return;
            }
            foreach (ChipElement current in this.file.mcd.chips)
            {
                this.ListViewResources.Items.Add(new ListViewItemResource(current.chipID, this));
            }
        }
        private void ListViewResources_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && this.ListViewResources.FocusedItem.Bounds.Contains(e.Location))
            {
                this.ContextMenuResource.Show(Cursor.Position);
            }
        }
        private void Menu_Resource_AddToLayout_Click(object sender, EventArgs e)
        {
            ListViewItemResource listViewItemResource = (ListViewItemResource)this.ListViewResources.FocusedItem;
            if (listViewItemResource == null)
            {
                return;
            }
            Act2DMapLayoutObject.Element element = Act2DMapLayoutObject.Element.CreateDefault(listViewItemResource.id);
            this.current_layout.elements.Add(element);
            this.ListViewLayout.Items.Add(new ListViewItemElement(element, this));
        }
        private void ListViewResources_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ListViewResources.SelectedItems.Count == 0)
            {
                return;
            }
            ListViewItemResource listViewItemResource = this.ListViewResources.SelectedItems.OfType<ListViewItemResource>().First<ListViewItemResource>();
            this.PictureResourcePreview.Image = this.file.CreateBitmapForResource(listViewItemResource.id);
        }
        private void PictureResourcePreview_DoubleClick(object sender, EventArgs e)
        {
            FormShowImage formShowImage = new FormShowImage(this.PictureResourcePreview.Image);
            formShowImage.Show(null);
        }
        private void Menu_Layer_ShowPreview_Click(object sender, EventArgs e)
        {
            LayerNode currentNodeAs = this.GetCurrentNodeAs<LayerNode>();
            if (currentNodeAs != null)
            {
                FormPreview formPreview = new FormPreview(currentNodeAs.layer.GetLayout(), this.file);
                formPreview.Show();
            }
        }
        private object GetDataFromElement(Act2DMapLayoutObject.Element e)
        {
            object result;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
                {
                    BinaryOutputStream s = new BinaryOutputStream(binaryWriter);
                    e.Write(s);
                    result = memoryStream.ToArray();
                }
            }
            return result;
        }
        private Act2DMapLayoutObject.Element GetElementFromData(object obj)
        {
            if (obj is byte[])
            {
                byte[] buffer = (byte[])obj;
                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (BinaryReader binaryReader = new BinaryReader(memoryStream))
                    {
                        BinaryInputStream s = new BinaryInputStream(binaryReader);
                        Act2DMapLayoutObject.Element element = new Act2DMapLayoutObject.Element();
                        element.Read(s);
                        return element;
                    }
                }
            }
            return null;
        }
        private void Menu_Element_Cut_Click(object sender, EventArgs e)
        {
            if (this.ListViewLayout.SelectedItems.Count == 1)
            {
                ListViewItemElement listViewItemElement = this.ListViewLayout.SelectedItems.OfType<ListViewItemElement>().First<ListViewItemElement>();
                Act2DMapLayoutObject.Element element = listViewItemElement.GetElement();
                Clipboard.SetData("ActMapElement", this.GetDataFromElement(element));
                this.current_layout.elements.RemoveAt(listViewItemElement.Index);
                listViewItemElement.Remove();
            }
        }
        private void ContextMenuElementList_Opening(object sender, CancelEventArgs e)
        {
            this.cutToolStripMenuItem.Enabled = (this.ListViewLayout.SelectedItems.Count == 1);
            this.copyToolStripMenuItem.Enabled = (this.ListViewLayout.SelectedItems.Count == 1);
            this.pasteBeforeToolStripMenuItem.Enabled = (Clipboard.ContainsData("ActMapElement") && this.ListViewLayout.SelectedItems.Count == 1);
            this.pasteAfterToolStripMenuItem.Enabled = (Clipboard.ContainsData("ActMapElement") && this.ListViewLayout.SelectedItems.Count == 1);
            this.deleteToolStripMenuItem.Enabled = (this.ListViewLayout.SelectedItems.Count == 1);
            this.clearAllToolStripMenuItem.Enabled = (this.ListViewLayout.Items.Count > 0);
        }
        private void ListViewLayout_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.ContextMenuElementList.Show(this.ListViewLayout, e.Location);
            }
        }
        private void Menu_Element_Copy_Click(object sender, EventArgs e)
        {
            if (this.ListViewLayout.SelectedItems.Count == 1)
            {
                ListViewItemElement listViewItemElement = this.ListViewLayout.SelectedItems.OfType<ListViewItemElement>().First<ListViewItemElement>();
                Act2DMapLayoutObject.Element element = listViewItemElement.GetElement();
                Clipboard.SetData("ActMapElement", this.GetDataFromElement(element));
            }
        }
        private void Menu_Element_PasteBefore_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsData("ActMapElement") && this.ListViewLayout.SelectedItems.Count == 1)
            {
                Act2DMapLayoutObject.Element elementFromData = this.GetElementFromData(Clipboard.GetData("ActMapElement"));
                if (elementFromData != null)
                {
                    int index = this.ListViewLayout.SelectedItems.OfType<ListViewItemElement>().First<ListViewItemElement>().Index;
                    this.current_layout.elements.Insert(index, elementFromData);
                    this.ListViewLayout.Items.Insert(index, new ListViewItemElement(elementFromData, this));
                }
            }
        }
        private void Menu_Element_PasteAfter_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsData("ActMapElement") && this.ListViewLayout.SelectedItems.Count == 1)
            {
                Act2DMapLayoutObject.Element elementFromData = this.GetElementFromData(Clipboard.GetData("ActMapElement"));
                if (elementFromData != null)
                {
                    int index = this.ListViewLayout.SelectedItems.OfType<ListViewItemElement>().First<ListViewItemElement>().Index + 1;
                    this.current_layout.elements.Insert(index, elementFromData);
                    this.ListViewLayout.Items.Insert(index, new ListViewItemElement(elementFromData, this));
                }
            }
        }
        private void Menu_Element_Delete_Click(object sender, EventArgs e)
        {
            if (this.ListViewLayout.SelectedItems.Count == 1)
            {
                ListViewItemElement listViewItemElement = this.ListViewLayout.SelectedItems.OfType<ListViewItemElement>().First<ListViewItemElement>();
                this.current_layout.elements.RemoveAt(listViewItemElement.Index);
                listViewItemElement.Remove();
            }
        }
        private void Menu_Layer_Delete_Click(object sender, EventArgs e)
        {
            LayerNode currentNodeAs = this.GetCurrentNodeAs<LayerNode>();
            if (currentNodeAs != null)
            {
                this.file.layers.Remove(currentNodeAs.layer);
                currentNodeAs.Remove();
            }
        }
        private void Menu_File_PreviewAllLayers_Click(object sender, EventArgs e)
        {
            new FormMultiPreview(this.file).Show();
        }
        private void Menu_Element_ClearAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Clear all elements?", "Clear all", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.current_layout.elements.Clear();
                this.ListViewLayout.Items.Clear();
            }
        }
        private void Menu_Layer_CalculateAABB_Click(object sender, EventArgs e)
        {
            LayerNode currentNodeAs = this.GetCurrentNodeAs<LayerNode>();
            if (currentNodeAs != null)
            {
                currentNodeAs.layer.CalculateAABB(this.file);
            }
        }
    }
}
