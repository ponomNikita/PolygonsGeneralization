using System;
using System.Collections.Generic;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Domain.Interfaces
{
    public interface IDbService
    {
        /// <summary>
        /// Сохранение карты
        /// </summary>
        void SaveMap(Map map);

        /// <summary>
        /// Получение карт
        /// </summary>
        /// <returns></returns>
        IEnumerable<Map> GetMaps();

        /// <summary>
        /// Получение экстримальных точек
        /// </summary>
        /// <param name="mapId">Идентификатор карты</param>
        /// <returns>Массив из 4х значений: minX, minY, maxX, maxY</returns>
        double[] GetExtrimalPoints(Guid mapId);

        Polygon[] GetPolygons(Guid mapId, Point leftDown, Point rightTop);
    }
}