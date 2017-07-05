using CK.Core;
using CK.Monitoring;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Glouton.CrashApplication
{
    public partial class Form1 : Form
    {
        GrandOutput _g;
        ActivityMonitor Monitor { get; set; }
        Random _random;
        int _timeCliked;

        public Form1(GrandOutput g)
        {
            _g = g;
            InitializeComponent();
            _random = new Random();
            _timeCliked = 0;
            Monitor = new ActivityMonitor(false);
            g.EnsureGrandOutputClient(Monitor);
            Monitor.MinimalFilter = LogFilter.Debug;
            Monitor.ShouldLogGroup(LogLevel.Trace);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void buttonLaunchDice_Click(object sender, EventArgs e)
        {
            Monitor.OpenTrace();
            Monitor.Trace().Send("User click");

            if (_timeCliked == 2)
            {
                try
                {
                    _random.Next(7, 1);
                } catch (Exception ex)
                {
                    Monitor.Fatal().Send(ex);
                    throw ex;
                }
            }

            int number = _random.Next(1, 7);
            Monitor.Info().Send($"Random number: {number}");

            labelResult.Text = number.ToString();
            labelResult.Refresh();

            _timeCliked++;
            Monitor.CloseGroup();
        }
    }
}
