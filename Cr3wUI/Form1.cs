using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cr3wUI
{
    public partial class Form1 : Form
    {
        private TextBox textBox1;
        private Button button1;
        private Button buttonStop; // Το νέο κουμπί STOP
        private ListBox listBox1;
        private Label label1;
        private ProgressBar progressBar1;

        // Αντικείμενο για την ακύρωση του σκαναρίσματος
        private CancellationTokenSource cts;

        public Form1()
        {
            InitializeComponent();
            CreateCustomUI();
        }

        private void CreateCustomUI()
        {
            // Ρυθμίσεις Παραθύρου (White Theme)
            this.Size = new Size(750, 650);
            this.Text = "hacker78010 cr3w - Ultimate Port Scanner Pro";
            this.BackColor = Color.FromArgb(245, 245, 245);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Label
            label1 = new Label();
            label1.Text = "Target IP Address:";
            label1.ForeColor = Color.Black;
            label1.Location = new Point(25, 25);
            label1.Size = new Size(180, 20);
            label1.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            this.Controls.Add(label1);

            // TextBox (IP)
            textBox1 = new TextBox();
            textBox1.Location = new Point(25, 50);
            textBox1.Size = new Size(340, 27);
            textBox1.BackColor = Color.White;
            textBox1.ForeColor = Color.Black;
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            textBox1.Font = new Font("Segoe UI", 11);
            textBox1.Text = "127.0.0.1";
            this.Controls.Add(textBox1);

            // Button START
            button1 = new Button();
            button1.Text = "START SCAN";
            button1.Location = new Point(380, 48);
            button1.Size = new Size(140, 30);
            button1.BackColor = Color.FromArgb(0, 120, 215); // Μπλε
            button1.ForeColor = Color.White;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            button1.FlatAppearance.BorderSize = 0;
            button1.Click += new EventHandler(button1_Click);
            this.Controls.Add(button1);

            // Button STOP (Αρχικά απενεργοποιημένο)
            buttonStop = new Button();
            buttonStop.Text = "STOP";
            buttonStop.Location = new Point(535, 48);
            buttonStop.Size = new Size(140, 30);
            buttonStop.BackColor = Color.FromArgb(232, 17, 35); // Κόκκινο
            buttonStop.ForeColor = Color.White;
            buttonStop.FlatStyle = FlatStyle.Flat;
            buttonStop.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            buttonStop.FlatAppearance.BorderSize = 0;
            buttonStop.Enabled = false;
            buttonStop.Click += new EventHandler(buttonStop_Click);
            this.Controls.Add(buttonStop);

            // ProgressBar
            progressBar1 = new ProgressBar();
            progressBar1.Location = new Point(25, 95);
            progressBar1.Size = new Size(650, 15);
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 65535;
            this.Controls.Add(progressBar1);

            // ListBox
            listBox1 = new ListBox();
            listBox1.Location = new Point(25, 125);
            listBox1.Size = new Size(650, 430);
            listBox1.BackColor = Color.White;
            listBox1.ForeColor = Color.FromArgb(30, 30, 30);
            listBox1.BorderStyle = BorderStyle.FixedSingle;
            listBox1.Font = new Font("Consolas", 10);
            this.Controls.Add(listBox1);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string targetIP = textBox1.Text.Trim();

            if (!IPAddress.TryParse(targetIP, out IPAddress ipAddress))
            {
                MessageBox.Show("Please enter a valid IP address!", "hacker78010 cr3w");
                return;
            }

            // Ενεργοποίηση/Απενεργοποίηση κατάλληλων κουμπιών
            button1.Enabled = false;
            button1.Text = "SCANNING...";
            buttonStop.Enabled = true;

            listBox1.Items.Clear();
            listBox1.Items.Add($"[!] Target IP: {targetIP}");
            listBox1.Items.Add("[!] Scanning 65535 ports... Press STOP to cancel.");
            listBox1.Items.Add("----------------------------------------------------------------------");
            progressBar1.Value = 0;

            // Αρχικοποίηση του token ακύρωσης
            cts = new CancellationTokenSource();

            // 150 παράλληλα tasks: Ιδανικό για ταχύτητα σε Local και Public IP χωρίς crash
            int maxParallelTasks = 150;
            List<Task> tasks = new List<Task>();

            try
            {
                await Task.Run(async () =>
                {
                    for (int port = 1; port <= 65535; port++)
                    {
                        // Έλεγχος αν ο χρήστης πάτησε STOP
                        if (cts.Token.IsCancellationRequested)
                            break;

                        int currentPort = port;
                        tasks.Add(ScanPortAsync(ipAddress, currentPort, cts.Token));

                        if (tasks.Count >= maxParallelTasks || port == 65535)
                        {
                            await Task.WhenAll(tasks);
                            tasks.Clear();

                            // Ενημέρωση μπάρας στο UI με ασφάλεια
                            this.Invoke((MethodInvoker)delegate {
                                progressBar1.Value = currentPort;
                            });
                        }
                    }
                }, cts.Token);

                if (cts.Token.IsCancellationRequested)
                {
                    listBox1.Items.Add("----------------------------------------------------------------------");
                    listBox1.Items.Add("[-] SCAN CANCELED BY USER.");
                }
                else
                {
                    progressBar1.Value = 65535;
                    listBox1.Items.Add("----------------------------------------------------------------------");
                    listBox1.Items.Add("[*] Full 65535 ports scan completed successfully.");
                }
            }
            catch (Exception ex)
            {
                listBox1.Items.Add($"[Error]: {ex.Message}");
            }
            finally
            {
                // Επαναφορά κουμπιών στην αρχική κατάσταση
                button1.Enabled = true;
                button1.Text = "START SCAN";
                buttonStop.Enabled = false;
                cts.Dispose();
            }
        }

        // Το event όταν πατάς το κόκκινο κουμπί STOP
        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (cts != null)
            {
                listBox1.Items.Add("[!] Stopping scan... canceling active connections...");
                cts.Cancel();
            }
        }

        private async Task ScanPortAsync(IPAddress ip, int port, CancellationToken token)
        {
            if (token.IsCancellationRequested) return;

            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                try
                {
                    var connectTask = socket.ConnectAsync(new IPEndPoint(ip, port));
                    var delayTask = Task.Delay(500, token); // Το timeout ακούει και αυτό το STOP

                    var completedTask = await Task.WhenAny(connectTask, delayTask);

                    if (completedTask == connectTask && socket.Connected && !token.IsCancellationRequested)
                    {
                        string serviceName = GetPortService(port);

                        this.Invoke((MethodInvoker)delegate {
                            listBox1.Items.Add($"[+] Port {port,-5} : OPEN  -> [{serviceName}]");
                            listBox1.SelectedIndex = listBox1.Items.Count - 1;
                        });
                    }
                }
                catch { }
            }
        }

        private string GetPortService(int port)
        {
            switch (port)
            {
                case 21: return "FTP";
                case 22: return "SSH (Remote Access)";
                case 23: return "Telnet";
                case 25: return "SMTP (Email)";
                case 53: return "DNS";
                case 80: return "HTTP (Web)";
                case 110: return "POP3";
                case 135: return "RPC";
                case 139: return "NetBIOS";
                case 143: return "IMAP";
                case 443: return "HTTPS (Secure Web)";
                case 445: return "SMB (Windows Share)";
                case 1433: return "MSSQL Database";
                case 3306: return "MySQL Database";
                case 3389: return "RDP (Remote Desktop)";
                case 8080: return "HTTP Proxy";
                default:
                    if (port >= 1 && port <= 1023) return "System Core Service";
                    if (port >= 1024 && port <= 49151) return "Registered App Service";
                    return "Dynamic/Private Port";
            }
        }
    }
}