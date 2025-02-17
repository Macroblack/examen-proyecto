﻿using RtfPipe.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Security.RightsManagement;
using System.Text;
using System.Windows.Forms;

namespace DiaryJournal.Net
{
    public partial class CustomFontDialog : Form
    {
        public bool editor = false;

        public Font font = null;
        public int size = 0;
        public bool bold = false;
        public bool italic = false;
        public bool underline = false;
        public bool strikeout = false;
        public Color fontColor = Color.Black;
        public Color fontBackColor = Color.White;
        public textFormatting formatting = null;

        public CustomFontDialog()
        {
            InitializeComponent();

            for (int size = 6; size <= 300; size++)
                lstSize.Items.Add(size);

            txtSize.Text = Convert.ToString(10);
        }
        private void lstFont_SelectedFontFamilyChanged(object sender, EventArgs e)
        {
            UpdateSampleText();
        }

        private void lstSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstSize.SelectedItem != null)
                txtSize.Text = lstSize.SelectedItem.ToString();
        }

        private void txtSize_TextChanged(object sender, EventArgs e)
        {
            UpdateSampleText();
        }

        private void txtSize_KeyDown(object sender, KeyEventArgs e)
        {

        }

        public static double WpfFontSizeToWinformsFontSize(double size)
        {
            return ((size * 72.0 / 96.0));
        }
        public static double WinformsFontSizeToWpfFontSize(double size)
        {
            return ((size * 96) / 72);
        }

        private void UpdateSampleText()
        {
            double size = txtSize.Text != "" ? double.Parse(txtSize.Text) : 1;

            FontStyle style = chbBold.Checked ? FontStyle.Bold : FontStyle.Regular;
            if (chbItalic.Checked) style |= FontStyle.Italic;
            if (chbStrikeout.Checked) style |= FontStyle.Strikeout;
            if (chbUnderline.Checked) style |= FontStyle.Underline;

            if (lvFonts.SelectedItems.Count <= 0)
                return;

            ListViewItem item = lvFonts.SelectedItems[0];
            float finalSize = ((editor) ? (float)(size * 72.0 / 96.0) : (float)size);
            Font newFont = new Font(item.Name, finalSize, style);
            lblSampleText.Font = newFont;

            // update colors
            lblSampleText.ForeColor = fontColor;
            lblSampleText.BackColor = fontBackColor;
        }

        /// <summary>
        /// Handles CheckedChanged event for Bold, 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chb_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSampleText();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            size = txtSize.Text != "" ? int.Parse(txtSize.Text) : 1;
            bold = chbBold.Checked;
            italic = chbItalic.Checked;
            underline = chbUnderline.Checked;
            strikeout = chbStrikeout.Checked;

            FontStyle style = lblSampleText.Font.Style;
            Font selectedFont = new Font(lblSampleText.Font.FontFamily, (float)size, style);
            font = selectedFont;
        }

        private void CustomFontDialog_Load(object sender, EventArgs e)
        {
            txtSize.Text = size.ToString();
            lstSize.SelectedIndex = lstSize.FindString(size.ToString());
            lstSize.TopIndex = lstSize.SelectedIndex;
            chbBold.Checked = bold;
            chbItalic.Checked = italic;
            chbStrikeout.Checked = strikeout;
            chbUnderline.Checked = underline;

            var webColors = commonMethods.getWebColors();// typeof(Color));
            foreach (Color knownColor in webColors)
            {
                ListViewItem item1 = new ListViewItem();
                item1.Text = knownColor.Name;
                item1.Name = knownColor.Name;
                item1.Tag = knownColor;
                item1.BackColor = knownColor;
                lvFontColor.Items.Add(item1);
                ListViewItem item2 = new ListViewItem();
                item2.Text = knownColor.Name;
                item2.Name = knownColor.Name;
                item2.Tag = knownColor;
                item2.BackColor = knownColor;
                lvFontBackColor.Items.Add(item2);
            }

            // set font colors
            Color fcolor = Color.Black;
            Color fbcolor = Color.White;
            if (fontColor == null) fontColor = fcolor;
            if (fontBackColor == null) fontBackColor = fbcolor;
            foreach (Color knownColor in webColors)
            {
                int knownColorArgb = knownColor.ToArgb();

                int fontColorArgb = fontColor.ToArgb();
                if (knownColorArgb == fontColorArgb)
                    fcolor = knownColor;

                int fontBackColorArgb = fontBackColor.ToArgb();
                if (knownColorArgb == fontBackColorArgb)
                    fbcolor = knownColor;
            }
            ListViewItem[] items = lvFontColor.Items.Find(fcolor.Name, false);
            ListViewItem defaultItem1 = items[0];
            items = lvFontBackColor.Items.Find(fbcolor.Name, false);
            ListViewItem defaultItem2 = items[0];
            defaultItem1.Selected = true;
            defaultItem2.Selected = true;
            defaultItem1.EnsureVisible();
            defaultItem2.EnsureVisible();

            // initialize all formatting config
            formatting = new textFormatting();
            foreach (String fontName in formatting.fontNames)
            {
                Font font = new Font(fontName, 12, FontStyle.Regular);
                ListViewItem item1 = new ListViewItem();
                item1.Text = fontName;
                item1.Name = fontName;
                item1.Font = font;
                lvFonts.Items.Add(item1);
            }

            if (this.font == null)
                this.font = new Font("Arial", 14, FontStyle.Regular);

            ListViewItem[] matchingFonts = lvFonts.Items.Find(font.Name, false);
            if (matchingFonts.Length > 0)
            {
                matchingFonts[0].Selected = true;
                matchingFonts[0].EnsureVisible();
                lvFonts.Focus();
                lvFonts.Select();
            }
        }

        private void lvFontColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListViewItem item = null;
            if (lvFontColor.SelectedItems.Count > 0)
                item = lvFontColor.SelectedItems[0];

            if (item == null)
                return;

            fontColor = (System.Drawing.Color) item.Tag;

            UpdateSampleText();

        }

        private void lvFontBackColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListViewItem item = null;
            if (lvFontBackColor.SelectedItems.Count > 0)
                item = lvFontBackColor.SelectedItems[0];

            if (item == null)
                return;

            fontBackColor = (System.Drawing.Color)item.Tag;

            UpdateSampleText();

        }

        private void chbUnderline_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSampleText();
        }

        private void lvFonts_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListViewItem item = null;
            if (lvFonts.SelectedItems.Count > 0)
                item = lvFonts.SelectedItems[0];

            if (item == null)
                return;

            txtFont.Text = item.Text;

            UpdateSampleText();
        }

        public Font? getNewFontComplete(out Color outBackColor, out Color outForeColor)
        {
            if (font == null)
            {
                outBackColor = outForeColor = Color.Empty;
                return null;
            }

            FontStyle style = bold ? FontStyle.Bold : FontStyle.Regular;
            if (italic) style |= FontStyle.Italic;
            if (strikeout) style |= FontStyle.Strikeout;
            if (underline) style |= FontStyle.Underline;
            outBackColor = fontBackColor;
            outForeColor = fontColor;
            return new Font(font.FontFamily.Name, size, style);
        }

        public static Font? getNewFontWithStyle(Font? font, float size, bool bold, bool italic, bool strikeout, bool underline)
        {
            if (font == null)
                return null;

            FontStyle style = bold ? FontStyle.Bold : FontStyle.Regular;
            if (italic) style |= FontStyle.Italic;
            if (strikeout) style |= FontStyle.Strikeout;
            if (underline) style |= FontStyle.Underline;
            return new Font(font.FontFamily.Name, size, style);
        }
        public static Font? getNewFontWithStyle(String fontName, float size, bool bold, bool italic, bool strikeout, bool underline)
        {
            if (fontName == "")
                return null;

            FontStyle style = bold ? FontStyle.Bold : FontStyle.Regular;
            if (italic) style |= FontStyle.Italic;
            if (strikeout) style |= FontStyle.Strikeout;
            if (underline) style |= FontStyle.Underline;
            return new Font(fontName, size, style);
        }

    }
}
