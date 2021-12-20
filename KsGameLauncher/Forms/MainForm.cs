﻿using System;
using System.Drawing;
using System.IO;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Threading.Tasks;
using KsGameLauncher.Forms;

#if DEBUG
using System.Diagnostics;
#endif

namespace KsGameLauncher
{
    public partial class MainForm : Form
    {

        private static NotifyIcon notifyIcon;

        private bool _launcherMutex = false;

        /// <summary>
        /// Left clicked icon
        /// </summary>
        private ContextMenuStrip menuStripMain;

        /// <summary>
        /// Disabling "Close" button on control box
        /// </summary>
        protected override CreateParams CreateParams
        {
            [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
            get
            {
                const int CP_NOCLOSE_BUTTON = 0x200;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CP_NOCLOSE_BUTTON;
                return cp;
            }
        }


        /// <summary>
        /// Finalizer
        /// </summary>
        public MainForm()
        {
            InitializeComponent();


            // Hide from taskbar
            CreateNotificationIcon();
            ShowInTaskbar = false;
            WindowState = FormWindowState.Minimized;
            contextMenuStrip_Sub.AutoClose = true;

        }


        ~MainForm()
        {
            notifyIcon.Visible = false;
            notifyIcon.Dispose();
        }

        private static NotifyIcon CreateNotifyIcon()
        {
            NotifyIcon notify = new NotifyIcon
            {
                Icon = Properties.Resources.app,
                Text = Resources.AppName,
                Visible = true,
                BalloonTipTitle = Resources.AppName,
            };

            return notify;
        }



        private void CreateNotificationIcon()
        {
            notifyIcon = CreateNotifyIcon();

            notifyIcon.MouseClick += NotifyIcon_MouseClick;

            LoadGamesMenu();
        }

        async public void LoadGamesMenu()
        {
            string json = await GetJson();
            if (json == null)
                menuStripMain = CreateInitialMenuStripItems();
            else
            {
                AppInfo.LoadFromJson(json);
            }
            menuStripMain = InitGameMenu();
        }

        /// <summary>
        /// Clicked notifyIcon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            this.ShowInTaskbar = false;
            switch (e.Button)
            {
                case MouseButtons.Right:
                    {
                        // Display sub menu
                        contextMenuStrip_Sub.Show(this, PointToClient(Cursor.Position));
                    }
                    break;

                case MouseButtons.Left:
                    {
                        // Display main menu (games)
                        menuStripMain.Show(this, PointToClient(Cursor.Position));
                    }
                    break;
            }
        }



        async private Task<string> GetJson()
        {
            string json;
            if (!File.Exists(Properties.Settings.Default.appInfoLocal))
            {
                // Load appinfo.json from the internet
                // Download from `Properties.Settings.Default.appInfoURL`
                await Utils.AppUtil.DownloadJson();
            }

            try
            {
                // Load appinfo.json from local
                using (StreamReader jsonStream = File.OpenText(Path.GetFullPath(Properties.Settings.Default.appInfoLocal)))
                {
                    json = jsonStream.ReadToEnd();
                    jsonStream.Close();
                }

#if DEBUG
                Debug.Write(json);
#endif

                return json;
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show(String.Format(Resources.FailedToLoadFile, Properties.Settings.Default.appInfoLocal),
                    Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }




        private ContextMenuStrip InitGameMenu()
        {
            ContextMenuStrip menu = new ContextMenuStrip
            {
                AutoClose = true,

            };
            menu.Items.Clear();

            Font font = GetFontSize();
            int iconSize = GetIconSize();

            if (AppInfo.GetList().Count > 0)
            {

                foreach (AppInfo appInfo in AppInfo.GetList())
                {
                    //#if DEBUG
                    //                    Debug.WriteLine(JsonSerializer.Serialize(appInfo));
                    //#endif
                    if (Utils.GameRegistry.IsInstalled(appInfo.Name))
                    {
                        var item = CreateNewMenuItem(appInfo, font, iconSize);
                        menu.Items.Add(item);
                    }
                }
            }
            menu.ImageScalingSize = new Size(iconSize, iconSize);


            if (menu.Items.Count < 1)
            {
                menu = CreateInitialMenuStripItems();
            }

            return menu;
        }

        private int GetIconSize()
        {
            switch (Properties.Settings.Default.ContextMenuSize)
            {
                case 1:
                    return Properties.Settings.Default.MenuIconSize_Large;
            }
            return Properties.Settings.Default.MenuIconSize_Default;
        }

        private Font GetFontSize()
        {
            Font font = Control.DefaultFont;
            switch (Properties.Settings.Default.ContextMenuSize)
            {
                case 1:
                    return new Font(Control.DefaultFont.FontFamily, (float)(Control.DefaultFont.Size * 1.5));
            }
            return font;
        }

        internal ToolStripMenuItem CreateNewMenuItem(AppInfo appInfo)
        {
            return CreateNewMenuItem(appInfo, GetFontSize(), GetIconSize());
        }

        internal ToolStripMenuItem CreateNewMenuItem(AppInfo appInfo, Font font, int iconSize)
        {
            ToolStripMenuItem item = new ToolStripMenuItem
            {
                ImageScaling = ToolStripItemImageScaling.None
            };

            item.Text = appInfo.Name;
            item.Font = font;
            item.Size = new Size(iconSize, iconSize);

            Icon icon = appInfo.GetIcon();

            if (icon != null)
                item.Image = (new Icon(icon, iconSize, iconSize)).ToBitmap();

            item.Click += delegate
            {
                if (_launcherMutex)
                {
                    MessageBox.Show(Resources.StartngLauncher, Resources.AppName,
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                _launcherMutex = true;
                try
                {
                    Launcher launcher = Launcher.Create();
                    launcher.StartApp(appInfo);
                }
                catch (LoginException ex)
                {
                    MessageBox.Show(String.Format(
                        "Launcher: {0}, Exception: {1}\nMessage: {2}\n\nSource: {3}\n\n{4}",
                        appInfo.Name, ex.GetType().Name, ex.Message, ex.Source, ex.StackTrace)
                    , Resources.ErrorWhileLogin, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (LauncherException ex)
                {
                    MessageBox.Show(String.Format(
                        "Launcher: {0}, Exception: {1}\nMessage: {2}\n\nSource: {3}\n\n{4}",
                        appInfo.Name, ex.GetType().Name, ex.Message, ex.Source, ex.StackTrace)
                    , ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format(
                        "Launcher: {0}, Exception: {1}\nMessage: {2}\n\nSource: {3}\n\n{4}",
                        appInfo.Name, ex.GetType().Name, ex.Message, ex.Source, ex.StackTrace)
                    , ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    _launcherMutex = false;
                }

            };
            return item;
        }

        private ContextMenuStrip CreateInitialMenuStripItems()
        {
            ToolStripMenuItem item = new ToolStripMenuItem()
            {
                Text = Resources.NoInstalledGames,
                Enabled = false,
                AutoSize = true,
            };
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add(item);

            return menu;
        }


        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.ShowConfirmExit)
            {
                ExitingProcess();
                return;
            }

            DialogResult result = MessageBox.Show(Resources.ConfirmExitDialogMessage, Resources.AppName,
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly);

            if (result == DialogResult.OK)
            {
                ExitingProcess();
            }
        }

        private void ExitingProcess()
        {
            notifyIcon.Visible = false;
            notifyIcon.Dispose();
            Properties.Settings.Default.Save();
            Application.Exit();
        }


        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm
            {
                ShowInTaskbar = false
            };
            aboutForm.ShowDialog(this);

        }

        private void ManageAccountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new AccountForm()).Show(this);
        }

        private void OptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new OptionsForm()).ShowDialog(this);
        }

        public static void DisplayToolTip(string message, int timeout)
        {
            if (Properties.Settings.Default.EnableNotification)
            {
                if (notifyIcon == null)
                    notifyIcon = CreateNotifyIcon();

                notifyIcon.BalloonTipText = message;
                notifyIcon.ShowBalloonTip(timeout);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Text = Resources.AppName;
        }


        private void AddNewGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new AddNewGame()).ShowDialog(this);
        }

        internal ContextMenuStrip GetMenuStrip()
        {
            return menuStripMain;
        }

        internal void SetMenuStrip(ContextMenuStrip menu)
        {
            menuStripMain = menu;
        }
    }
}