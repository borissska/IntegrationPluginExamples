using Alarus;
using Alarus.GUI;
using System;
using System.Windows.Media;
using Alarus.RealTimeFrameProviding;
using FaceRecognition.Events;
using System.Windows;
using System.Collections.Generic;

namespace FaceSelectionExample
{
    // Наследование PluginVisualizer, который необходим для отрисовки на экране примитивов.
    /// <summary>
    /// Плагин для выделения лица на экране.
    /// </summary>
    internal class FaceSelection : PluginVisualiser
    {
        /// <summary>
        /// Визуальный объект для отрисовки векторной графики на экране.
        /// </summary>
        private DrawingVisual _drawingVisual = new DrawingVisual();
        /// <summary>
        /// Заливка графических объектов (null - без заливки).
        /// </summary>
        private readonly Brush _brush = null;
        /// <summary>
        /// Способ отрисовки контура фигуры (null - без контура).
        /// </summary>
        private readonly Pen _pen = new Pen(new SolidColorBrush(Colors.Red), 5.0);

        /// <summary>
        /// Наследование уникального идентификатора от базового класса.
        /// </summary>
        /// <param name="analystPluginId"> Уникальный идентификатор. </param>
        public FaceSelection(Guid analystPluginId) : base(analystPluginId)
        {
        }

        /// <summary>
        /// Инициализация визуализатора FaceSelection.
        /// </summary>
        /// <param name="channelId"> Идентификатор канала. </param>
        /// <param name="pluginClientToolSet"> Набор методов для определения параметров клиента. </param>
        /// <param name="visualiserSet"> Набор элементов для отрисовки. </param>
        public override void Initialize(Guid channelId, IPluginClientToolSet pluginClientToolSet, IPluginVisualizerSet visualiserSet)
        {
            base.Initialize(channelId, pluginClientToolSet, visualiserSet);
            DrawPanel.AddVisual(_drawingVisual);
        }

        /// <summary>
        /// Прием нового визуального объекта.
        /// </summary>
        public override DrawingVisual NullableDrawingVisual => _drawingVisual;

        /// <summary>
        /// Очистка всего, что нарисовано визуализатором.
        /// </summary>
        public override void Clear()
        {
        }

        /// <summary>
        /// Освобождение всех ресурсов. В перегружаемых метода в наследниках обязательно
        /// вызывать Release базового класса.
        /// </summary>
        public override void Release()
        {
            base.Release();
        }

        /// <summary>
        /// Прием и обработка события визуализатором.
        /// </summary>
        /// <param name="channelId"> Идентификатор канала. </param>
        /// <param name="chooseEvent"> Приходящее событие. </param>
        /// <param name="isAlarm"> Является ли событие тревожным. </param>
        public override void ProcessEvent(Guid channelId, RawEvent chooseEvent, bool isAlarm)
        {
            if (chooseEvent is FaceDetectedNotifyEvent faceDetectedEvent)
            {
                DrawFaceRects(faceDetectedEvent.FaceWidth, faceDetectedEvent.FaceHeight, faceDetectedEvent.FaceTop, faceDetectedEvent.FaceLeft, _drawingVisual);
            }
            else if (chooseEvent is FaceRectsUpdatedEvent faceRectUpdatedEvent)
            {
                DrawFaceRects(faceRectUpdatedEvent.FaceRects, _drawingVisual);
            }
        }

        /// <summary>
        /// Отрисовка прямоугольника в визуальном объекте.
        /// </summary>
        /// <param name="faceRects"> Коллекция прямоугольников для отрисовки. </param>
        private void DrawFaceRects(IEnumerable<Rect> faceRects, DrawingVisual drawingVisual)
        {
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            {
                double panelWidth = DrawPanel.PanelWidth;
                double panelHeight = DrawPanel.PanelHeight;
                foreach (Rect faceRect in faceRects)
                {

                    Rect rect = new Rect
                    {
                        X = faceRect.X * panelWidth,
                        Y = faceRect.Y * panelHeight,
                        Width = faceRect.Width * panelWidth,
                        Height = faceRect.Height * panelHeight
                    };

                    drawingContext.DrawRectangle(_brush, _pen, rect);
                }
            }
            drawingContext.Close();
        }

        /// <summary>
        /// Отрисовка прямоугольника в визуальном объекте.
        /// </summary>
        /// <param name="width"> Ширина прямоугольника (в % от ширины панели). </param>
        /// <param name="height"> Высота прямоугольника (в % от высоты панели). </param>
        /// <param name="top"> Координаты верхней стороны прямоугольника (в % от выстоы панели). </param>
        /// <param name="left"> Координаты левой стороны прямоугольника (в % от ширины панели). </param>
        private void DrawFaceRects(double width, double height, double top, double left, DrawingVisual drawingVisual)
        {
            double panelWidth = DrawPanel.PanelWidth;
            double panelHeight = DrawPanel.PanelHeight;

            Point point = new Point(top * panelHeight, left * panelWidth);

            Rect rect = new Rect
            {
                Width = width * panelWidth,
                Height = height * panelHeight,
                Location = point
            };

            DrawingContext drawingContext = drawingVisual.RenderOpen();
            drawingContext.DrawRectangle(_brush, _pen, rect);
            drawingContext.Close();
        }

        /// <summary>
        /// Отрисовка прямоугольника в визуальном объекте.
        /// </summary>
        /// <param name="width"> Ширина прямоугольника (в % от ширины панели). </param>
        /// <param name="height"> Высота прямоугольника (в % от высоты панели). </param>
        /// <param name="top"> Координаты верхней стороны прямоугольника (в % от выстоы панели). </param>
        /// <param name="left"> Координаты левой стороны прямоугольника (в % от ширины панели). </param>
        private void DrawFaceRects(Point point, Size size, DrawingVisual drawingVisual)
        {
            double panelWidth = DrawPanel.PanelWidth;
            double panelHeight = DrawPanel.PanelHeight;

            Point point1 = new Point(point.Y * panelHeight, point.X * panelWidth);

            Rect rect = new Rect
            {
                Width = size.Width * panelWidth,
                Height = size.Height * panelHeight,
                Location = point1
            };

            DrawingContext drawingContext = drawingVisual.RenderOpen();
            drawingContext.DrawRectangle(_brush, _pen, rect);
            drawingContext.Close();
        }
    }
}
