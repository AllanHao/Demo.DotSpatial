using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Composition;
using DotSpatial.Controls.Docking;

namespace Demo.DotGIS
{
    public partial class MainForm : Form
    {
        [Export("Shell", typeof(ContainerControl))]
        private static ContainerControl Shell;
        public MainForm()
        {
            InitializeComponent();
            if (DesignMode) return;
            Shell = this;
            appManager1.LoadExtensions();
            //this.appManager1.DockManager.Add(new DockablePanel("kMap", "Map", map1, DockStyle.Fill));


            //this.appManager1.Legend = this.map1.Legend;
            //  new DotSpatial.Controls.DefaultMenuBars(appManager1).Initialize(appManager1.HeaderControl);
            //new DotSpatial.Controls.DefaultMenuBars(appManager1).Initialize(appManager1.HeaderControl);
        }

    }
}
