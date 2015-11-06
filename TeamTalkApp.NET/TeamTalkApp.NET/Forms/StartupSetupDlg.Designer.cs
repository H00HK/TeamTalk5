namespace TeamTalkApp.NET.Forms
{
    partial class StartupSetupDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.volumeTrackBar = new System.Windows.Forms.TrackBar();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.wasapiRadioButton = new System.Windows.Forms.RadioButton();
            this.echocancelCheckBox = new System.Windows.Forms.CheckBox();
            this.duplexCheckBox = new System.Windows.Forms.CheckBox();
            this.winmmRadioButton = new System.Windows.Forms.RadioButton();
            this.dsoundRadioButton = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.sndinputComboBox = new System.Windows.Forms.ComboBox();
            this.sndoutputComboBox = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.volumeTrackBar)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.volumeTrackBar);
            this.groupBox4.Location = new System.Drawing.Point(19, 228);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox4.Size = new System.Drawing.Size(445, 89);
            this.groupBox4.TabIndex = 18;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Ustawienie głośności w programie";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 27);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(115, 17);
            this.label5.TabIndex = 6;
            this.label5.Text = "Główna głośność";
            // 
            // volumeTrackBar
            // 
            this.volumeTrackBar.Location = new System.Drawing.Point(181, 25);
            this.volumeTrackBar.Margin = new System.Windows.Forms.Padding(4);
            this.volumeTrackBar.Name = "volumeTrackBar";
            this.volumeTrackBar.Size = new System.Drawing.Size(253, 56);
            this.volumeTrackBar.TabIndex = 2;
            this.volumeTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.volumeTrackBar.ValueChanged += new System.EventHandler(this.volumeTrackBar_ValueChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.wasapiRadioButton);
            this.groupBox5.Controls.Add(this.echocancelCheckBox);
            this.groupBox5.Controls.Add(this.duplexCheckBox);
            this.groupBox5.Controls.Add(this.winmmRadioButton);
            this.groupBox5.Controls.Add(this.dsoundRadioButton);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.sndinputComboBox);
            this.groupBox5.Controls.Add(this.sndoutputComboBox);
            this.groupBox5.Location = new System.Drawing.Point(19, 13);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox5.Size = new System.Drawing.Size(445, 208);
            this.groupBox5.TabIndex = 10;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Urządzenia dźwiękowe";
            // 
            // wasapiRadioButton
            // 
            this.wasapiRadioButton.AutoSize = true;
            this.wasapiRadioButton.Checked = true;
            this.wasapiRadioButton.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.wasapiRadioButton.Location = new System.Drawing.Point(23, 38);
            this.wasapiRadioButton.Margin = new System.Windows.Forms.Padding(4);
            this.wasapiRadioButton.Name = "wasapiRadioButton";
            this.wasapiRadioButton.Size = new System.Drawing.Size(81, 21);
            this.wasapiRadioButton.TabIndex = 0;
            this.wasapiRadioButton.TabStop = true;
            this.wasapiRadioButton.Text = "WASAPI";
            this.wasapiRadioButton.UseVisualStyleBackColor = true;
            this.wasapiRadioButton.Click += new System.EventHandler(this.UpdateSoundSystem);
            // 
            // echocancelCheckBox
            // 
            this.echocancelCheckBox.AutoSize = true;
            this.echocancelCheckBox.Enabled = false;
            this.echocancelCheckBox.Location = new System.Drawing.Point(23, 181);
            this.echocancelCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.echocancelCheckBox.Name = "echocancelCheckBox";
            this.echocancelCheckBox.Size = new System.Drawing.Size(331, 21);
            this.echocancelCheckBox.TabIndex = 8;
            this.echocancelCheckBox.Text = "Aktywuj usuwanie echa (usuń echo z głośników)";
            this.echocancelCheckBox.UseVisualStyleBackColor = true;
            // 
            // duplexCheckBox
            // 
            this.duplexCheckBox.AutoSize = true;
            this.duplexCheckBox.Location = new System.Drawing.Point(23, 153);
            this.duplexCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.duplexCheckBox.Name = "duplexCheckBox";
            this.duplexCheckBox.Size = new System.Drawing.Size(332, 21);
            this.duplexCheckBox.TabIndex = 7;
            this.duplexCheckBox.Text = "Aktywuj dupleks (wymagany dla usunięcia echa)";
            this.duplexCheckBox.UseVisualStyleBackColor = true;
            this.duplexCheckBox.CheckedChanged += new System.EventHandler(this.duplexCheckBox_CheckedChanged);
            // 
            // winmmRadioButton
            // 
            this.winmmRadioButton.AutoSize = true;
            this.winmmRadioButton.Location = new System.Drawing.Point(240, 38);
            this.winmmRadioButton.Margin = new System.Windows.Forms.Padding(4);
            this.winmmRadioButton.Name = "winmmRadioButton";
            this.winmmRadioButton.Size = new System.Drawing.Size(111, 21);
            this.winmmRadioButton.TabIndex = 2;
            this.winmmRadioButton.Text = "Windows MM";
            this.winmmRadioButton.UseVisualStyleBackColor = true;
            this.winmmRadioButton.Click += new System.EventHandler(this.UpdateSoundSystem);
            // 
            // dsoundRadioButton
            // 
            this.dsoundRadioButton.AutoSize = true;
            this.dsoundRadioButton.Location = new System.Drawing.Point(120, 38);
            this.dsoundRadioButton.Margin = new System.Windows.Forms.Padding(4);
            this.dsoundRadioButton.Name = "dsoundRadioButton";
            this.dsoundRadioButton.Size = new System.Drawing.Size(107, 21);
            this.dsoundRadioButton.TabIndex = 1;
            this.dsoundRadioButton.Text = "DirectSound";
            this.dsoundRadioButton.UseVisualStyleBackColor = true;
            this.dsoundRadioButton.ClientSizeChanged += new System.EventHandler(this.UpdateSoundSystem);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 76);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(129, 17);
            this.label6.TabIndex = 3;
            this.label6.Text = "Urządzenia wejścia";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 116);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(128, 17);
            this.label7.TabIndex = 5;
            this.label7.Text = "Urządzenia wyjścia";
            // 
            // sndinputComboBox
            // 
            this.sndinputComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sndinputComboBox.FormattingEnabled = true;
            this.sndinputComboBox.Location = new System.Drawing.Point(155, 73);
            this.sndinputComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.sndinputComboBox.Name = "sndinputComboBox";
            this.sndinputComboBox.Size = new System.Drawing.Size(237, 24);
            this.sndinputComboBox.TabIndex = 4;
            this.sndinputComboBox.SelectedIndexChanged += new System.EventHandler(this.UpdateSelectedSoundDevices);
            // 
            // sndoutputComboBox
            // 
            this.sndoutputComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sndoutputComboBox.FormattingEnabled = true;
            this.sndoutputComboBox.Location = new System.Drawing.Point(155, 113);
            this.sndoutputComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.sndoutputComboBox.Name = "sndoutputComboBox";
            this.sndoutputComboBox.Size = new System.Drawing.Size(237, 24);
            this.sndoutputComboBox.TabIndex = 6;
            this.sndoutputComboBox.SelectedIndexChanged += new System.EventHandler(this.UpdateSelectedSoundDevices);
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(364, 325);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 28);
            this.button1.TabIndex = 19;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // treeView1
            // 
            this.treeView1.LineColor = System.Drawing.Color.Empty;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(121, 97);
            this.treeView1.TabIndex = 0;
            // 
            // StartupSetupDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 376);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "StartupSetupDlg";
            this.Text = "Ręczne ustawienia głosowe";
            this.Load += new System.EventHandler(this.StartupSetupDlg_Load);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.volumeTrackBar)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TrackBar volumeTrackBar;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton wasapiRadioButton;
        private System.Windows.Forms.CheckBox echocancelCheckBox;
        private System.Windows.Forms.CheckBox duplexCheckBox;
        private System.Windows.Forms.RadioButton winmmRadioButton;
        private System.Windows.Forms.RadioButton dsoundRadioButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox sndinputComboBox;
        private System.Windows.Forms.ComboBox sndoutputComboBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TreeView treeView1;
    }
}