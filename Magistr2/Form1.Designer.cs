namespace Magistr2
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.ImagePlace = new System.Windows.Forms.PictureBox();
            this.OpenImg = new System.Windows.Forms.Button();
            this.ResultRes = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.ImagePlace)).BeginInit();
            this.SuspendLayout();
            // 
            // ImagePlace
            // 
            this.ImagePlace.Location = new System.Drawing.Point(12, 12);
            this.ImagePlace.Name = "ImagePlace";
            this.ImagePlace.Size = new System.Drawing.Size(433, 426);
            this.ImagePlace.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ImagePlace.TabIndex = 0;
            this.ImagePlace.TabStop = false;
            // 
            // OpenImg
            // 
            this.OpenImg.Location = new System.Drawing.Point(451, 415);
            this.OpenImg.Name = "OpenImg";
            this.OpenImg.Size = new System.Drawing.Size(75, 23);
            this.OpenImg.TabIndex = 1;
            this.OpenImg.Text = "Открыть";
            this.OpenImg.UseVisualStyleBackColor = true;
            this.OpenImg.Click += new System.EventHandler(this.OpenImg_Click);
            // 
            // ResultRes
            // 
            this.ResultRes.Location = new System.Drawing.Point(452, 13);
            this.ResultRes.Name = "ResultRes";
            this.ResultRes.Size = new System.Drawing.Size(336, 396);
            this.ResultRes.TabIndex = 2;
            this.ResultRes.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ResultRes);
            this.Controls.Add(this.OpenImg);
            this.Controls.Add(this.ImagePlace);
            this.Name = "Form1";
            this.Text = "Текстурные признаки";
            ((System.ComponentModel.ISupportInitialize)(this.ImagePlace)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox ImagePlace;
        private System.Windows.Forms.Button OpenImg;
        private System.Windows.Forms.RichTextBox ResultRes;
    }
}

