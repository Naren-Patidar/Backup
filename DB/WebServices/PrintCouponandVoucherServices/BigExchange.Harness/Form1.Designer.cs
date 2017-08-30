namespace BigExchange.Harness
{
    partial class Form1
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
            this.txtResult = new System.Windows.Forms.TextBox();
            this.lblResult = new System.Windows.Forms.Label();
            this.btnGetBalance = new System.Windows.Forms.Button();
            this.btnBalanceLocalRef = new System.Windows.Forms.Button();
            this.btnProducts = new System.Windows.Forms.Button();
            this.btnCategories = new System.Windows.Forms.Button();
            this.btnCatInc = new System.Windows.Forms.Button();
            this.btnPrintReason = new System.Windows.Forms.Button();
            this.btnValidateVchr = new System.Windows.Forms.Button();
            this.btnProcessBooking = new System.Windows.Forms.Button();
            this.btnBookingErr = new System.Windows.Forms.Button();
            this.btnSavetotraining = new System.Windows.Forms.Button();
            this.btnGetBookingsForCC = new System.Windows.Forms.Button();
            this.btnRePrintTokenScript = new System.Windows.Forms.Button();
            this.btnTillTokenScript = new System.Windows.Forms.Button();
            this.btnKioskURl = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnKioskPrintVoucher = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(46, 144);
            this.txtResult.Margin = new System.Windows.Forms.Padding(2);
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(134, 20);
            this.txtResult.TabIndex = 8;
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(0, 146);
            this.lblResult.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(37, 13);
            this.lblResult.TabIndex = 7;
            this.lblResult.Text = "Result";
            // 
            // btnGetBalance
            // 
            this.btnGetBalance.Location = new System.Drawing.Point(46, 99);
            this.btnGetBalance.Margin = new System.Windows.Forms.Padding(2);
            this.btnGetBalance.Name = "btnGetBalance";
            this.btnGetBalance.Size = new System.Drawing.Size(140, 19);
            this.btnGetBalance.TabIndex = 9;
            this.btnGetBalance.Text = "GetBalanceViaService";
            this.btnGetBalance.UseVisualStyleBackColor = true;
            this.btnGetBalance.Click += new System.EventHandler(this.btnGetBalance_Click);
            // 
            // btnBalanceLocalRef
            // 
            this.btnBalanceLocalRef.Location = new System.Drawing.Point(46, 64);
            this.btnBalanceLocalRef.Margin = new System.Windows.Forms.Padding(2);
            this.btnBalanceLocalRef.Name = "btnBalanceLocalRef";
            this.btnBalanceLocalRef.Size = new System.Drawing.Size(140, 19);
            this.btnBalanceLocalRef.TabIndex = 10;
            this.btnBalanceLocalRef.Text = "GetBalanceViaLocalRef";
            this.btnBalanceLocalRef.UseVisualStyleBackColor = true;
            this.btnBalanceLocalRef.Click += new System.EventHandler(this.btnBalanceLocalRef_Click);
            // 
            // btnProducts
            // 
            this.btnProducts.Location = new System.Drawing.Point(245, 49);
            this.btnProducts.Name = "btnProducts";
            this.btnProducts.Size = new System.Drawing.Size(75, 23);
            this.btnProducts.TabIndex = 11;
            this.btnProducts.Text = "Products";
            this.btnProducts.UseVisualStyleBackColor = true;
            this.btnProducts.Click += new System.EventHandler(this.btnProducts_Click);
            // 
            // btnCategories
            // 
            this.btnCategories.Location = new System.Drawing.Point(245, 94);
            this.btnCategories.Name = "btnCategories";
            this.btnCategories.Size = new System.Drawing.Size(75, 23);
            this.btnCategories.TabIndex = 12;
            this.btnCategories.Text = "Categories";
            this.btnCategories.UseVisualStyleBackColor = true;
            this.btnCategories.Click += new System.EventHandler(this.btnCategories_Click);
            // 
            // btnCatInc
            // 
            this.btnCatInc.Location = new System.Drawing.Point(245, 136);
            this.btnCatInc.Name = "btnCatInc";
            this.btnCatInc.Size = new System.Drawing.Size(106, 23);
            this.btnCatInc.TabIndex = 13;
            this.btnCatInc.Text = "CategoryIncludes";
            this.btnCatInc.UseVisualStyleBackColor = true;
            this.btnCatInc.Click += new System.EventHandler(this.btnCatInc_Click);
            // 
            // btnPrintReason
            // 
            this.btnPrintReason.Location = new System.Drawing.Point(245, 172);
            this.btnPrintReason.Name = "btnPrintReason";
            this.btnPrintReason.Size = new System.Drawing.Size(97, 23);
            this.btnPrintReason.TabIndex = 14;
            this.btnPrintReason.Text = "PrintReasons";
            this.btnPrintReason.UseVisualStyleBackColor = true;
            this.btnPrintReason.Click += new System.EventHandler(this.btnPrintReason_Click);
            // 
            // btnValidateVchr
            // 
            this.btnValidateVchr.Location = new System.Drawing.Point(245, 207);
            this.btnValidateVchr.Name = "btnValidateVchr";
            this.btnValidateVchr.Size = new System.Drawing.Size(125, 23);
            this.btnValidateVchr.TabIndex = 15;
            this.btnValidateVchr.Text = "ValidateVoucher";
            this.btnValidateVchr.UseVisualStyleBackColor = true;
            this.btnValidateVchr.Click += new System.EventHandler(this.btnValidateVchr_Click);
            // 
            // btnProcessBooking
            // 
            this.btnProcessBooking.Location = new System.Drawing.Point(245, 246);
            this.btnProcessBooking.Name = "btnProcessBooking";
            this.btnProcessBooking.Size = new System.Drawing.Size(108, 23);
            this.btnProcessBooking.TabIndex = 16;
            this.btnProcessBooking.Text = "Process Booking";
            this.btnProcessBooking.UseVisualStyleBackColor = true;
            this.btnProcessBooking.Click += new System.EventHandler(this.btnProcessBooking_Click);
            // 
            // btnBookingErr
            // 
            this.btnBookingErr.Location = new System.Drawing.Point(12, 172);
            this.btnBookingErr.Name = "btnBookingErr";
            this.btnBookingErr.Size = new System.Drawing.Size(128, 23);
            this.btnBookingErr.TabIndex = 17;
            this.btnBookingErr.Text = "Booking Error";
            this.btnBookingErr.UseVisualStyleBackColor = true;
            this.btnBookingErr.Click += new System.EventHandler(this.btnBookingErr_Click);
            // 
            // btnSavetotraining
            // 
            this.btnSavetotraining.Location = new System.Drawing.Point(12, 201);
            this.btnSavetotraining.Name = "btnSavetotraining";
            this.btnSavetotraining.Size = new System.Drawing.Size(128, 23);
            this.btnSavetotraining.TabIndex = 18;
            this.btnSavetotraining.Text = "Save to training";
            this.btnSavetotraining.UseVisualStyleBackColor = true;
            this.btnSavetotraining.Click += new System.EventHandler(this.btnSavetotraining_Click);
            // 
            // btnGetBookingsForCC
            // 
            this.btnGetBookingsForCC.Location = new System.Drawing.Point(13, 230);
            this.btnGetBookingsForCC.Name = "btnGetBookingsForCC";
            this.btnGetBookingsForCC.Size = new System.Drawing.Size(127, 20);
            this.btnGetBookingsForCC.TabIndex = 19;
            this.btnGetBookingsForCC.Text = "Get Bookings For CC";
            this.btnGetBookingsForCC.UseVisualStyleBackColor = true;
            this.btnGetBookingsForCC.Click += new System.EventHandler(this.btnGetBookingsForCC_Click);
            // 
            // btnRePrintTokenScript
            // 
            this.btnRePrintTokenScript.Location = new System.Drawing.Point(12, 256);
            this.btnRePrintTokenScript.Name = "btnRePrintTokenScript";
            this.btnRePrintTokenScript.Size = new System.Drawing.Size(128, 34);
            this.btnRePrintTokenScript.TabIndex = 20;
            this.btnRePrintTokenScript.Text = "Get Reprint Till TokenScript";
            this.btnRePrintTokenScript.UseVisualStyleBackColor = true;
            this.btnRePrintTokenScript.Click += new System.EventHandler(this.btnRePrintTokenScript_Click);
            // 
            // btnTillTokenScript
            // 
            this.btnTillTokenScript.Location = new System.Drawing.Point(13, 296);
            this.btnTillTokenScript.Name = "btnTillTokenScript";
            this.btnTillTokenScript.Size = new System.Drawing.Size(128, 34);
            this.btnTillTokenScript.TabIndex = 21;
            this.btnTillTokenScript.Text = "Get Till TokenScript";
            this.btnTillTokenScript.UseVisualStyleBackColor = true;
            this.btnTillTokenScript.Click += new System.EventHandler(this.btnTillTokenScript_Click);
            // 
            // btnKioskURl
            // 
            this.btnKioskURl.Location = new System.Drawing.Point(12, 336);
            this.btnKioskURl.Name = "btnKioskURl";
            this.btnKioskURl.Size = new System.Drawing.Size(129, 23);
            this.btnKioskURl.TabIndex = 22;
            this.btnKioskURl.Text = "Get Kiosk URL para";
            this.btnKioskURl.UseVisualStyleBackColor = true;
            this.btnKioskURl.Click += new System.EventHandler(this.btnKioskURl_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 23;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnKioskPrintVoucher);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(403, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(206, 240);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Print Vouchers At Kiosk";
            // 
            // btnKioskPrintVoucher
            // 
            this.btnKioskPrintVoucher.Location = new System.Drawing.Point(6, 48);
            this.btnKioskPrintVoucher.Name = "btnKioskPrintVoucher";
            this.btnKioskPrintVoucher.Size = new System.Drawing.Size(75, 23);
            this.btnKioskPrintVoucher.TabIndex = 24;
            this.btnKioskPrintVoucher.Text = "Get Kiosk IP";
            this.btnKioskPrintVoucher.UseVisualStyleBackColor = true;
            this.btnKioskPrintVoucher.Click += new System.EventHandler(this.btnKioskPrintVoucher_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(631, 410);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnKioskURl);
            this.Controls.Add(this.btnTillTokenScript);
            this.Controls.Add(this.btnRePrintTokenScript);
            this.Controls.Add(this.btnGetBookingsForCC);
            this.Controls.Add(this.btnSavetotraining);
            this.Controls.Add(this.btnBookingErr);
            this.Controls.Add(this.btnProcessBooking);
            this.Controls.Add(this.btnValidateVchr);
            this.Controls.Add(this.btnPrintReason);
            this.Controls.Add(this.btnCatInc);
            this.Controls.Add(this.btnCategories);
            this.Controls.Add(this.btnProducts);
            this.Controls.Add(this.btnBalanceLocalRef);
            this.Controls.Add(this.btnGetBalance);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.lblResult);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Button btnGetBalance;
        private System.Windows.Forms.Button btnBalanceLocalRef;
        private System.Windows.Forms.Button btnProducts;
        private System.Windows.Forms.Button btnCategories;
        private System.Windows.Forms.Button btnCatInc;
        private System.Windows.Forms.Button btnPrintReason;
        private System.Windows.Forms.Button btnValidateVchr;
        private System.Windows.Forms.Button btnProcessBooking;
        private System.Windows.Forms.Button btnBookingErr;
        private System.Windows.Forms.Button btnSavetotraining;
        private System.Windows.Forms.Button btnGetBookingsForCC;
        private System.Windows.Forms.Button btnRePrintTokenScript;
        private System.Windows.Forms.Button btnTillTokenScript;
        private System.Windows.Forms.Button btnKioskURl;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnKioskPrintVoucher;
    }
}

