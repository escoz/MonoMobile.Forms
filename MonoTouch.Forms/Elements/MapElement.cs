using MonoTouch.Dialog;
using MonoTouch.MapKit;
using MonoTouch.Foundation;
using MonoTouch.CoreLocation;
using MonoTouch.UIKit;
using System.Drawing;
using System;
using System.Linq;
namespace MonoTouch.Forms.Elements {
	
	public class MapElement : StringElement
	{
		private CLLocationCoordinate2D _location;
		public virtual CLLocationCoordinate2D Location {
			get {return _location;}
			set {
				_location = value;
			}
		}
		
	    public MapElement (string aCaption, string aValue,CLLocationCoordinate2D location ) : base (aCaption, aValue)
	    {
			_location = location;
	    }
		
	    public override UITableViewElementCell GetCell (UITableView tv)
	    {
	        var cell = (UITableViewElementCell) base.GetCell (tv);
	        cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
	        return cell;
	    }
	
		
	    public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
	    {
            var _mapViewController = new MapViewController(Location) { Title = Caption } ;
            dvc.ActivateController(_mapViewController, dvc);
	    }
	}
	
	class MapViewController : UIViewController
    {
		private MKMapView _mapView;
		public MKAnnotation _geocodeAnnotation;
		private CLLocationCoordinate2D _currLocation;
		
        public MapViewController (CLLocationCoordinate2D newLocation) {
			_mapView = _createMapView();
			_currLocation = newLocation;
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			View = _mapView;
		}
		
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			UpdateLocation(_currLocation, false);
		}
		
		public void UpdateLocation(CLLocationCoordinate2D newLocation, bool animated){
            var span = new MKCoordinateSpan(0.1,0.1);
			var region = new MKCoordinateRegion(newLocation, span);
			_mapView.SetRegion(region, animated);
			if (_geocodeAnnotation!=null)
				_mapView.RemoveAnnotation(_geocodeAnnotation);
			_geocodeAnnotation = new MapViewAnnotation(newLocation);
			_mapView.AddAnnotationObject(_geocodeAnnotation);
		}
		
		private MKMapView _createMapView()
	    {
	        var map = new MKMapView()
	        {
	            Delegate = new MapViewDelegate(),
	            ZoomEnabled = true,
	            ScrollEnabled = true,
	            ShowsUserLocation = true,
	
	            MapType = MonoTouch.MapKit.MKMapType.Standard,
	
	            UserInteractionEnabled = true,
	            MultipleTouchEnabled = true,
	
	            ClearsContextBeforeDrawing = true,
	            ClipsToBounds = true,
	            AutosizesSubviews = true,
	        };
	
	        return map;
	    }   

    }

    public class MapViewDelegate : MKMapViewDelegate
    {
        public override MKAnnotationView GetViewForAnnotation (MKMapView mapView, NSObject annotation)
        {
            var anv = mapView.DequeueReusableAnnotation("thislocation");
            if (anv == null)
            {
                var pinanv = new MKPinAnnotationView(annotation, "thislocation");
                pinanv.AnimatesDrop = true;
                pinanv.PinColor = MKPinAnnotationColor.Green;
                pinanv.CanShowCallout = false;
                anv = pinanv;
            }
            else
            {
                anv.Annotation = annotation;
            }
            return anv;
        }
    }

    public class MapViewAnnotation : MKAnnotation
    {
        private CLLocationCoordinate2D _coordinate;
        public override CLLocationCoordinate2D Coordinate {
            get {
                return _coordinate;
            }
			set {
				_coordinate = value;
			}
        }

        public MapViewAnnotation (CLLocationCoordinate2D coord) : base()
        {
            _coordinate=coord;
        }
    }
	
	
}