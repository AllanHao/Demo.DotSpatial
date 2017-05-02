using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Demo.DotGIS.Util;

namespace Demo.DotGIS
{
    public partial class DemoForm : Form
    {
        public DemoForm()
        {
            InitializeComponent();
            this.initForm();
        }
        #region 声明变量
        private string shpFileName = Application.StartupPath + @"\Data\F_1.shp";
        private DotSpatial.Controls.Map map;
        private DotSpatial.Controls.Legend legent;
        List<GeoAPI.Geometries.Coordinate> coorList = new List<GeoAPI.Geometries.Coordinate>();
        #endregion

        private void initForm()
        {
            this.map = new DotSpatial.Controls.Map();
            this.splitContainer2.Panel1.Controls.Add(this.map);
            this.map.Dock = DockStyle.Fill;
            this.legent = new DotSpatial.Controls.Legend();
            this.splitContainer1.Panel1.Controls.Add(this.legent);
            this.legent.Dock = DockStyle.Fill;
            this.map.Legend = this.legent;
            DotSpatial.Controls.MapPolygonLayer polygonLayer = this.map.AddLayer(shpFileName) as DotSpatial.Controls.MapPolygonLayer;

            map.ActivateMapFunction(new DotSpatial.Controls.MapFunctionZoom(map));
            map.ActivateMapFunction(new DotSpatial.Controls.MapFunctionSelect(map));
            map.MouseClick += map_MouseClick;
        }

        void map_MouseClick(object sender, MouseEventArgs e)
        {
            GeoAPI.Geometries.Coordinate c = new GeoAPI.Geometries.Coordinate();

            System.Drawing.Point p = new System.Drawing.Point();
            p.X = e.X;
            p.Y = e.Y;
            c = map.PixelToProj(p);

            coorList.Add(c);

        }
        private void Demo_Load(object sender, EventArgs e)
        {
            this.OutputLog("加载成功");
        }
        public void OutputLog(string msg)
        {
            if (this.IsHandleCreated)
            {
                this.tbMsg.Invoke((MethodInvoker)delegate ()
                {
                    lock (this.tbMsg)
                    {
                        DateTime dtime = DateTime.Now;
                        msg = string.Format("[{0}] {1}\r\n", dtime, msg);
                        this.tbMsg.AppendText(msg);
                        MessageConsole.Write(msg);
                    }
                });
            }
        }

        private void btnDrawPoint_Click(object sender, EventArgs e)
        {
            this.map.
        }
    }
}
