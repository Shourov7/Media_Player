using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Speech;
using System.Speech.Recognition;
using System.Diagnostics;
using System.Speech.Synthesis;
using System.Threading;


namespace Media_Player
{
    public partial class Form1 : Form
    {
        SpeechSynthesizer speechsynth = new SpeechSynthesizer();
        SpeechRecognitionEngine receng = new SpeechRecognitionEngine();
        Choices choice = new Choices();


        public SpeechRecognitionEngine recognizer;

        public Grammar grammar;

        public Thread RecThread;

        public Boolean RecognizerState = true;


        public Form1()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Multiselect = true, ValidateNames = true, Filter = "WMV|*.wmv|WAV|*.wav|MP3|*.mp3|MP4|*.mp4|MKV|*.mkv" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    List<MediaFile> files = new List<MediaFile>();
                    foreach (string fileName in ofd.FileNames)
                    {
                        FileInfo fi = new FileInfo(fileName);
                        files.Add(new MediaFile() { FileName = Path.GetFileNameWithoutExtension(fi.FullName), Path = fi.FullName });

                    }
                    listFile.DataSource = files;
                    
                    
                }
            }
        }

        private void listFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            MediaFile file = listFile.SelectedItem as MediaFile;
            if (file != null)
            {
                axWindowsMediaPlayer.URL = file.Path;
                axWindowsMediaPlayer.Ctlcontrols.play();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listFile.ValueMember = "Path";
            listFile.DisplayMember = "FileName";
        
            GrammarBuilder build = new GrammarBuilder();
            build.AppendDictation();

            grammar = new Grammar(build);

            recognizer = new SpeechRecognitionEngine();
            recognizer.LoadGrammar(grammar);
            recognizer.SetInputToDefaultAudioDevice();

            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);


            RecognizerState = true;
            RecThread = new Thread(new ThreadStart(RecThreadFunction));
            RecThread.Start();

        }
        public void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (RecognizerState)
                return;

            this.Invoke((MethodInvoker)delegate
            {
                textBox1.Text += (" " + e.Result.Text.ToLower());
            });
        }
        public void RecThreadFunction()
        {
            while (true)
            {
                try
                {
                    recognizer.Recognize();
                }
                catch
                {

                }
            }
        }


       

       

       

        private void axWindowsMediaPlayer_Enter(object sender, EventArgs e)
        {
            
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer.URL = openFileDialog1.FileName;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer.Ctlcontrols.pause();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer.Ctlcontrols.play();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer.Ctlcontrols.stop();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer.Ctlcontrols.fastForward();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer.Ctlcontrols.fastReverse();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            axWindowsMediaPlayer.settings.volume = trackBar1.Value;
        }

        private void list1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void btn12_Click(object sender, EventArgs e)
        {
            RecognizerState = true;
        }

        private void btn13_Click(object sender, EventArgs e)
        {
            RecognizerState = false;
        }
      

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            RecThread.Abort();
            RecThread = null;

            recognizer.UnloadAllGrammars();
            recognizer.Dispose();

            grammar = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.youtube.com/results?search_query=" + (textBox1.Text));
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.google.com/#q=" + (textBox1.Text));
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btngood_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.youtube.com/watch?v=IcrbM1l_BoI&list=PL_0_c6VNzM8xEcaO81hbPKw-Au_uJf8Hy");
        }

        private void startbtn_Click(object sender, EventArgs e)
        {
            startbtn.Enabled = false;
            stopbtn.Enabled = true;
            choice.Add(new string[] { "hello", "fine", "what is the current time", "Normal", "Bad", "close" });
            Grammar gr = new Grammar(new GrammarBuilder(choice));
            try
            {
                receng.RequestRecognizerUpdate();
                receng.LoadGrammar(gr);
                receng.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(receng_SpeechRecognized);
                receng.SetInputToDefaultAudioDevice();
                receng.RecognizeAsync(RecognizeMode.Multiple);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        void receng_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            switch (e.Result.Text.ToString())
            {
                case "hello":
                    speechsynth.SpeakAsync("hai Boss. How is your mood");
                    break;
                case "fine":
                    System.Diagnostics.Process.Start( "https://www.youtube.com/watch?v=mk48xRzuNvA&list=PLynl2AdLA6UGR4YE-5Rc8UNAgt6DXwW4V");
                    break;
                case "what is the current time":
                    speechsynth.SpeakAsync("right now it is " + DateTime.Now.ToLongTimeString());
                    break;
                case "Bad":
                   System.Diagnostics.Process.Start( "https://www.youtube.com/watch?v=C7dPqrmDWxs&list=PLC3M2mpOZuYgsWdDGm7FDnNAiXts98U8y");
                    break;
                case "Normal":
                    System.Diagnostics.Process.Start( "https://www.youtube.com/watch?v=wyx6JDQCslE");
                    break;
                case "close":
                    speechsynth.Speak("see you later. bye bye");
                    Application.Exit();
                    break;

            }
            list1.Items.Add(e.Result.Text.ToString());
        }

        private void stopbtn_Click(object sender, EventArgs e)
        {
            receng.RecognizeAsyncStop();
            startbtn.Enabled = true;
            stopbtn.Enabled = false;
        }

        private void btnFullScreen_Click(object sender, EventArgs e)
        {
            if (axWindowsMediaPlayer.URL.Length > 0)
            {
                axWindowsMediaPlayer.fullScreen = true;
            }
        }

        private void btnTV_Click(object sender, EventArgs e)
        {
           // axWindowsMediaPlayer.URL = "https://youtu.be/maGIAkiepR8";//
            System.Diagnostics.Process.Start("http://dhakasports.com/gazi-tv-bd.php");
            
        }

        

      
    }
}
