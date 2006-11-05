/*
Autowikibrowser
Copyright (C) 2006 Martin Richards

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Text.RegularExpressions;
using WikiFunctions;

namespace WikiFunctions.Lists
{
    public partial class specialFilter : Form
    {
        ListBox2 lb;
        public specialFilter(ListBox2 listbox)
        {
            InitializeComponent();
            lb = listbox;
            UpdateText();
        }

        List<Article> list = new List<Article>();

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (chkRemoveDups.Checked)
            {
                foreach (Article a in lb)
                {
                    if (!list.Contains(a))
                        list.Add(a);
                }
            }
            else
            {
                foreach (Article a in lb)
                {
                    list.Add(a);
                }
            }

            bool does = (chkContains.Checked && txtContains.Text != "");
            bool doesnot = (chkNotContains.Checked && txtDoesNotContain.Text != "");

            if (lbRemove.Items.Count > 0)
                FilterList();

            if (does || doesnot)
                FilterMatches(does, doesnot);

            if (lb.Items[0] is Article)
                FilterNamespace();

            lb.Items.Clear();

            foreach (Article a in list)
                lb.Items.Add(a);

            this.Close();
        }

        private void FilterNamespace()
        {
            int i = 0;

            while (i < list.Count)
            {
                if (list[i].NameSpaceKey == 0)
                {
                    if (chkArticle.Checked)
                    {
                        i++;
                        continue;
                    }
                    else
                        list.RemoveAt(i);
                }
                else if (list[i].NameSpaceKey == 1)
                {
                    if (chkArticleTalk.Checked)
                    {
                        i++;
                        continue;
                    }
                    else
                        list.RemoveAt(i);
                }
                else if (list[i].NameSpaceKey == 2)
                {
                    if (chkUser.Checked)
                    {
                        i++;
                        continue;
                    }
                    else
                        list.RemoveAt(i);
                }
                else if (list[i].NameSpaceKey == 3)
                {
                    if (chkUserTalk.Checked)
                    {
                        i++;
                        continue;
                    }
                    else
                        list.RemoveAt(i);
                }
                else if (list[i].NameSpaceKey == 4)
                {
                    if (chkWikipedia.Checked)
                    {
                        i++;
                        continue;
                    }
                    else
                        list.RemoveAt(i);
                }
                else if (list[i].NameSpaceKey == 5)
                {
                    if (chkWikipediaTalk.Checked)
                    {
                        i++;
                        continue;
                    }
                    else
                        list.RemoveAt(i);
                }
                else if (list[i].NameSpaceKey == 6)
                {
                    if (chkImage.Checked)
                    {
                        i++;
                        continue;
                    }
                    else
                        list.RemoveAt(i);
                }
                else if (list[i].NameSpaceKey == 7)
                {
                    if (chkImageTalk.Checked)
                    {
                        i++;
                        continue;
                    }
                    else
                        list.RemoveAt(i);
                }
                else if (list[i].NameSpaceKey == 8)
                {
                    if (chkMediaWiki.Checked)
                    {
                        i++;
                        continue;
                    }
                    else
                        list.RemoveAt(i);
                }
                else if (list[i].NameSpaceKey == 9)
                {
                    if (chkMediaWikiTalk.Checked)
                    {
                        i++;
                        continue;
                    }
                    else
                        list.RemoveAt(i);
                }
                else if (list[i].NameSpaceKey == 10)
                {
                    if (chkTemplate.Checked)
                    {
                        i++;
                        continue;
                    }
                    else
                        list.RemoveAt(i);
                }
                else if (list[i].NameSpaceKey == 11)
                {
                    if (chkTemplateTalk.Checked)
                    {
                        i++;
                        continue;
                    }
                    else
                        list.RemoveAt(i);
                }
                else if (list[i].NameSpaceKey == 12)
                {
                    if (chkHelp.Checked)
                    {
                        i++;
                        continue;
                    }
                    else
                        list.RemoveAt(i);
                }
                else if (list[i].NameSpaceKey == 13)
                {
                    if (chkHelpTalk.Checked)
                    {
                        i++;
                        continue;
                    }
                    else
                        list.RemoveAt(i);
                }
                else if (list[i].NameSpaceKey == 14)
                {
                    if (chkCategory.Checked)
                    {
                        i++;
                        continue;
                    }
                    else
                        list.RemoveAt(i);
                }
                else if (list[i].NameSpaceKey == 15)
                {
                    if (chkCategoryTalk.Checked)
                    {
                        i++;
                        continue;
                    }
                    else
                        list.RemoveAt(i);
                }
                else if (list[i].NameSpaceKey == 100)
                {
                    if (chkPortal.Checked)
                    {
                        i++;
                        continue;
                    }
                    else
                        list.RemoveAt(i);
                }
                else if (list[i].NameSpaceKey == 101)
                {
                    if (chkPortalTalk.Checked)
                    {
                        i++;
                        continue;
                    }
                    else
                        list.RemoveAt(i);
                }
                else if (list[i].NameSpaceKey > 101)
                {
                    // Filter out all obscure namespaces
                    list.RemoveAt(i);
                }
                else
                    i++;
            }
        }

        private void FilterMatches(bool does, bool doesnot)
        {
            string strMatch = txtContains.Text;
            string strNotMatch = txtDoesNotContain.Text;

            if (!chkIsRegex.Checked)
            {
                strMatch = Regex.Escape(strMatch);
                strNotMatch = Regex.Escape(strNotMatch);
            }

            Regex match = new Regex(strMatch);
            Regex notMatch = new Regex(strNotMatch);

            int i = 0;
            while (i < list.Count)
            {
                if (does && match.IsMatch(list[i].Name))
                    list.RemoveAt(i);
                else if (doesnot && !notMatch.IsMatch(list[i].Name))
                    list.RemoveAt(i);
                else
                    i++;
            }
        }

        private void FilterList()
        {
            if (cbOpType.SelectedIndex == 0) foreach (Article a in lbRemove)
                {
                    list.Remove(a);
                }
            else
            {
                List<Article> list2 = new List<Article>();
                foreach (Article a in list)
                {
                    if (lbRemove.Items.Contains(a)) list2.Add(a);
                }
                list = list2;
            }
        }

        private void btnGetList_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            List<Article> list2 = new List<Article>();

            if (of.ShowDialog() == DialogResult.OK)
            {
                list2 = GetLists.FromTextFile(of.FileName);

                foreach (Article a in list2)
                    lbRemove.Items.Add(a);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lbRemove.Items.Clear();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkContains_CheckedChanged(object sender, EventArgs e)
        {
            txtContains.Enabled = chkContains.Checked;
            chkIsRegex.Enabled = chkContains.Checked || chkNotContains.Checked;
        }

        private void chkNotContains_CheckedChanged(object sender, EventArgs e)
        {
            txtDoesNotContain.Enabled = chkNotContains.Checked;
            chkIsRegex.Enabled = chkContains.Checked || chkNotContains.Checked;
        }

        public void UpdateText()
        {
            chkArticleTalk.Text = Variables.Namespaces[1];
            chkUser.Text = Variables.Namespaces[2];
            chkUserTalk.Text = Variables.Namespaces[3];
            chkWikipedia.Text = Variables.Namespaces[4];
            chkWikipediaTalk.Text = Variables.Namespaces[5];
            chkImage.Text = Variables.Namespaces[6];
            chkImageTalk.Text = Variables.Namespaces[7];
            chkMediaWiki.Text = Variables.Namespaces[8];
            chkMediaWikiTalk.Text = Variables.Namespaces[9];
            chkTemplate.Text = Variables.Namespaces[10];
            chkTemplateTalk.Text = Variables.Namespaces[11];
            chkHelp.Text = Variables.Namespaces[12];
            chkHelpTalk.Text = Variables.Namespaces[13];
            chkCategory.Text = Variables.Namespaces[14];
            chkCategoryTalk.Text = Variables.Namespaces[15];
            if (Variables.Namespaces.ContainsKey(100))
            {
                chkPortal.Text = Variables.Namespaces[100];
                chkPortalTalk.Text = Variables.Namespaces[101];
                chkPortal.Visible = true;
                chkPortalTalk.Visible = true;
            }
            else
            {
                chkPortal.Visible = false;
                chkPortalTalk.Visible = false;
            }
        }

        #region contextMenu

        private void nonTalkOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (CheckBox cb in groupBox1.Controls)
            {
                if (cb.Name.Contains("Talk"))
                    cb.Checked = false;
                else
                    cb.Checked = true;

            }
        }

        private void talkSpaceOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (CheckBox cb in this.groupBox1.Controls)
            {
                if (cb.Name.Contains("Talk"))
                    cb.Checked = true;
                else
                    cb.Checked = false;

            }
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (CheckBox cb in this.groupBox1.Controls)
            {
                cb.Checked = true;
            }
        }

        private void deselectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (CheckBox cb in this.groupBox1.Controls)
            {
                cb.Checked = false;
            }
        }

        #endregion

        private void specialFilter_Load(object sender, EventArgs e)
        {
            cbOpType.SelectedIndex = 0;
        }


    }
}