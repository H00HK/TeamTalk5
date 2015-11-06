using BearWare;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using TeamTalkApp.Utils;
using TeamTalkLib.Settings;

namespace TeamTalkApp.NET.Forms
{
    public partial class StartupSetupDlg : Form
    {
        TeamTalk ttclient;
        Settings settings;
        //CommonSettings userSettings;

        public event EventHandler ChangeSoundOutputVolume;

        public StartupSetupDlg(TeamTalk ttclient, Settings settings)
        {

            this.settings = settings;
            this.ttclient = ttclient;
            InitializeComponent();

           
            //inputProgressBar.Minimum = SoundLevel.SOUND_VU_MIN;
            //inputProgressBar.Maximum = SoundLevel.SOUND_VU_MAX;
            //voiceactTrackBar.Minimum = SoundLevel.SOUND_VU_MIN;
            //voiceactTrackBar.Maximum = SoundLevel.SOUND_VU_MAX;


            volumeTrackBar.Minimum = SoundLevel.SOUND_VOLUME_MIN;
            volumeTrackBar.Maximum = SoundLevel.SOUND_VOLUME_MAX;

            //voiceactTrackBar.Value = ttclient.GetVoiceActivationLevel();

            volumeTrackBar.Value = ttclient.GetSoundOutputVolume();

            //voxtxCheckBox.Checked = AutoMicroConf.voxtxCheckBoxAC;
            duplexCheckBox.Checked = AutoMicroConf.duplexAC;
            echocancelCheckBox.Checked = AutoMicroConf.echoAC;

            dsoundRadioButton.Checked = settings.soundsystem == SoundSystem.SOUNDSYSTEM_DSOUND;
            winmmRadioButton.Checked = settings.soundsystem == SoundSystem.SOUNDSYSTEM_WINMM;
            
            UpdateSoundSystem(null, null);

        }

        public delegate void VolumeChangedHandler(object sender, int volume);
        public event VolumeChangedHandler VolumeChanged = delegate { }; // add empty delegate!

        private void StartupSetupDlg_Load(object sender, EventArgs e)
        {

            //Forms.AutoMicroConf auto = new Forms.AutoMicroConf(ttclient, settings);
            //auto.ShowDialog();


        }



        //private void voxtxCheckBox_CheckedChanged(object sender, EventArgs e)
        //{
        //    voiceactTrackBar.Enabled = voxtxCheckBox.Checked;
        //    ttclient.EnableVoiceActivation(voxtxCheckBox.Checked);
        //    AutoMicroConf.voxtxCheckBoxAC = voxtxCheckBox.Checked;
        //}

        private void volumeTrackBar_ValueChanged(object sender, EventArgs e)
        {
            ttclient.SetSoundOutputVolume(volumeTrackBar.Value);
            
            //VolumeChanged(this, volumeTrackBar.Value);
        }

        private void duplexCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            echocancelCheckBox.Enabled = duplexCheckBox.Checked;
        }

        private void UpdateSelectedSoundDevices(object sender, EventArgs e)
        {
            if (sndinputComboBox.SelectedItem == null ||
               sndoutputComboBox.SelectedItem == null)
                return;

            int inputid = ((ItemData)sndinputComboBox.SelectedItem).id;
            int outputid = ((ItemData)sndoutputComboBox.SelectedItem).id;

            SoundDevice[] devs;
            TeamTalk.GetSoundDevices(out devs);

            int in_samplerate = 0, out_samplerate = 0;
            foreach (SoundDevice dev in devs)
            {
                if (dev.nDeviceID == inputid)
                    in_samplerate = dev.nDefaultSampleRate;
                if (dev.nDeviceID == outputid)
                    out_samplerate = dev.nDefaultSampleRate;
            }

            //for duplex mode both input and output sound device must support the same sample rate
            duplexCheckBox.Enabled = in_samplerate == out_samplerate;
            if (in_samplerate != out_samplerate)
            {
                duplexCheckBox.Checked = false;
                echocancelCheckBox.Checked = false;
            }
            echocancelCheckBox.Enabled = duplexCheckBox.Checked;
        }

        void UpdateSoundSystem(object sender, EventArgs e)
        {
            sndinputComboBox.Items.Clear();
            sndoutputComboBox.Items.Clear();

            SoundDevice[] devs;
            TeamTalk.GetSoundDevices(out devs);

            SoundSystem soundsystem = SoundSystem.SOUNDSYSTEM_WASAPI;
            if (dsoundRadioButton.Checked)
            {
                soundsystem = SoundSystem.SOUNDSYSTEM_DSOUND;
                Debug.WriteLine("DirectSound devices");
            }
            else if (winmmRadioButton.Checked)
            {
                soundsystem = SoundSystem.SOUNDSYSTEM_WINMM;
                Debug.WriteLine("WinMM devices");
            }
            else
                Debug.WriteLine("WASAPI devices");

            Debug.WriteLine("INPUT DEVICES");
            foreach (SoundDevice dev in devs)
            {
                if (dev.nSoundSystem != soundsystem)
                    continue;
                Debug.WriteLine("Name " + dev.szDeviceName);
                Debug.WriteLine("\tID #" + dev.nDeviceID);
                Debug.WriteLine("\tUnique ID #" + dev.szDeviceID);
                Debug.WriteLine("\tWaveDeviceID #" + dev.nWaveDeviceID);
                string tmp = "";
                if (WindowsMixer.GetWaveInName(dev.nWaveDeviceID, ref tmp))
                    Debug.WriteLine("\tMixer name: " + tmp);
                for (int i = 0; i < WindowsMixer.GetWaveInControlCount(dev.nWaveDeviceID); i++)
                    if (WindowsMixer.GetWaveInControlName(dev.nWaveDeviceID, i, ref tmp))
                    {
                        Debug.WriteLine("\t\tControl name: " + tmp);
                        Debug.WriteLine("\t\tSelected: " + WindowsMixer.GetWaveInControlSelected(dev.nWaveDeviceID, i));
                    }

                if (dev.nMaxInputChannels > 0)
                {
                    int index = sndinputComboBox.Items.Add(new ItemData(dev.szDeviceName, dev.nDeviceID));
                    if (dev.nDeviceID == settings.sndinputid)
                        sndinputComboBox.SelectedIndex = index;
                }
                if (dev.nMaxOutputChannels > 0)
                {
                    int index = sndoutputComboBox.Items.Add(new ItemData(dev.szDeviceName, dev.nDeviceID));
                    if (dev.nDeviceID == settings.sndoutputid)
                        sndoutputComboBox.SelectedIndex = index;
                }
            }
            if (sndinputComboBox.SelectedIndex < 0 && sndinputComboBox.Items.Count > 0)
                sndinputComboBox.SelectedIndex = 0;
            if (sndoutputComboBox.SelectedIndex < 0 && sndoutputComboBox.Items.Count > 0)
                sndoutputComboBox.SelectedIndex = 0;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            
            // OK INHERITED FROM PREFERENCES
            
            ClientFlag flags = ttclient.GetFlags();

            //Audio-tab
            if ((ttclient.Flags & ClientFlag.CLIENT_SNDINOUTPUT_DUPLEX) ==
                ClientFlag.CLIENT_SNDINOUTPUT_DUPLEX)
                ttclient.CloseSoundDuplexDevices();
            else
            {
                ttclient.CloseSoundInputDevice();
                ttclient.CloseSoundOutputDevice();
            }
            ItemData inputItem;
            try
            {
                inputItem = (ItemData)sndinputComboBox.SelectedItem;
                settings.sndinputid = inputItem.id;
            }
            catch(System.NullReferenceException)
            {
                Forms.MicroWarning microDialog = new Forms.MicroWarning();
                microDialog.ShowDialog();
            }
            ItemData outputItem = (ItemData)sndoutputComboBox.SelectedItem;
            
            settings.sndoutputid = outputItem.id;

            if (duplexCheckBox.Checked)
            {
                if (!ttclient.InitSoundDuplexDevices(settings.sndinputid, settings.sndoutputid))
                    MessageBox.Show("Failed to init sound devices");

                SpeexDSP spxdsp = new SpeexDSP(false);
                ttclient.GetSoundInputPreprocess(ref spxdsp);
                spxdsp.nEchoSuppress = SpeexDSPConstants.DEFAULT_ECHO_SUPPRESS;
                spxdsp.nEchoSuppressActive = SpeexDSPConstants.DEFAULT_ECHO_SUPPRESS_ACTIVE;
                spxdsp.bEnableEchoCancellation = echocancelCheckBox.Checked;
                ttclient.SetSoundInputPreprocess(spxdsp);
            }
            else
            {
                if (!ttclient.InitSoundInputDevice(settings.sndinputid))
                    MessageBox.Show("Failed to init sound input device");

                if (!ttclient.InitSoundOutputDevice(settings.sndoutputid))
                    MessageBox.Show("Failed to init sound output device");
            }

            if (wasapiRadioButton.Checked)
                settings.soundsystem = SoundSystem.SOUNDSYSTEM_WASAPI;
            else if (dsoundRadioButton.Checked)
                settings.soundsystem = SoundSystem.SOUNDSYSTEM_DSOUND;
            else if (winmmRadioButton.Checked)
                settings.soundsystem = SoundSystem.SOUNDSYSTEM_WINMM;


            // OK INHERITED FROM CHANNEL

            //chan.audiocfg.bEnableAGC = agcCheckBox.Checked;
            //chan.audiocfg.nGainLevel = gainlevelTrackBar.Value;

            //ttclient.DoUpdateChannel(chan);
            
            //AutoMicroConf.voxtxCheckBoxAC = voxtxCheckBox.Checked;
            AutoMicroConf.duplexAC = duplexCheckBox.Checked;
            AutoMicroConf.echoAC = echocancelCheckBox.Checked;
            AutoMicroConf.outputVolume = volumeTrackBar.Value; 
        }
        

        //private void voiceactTrackBar_ValueChanged(object sender, EventArgs e)
        //{
        //    ttclient.SetVoiceActivationLevel(voiceactTrackBar.Value);
        //}

        public void disableAllGroups()
        {
            //groupBox1.Enabled = false;
            groupBox4.Enabled = false;
            groupBox5.Enabled = false;
        }

        public void refreshTt(TeamTalk ttclient)
        {
            this.ttclient = ttclient;
        }
    }
}
