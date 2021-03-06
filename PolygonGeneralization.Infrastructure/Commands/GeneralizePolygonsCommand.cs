﻿using System.Collections.Generic;
using System.Linq;
using PolygonGeneralization.Domain.Interfaces;
using PolygonGeneralization.Domain.LinearGeneralizer;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Infrastructure.Commands
{
    public class GeneralizePolygonsCommand : BaseCommand
    {
        private readonly IGeneralizer _generalizer;
        private List<Polygon> _polygons;
        private readonly double _minDistance;
        private readonly ILinearGeneralizer _linearGeneralizer;

        public GeneralizePolygonsCommand(IGeneralizer generalizer,
            List<Polygon> polygons, ILinearGeneralizer linearGeneralizer,
            double minDistance)
        {
            _generalizer = generalizer;
            _polygons = polygons;
            _minDistance = minDistance;
            _linearGeneralizer = linearGeneralizer;
        }

        public override string CommandName => "Generalization";
        
        public List<Polygon> Result;
        
        protected override void HandleImpl()
        {
            // removing invalid polygons
            _polygons = _polygons.Where(it => it.Paths.All(p => p.Points.Count > 1)).ToList();
            
            foreach (var polygon in _polygons)
            {
                polygon.Paths[0].Points = _linearGeneralizer.Simplify(polygon.Paths[0].Points.ToArray()).ToList();
                polygon.CalculateMassCenter();
                polygon.CalculateDiameter();
            }
            
            Result = _generalizer.Generalize(_polygons, _minDistance).Result;
            
            foreach (var polygon in Result)
            {
                polygon.Paths[0].Points = _linearGeneralizer.Simplify(polygon.Paths[0].Points.ToArray()).ToList();
            }
        }
    }
}