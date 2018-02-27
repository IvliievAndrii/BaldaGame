namespace BaldaGameWFA
{
    partial class LevelForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LevelForm));
            this.easyLevelRadioButton = new System.Windows.Forms.RadioButton();
            this.middleLevelRadioButton = new System.Windows.Forms.RadioButton();
            this.hardLevelRadioButton = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // easyLevelRadioButton
            // 
            this.easyLevelRadioButton.AutoSize = true;
            this.easyLevelRadioButton.Location = new System.Drawing.Point(63, 54);
            this.easyLevelRadioButton.Name = "easyLevelRadioButton";
            this.easyLevelRadioButton.Size = new System.Drawing.Size(68, 17);
            this.easyLevelRadioButton.TabIndex = 0;
            this.easyLevelRadioButton.TabStop = true;
            this.easyLevelRadioButton.Text = "Новичок";
            this.easyLevelRadioButton.UseVisualStyleBackColor = true;
            this.easyLevelRadioButton.Click += new System.EventHandler(this.easyLevelRadioButton_Click);
            // 
            // middleLevelRadioButton
            // 
            this.middleLevelRadioButton.AutoSize = true;
            this.middleLevelRadioButton.Location = new System.Drawing.Point(63, 107);
            this.middleLevelRadioButton.Name = "middleLevelRadioButton";
            this.middleLevelRadioButton.Size = new System.Drawing.Size(72, 17);
            this.middleLevelRadioButton.TabIndex = 1;
            this.middleLevelRadioButton.TabStop = true;
            this.middleLevelRadioButton.Text = "Бывалый";
            this.middleLevelRadioButton.UseVisualStyleBackColor = true;
            this.middleLevelRadioButton.Click += new System.EventHandler(this.middleLevelRadioButton_Click);
            // 
            // hardLevelRadioButton
            // 
            this.hardLevelRadioButton.AutoSize = true;
            this.hardLevelRadioButton.Location = new System.Drawing.Point(63, 157);
            this.hardLevelRadioButton.Name = "hardLevelRadioButton";
            this.hardLevelRadioButton.Size = new System.Drawing.Size(63, 17);
            this.hardLevelRadioButton.TabIndex = 2;
            this.hardLevelRadioButton.TabStop = true;
            this.hardLevelRadioButton.Text = "Мастер";
            this.hardLevelRadioButton.UseVisualStyleBackColor = true;
            this.hardLevelRadioButton.Click += new System.EventHandler(this.hardLevelRadioButton_Click);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(79, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 23);
            this.label1.TabIndex = 3;
            this.label1.Text = "От 3 до 4 букв в слове";
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(79, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 23);
            this.label2.TabIndex = 4;
            this.label2.Text = "От 3 до 5 букв в слове";
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Location = new System.Drawing.Point(79, 177);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 23);
            this.label3.TabIndex = 5;
            this.label3.Text = "Любое количество букв в слове";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(263, 23);
            this.label4.TabIndex = 6;
            this.label4.Text = "Выберите уровень сложности для игры в \"Балду\":";
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(105, 218);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 7;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // LevelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 253);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.hardLevelRadioButton);
            this.Controls.Add(this.middleLevelRadioButton);
            this.Controls.Add(this.easyLevelRadioButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LevelForm";
            this.Text = "Уровень сложности";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton easyLevelRadioButton;
        private System.Windows.Forms.RadioButton middleLevelRadioButton;
        private System.Windows.Forms.RadioButton hardLevelRadioButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonOK;
    }
}