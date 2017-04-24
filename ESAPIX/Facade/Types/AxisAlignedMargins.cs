using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Dynamic;
using X = ESAPIX.Facade.XContext;

namespace ESAPIX.Facade.Types
{
    public class AxisAlignedMargins
    {
        internal dynamic _client;
        public AxisAlignedMargins() { _client = new ExpandoObject(); }
        public AxisAlignedMargins(dynamic client) { _client = client; }
        public bool IsLive { get { return !DefaultHelper.IsDefault(_client); } }
        public ESAPIX.Facade.Types.StructureMarginGeometry Geometry { get { return (ESAPIX.Facade.Types.StructureMarginGeometry)_client.Geometry; } }
        public System.Double X1 { get { return _client.X1; } }
        public System.Double Y1 { get { return _client.Y1; } }
        public System.Double Z1 { get { return _client.Z1; } }
        public System.Double X2 { get { return _client.X2; } }
        public System.Double Y2 { get { return _client.Y2; } }
        public System.Double Z2 { get { return _client.Z2; } }
        public AxisAlignedMargins(ESAPIX.Facade.Types.StructureMarginGeometry geometry, System.Double x1, System.Double y1, System.Double z1, System.Double x2, System.Double y2, System.Double z2)
        {
            if (X.Instance.CurrentContext != null)
                X.Instance.CurrentContext.Thread.Invoke(() => { _client = VMSConstructor.ConstructAxisAlignedMargins(geometry, x1, y1, z1, x2, y2, z2); });
            else
            {
                _client = new ExpandoObject();
                _client.Geometry = geometry;
                _client.X1 = x1;
                _client.Y1 = y1;
                _client.Z1 = z1;
                _client.X2 = x2;
                _client.Y2 = y2;
                _client.Z2 = z2;
            }
        }
        public System.String ToString()
        {
            var local = this;
            var retVal = X.Instance.CurrentContext.GetValue((sc) => { return local._client.ToString(); });
            return retVal;

        }
    }
}