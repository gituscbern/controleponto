namespace ControlePonto.ConsoleApp
{
    partial class main
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnAddPonto = new System.Windows.Forms.Button();
            this.btnEnviarPonto = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnAddPonto
            // 
            this.btnAddPonto.Location = new System.Drawing.Point(57, 12);
            this.btnAddPonto.Name = "btnAddPonto";
            this.btnAddPonto.Size = new System.Drawing.Size(212, 23);
            this.btnAddPonto.TabIndex = 0;
            this.btnAddPonto.Text = "Adicionar Ponto";
            this.btnAddPonto.UseVisualStyleBackColor = true;
            this.btnAddPonto.Click += new System.EventHandler(this.BtnAddPonto_Click);
            // 
            // btnEnviarPonto
            // 
            this.btnEnviarPonto.Enabled = false;
            this.btnEnviarPonto.Location = new System.Drawing.Point(57, 41);
            this.btnEnviarPonto.Name = "btnEnviarPonto";
            this.btnEnviarPonto.Size = new System.Drawing.Size(212, 23);
            this.btnEnviarPonto.TabIndex = 1;
            this.btnEnviarPonto.Text = "Enviar ponto";
            this.btnEnviarPonto.UseVisualStyleBackColor = true;
            this.btnEnviarPonto.Click += new System.EventHandler(this.BtnEnviarPonto_Click);
            // 
            // main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(322, 76);
            this.Controls.Add(this.btnEnviarPonto);
            this.Controls.Add(this.btnAddPonto);
            this.MaximizeBox = false;
            this.Name = "main";
            this.Text = "Controle de Ponto";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAddPonto;
        private System.Windows.Forms.Button btnEnviarPonto;
    }
}

