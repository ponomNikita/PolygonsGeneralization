﻿using System;
using PolygonGeneralization.Domain.Models;
using PolygonGeneralization.WinForms.Models;

namespace PolygonGeneralization.WinForms.ScreenAdapter
{
    public class ScreenAdapter
    {
        private const double DEFAULT_SCALE = 1.1;
        private double _scale;
        private readonly double[] _extrimalValues;

        public ScreenAdapter(int mapWidth, int mapHeight, double[] extrimalValues, double scale = DEFAULT_SCALE)
            :this(mapWidth, mapHeight, scale)
        {
            if (extrimalValues == null || extrimalValues.Length != 4)
            {
                throw new ArgumentNullException(nameof(extrimalValues));
            }

            _extrimalValues = new double[extrimalValues.Length];
            extrimalValues.CopyTo(_extrimalValues, 0);

            Setup(extrimalValues);
        }

        public ScreenAdapter(int mapWidth, int mapHeight, Point[] points, double scale = DEFAULT_SCALE)
            : this(mapWidth, mapHeight, GetExtrimalValues(points), scale)
        { }
        
        public ScreenAdapter(int mapWidth, int mapHeight, double scale = DEFAULT_SCALE)
        {
            if (Math.Abs(scale) < Double.Epsilon)
            {
                throw new ArgumentNullException(nameof(scale));
            }

            MapWidth = mapWidth;
            MapHeight = mapHeight;
            _scale = scale;
        }

        public Point ZeroPoint { get; set; }
        public double Zoom { get; set; }

        public BBox Bbox { get; private set; }
        public int MapWidth { get; }
        public int MapHeight{ get; }

        /// <summary>
        /// Преобразование точки из декартовых координат в экранные
        /// </summary>
        /// <param name="point">Точка в декартовых координатах</param>
        /// <returns></returns>
        public Point2D ToPixel(Point point)
        {
            return new Point2D((int)((point.X - Bbox.LeftDown.X) / Zoom), (int)((point.Y - Bbox.LeftDown.Y) / Zoom));
        }

        public void Scroll(int scrollNumber)
        {
            _scale -= (double)scrollNumber/100;
            Setup(_extrimalValues);
        }

        /// <summary>
        /// Инициализация //TODO Добавить описание
        /// </summary>
        /// <param name="locations">Точки с географическими координатами(широта долгота в градусах)</param>
        /// <param name="extrimalValues">Массив экстримальных значений карты</param>
        private void Setup(double[] extrimalValues)
        {
            var minX = extrimalValues[0];
            var minY = extrimalValues[1];
            var maxX = extrimalValues[2];
            var maxY = extrimalValues[3];

            ZeroPoint = new Point(minX, minY);

            var factor = maxX - minX < maxY - minY ? (double)MapWidth / MapHeight : (double)MapHeight / MapWidth;

            double width;
            double height;

            if (maxX - minX < maxY - minY)
            {
                height = (maxY - minY) * _scale;
                width = height * factor;
            }
            else
            {
                width = (maxX - minX)*_scale;
                height = width * factor;
            }

            var widthDelta = (maxX - minX - width) / 2;
            var heightDelta = (maxY - minY - height) / 2;

            Bbox = new BBox(new Point(minX + widthDelta, minY + heightDelta),
                new Point(maxX - widthDelta, maxY - heightDelta));
            Zoom = width / MapWidth;
        }
        
        private static double[] GetExtrimalValues(Point[] points)
        {
            var minX = double.MaxValue;
            var minY = double.MaxValue;
            var maxX = double.MinValue;
            var maxY = double.MinValue;

            foreach (var point in points)
            {
                minX = point.X < minX ? point.X : minX;
                maxX = point.X > maxX ? point.X : maxX;

                minY = point.Y < minY ? point.Y : minY;
                maxY = point.Y > maxY ? point.Y : maxY;
            }

            return new[] {minX, minY, maxX, maxY};
        }
    }
}