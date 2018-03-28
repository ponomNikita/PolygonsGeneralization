using System.Collections.Generic;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Domain.Interfaces
{
    public interface IGeneralizer
    {
        /// <summary>
        /// Генерализация полигонов
        /// </summary>
        /// <param name="polygons">Оригинальный массив полигонов</param>
        /// <param name="minDistance">Минимальное расстояние, при котором необходимо объединять полигоны</param>
        /// <returns>Генерализированный массив полигонов</returns>
        List<Polygon> Generalize(List<Polygon> polygons, double minDistance);
    }
}