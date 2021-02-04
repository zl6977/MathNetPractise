
namespace mathnetplotform
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button_calc = new System.Windows.Forms.Button();
            this.formsPlot1 = new ScottPlot.FormsPlot();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button_default = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // button_calc
            // 
            this.button_calc.Location = new System.Drawing.Point(1035, 666);
            this.button_calc.Name = "button_calc";
            this.button_calc.Size = new System.Drawing.Size(112, 34);
            this.button_calc.TabIndex = 3;
            this.button_calc.Text = "Calculate";
            this.button_calc.UseVisualStyleBackColor = true;
            this.button_calc.Click += new System.EventHandler(this.button_calc_Click);
            // 
            // formsPlot1
            // 
            this.formsPlot1.Location = new System.Drawing.Point(15, 15);
            this.formsPlot1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(800, 685);
            this.formsPlot1.TabIndex = 4;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(813, 30);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 32;
            this.dataGridView1.Size = new System.Drawing.Size(356, 617);
            this.dataGridView1.TabIndex = 5;
            // 
            // button_default
            // 
            this.button_default.Location = new System.Drawing.Point(860, 666);
            this.button_default.Name = "button_default";
            this.button_default.Size = new System.Drawing.Size(112, 34);
            this.button_default.TabIndex = 6;
            this.button_default.Text = "Default";
            this.button_default.UseVisualStyleBackColor = true;
            this.button_default.Click += new System.EventHandler(this.button_default_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1181, 715);
            this.Controls.Add(this.button_default);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.formsPlot1);
            this.Controls.Add(this.button_calc);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_calc;
        private ScottPlot.FormsPlot formsPlot1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button_default;
    }
}

