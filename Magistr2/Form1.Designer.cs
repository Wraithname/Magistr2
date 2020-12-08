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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.ImagePlace = new System.Windows.Forms.PictureBox();
            this.OpenImg = new System.Windows.Forms.Button();
            this.ResultRes = new System.Windows.Forms.RichTextBox();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.ImagePlace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // ImagePlace
            // 
            this.ImagePlace.Location = new System.Drawing.Point(12, 12);
            this.ImagePlace.Name = "ImagePlace";
            this.ImagePlace.Size = new System.Drawing.Size(257, 252);
            this.ImagePlace.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ImagePlace.TabIndex = 0;
            this.ImagePlace.TabStop = false;
            // 
            // OpenImg
            // 
            this.OpenImg.Location = new System.Drawing.Point(547, 12);
            this.OpenImg.Name = "OpenImg";
            this.OpenImg.Size = new System.Drawing.Size(75, 23);
            this.OpenImg.TabIndex = 1;
            this.OpenImg.Text = "Открыть";
            this.OpenImg.UseVisualStyleBackColor = true;
            this.OpenImg.Click += new System.EventHandler(this.OpenImg_Click);
            // 
            // ResultRes
            // 
            this.ResultRes.Location = new System.Drawing.Point(275, 12);
            this.ResultRes.Name = "ResultRes";
            this.ResultRes.Size = new System.Drawing.Size(266, 252);
            this.ResultRes.TabIndex = 2;
            this.ResultRes.Text = "";
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(12, 270);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(864, 407);
            this.chart1.TabIndex = 3;
            this.chart1.Text = "chart1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(888, 689);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.ResultRes);
            this.Controls.Add(this.OpenImg);
            this.Controls.Add(this.ImagePlace);
            this.Name = "Form1";
            this.Text = "Текстурные признаки";
            ((System.ComponentModel.ISupportInitialize)(this.ImagePlace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox ImagePlace;
        private System.Windows.Forms.Button OpenImg;
        private System.Windows.Forms.RichTextBox ResultRes;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
    }
}

