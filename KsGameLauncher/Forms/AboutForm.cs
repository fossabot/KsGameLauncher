﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KsGameLauncher
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            Icon = Properties.Resources.appIcon;
        }

        private void About_Load(object sender, EventArgs e)
        {
            Icon = Properties.Resources.appIcon;
            Text = Resources.AboutThisApp;
            label_Application.Text = Resources.AppName;
            label_Version.Text = "ver. " + Application.ProductVersion;
            label_Develop.Text = Resources.LabelDeveloper;
            AppDeveloper.Text = Properties.Resources.Developers;
            string installedGameRights = string.Format("\"{0}\"", string.Join("\", \"", GetInstalledGamesName()));
            textBox_Copyrights.Text = string.Format("{0} are KONAMI Amusement All Rights Reserved.\r\n", installedGameRights) +
                Properties.Resources.Copyrights;
            linkLabel_Support.Text = Properties.Resources.SupportLabelText;
            linkLabel_License.Text = Resources.ShowLicense;

            button_Ok.Focus();
        }

        private string[] GetInstalledGamesName()
        {
            List<AppInfo> appInfo = AppInfo.GetList();
            string[] gameList = new string[appInfo.Count];
            int i = 0;
            AppInfo.GetList().ForEach(info =>
            {
                gameList[i++] = info.Name;
            });
            return gameList;
        }

        private void LinkLabel_Support_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(Properties.Resources.SupportLabelURL);
        }

        private void Button_Ok_Click(object sender, EventArgs e) => Close();

        private void LinkLabel_License_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            (new LicensesForm()
            {
                ShowInTaskbar = false,
            }).ShowDialog(this);
        }

        private void AboutForm_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
    }
}
