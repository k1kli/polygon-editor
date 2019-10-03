using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Figures
{
    public class Polygon
    {
        LinkedList<PolyPoint> points;
        Color color;
        private LinkedListNode<PolyPoint> selectedPointNode;
        public PolyPoint? SelectedPoint => selectedPointNode?.Value;
        public (PolyPoint p1, PolyPoint p2)? SelectedEdge =>
            selectedPointNode == null ?
            ((PolyPoint p1, PolyPoint p2) ?) null :
            (selectedPointNode.Value, selectedPointNode.Next?.Value ?? points.First.Value);
        public Polygon(PolyPoint p1, PolyPoint p2, PolyPoint p3, Color color)
        {
            points = new LinkedList<PolyPoint>();
            points.AddLast(p1);
            points.AddLast(p2);
            points.AddLast(p3);
            this.color = color;
        }
        public void Draw(ArrayBitmap bitmap)
        {
            LinkedListNode<PolyPoint> node = points.First;
            while(node.Next != null)
            {
                DrawPoint(bitmap, node.Value);
                bitmap.DrawLine(node.Value, node.Next.Value, color);
                node = node.Next;
            }
            DrawPoint(bitmap, node.Value);
            bitmap.DrawLine(node.Value, points.First.Value, color);
        }

        const int pointSize = 10;
        const int pointSizeDiv2 = pointSize / 2;
        public void DrawPoint(ArrayBitmap bitmap, PolyPoint point)
        {
            int startX = point.X - pointSizeDiv2, endX = point.X + pointSizeDiv2;
            int startY = point.Y - pointSizeDiv2, endY = point.Y + pointSizeDiv2;
            for (int x = startX; x <= endX; x++)
                for (int y = startY; y <= endY; y++)
                {
                    bitmap.SetPixel(x, y, color);
                }
        }

        public int TrySelectPoint(int x, int y, int distanceLimit)
        {
            LinkedListNode<PolyPoint> res = null;
            LinkedListNode<PolyPoint> node = points.First ;
            int minDist = Int32.MaxValue;
            while(node != null)
            {
                int xDist = Math.Abs(node.Value.X - x);
                int yDist = Math.Abs(node.Value.Y - y);
                if ( xDist + yDist  < distanceLimit)
                {
                    if(res == null || xDist + yDist < minDist)
                    {
                        res = node;
                        minDist = xDist + yDist;
                    }
                }
                node = node.Next;
            }
            selectedPointNode = res;
            return selectedPointNode == null ? -1 : minDist;
        }

        public int TrySelectEdge(int x, int y, int distanceLimit)
        {
            LinkedListNode<PolyPoint> res = null;
            LinkedListNode<PolyPoint> node = points.First;
            int minDistDifference = Int32.MaxValue;
            int distDifference;
            while (node != null)
            {
                distDifference = DistDifference(x, y, node.Value, node.Next.Value);
                if (distDifference < distanceLimit)
                {
                    if (res == null || distDifference < minDistDifference)
                    {
                        res = node;
                        minDistDifference = distDifference;
                    }
                }
                node = node.Next;
            }
            distDifference = DistDifference(x, y, node.Value, points.First.Value);
            if (distDifference < distanceLimit)
            {
                if (res == null || distDifference < minDistDifference)
                {
                    res = node;
                    minDistDifference = distDifference;
                }
            }
            selectedPointNode = res;
            return selectedPointNode == null ? -1 : minDistDifference;
        }

        private int DistDifference(int x, int y, PolyPoint p1, PolyPoint p2)
        {
            int distSqr1 = (x - p1.X) * (x - p1.X) + (y - p1.Y) * (y - p1.Y);
            int distSqr2 = (x - p2.X) * (x - p2.X) + (y - p2.Y) * (y - p2.Y);
            int distSqrBetween = (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y);
            return distSqr1 + distSqr2 - distSqrBetween;
        }

        public void MoveSelectedPoint(int newX, int newY)
        {
            selectedPointNode.Value = (newX,newY);
        }
    }
}
