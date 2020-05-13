using ConsoleRedirection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xlocation
{
    public partial class clientUI : Form
    {
        public clientUI()
        {
            InitializeComponent();
            Console.SetOut(new TextBoxStreamWriter(TextBox1));
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void InputBox_TextChanged(object sender, EventArgs e)
        {


        }

        public void Send_Click(object sender, EventArgs e)
        {
            //TextBox1.Text = "aaa";
            


            String[] inputBox = new String[] { InputBox.Text };
            Location location = new Location();
            location.Run(inputBox);

        }

    }
}
