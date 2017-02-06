using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {



        public Form1()
        {
            InitializeComponent();
            
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void makeSmallBrealToolStripMenuItem_Click(object sender, EventArgs e)
        {
            make_short_break();
            //contextMenuStrip1.Show(Control.MousePosition, ToolStripDropDownDirection.AboveLeft);
            showPopup();
        }

        private void pomodoro_button_Click(object sender, EventArgs e)
        {
            make_pomodoro();
            //contextMenuStrip1.Show(Control.MousePosition, ToolStripDropDownDirection.AboveLeft);
            showPopup();
        }

        private void large_button_Click(object sender, EventArgs e)
        {
            make_large_break();
            //contextMenuStrip1.Show(Control.MousePosition, ToolStripDropDownDirection.AboveLeft);
            showPopup();
        }

        private void pause_play_button_Click(object sender, EventArgs e)
        {

        }

        const int STATE_OFF = 0;
        const int STATE_POMODORO = 1;
        const int STATE_LARGE = 2;
        const int STATE_SHORT = 3;

        const int LARGE_EVERY = 4;
        const int POPUP_SEC = 10;

        TimeSpan POMODORO_LENGTH = new TimeSpan(0, 25, 0);
        TimeSpan LARGE_LENGTH = new TimeSpan(0, 15, 0);
        TimeSpan SHORT_LENGTH = new TimeSpan(0, 5, 0);

        int state = STATE_OFF;
        DateTime current_interval_start;
        TimeSpan current_interval_length;
        DateTime minimize_at;
        int num_pomodoro = 0;

        TimeSpan total_pomodoro_time;

        private void timer1_Tick(object sender, EventArgs e)
        {
 
            switch (state) {
                case STATE_OFF:
                    break;
                case STATE_POMODORO:
                case STATE_LARGE:
                case STATE_SHORT:
                    if (elapsed() > current_interval_length)
                    {
                        next();
                    }
                    break;
            }

            current_label.Text = descr();
            label1.Text = descr();
            centerTextField();
            this.Text = descr();
            totalpomodoro_label.Text = "Pomodoro total: " + total_pomodoro_time.ToString(@"hh\:mm\:ss");
            BackColor = color();

            if (minimize_at < System.DateTime.Now)
            {
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
            }
        }

        private void centerTextField()
        {
            label1.Location = new Point((this.Width - label1.Width)/2,
                (this.Height - label1.Height)/2);
        }

        private String descr()
        {
            TimeSpan remaining = current_interval_length - elapsed();
            switch (state)
            {
                case STATE_OFF:
                    return "Off";
                case STATE_POMODORO:
                    return "Pomodoro: " + remaining.ToString(@"mm\:ss") + " left";
                case STATE_LARGE:
                    return "Break: " + remaining.ToString(@"mm\:ss") + " left";
                case STATE_SHORT:
                    return "Break: " + remaining.ToString(@"mm\:ss") + " left";
            }
            return "Err";
        }

        private Color color()
        {
            switch (state)
            {
                case STATE_OFF:
                    return Color.Gray;
                case STATE_POMODORO:
                    return Color.Red;
                case STATE_LARGE:
                    return Color.LawnGreen;
                case STATE_SHORT:
                    return Color.LawnGreen;
            }
            return Color.Beige;
        }

        private void next()
        {
            switch (state) {
                case STATE_OFF:
                    make_pomodoro();
                    break;
                case STATE_POMODORO:
                    if (num_pomodoro == 0)
                    {
                        make_large_break();
                    } else {
                        make_short_break();
                    }
                    break;
                case STATE_LARGE:
                    make_pomodoro();
                    break;
                case STATE_SHORT:
                    make_pomodoro();
                    break;
            }

            showPopup();
        }

        private void showPopup()
        {
            this.WindowState = FormWindowState.Normal;
            this.TopMost = true;
            minimize_at = System.DateTime.Now.AddSeconds(POPUP_SEC);
        }

        private TimeSpan elapsed()
        {
            return System.DateTime.Now - current_interval_start;
        }

        private void addTotalPomodoroTime()
        {
            if (state == STATE_POMODORO) {
                total_pomodoro_time += System.DateTime.Now - current_interval_start;
            }

        }

        private void make_pomodoro()
        {
            addTotalPomodoroTime();
            current_interval_start = System.DateTime.Now;
            num_pomodoro = (num_pomodoro + 1) % LARGE_EVERY;
            current_interval_length = POMODORO_LENGTH;
            state = STATE_POMODORO;
        }

        private void make_short_break()
        {
            addTotalPomodoroTime();
            current_interval_start = System.DateTime.Now;
            current_interval_length = SHORT_LENGTH;
            state = STATE_SHORT;
        }

        private void make_large_break()
        {
            addTotalPomodoroTime();
            current_interval_start = System.DateTime.Now;
            current_interval_length = LARGE_LENGTH;
            state = STATE_LARGE;
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) {
                contextMenuStrip1.Show(Control.MousePosition, ToolStripDropDownDirection.AboveLeft);
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
