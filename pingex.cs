using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Drawing;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;


namespace wping11
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //tbload();
        }

        public Form1(string[] args)
        {
            this.args = args;
            InitializeComponent();
        }

        string[] s0 = { "line 0", "line 1", "line 2", "line SOD" };
        string[][] s2 = { };
        //{"one", "1" },
        //{"two", "2" },
        //{"three", "3" }
        //};
        List<Pitm> klst = null;
        public void tbload()
        {
            dataGridView1.ColumnCount = 3;
            dataGridView1.Columns[0].Name = "IP addr";
            dataGridView1.Columns[1].Name = "Description";
            dataGridView1.Columns[2].Name = "check";
            /*
            string[] row = new string[] { "132.64.227.11", "example server" };
            dataGridView1.Rows.Add(row);
            row = new string[] { "127.0.0.1", "local host", "A" };
            dataGridView1.Rows.Add(row);
            row = new string[] { "oracleapp", "windows server x", "B" };
            dataGridView1.Rows.Add(row);
            */
            //CSV2tb("c.txt");
            List<Pitm> ptab;
            ptab = CSV2tb(args[0]);

            foreach (var lin in ptab) {
                //dataGridView1.Rows.Add(lin);
                dataGridView1.Rows.Add(new string[] {lin.addr, lin.desc, lin.chk} );
                
            }
            dataGridView1.BorderStyle = BorderStyle.Fixed3D;
            klst = ptab;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.DefaultExt = "txt";
            //var opfdlg = openFileDialog1.ShowDialog();
            //var dlg1 = openFileDialog1.FileName;
            //Console.WriteLine("*selected file {0}", dlg1);

            //var opf = openFileDialog1.OpenFile();

            var r = new List<string>();
            /*
            Pitm p0 = new Pitm() {
                addr="127.0.0.1", desc="basic local host", chk="Y"
            };
            r.Add(p0.ToString());
            r.Add((new Pitm() { addr = "132.64.227.11", desc="a non win server", chk="X" }).ToString());
            */
            //var t = new string[] { "1", "2", "3" };
            List<string[]> q = new List<string[]>();
            q.Add(new string[] { "one", "1" });
            q.Add(new string[] { "two", "2" });
            q.Add(new string[] { "three", "3" });

            listBox1.DataSource = r;
            listBox1.BorderStyle = BorderStyle.FixedSingle;
            //dataGridView1.ColumnCount = 2;
            //dataGridView1.DataSource = r;
            tbload();
        }
        static string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        static byte[] buffer = Encoding.ASCII.GetBytes(data);
        private string[] args;

        //public static List<string[]> CSV2tb(string path)
        public static List<Pitm> CSV2tb(string path)
        {
            //List<string[]> res = new List<string[]>();
            List<Pitm> res = new List<Pitm>();
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();
            options.DontFragment = true;

            string[] lines = System.IO.File.ReadAllLines(path);
            foreach (string line in lines)
            {
                //Console.WriteLine();
                //Console.WriteLine("csvline:{0}", line);
                string[] columns = line.Split(',');
                if (columns.Length < 3)
                {
                    //string[] columns3 = (line + ",Z").Split(',');
                    res.Add(new Pitm() {addr=columns[0], desc=columns[1], chk="Z" });
                } else {
                    res.Add(new Pitm() {addr=columns[0], desc=columns[1] } );
                }

                foreach (string column in columns)
                {
                    //Console.Write("{0} ", column.Trim());
                }
                //Console.WriteLine();
                //ShowHostEntry(columns[0]);

                if (columns[0] == "") { continue; }
                //PingReply reply = pingSender.Send(columns[0], 120, buffer, options);
                //Console.WriteLine("Reply status: {0}", reply.Status);

            }
            Console.WriteLine("read " + res.Count + " lines in res");
            return res;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();
            options.DontFragment = true;
            PingReply reply;
            int i = 0;
            var svCurs = this.Cursor;
            this.Cursor = Cursors.WaitCursor;
            foreach (var itm in klst)
            {
                Console.WriteLine("*ck*"+itm.addr);
                reply = pingSender.Send(itm.addr, 200, buffer, options);
                itm.chk = reply.Status.ToString();
                Console.WriteLine("{0}: Reply status: {1}", itm.addr, reply.Status);
                i++;
            }
            this.Cursor = svCurs;
            //dataGridView1.DataSource = new List<Pitm>();
            int j = 0;
            //taGridView1.Rows.Remove(r0);
            foreach (var lin in klst)
            {
                DataGridViewRow rx = dataGridView1.Rows[0];
                dataGridView1.Rows.Remove(rx);
                j = dataGridView1.Rows.Add(new string[] { lin.addr, lin.desc, lin.chk });
                if (lin.chk != "Success")
                {
                    //dataGridView1.Rows[j].Cells[2].Style.BackColor = System.Drawing.Color.LightSalmon;
                    dataGridView1.Rows[j].DefaultCellStyle.BackColor = System.Drawing.Color.LightSalmon;
                }
                j++;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            button1.Text = "load";
            button3.Text = "select";
            var opd = openFileDialog1.ShowDialog();
            if(opd == DialogResult.OK)
            {
                var fname = openFileDialog1.FileName;
                var fonly = System.IO.Path.GetFileName(fname);
                button1.Text = "load " + fonly;
                args = new string[] { fname };
            } else {
                button3.Text = "select error:" + opd.ToString();
            }
        }
    }
}
