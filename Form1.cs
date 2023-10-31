using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace MusicPlayerApp
{
    public partial class MusicPlayerApp : Form
    {
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        ///
        /// Handling the window messages
        ///
        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);

            if (message.Msg == WM_NCHITTEST && (int)message.Result == HTCLIENT)
                message.Result = (IntPtr)HTCAPTION;
        }

        Music model = new Music();

        public MusicPlayerApp()
        {
            InitializeComponent();
        }

        // Create Global Variables of String Type Array ro save the titles or name of the tracks and path of the track
        string[] paths, files;

        private void MusicPlayerApp_Load(object sender, EventArgs e)
        {
            PopulateListBoxSongs();
        }

        void PopulateListBoxSongs()
        {
            using (DbEntities db = new DbEntities())
            {
                var list = db.Musics.ToList();

                for (int i = 0; i < list.Count; i++)
                {
                    listBoxSongs.Items.Add(list[i].Name);
                }
            }
        }

        private void btnSelectSong_Click(object sender, EventArgs e)
        {
            // Code to add songs
            OpenFileDialog ofd = new OpenFileDialog();
            // Code to add multiple files
            ofd.Multiselect = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                
                files = ofd.SafeFileNames; // Save the names of the track in files array
                paths = ofd.FileNames; // Save the path of the track in path array

                for (int i = 0; i < files.Length; i++)
                {
                    // Skip if the song already added
                    if (listBoxSongs.Items.Contains(files[i])) continue;
                    if(!files[i].ToLower().EndsWith(".mp3")) continue;

                    // Add the song to db
                    model.Name = files[i];
                    model.Path = paths[i];
                    using (DbEntities db = new DbEntities())
                    {
                        db.Musics.Add(model);
                        db.SaveChanges();
                    }

                    // Add the song to listBoxSong
                    listBoxSongs.Items.Add(files[i]);
                } 
            }
        }

        private void listBoxSongs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxSongs.SelectedIndex == -1) return;

            using (DbEntities db = new DbEntities())
            {
                var name = listBoxSongs.SelectedItem.ToString();
                model = db.Musics.Where(x => x.Name == name).FirstOrDefault();
            }
            // Code to play music
            axWindowsMediaPlayerMusic.URL = model.Path;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Code to close the app
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listBoxSongs.SelectedIndex == -1) return;
            using(DbEntities db = new DbEntities())
            {
                var name = listBoxSongs.SelectedItem.ToString();
                model = db.Musics.Where(x => x.Name == name).FirstOrDefault();

                db.Musics.Remove(model);
                db.SaveChanges(); 
            }
            listBoxSongs.Items.Remove(model.Name);
            axWindowsMediaPlayerMusic.Ctlcontrols.stop();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}
