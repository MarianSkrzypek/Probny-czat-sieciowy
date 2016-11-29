using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using Czatsieciowy;

namespace Czatsieciowy
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private Chat chat;
        private HttpChannel channel;
        private bool isConnected = false;

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (isConnected)
            {
                MessageBox.Show("Podlaczono", "Bd");
                return;
            }
            try
            {
                channel = new HttpChannel();
                ChannelServices.RegisterChannel(channel, false);
                RemotingConfiguration.RegisterWellKnownClientType(typeof(Chat), "http://localhost:25000/Chat");
                chat = new Chat();
                bool status = chat.AddUser(textBox2.Text.Trim());
                if (status == false)
                {
                    MessageBox.Show("Jest juz taki uzytkownik", "Bd");
                    ChannelServices.UnregisterChannel(channel);
                    timer1.Enabled = false;
                    isConnected = false;
                    button1.Enabled = true;
                    textBox2.ReadOnly = false;
                    return;
                }
                chat.AddMessage("Uzytkownik [" + textBox2.Text + "] dolaczyl do rozmowy\n");
                timer1.Enabled = true;
                isConnected = true;
                button1.Enabled = false;
                textBox2.ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Bd");
                textBox3.Text = "Nie udalo sie nawiazac polaczenia wlasnie tutaj";
                ChannelServices.UnregisterChannel(channel);
                timer1.Enabled = false;
                isConnected = false;
                button1.Enabled = true;
                textBox2.ReadOnly = false;
                channel = null;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (isConnected)
            {
                if (textBox2.Text != null && textBox2.Text.Trim() != null)
                {
                    chat.AddMessage("[" + textBox2.Text + "] " + textBox1.Text);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ArrayList users = chat.UsersList;
            listBox1.Items.Clear();
            foreach (string user in users)
            listBox1.Items.Add(user);
            textBox3.Clear();
            textBox3.Text = chat.Talk;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (isConnected)
            {
                chat.AddMessage("Uzytkownik [" + textBox2.Text + "] opucil rozmowe");
                chat.RemoveUser(textBox2.Text);
                listBox1.Items.Clear();
                timer1.Enabled = false;
                ChannelServices.UnregisterChannel(channel);
                isConnected = false;
                button1.Enabled = true;
                textBox2.ReadOnly = false;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}