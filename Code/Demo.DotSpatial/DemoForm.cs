using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Demo.DotGIS.Util;
using DotSpatial.Plugins.ShapeEditor;
using DotSpatial.Symbology;
using DotSpatial.Data;

namespace Demo.DotGIS
{
    public partial class DemoForm : Form
    {
        public DemoForm()
        {
            InitializeComponent();
            this.FormClosing += MainForm_FormClosing;
            this._context = new ContextMenu();
            this._deleteFeature = new MenuItem("删除地物", DeleteFeature);
            this._context.MenuItems.Add(this._deleteFeature);
            this.initForm();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
        #region 声明变量
        private string shpRName = Application.StartupPath + @"\Data\F_1.shp";
        private string shpLName = Application.StartupPath + @"\Data\L.shp";
        private string shpPName = Application.StartupPath + @"\Data\P.shp";
        private DotSpatial.Controls.Map map;
        private DotSpatial.Controls.Legend legent;
     //   List<GeoAPI.Geometries.Coordinate> coorList = new List<GeoAPI.Geometries.Coordinate>();
        static DotSpatial.Plugins.ShapeEditor.AddShapeFunction _addShapeFunction = null;
        static object locker = new object();

        DotSpatial.Plugins.ShapeEditor.AddShapeFunction addShapeFunction
        {
            get
            {
                if (_addShapeFunction == null)
                {
                    lock (locker)
                    {
                        if (_addShapeFunction == null)
                        {
                            _addShapeFunction = new AddShapeFunction(this.map);
                            this.map.MapFunctions.Add(_addShapeFunction);
                        }
                    }
                }
                return _addShapeFunction;
            }
        }
        DotSpatial.Controls.MapPolygonLayer polygonLayer;
        DotSpatial.Controls.MapPointLayer pointLayer;
        DotSpatial.Controls.MapLineLayer lineLayer;

        DotSpatial.Controls.IMapLayer selectedLayer
        {
            get
            {
                DotSpatial.Controls.IMapLayer laySelected = null;
                DotSpatial.Symbology.FeatureLayer fea = null;
                //string layerSelected = "";

                foreach (DotSpatial.Controls.IMapLayer lay in this.map.GetFeatureLayers())
                {
                    fea = (FeatureLayer)lay;
                    ISelection sel = fea.Selection;
                    // F = sel.ToFeatureSet();
                    List<IFeature> ls1 = new List<IFeature>();

                    ls1 = sel.ToFeatureList();
                    if (ls1.Count > 0)
                    {
                        laySelected = lay;
                        break;
                    }
                }
                return laySelected;
            }
        }
        bool isDelete = false;
        #region 地图右键控件
        private ContextMenu _context;
        private MenuItem _deleteFeature;
        #endregion
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
            //DotSpatial.Controls.MapPolygonLayer polygonLayer = this.map.AddLayer(shpFileName) as DotSpatial.Controls.MapPolygonLayer;



        }


        private void Demo_Load(object sender, EventArgs e)
        {
            this.polygonLayer = map.AddLayer(shpRName) as DotSpatial.Controls.MapPolygonLayer;
            this.map.ZoomToMaxExtent();
            this.map.SizeChanged += map_SizeChanged;
            // this.map.MouseClick += map_MouseClick;
            if (polygonLayer != null)
            {
            }
            map.ActivateMapFunction(new DotSpatial.Controls.MapFunctionZoom(map));
            map.ActivateMapFunction(new DotSpatial.Controls.MapFunctionSelect(map));
            map.MouseMove += map_MouseMove;

            this.pointLayer = map.AddLayer(shpPName) as DotSpatial.Controls.MapPointLayer;
            if (this.pointLayer == null)
            {
                throw new Exception("加载点数据失败");
            }
            this.lineLayer = map.AddLayer(shpLName) as DotSpatial.Controls.MapLineLayer;
            if (this.lineLayer == null)
            {
                throw new Exception("加载线数据失败");
            }
            this.lineLayer.SelectionChanged += lineLayer_SelectionChanged;
            this.pointLayer.SelectionChanged += pointLayer_SelectionChanged;
            this.polygonLayer.SelectionChanged += polygonLayer_SelectionChanged;
            this.OutputLog("加载成功");
        }

        void polygonLayer_SelectionChanged(object sender, EventArgs e)
        {
            if (this.isDelete)
            {
                List<DotSpatial.Data.IFeature> feaList = this.polygonLayer.Selection.ToFeatureList();
                int count = feaList.Count;
                if (count > 0)
                {
                    if (MessageBox.Show("确认删除该面对象？", "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        for (int i = count - 1; i >= 0; i--)
                        {
                            this.polygonLayer.EditMode = true;
                         IFeature fea = this.polygonLayer.FeatureSet.Features.ToList().Find((feature) => { return feature.Fid == feaList[i].Fid && feature.FeatureType == feaList[i].FeatureType && feature.Geometry.Area == feaList[i].Geometry.Area; });
                            if (fea != null)
                            {
                                this.polygonLayer.FeatureSet.Features.Remove(feaList[i]);
                            }
                        }
                        this.polygonLayer.FeatureSet.Save();

                        this.map.Refresh();
                    }
                }
            }
        }

        void pointLayer_SelectionChanged(object sender, EventArgs e)
        {
            if (this.isDelete)
            {
                List<DotSpatial.Data.IFeature> feaList = this.pointLayer.Selection.ToFeatureList();
                int count = feaList.Count;
                if (count > 0)
                {
                    if (MessageBox.Show("确认删除该点对象？", "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        for (int i = count - 1; i >= 0; i--)
                        {
                            bool result = this.pointLayer.DataSet.Features.Remove(feaList[i]);
                        }
                        this.pointLayer.DataSet.Save();

                        this.map.Refresh();
                    }
                }
            }
        }

        void lineLayer_SelectionChanged(object sender, EventArgs e)
        {
            if (this.isDelete)
            {
                List<DotSpatial.Data.IFeature> feaList = lineLayer.Selection.ToFeatureList();
                int count = feaList.Count;
                if (count > 0)
                {
                    if (MessageBox.Show("确认删除该线对象？", "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        for (int i = count - 1; i >= 0; i--)
                        {
                            bool result = lineLayer.DataSet.Features.Remove(feaList[i]);
                        }
                        lineLayer.DataSet.Save();

                        this.map.Refresh();
                    }
                }
            }
        }
        void map_MouseMove(object sender, MouseEventArgs e)
        {
            DotSpatial.Controls.GeoMouseArgs args = new DotSpatial.Controls.GeoMouseArgs(e, this.map);
            //求X、Y轴坐标    
            string xpanel = String.Format("X: {0:0.00000}", args.GeographicLocation.X);
            string ypanel = String.Format("Y: {0:0.00000}", args.GeographicLocation.Y);
            this.lbPosition.Text = xpanel + " " + ypanel;
        }
        void map_SizeChanged(object sender, EventArgs e)
        {
            this.map.ZoomToMaxExtent();
        }

        private void DeleteFeature(object sender, EventArgs e)
        {

        }



        //private void btnDeleteFeature_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    if (isDelete)
        //    {
        //        isDelete = false;
        //        this.btnDeleteFeature.Down = false;
        //        this.btnDrawLine.Enabled = true;
        //        this.btnDrawPoint.Enabled = true;
        //        this.btnDrawRegion.Enabled = true;
        //    }
        //    else
        //    {
        //        isDelete = true;
        //        this.btnDeleteFeature.Down = true;
        //        this.btnDeleteFeature.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.Check;
        //        this.btnDrawLine.Enabled = false;
        //        this.btnDrawPoint.Enabled = false;
        //        this.btnDrawRegion.Enabled = false;
        //    }
        //}

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
            //选中点图层
            this.pointLayer.LockDispose();
            map.Layers.Remove(this.pointLayer);
            map.Layers.Add(this.pointLayer);
            map.Layers.SelectedLayer = this.pointLayer;
            this.pointLayer.UnlockDispose();
            this.addShapeFunction.FinishShape(sender, e);
            this.coorList.Clear();
            this.addShapeFunction.Deactivate();
            this.addShapeFunction.Layer = this.pointLayer;
            this.addShapeFunction.Activate();
        }
    }
}
