//Code from: https://github.com/IkeThermite/GameDevGuide-CustomTabsAndFlexibleGrid/blob/master/Custom%20Tabs%20and%20Flexible%20Grid/Assets/Scripts/FlexibleGridLayout.cs
//Based on: https://www.youtube.com/watch?v=CGsEJToeXmA

/* FlexibleGridLayout.cs
 * From: Game Dev Guide - Fixing Grid Layouts in Unity With a Flexible Grid Component
 * Created: June 2020, NowWeWake
 */

using UnityEngine;
using UnityEngine.UI;

namespace LoopingBee.Shared.Utils
{
    public class FlexibleLayoutGroup : LayoutGroup
    {
        public enum FitType
        {
            Grid,
            Horizontal,
            Vertical,
            GridFixedRows,
            GridFixedColumns,
            GridFixedAll,
        }

        [Header("Flexible Grid")]
        public FitType fitType = FitType.Grid;

        public int rows;
        public int columns;
        public Vector2 cellSize;

        public bool relativeSpacing;
        public Vector2 spacing;

        public bool fitX;
        public bool expandX;

        public bool fitY;
        public bool expandY;

        public bool keepCellAspectRatio;
        public float aspectRatio;

        public bool reverseArrangement;

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();

            if (rows < 0)
                rows = 1;

            if (columns < 0)
                columns = 1;

            switch (fitType)
            {
                case FitType.Horizontal:
                    fitX = true;
                    columns = rectChildren.Count;
                    rows = 1;
                    expandY = false;
                    break;
                case FitType.Vertical:
                    fitY = true;
                    columns = 1;
                    expandX = false;
                    rows = rectChildren.Count;
                    break;
                case FitType.Grid:
                    fitX = fitY = true;
                    keepCellAspectRatio = false;
                    var squareRoot = Mathf.Sqrt(rectChildren.Count);
                    rows = columns = Mathf.CeilToInt(squareRoot);
                    break;
                case FitType.GridFixedRows:
                    columns = Mathf.CeilToInt(rectChildren.Count / (float)rows);
                    break;
                case FitType.GridFixedColumns:
                    rows = Mathf.CeilToInt(rectChildren.Count / (float)columns);
                    break;
                case FitType.GridFixedAll:
                    fitX = fitY = true;
                    keepCellAspectRatio = false;
                    break;
            }

            if (!fitX)
                expandX = false;

            if (!fitY)
                expandY = false;

            var parentWidth = rectTransform.rect.width - padding.horizontal;
            var parentHeight = rectTransform.rect.height - padding.vertical;

            var calculatedSpacing = relativeSpacing ? spacing * new Vector2(parentWidth, parentHeight) : spacing;

            var cellWidth = (parentWidth / columns) - (calculatedSpacing.x / columns * (columns - 1));
            var cellHeight = (parentHeight / rows) - (calculatedSpacing.y / rows * (rows - 1));

            cellSize.x = fitX ? cellWidth : cellSize.x;
            cellSize.y = fitY ? cellHeight : cellSize.y;

            var preferredCellHeight = cellSize.x / aspectRatio;
            var preferredCellWidth = cellSize.y * aspectRatio;

            if (keepCellAspectRatio)
            {
                switch (fitType)
                {
                    case FitType.Horizontal:
                    case FitType.GridFixedColumns:
                        {
                            var idealHeight = cellSize.x / aspectRatio;
                            if (fitY && idealHeight > cellHeight)
                            {
                                idealHeight = cellHeight;

                                cellSize.x = aspectRatio * idealHeight;
                            }

                            cellSize.y = idealHeight;
                            break;
                        }
                    case FitType.Vertical:
                    case FitType.GridFixedRows:
                        {
                            var idealWidth = cellSize.y * aspectRatio;
                            if (fitX && idealWidth > cellWidth)
                            {
                                idealWidth = cellWidth;

                                cellSize.y = idealWidth / aspectRatio;
                            }

                            cellSize.x = idealWidth;
                            break;
                        }
                }
            }

            var xAlignment = GetAlignmentOnAxis(0);
            var yAlignment = GetAlignmentOnAxis(1);

            if (!keepCellAspectRatio)
                aspectRatio = cellSize.x / cellSize.y;

            var startIndex = reverseArrangement ? rectChildren.Count - 1 : 0;
            var endIndex = reverseArrangement ? 0 : rectChildren.Count;
            var increment = reverseArrangement ? -1 : 1;

            var cellIndex = 0;
            for (var i = startIndex; reverseArrangement ? i >= endIndex : i < endIndex; i += increment)
            {
                var localColumns = columns;
                var localRows = rows;

                var columnCount = cellIndex % columns;
                var rowCount = cellIndex / columns;

                var item = rectChildren[i];

                if (fitType == FitType.GridFixedRows && rectChildren.Count / (float)rows % 1 != 0 && cellIndex >= columns)
                {
                    localColumns = rectChildren.Count - columns;
                    columnCount = (cellIndex - 1) % localColumns;
                    rowCount = cellIndex / columns;
                }

                if (fitType == FitType.GridFixedColumns && rectChildren.Count / (float)columns % 1 != 0 && (cellIndex % columns) != 0)
                {
                    localRows = rectChildren.Count - rows;
                    columnCount = cellIndex % localColumns;
                    rowCount = cellIndex / columns;
                }

                if (expandX && fitX && localColumns > 1)
                {
                    var extraWidth = parentWidth - (cellSize.x * localColumns);
                    calculatedSpacing.x = extraWidth / (localColumns - 1);
                }

                if (expandY && fitY && localRows > 1)
                {
                    var extraHeight = parentHeight - (cellSize.y * localRows);
                    calculatedSpacing.y = extraHeight / (localRows - 1);
                }

                var xOffset = (cellSize.x + calculatedSpacing.x) * (localColumns - 1) * xAlignment;
                var yOffset = (cellSize.y + calculatedSpacing.y) * (localRows - 1) * yAlignment;

                var xOrigin = (xAlignment * parentWidth) - xOffset + padding.left;
                var yOrigin = (yAlignment * parentHeight) - yOffset + padding.top;

                var xPos = xOrigin + ((cellSize.x + calculatedSpacing.x) * columnCount) - (cellSize.x * xAlignment);
                var yPos = yOrigin + ((cellSize.y + calculatedSpacing.y) * rowCount) - (cellSize.y * yAlignment);

                SetChildAlongAxis(item, 0, xPos, cellSize.x);
                SetChildAlongAxis(item, 1, yPos, cellSize.y);

                cellIndex++;
            }

            var minWidth = (cellSize.x * columns) + (calculatedSpacing.x * (columns - 1)) + padding.horizontal;
            var preferredWidth = (preferredCellWidth * columns) + (calculatedSpacing.x * (columns - 1)) + padding.horizontal;
            SetLayoutInputForAxis(minWidth, preferredWidth, -1, 0);

            var minHeight = (cellSize.y * rows) + (calculatedSpacing.y * (rows - 1)) + padding.vertical;
            var preferredHeight = (preferredCellHeight * rows) + (calculatedSpacing.y * (rows - 1)) + padding.vertical;
            SetLayoutInputForAxis(minHeight, preferredHeight, -1, 1);
        }

        public override void CalculateLayoutInputVertical()
        {
        }

        public override void SetLayoutHorizontal()
        {
        }

        public override void SetLayoutVertical()
        {
        }
    }
}
