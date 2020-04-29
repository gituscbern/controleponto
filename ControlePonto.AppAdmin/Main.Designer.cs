namespace ControlePonto.AppAdmin
{
    partial class Main
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
            this.buttonEnviar = new System.Windows.Forms.Button();
            this.buttonBaixar = new System.Windows.Forms.Button();
            this.buttonCalculate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonEnviar
            // 
            this.buttonEnviar.Location = new System.Drawing.Point(67, 28);
            this.buttonEnviar.Name = "buttonEnviar";
            this.buttonEnviar.Size = new System.Drawing.Size(175, 23);
            this.buttonEnviar.TabIndex = 0;
            this.buttonEnviar.Text = "Enviar Planilhas";
            this.buttonEnviar.UseVisualStyleBackColor = true;
            this.buttonEnviar.Click += new System.EventHandler(this.ButtonEnviar_Click);
            // 
            // buttonBaixar
            // 
            this.buttonBaixar.Location = new System.Drawing.Point(67, 58);
            this.buttonBaixar.Name = "buttonBaixar";
            this.buttonBaixar.Size = new System.Drawing.Size(175, 23);
            this.buttonBaixar.TabIndex = 1;
            this.buttonBaixar.Text = "Baixar Planilhas";
            this.buttonBaixar.UseVisualStyleBackColor = true;
            // 
            // buttonCalculate
            // 
            this.buttonCalculate.Location = new System.Drawing.Point(67, 88);
            this.buttonCalculate.Name = "buttonCalculate";
            this.buttonCalculate.Size = new System.Drawing.Size(175, 23);
            this.buttonCalculate.TabIndex = 2;
            this.buttonCalculate.Text = "Extrair Cálculos";
            this.buttonCalculate.UseVisualStyleBackColor = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(309, 137);
            this.Controls.Add(this.buttonCalculate);
            this.Controls.Add(this.buttonBaixar);
            this.Controls.Add(this.buttonEnviar);
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "Controle de ponto - RH";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonEnviar;
        private System.Windows.Forms.Button buttonBaixar;
        private System.Windows.Forms.Button buttonCalculate;
    }
}

