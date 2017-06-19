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

namespace Glouton.LogGenerator
{
    public partial class Form1 : Form
    {
        GrandOutput _g;
        ActivityMonitor Monitor { get; set; }


        public Form1(GrandOutput g)
        {
            InitializeComponent();
            _g = g;
            if (Monitor != null) return;
            Monitor = new ActivityMonitor(false);
            g.EnsureGrandOutputClient(Monitor);
        }

        private void buttonOpenGroup_Click(object sender, EventArgs e)
        {
            switch (listLogLevel.SelectedItem.ToString())
            {
                case "Trace":
                    Monitor.OpenTrace();
                    break;
                case "Info":
                    Monitor.OpenInfo();
                    break;
                case "Warn":
                    Monitor.OpenWarn();
                    break;
                case "Error":
                    Monitor.OpenError();
                    break;
                case "Fatal":
                    Monitor.OpenFatal();
                    break;
                case "Debug":
                    Monitor.OpenDebug();
                    break;
                default: throw new ArgumentOutOfRangeException();
            }

        }

        private void buttonCloseGroup_Click(object sender, EventArgs e)
        {
            Monitor.CloseGroup();
        }

        private void buttonSendLine_Click(object sender, EventArgs e)
        {
            switch(listLogLevel.SelectedItem.ToString())
            {
                case "Trace":
                    Monitor.Trace().Send(textBoxLine.Text);
                    break;
                case "Info":
                    Monitor.Info().Send(textBoxLine.Text);
                    break;
                case "Warn":
                    Monitor.Warn().Send(textBoxLine.Text);
                    break;
                case "Error":
                    Monitor.Error().Send(textBoxLine.Text);
                    break;
                case "Fatal":
                    Monitor.Fatal().Send(textBoxLine.Text);
                    break;
                case "Debug":
                    Monitor.Debug().Send(textBoxLine.Text);
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
