using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicPlayerApp
{
    public partial class MusicPlayerApp : Form
    {
        public MusicPlayerApp()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        // Create Global Variables of String Type Array ro save the titles or name of the tracks and path of the track
        string[] paths, files;

        private void btnSelectSong_Click(object sender, EventArgs e)
        {
            // Code to select songs
            OpenFileDialog ofd = new OpenFileDialog();
            // Code to select multiple files
            ofd.Multiselect = true;

            if(ofd.ShowDialog() == DialogResult.OK)
            {
                files = ofd.SafeFileNames; // Save the names of the track in files array
                paths = ofd.FileNames; // Save the path of the track in path array

                // Display the music titles in listbox
                for (int i = 0; i < files.Length; i++)
                {
                    listBoxSongs.Items.Add(files[i]); 
                }
            }
        }

        private void listBoxSongs_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Code to play music
            axWindowsMediaPlayerMusic.URL = paths[listBoxSongs.SelectedIndex];
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Code to close the app
            this.Close();
        }
    }
}
