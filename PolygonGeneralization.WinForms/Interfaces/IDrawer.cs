﻿using System;
using PolygonGeneralization.WinForms.Models;

namespace PolygonGeneralization.WinForms.Interfaces
{
    public interface IDrawer : IDisposable
    {
        void DrawLine(Point2D a, Point2D b);
        void FillPolygon(Point2D[] points);
    }
}