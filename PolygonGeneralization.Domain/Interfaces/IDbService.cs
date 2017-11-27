using System;
using System.Collections.Generic;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Domain.Interfaces
{
    public interface IDbService
    {
        void SaveMap(Map map);
        IEnumerable<Map> GetMaps();
        Map GetMap(Guid mapId);
    }
}