using System.Collections.Generic;
using PolygonGeneralization.Domain.Interfaces;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Infrastructure.Commands
{
    public class GeneralizePolygonsCommand : BaseCommand
    {
        private readonly IGeneralizer _generalizer;
        private readonly List<Polygon> _polygons;
        private readonly double _minDistance;

        public GeneralizePolygonsCommand(IGeneralizer generalizer,
            List<Polygon> polygons, 
            double minDistance)
        {
            _generalizer = generalizer;
            _polygons = polygons;
            _minDistance = minDistance;
        }

        public override string CommandName => "Generalization";
        
        public List<Polygon> Result;
        
        protected override void HandleImpl()
        {
            foreach (var polygon in _polygons)
            {
                polygon.CalculateMassCenter();
            }
            
            Result = _generalizer.Generalize(_polygons, _minDistance).Result;
        }
    }
}