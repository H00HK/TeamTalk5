using BearWare;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.Globalization;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;
using TeamTalkApp.NET.Properties;
using NAudio.Wave;
using TeamTalkLib.Settings;

namespace TeamTalkApp.NET.Forms
{
    public partial class AutoMicroConf : Form
    {
        TeamTalk ttclient;
        TeamTalk ttclientListener;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;

        ConnectionSettings settings;
        Settings set;

        string IPADDR;
        int TCPPORT;
        int UDPPORT;

        //const string IPADDR = "192.168.56.1";
        //const int TCPPORT = 10333;
        //const int UDPPORT = 10333;

        bool ENCRYPTED;
        string USERNAME;
        string PASSWORD;
        string NICKNAME;
        const int TIME = 10;
        const int dt = 0;

        string recordedWavePath;

        private int counter1 = TIME;
        private int counter2 = TIME;
        private int counter3 = TIME;

        List<TeamTalk> ttclients = new List<TeamTalk>();

        private IWavePlayer wavePlayer;
        private AudioFileReader file;

        int currentTT = -1;

        IntPtr soundloop;

        int firstTime = 0;

        public static bool voxtxCheckBoxAC = false;
        public static bool echoAC = false;
        public static bool duplexAC = false;
        public static int outputVolume;

        private Channel chan;

        private bool voiceActivation;
        Forms.StartupSetupDlg test;

        private SpeexDSP spxTt;

        public AutoMicroConf(TeamTalk ttclient, ConnectionSettings settings, Settings set)
        {
            this.ttclient = ttclient;
            this.settings = settings;
            this.set = set;

            this.USERNAME = settings.User.Login;
            this.PASSWORD = settings.User.Password;
            this.NICKNAME = settings.User.Nick;
            this.UDPPORT = settings.Server.UDPPort;
            this.TCPPORT = settings.Server.TCPPort;
            this.IPADDR = settings.Server.IP;
            this.ENCRYPTED = settings.Server.Encrypted;

            //TeamTalk ttclientTest = NewClientInstance();

            InitializeComponent();
            volumeTrackBar.Minimum = SoundLevel.SOUND_GAIN_MIN;
            volumeTrackBar.Maximum = SoundLevel.SOUND_GAIN_MAX;
            volumeTrackBar.Value = 24000;


            inputProgressBar.Minimum = SoundLevel.SOUND_VU_MIN;
            inputProgressBar.Maximum = SoundLevel.SOUND_VU_MAX;

            button2.Enabled = false;
            button3.Enabled = false;
            label1.Visible = false;
            ttclient.GetChannel(ttclient.ChannelID, ref chan);
            voiceActivation = chan.audiocfg.bEnableAGC;

            invisible(true);

            TeamTalk ttclientTest = NewClientInstance();
            ttclientTest.SetSoundInputGainLevel(volumeTrackBar.Value);
            ttclientTest.SetSoundOutputVolume(SoundLevel.SOUND_VOLUME_MAX);
            AutoMicroConf.outputVolume = SoundLevel.SOUND_VOLUME_MAX;
            ttclients.Add(ttclientTest);
            InitInputSound(ttclientTest);


            ttclientListener = NewClientInstance();
            ttclientListener.SetSoundOutputVolume(0);
            connectListenerClient();

            currentTT++;
            inputMeterTimer.Start();


            test = new Forms.StartupSetupDlg(this.ttclients.ElementAt(currentTT), set);
            test.VolumeChanged += test_VolumeChanged;
        }

        void test_VolumeChanged(object sender, int volume)
        {
            ttclients.ElementAt(currentTT).SetSoundInputGainLevel(volume);
            volumeTrackBar.Value = volume;
        }

        private void volumeTrackBar_ValueChanged(object sender, EventArgs e)
        {
            if (currentTT > -1)
            {
                //ttclients.ElementAt(currentTT).SetSoundOutputVolume(volumeTrackBar.Value);
                ttclients.ElementAt(currentTT).SetSoundInputGainLevel(volumeTrackBar.Value);
            }
            //if (test != null) test.setVolumetTrackBar(volumeTrackBar.Value);


        }

        public void setVolumeTrackBar(int value)
        {
            volumeTrackBar.Value = value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            invisible(false);
            label4.Text = "";
            if (voiceActivation)
            {
                config4AutomaticActivation();
            }
            else
            {
                configNot4AutomaticActivation();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            counter1--;
            progressBar1.Value = TIME - counter1;
            label1.Text = (counter1 - dt).ToString() + " s";

            if (counter1 == 0)
            {
                label1.Text = (counter1 - dt).ToString() + " s";
                progressBar1.Value = TIME - counter1;
                timer1.Stop();
                testVoidRecording();
            }

        }

        private void timer2_Tick(object sender, EventArgs e)
        {

            counter2--;
            label1.Text = (counter2 - dt).ToString() + " s";
            progressBar1.Value = (TIME - counter2 + 1) > 10 ? 10 : (TIME - counter2 + 1);

            if (counter2 == 0)
            {
                label1.Text = (counter2 - dt).ToString() + " s";
                progressBar1.Value = TIME - (counter2 - dt);
                timer2.Stop();
                if ((double)maxInputSoundLevel / (double)lowInputSound > 0.2d)
                {
                    linkLabelOK.Text = "OK!";
                    linkLabelOK.LinkColor = Color.DarkGreen;
                    linkLabelOK.Visible = true;
                }
                else
                {
                    linkLabelOK.Text = "Uwaga";
                    linkLabelOK.LinkColor = Color.DarkRed;
                    linkLabelOK.Visible = true;
                }

                //ttclients.ElementAt(currentTT).SetUserMediaStorageDir(ttclient.UserID, "", "", AudioFileFormat.AFF_WAVE_FORMAT);
                ttclientListener.StopRecordingMuxedAudioFile();
                ttclients.ElementAt(currentTT).EnableVoiceTransmission(false);
                ttclients.ElementAt(currentTT).SetSoundOutputVolume(AutoMicroConf.outputVolume);
                button2.Enabled = true;
                SoundPlayer simpleSound = new SoundPlayer(Application.StartupPath + "\\sounds\\ding.wav");
                simpleSound.Play();
            }
        }

        private void testVoidRecording()
        {

            this.counter1 = TIME;
            label4.Text = "Mów:";
            label4.Focus();
            linkLabelOK.Visible = false;
            maxInputSoundLevel = 0;
            lowInputSound = 0;
            //Thread.Sleep(1000);


            recordTestSample(ttclients.ElementAt(currentTT));

            timer2 = new System.Windows.Forms.Timer();
            timer2.Tick += new EventHandler(timer2_Tick);
            timer2.Interval = 1000; // 1 second
            timer2.Start();
            progressBar1.Value = 0;
        }

        private void recordTestSample(TeamTalk ttclient)
        {

            ttclient.StopRecordingMuxedAudioFile();

            //testing(ttclient);

            Channel chan = new Channel();
            if (!ttclient.GetChannel(ttclient.ChannelID, ref chan))
            {
                MessageBox.Show("Musisz być zalogowany na serwerze, aby przeprowadzić test");
                return;
            }

            ttclient.EnableVoiceTransmission(true);

            WaitForEvent(ttclient, ClientEvent.CLIENTEVENT_USER_AUDIOBLOCK, 1000);
            ttclient.EnableAudioBlockEvent(ttclient.UserID, StreamType.STREAMTYPE_VOICE, true);

            string timestamp = DateTime.Now.ToString();
            timestamp = timestamp.Replace(":", "");
            timestamp = timestamp.Replace("/", "");
            timestamp = timestamp.Replace(" ", "");

            string extension = ".wav";
            string directory = Path.GetTempPath();
            string filename = directory + "_" + timestamp + "_audio-test" + extension;
            this.recordedWavePath = filename;

            ttclient.SetSoundOutputVolume(0);
            int l = ttclient.GetSoundInputGainLevel();
            ttclient.SetSoundOutputMute(true);
            TTMessage msg = new TTMessage();
            WaitForEvent(ttclient, ClientEvent.CLIENTEVENT_USER_STATECHANGE, 1000, ref msg);


            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("ja-JP");
            Thread.CurrentThread.CurrentCulture = currentCulture;

            SoundPlayer simpleSound = new SoundPlayer(Application.StartupPath + "\\sounds\\ding.wav");
            simpleSound.Play();
            SpeexDSP spx = new SpeexDSP();
            ttclient.GetSoundInputPreprocess(ref spx);
            ttclientListener.StartRecordingMuxedAudioFile(chan.audiocodec, filename, AudioFileFormat.AFF_WAVE_FORMAT);

        }

        private static bool Login(TeamTalk ttclient, string nick, string username, string passwd)
        {
            int cmdid = ttclient.DoLogin(nick, username, passwd);

            TTMessage msg = new TTMessage();
            bool lginmslf = WaitForEvent(ttclient, ClientEvent.CLIENTEVENT_CMD_MYSELF_LOGGEDIN, 1000, ref msg);
            bool clauth = ttclient.Flags.HasFlag(ClientFlag.CLIENT_AUTHORIZED);
            bool wtcmdcmpl = WaitCmdComplete(ttclient, cmdid, 1000);

            UserAccount account = (UserAccount)msg.DataToObject();

            bool usrequ = object.Equals(username, account.szUsername);

            return cmdid > 0 && usrequ && lginmslf && clauth && wtcmdcmpl;
        }

        private bool testing(TeamTalk ttclient)
        {
            // bool initsf = InitInputSound(ttclient);
            bool initsf = true;
            so = ttclient.GetSoundOutputVolume();
            si = ttclient.GetSoundInputGainLevel();
            bool conf = Connect(ttclient);

            bool loginf = Login(ttclient, NICKNAME, USERNAME, PASSWORD);

            bool jnroot = JoinRoot(ttclient);

            bool evrthngcool = conf && initsf && loginf && jnroot;

            return evrthngcool;
        }

        private bool connectListenerClient()
        {
            int devin = 0, devout = 0;

            //bool defsnddev = TeamTalk.GetDefaultSoundDevicesEx(SoundSystem.SOUNDSYSTEM_WASAPI, ref devin, ref devout);

            devin = this.set.sndinputid;
            devout = this.set.sndoutputid;
                


            bool isod = ttclientListener.InitSoundOutputDevice(devout);
            bool ttfhco = ttclientListener.Flags.HasFlag(ClientFlag.CLIENT_SNDOUTPUT_READY);
            bool initsf = true;
            bool conf = Connect(ttclientListener);

            bool loginf = Login(ttclientListener, NICKNAME, USERNAME, PASSWORD);

            bool jnroot = JoinRoot(ttclientListener);

            bool evrthngcool = conf && initsf && loginf && jnroot;

            return evrthngcool;
        }

        private static AudioCodec BuildSpeexCodec()
        {
            AudioCodec codec = new AudioCodec();
            codec.nCodec = Codec.SPEEX_CODEC;
            codec.speex.nBandmode = 1;
            codec.speex.nQuality = 4;
            codec.speex.nTxIntervalMSec = 40;
            codec.speex.bStereoPlayback = false;

            return codec;
        }

        private bool InitInputSound(TeamTalk ttclient)
        {
            int devin = 0, devout = 0;

            //bool defsnddev = TeamTalk.GetDefaultSoundDevicesEx(SoundSystem.SOUNDSYSTEM_WASAPI, ref devin, ref devout);

            devin = this.set.sndinputid;
            devout = this.set.sndoutputid;

            SpeexDSP spxdsp = new SpeexDSP();
            spxdsp.bEnableAGC = true;
            spxdsp.bEnableEchoCancellation = true;
            spxdsp.bEnableDenoise = true;

            spxdsp.nGainLevel = volumeTrackBar.Value;
            spxdsp.nMaxIncDBSec = SpeexDSPConstants.DEFAULT_AGC_INC_MAXDB;
            spxdsp.nMaxDecDBSec = SpeexDSPConstants.DEFAULT_AGC_DEC_MAXDB;
            spxdsp.nMaxGainDB = SpeexDSPConstants.DEFAULT_AGC_GAINMAXDB;

            spxdsp.nMaxNoiseSuppressDB = SpeexDSPConstants.DEFAULT_DENOISE_SUPPRESS;

            spxdsp.nEchoSuppress = SpeexDSPConstants.DEFAULT_ECHO_SUPPRESS;
            spxdsp.nEchoSuppressActive = SpeexDSPConstants.DEFAULT_ECHO_SUPPRESS_ACTIVE;

            this.spxTt = spxdsp;
            ttclient.SetSoundInputPreprocess(spxdsp);

            bool isid = ttclient.InitSoundInputDevice(devin);
            ttclient.SetSoundInputGainLevel(volumeTrackBar.Value);
            bool ttfhci = ttclient.Flags.HasFlag(ClientFlag.CLIENT_SNDINPUT_READY);

            return isid && ttfhci;
        }

        private bool Connect(TeamTalk ttclient)
        {
            bool tcon = ttclient.Connect(IPADDR, TCPPORT, UDPPORT, 0, 0, ENCRYPTED);
            bool wfe = WaitForEvent(ttclient, ClientEvent.CLIENTEVENT_CON_SUCCESS, 1000);

            return wfe && tcon;
        }

        private static bool WaitForEvent(TeamTalk ttclient, ClientEvent e, int waittimeout, ref TTMessage msg)
        {
            long start = DateTime.Now.Ticks / 10000;
            TTMessage tmp = new TTMessage();
            while (ttclient.GetMessage(ref tmp, waittimeout) && tmp.nClientEvent != e)
            {
                ttclient.ProcessMsg(tmp);

                if (DateTime.Now.Ticks / 10000 - start >= waittimeout)
                    break;
            }

            if (tmp.nClientEvent == e)
            {
                ttclient.ProcessMsg(tmp);

                msg = tmp;
            }
            return tmp.nClientEvent == e;
        }

        private static bool WaitForEvent(TeamTalk ttclient, ClientEvent e, int waittimeout)
        {
            TTMessage msg = new TTMessage();
            return WaitForEvent(ttclient, e, waittimeout, ref msg);
        }

        private static bool WaitCmdComplete(TeamTalk ttclient, int cmdid, int waittimeout)
        {
            TTMessage msg = new TTMessage();

            while (WaitForEvent(ttclient, ClientEvent.CLIENTEVENT_CMD_PROCESSING, waittimeout, ref msg))
            {
                if (msg.nSource == cmdid && (bool)msg.DataToObject() == false)
                    return true;
            }
            return false;
        }

        private static bool JoinRoot(TeamTalk ttclient)
        {
            bool usrauth = ttclient.Flags.HasFlag(ClientFlag.CLIENT_AUTHORIZED);
            bool rtchid = ttclient.GetRootChannelID() > 0;

            int cmdid = ttclient.DoJoinChannelByID(ttclient.GetRootChannelID(), "");

            bool wcmdcompl = WaitCmdComplete(ttclient, cmdid, 1000);

            return usrauth && rtchid && cmdid > 0 && wcmdcompl;
        }

        TeamTalk NewClientInstance()
        {
            TeamTalk ttclient = new TeamTalk(true);
            return ttclient;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Text = "Jeszcze raz";
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            sndTestCheckBox.Enabled = false;
            timer1 = new System.Windows.Forms.Timer();
            timer1.Tick += new EventHandler(timer3_Tick);
            timer1.Interval = 1000; // 1 second
            timer1.Start();
            counter3 = TIME;
            BeginPlayback(this.recordedWavePath);
            label4.Focus();

        }

        private void BeginPlayback(string filename)
        {
            this.wavePlayer = new WaveOut(WaveCallbackInfo.FunctionCallback());
            this.wavePlayer.PlaybackStopped += (pbss, pbse) =>
            {
                this.wavePlayer.Stop();
                this.wavePlayer.Dispose();
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                sndTestCheckBox.Enabled = true;

                linkLabel1.Enabled = linkLabel2.Enabled = linkLabel3.Enabled = true;
            };
            this.file = new AudioFileReader(filename);
            this.wavePlayer.Init(this.file);
            this.file.Volume = 1.0f;
            this.wavePlayer.Init(file);

            this.wavePlayer.Play();

            label4.Text = "Odtwarzanie:";
            label1.Text = counter3.ToString() + " s";
            progressBar1.Value = 0;
            button2.Enabled = false;
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            counter3--;

            progressBar1.Value = (TIME - counter3 + 1) > 10 ? 10 : (TIME - counter3 + 1);
            label1.Text = counter3.ToString() + " s";

            if (counter3 == 0)
            {
                timer1.Stop();
            }
        }


        private void sndTestCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            int devin = 0, devout = 0;

            //bool defsnddev = TeamTalk.GetDefaultSoundDevicesEx(SoundSystem.SOUNDSYSTEM_WASAPI, ref devin, ref devout);

            devin = this.set.sndinputid;
            devout = this.set.sndoutputid;

            if (!this.set.soundsystem.Equals(SoundSystem.SOUNDSYSTEM_WASAPI))
            {
                MessageBox.Show("Urządzenie dźwiękowe WASAPI najlepiej wspiera tą funkcjonalność");
            }


            if (sndTestCheckBox.Checked)
            {
                //Extract input device and get its default samplerate.
                //WASAPI devices only support one sample rate so it's important to use the correct one.
                SoundDevice[] devs;
                TeamTalk.GetSoundDevices(out devs);

                int in_samplerate = 0;
                foreach (SoundDevice dev in devs)
                {
                    if (dev.nDeviceID == devin)
                        in_samplerate = dev.nDefaultSampleRate;
                }

                SpeexDSP spxdsp = new SpeexDSP(true);
                spxdsp.bEnableAGC = true;
                spxdsp.bEnableDenoise = true;
                spxdsp.bEnableEchoCancellation = true;
                soundloop = TeamTalk.StartSoundLoopbackTest(devin, devout, in_samplerate, 1, true, spxdsp);
                if (soundloop == IntPtr.Zero)
                {
                    MessageBox.Show("Wystąpił problem testowania wybranego urządzenia dźwięku");
                    sndTestCheckBox.Checked = false;
                }
            }
            else
                TeamTalk.CloseSoundLoopbackTest(soundloop);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            TeamTalk tt = this.ttclients.ElementAt(currentTT);
            Properties.Settings.Default.AudioInLevel = tt.GetSoundInputGainLevel();
            Properties.Settings.Default.AudioOutLevel = tt.GetSoundOutputVolume();
            SpeexDSP spdx = new SpeexDSP();
            tt.GetSoundInputPreprocess(ref spdx);
            Properties.Settings.Default.AudioSaved = true;
            Properties.Settings.Default.bEnableAGC = spdx.bEnableAGC;
            Properties.Settings.Default.bEnableDenoise = spdx.bEnableDenoise;
            Properties.Settings.Default.bEnableEchoCancellation = spdx.bEnableEchoCancellation;
            Properties.Settings.Default.Save();
            this.ttclient = tt;
            this.ttclient.DoLogout();
            this.ttclientListener.DoLogout();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            test.refreshTt(this.ttclients.ElementAt(currentTT));
            test.ShowDialog();
        }

        private bool checkSettings()
        {
            return Properties.Settings.Default.AudioInLevel > 0 && Properties.Settings.Default.AudioOutLevel > 0;
        }


        public int si { get; set; }

        public int so { get; set; }

        public TeamTalk tt { get; set; }

        public void invisible(bool trigger)
        {

            if (trigger)
            {
                label4.Visible = true;
                label1.Visible = false;
                progressBar1.Visible = true;
            }
            else
            {
                label4.Visible = true;
                label1.Visible = true;
                progressBar1.Visible = true;
            }

        }

        public void config4AutomaticActivation()
        {
            if (currentTT > -1)
            {
                ttclients.ElementAt(currentTT).DoLogout();
            }
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            sndTestCheckBox.Enabled = false;
            linkLabel1.Enabled = linkLabel2.Enabled = linkLabel3.Enabled = false;
            label4.Text = "Zachowaj ciszę:";
            label4.Focus();

            if (firstTime == 0)
            {
                testing(ttclients.ElementAt(currentTT));
            }
            else
            {
                //TeamTalk ttclientTest = NewClientInstance();
                //ttclientTest.SetSoundInputGainLevel(volumeTrackBar.Value);
                //ttclients.Add(ttclientTest);
                //testing(ttclients.ElementAt(++currentTT));
            }
            firstTime = 1;

            counter1 = TIME;
            counter2 = TIME;
            counter3 = TIME;

            timer1 = new System.Windows.Forms.Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 1000; // 1 second
            timer1.Start();
            label1.Text = (counter1 - dt).ToString() + " s";
            progressBar1.Value = 0;
        }

        public void configNot4AutomaticActivation()
        {
            if (currentTT > -1)
            {
                //ttclients.ElementAt(currentTT).DoLogout();
            }
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            sndTestCheckBox.Enabled = false;

            linkLabel1.Enabled = linkLabel2.Enabled = linkLabel3.Enabled = false;
            label4.Focus();

            if (firstTime == 0)
            {

                testing(ttclients.ElementAt(currentTT));
            }
            else
            {
                //TeamTalk ttclientTest = NewClientInstance();
                //ttclientTest.SetSoundInputGainLevel(volumeTrackBar.Value);
                //ttclients.Add(ttclientTest);
                //testing(ttclients.ElementAt(++currentTT));
            }
            firstTime = 1;

            counter1 = TIME;
            counter2 = TIME;
            counter3 = TIME;

            testVoidRecording();
            //progressBar1.Value = 0;
        }

        private void inputMeterTimer_Tick(object sender, EventArgs e)
        {

            if (ttclients.ElementAt(currentTT).GetSoundInputLevel() > SoundLevel.SOUND_VU_MAX / 5) maxInputSoundLevel++;
            lowInputSound++;
            inputProgressBar.Value = ttclients.ElementAt(currentTT).GetSoundInputLevel();
        }


        public int maxInputSoundLevel { get; set; }

        public int lowInputSound { get; set; }

        private void volumeTrackBar_Scroll(object sender, EventArgs e)
        {

        }

        private void AutoMicroConf_Load(object sender, EventArgs e)
        {

        }

        private void AutoMicroConf_FormClosing(object sender, FormClosingEventArgs e)
        {
            ttclientListener.DoLogout();

            ttclientListener.Disconnect();
            ttclients.ElementAt(currentTT).DoLogout();
            ttclients.ElementAt(currentTT).Disconnect();
            ttclientListener.Dispose();
        }

    }

}
