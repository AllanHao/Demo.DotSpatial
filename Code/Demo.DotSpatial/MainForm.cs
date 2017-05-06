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
        DotSpatial.Plugins.SimpleMap.SimpleMapPlugin mapControl = null;
        private string shpRName = Application.StartupPath + @"\Data\F_1.shp";
        private string shpLName = Application.StartupPath + @"\Data\L.shp";
        private string shpPName = Application.StartupPath + @"\Data\P.shp";
        public MainForm()
        {
            InitializeComponent();
            if (DesignMode) return;
            Shell = this;
            this.appManager1.LoadExtensions();
            this.mapControl = appManager1.GetExtension("DotSpatial.Plugins.SimpleMap") as DotSpatial.Plugins.SimpleMap.SimpleMapPlugin;

            this.appManager1.Map.ActivateMapFunction(new DotSpatial.Controls.MapFunctionZoom(appManager1.Map));
            appManager1.Map.ActivateMapFunction(new DotSpatial.Controls.MapFunctionSelect(appManager1.Map));

            //new DotSpatial.Controls.DefaultMenuBars(appManager1).Initialize(appManager1.HeaderControl);
        }
        public void OutputLog(string msg)
        {
            if (this.IsHandleCreated && this.mapControl != null)
            {
                this.mapControl.MsgControl.Invoke((MethodInvoker)delegate ()
                {
                    lock (this.mapControl.MsgControl)
                    {
                        DateTime dtime = DateTime.Now;
                        msg = string.Format("[{0}] {1}\r\n", dtime, msg);
                        this.mapControl.MsgControl.AppendText(msg);
                        Util.MessageConsole.Write(msg);
                    }
                });
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //加载Demo数据
            this.appManager1.Map.AddLayer(shpRName);
            this.appManager1.Map.AddLayer(shpLName);
            this.appManager1.Map.AddLayer(shpPName);
            this.OutputLog("加载地图成功");
        }
    }
}
