using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Docking;
using DotSpatial.Plugins.SimpleMap.Properties;

namespace DotSpatial.Plugins.SimpleMap
{
    public class SimpleMapPlugin : Extension
    {
        private Map _map;
        public TextBox MsgControl;
        public SimpleMapPlugin()
        {
            DeactivationAllowed = false;
        }

        public override int Priority
        {
            get { return -10000; }
        }

        public override void Activate()
        {
            ShowMap();
            base.Activate();
        }

        public override void Deactivate()
        {
            App.HeaderControl.RemoveAll();
            App.DockManager.Remove("kMap");
            if (App.Map == _map) App.Map = null;
            base.Deactivate();
        }

        private void ShowMap()
        {
            _map = new Map { Text = "Map", Name = "map1", Legend = App.Legend };
            App.Map = _map;
            DotSpatial.Controls.SpatialDockManager _dock = new SpatialDockManager();
            _dock.Orientation = Orientation.Horizontal;
            _dock.Panel1.Controls.Add(_map);
            _map.Dock = DockStyle.Fill;

            //初始化日志输出
            GroupBox groupBox1 = new GroupBox();
            groupBox1.Text = "输出";
            System.Windows.Forms.TextBox tbMsg = new TextBox();
            groupBox1.Controls.Add(tbMsg);
            tbMsg.Dock = DockStyle.Fill;
            tbMsg.Multiline = true;
            tbMsg.ScrollBars = ScrollBars.Vertical;
            _dock.Panel2.Controls.Add(groupBox1);
            groupBox1.Dock = DockStyle.Fill;
            //设置上容器高度
            _dock.SplitterDistance = 370;
            this.MsgControl = tbMsg;
            App.DockManager.Add(new DockablePanel("kMap", Resource.Map, _dock, DockStyle.Fill));
        }
    }
}
