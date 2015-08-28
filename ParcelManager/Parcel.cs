using System;

namespace ParcelManager
{
    /// <summary>
    /// Class to hold parcel attributes
    /// </summary>
    public class Parcel
    {
        private double _weight;
        private double _height;
        private double _width;
        private double _depth;

        public double Weight
        {
            get
            {
                return _weight;
            }

            set 
            {
                if (value <= 0)
                    throw new Exception("Invalid Value for Weight");
                else
                    _weight = value;
            }
        }

        public double Height
        {
            get
            {
                return _height;
            }

            set
            {
                if (value <= 0)
                    throw new Exception("Invalid Value for Height");
                else
                    _height = value;
            }
        }

        public double Width
        {
            get
            {
                return _width;
            }

            set
            {
                if (value <= 0)
                    throw new Exception("Invalid Value for Width");
                else
                    _width = value;
            }
        }

        public double Depth
        {
            get
            {
                return _depth;
            }

            set
            {
                if (value <= 0)
                    throw new Exception("Invalid Value for Depth");
                else
                    _depth = value;
            }
        }

        public double Volume
        {
            get
            {
                return Height * Width * Depth;
            }
        }

        public Parcel()
        { }

        public Parcel(double weight, double height, double width, double depth)
        {
            this.Weight = weight;
            this.Height = height;
            this.Width = width;
            this.Depth = depth;
        }

    }
}
