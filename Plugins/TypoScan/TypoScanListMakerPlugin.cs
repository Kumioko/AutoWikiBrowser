﻿/*
TypoScan ListMakerPlugin
Copyright (C) 2008 Sam Reed

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
using System.Text;

using System.Xml;
using System.IO;

using WikiFunctions;
using WikiFunctions.Plugin;

namespace WikiFunctions.Plugins.ListMaker.TypoScan
{
    public class TypoScanListMakerPlugin : IListMakerPlugin
    {
        public string Name
        {
            get { return "TypoScan ListMaker Plugin"; }
        }

        public List<Article> MakeList(params string[] searchCriteria)
        {
            List<Article> articles = new List<Article>();

            string html = Tools.GetHTML("http://typoscan.reedyboy.net/index.php?action=displayarticles");

            using (XmlTextReader reader = new XmlTextReader(new StringReader(html)))
            {
                while (reader.Read())
                {
                    if (reader.Name.Equals("article"))
                    {
                        reader.MoveToAttribute("id");
                        int id = int.Parse(reader.Value);
                        string title = reader.ReadString();
                        articles.Add(new WikiFunctions.Article(title));
                        TypoScanAWBPlugin.PageList.Add(title, id);
                    }
                }
            }

            return articles;
        }

        public string DisplayText
        {
            get { return "TypoScan"; }
        }

        public string UserInputTextBoxText
        {
            get { return ""; }
        }

        public bool UserInputTextBoxEnabled
        {
            get { return false; }
        }

        public void Selected()
        { }

        public bool RunOnSeparateThread
        {
            get { return true; }
        }
    }
}
