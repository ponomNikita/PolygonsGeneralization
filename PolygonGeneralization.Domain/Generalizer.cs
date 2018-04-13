using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using PolygonGeneralization.Domain.Interfaces;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Domain
{
    public class Generalizer : IGeneralizer
    {
        private readonly IGeneralizePolygonStrategy _generalizationStrategy;

        public Generalizer(IGeneralizePolygonStrategy generalizationStrategy)
        {
            _generalizationStrategy = generalizationStrategy;
        }

        public async Task<List<Polygon>> Generalize(List<Polygon> polygons, double minDistance)
        {
            var clasters = new List<Claster>();

            while (polygons.Any())
            {
                var claster = new Claster();
                claster.Polygons.Add(polygons.Last());
                polygons.RemoveAt(polygons.Count - 1);

                while (FindNeighbor(polygons, claster, minDistance))
                {}
                
                clasters.Add(claster);
            }

            return await _generalizationStrategy.Generalize(clasters, minDistance);
        }

        private bool FindNeighbor(List<Polygon> polygons, Claster claster, double minSqrDistance)
        {
            foreach (var polygon in polygons)
            {
                foreach (var polygonInClaster in claster.Polygons)
                {
                    if (SqrDistance(polygonInClaster.MassCenter, polygon.MassCenter) <= minSqrDistance)
                    {
                        claster.Polygons.Add(polygon);
                        polygons.Remove(polygon);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Квадрат расстояния между точками
        /// </summary>
        private double SqrDistance(Point a, Point b)
        {
            return Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }
    }
}