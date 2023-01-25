using System;
using UnityEngine;
using UnityEngine.UI;

namespace LineUI
{
	[RequireComponent(typeof(CanvasRenderer))]
	[RequireComponent(typeof(RectTransform))]
	public class HoleUI : Graphic
	{
		[SerializeField] private Color holeOutColor;
		[SerializeField] private Vector2 center;
		[SerializeField] private float rectWidth = 100;
		[SerializeField] private float rectHeight = 100;
		[Min(0)][SerializeField] private float roundRadius = 10;
		[SerializeField] private float thiknesGradient = 10;
		[Range(0, 50)][SerializeField] private int roundDetails;
		private float roundOffset;
		
        public RectTransform RectTr { get => rectTransform; }

		public void SetHole(Vector2 center)
		{
			this.center = center;
			SetVerticesDirty();
		}

		public void SetHole(Vector2 center, float rectWidth, float rectHeight)
		{
			this.center = center;
			this.rectWidth = rectWidth;
			this.rectHeight = rectHeight;
			CheckFields();
			SetVerticesDirty();
		}

		public void SetHole(Vector2 center, float rectWidth, float rectHeight, float roundRadius, float thiknesGradient)
		{
			this.center = center;
			this.rectWidth = rectWidth;
			this.rectHeight = rectHeight;
			this.roundRadius = roundRadius;
			this.thiknesGradient = thiknesGradient;
			CheckFields();
			SetVerticesDirty();
		}
		public void SetHole(Vector2 center, float rectWidth, float rectHeight, float roundRadius, float thiknesGradient, int roundDetails)
		{
			this.center = center;
			this.rectWidth = rectWidth;
			this.rectHeight = rectHeight;
			this.roundRadius = roundRadius;
			this.thiknesGradient = thiknesGradient;
			this.roundDetails = roundDetails;
			CheckFields();
			SetVerticesDirty();
		}

		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
			DrawHole(vh);
		}

		private void DrawHole(VertexHelper vertexHelper)
		{

			float width = RectTr.rect.width;
			float height = RectTr.rect.height;

			roundOffset = roundRadius;

			SetConerVertex(vertexHelper, (centerX, offsetX) => centerX - offsetX, (centerY, offsetY) => centerY - offsetY, Vector2.down, new Vector2(0, 0));
			SetConerVertex(vertexHelper, (centerX, offsetX) => centerX - offsetX, (centerY, offsetY) => centerY + offsetY, Vector2.left, new Vector2(0, height));
			SetConerVertex(vertexHelper, (centerX, offsetX) => centerX + offsetX, (centerY, offsetY) => centerY + offsetY, Vector2.up, new Vector2(width, height));
			SetConerVertex(vertexHelper, (centerX, offsetX) => centerX + offsetX, (centerY, offsetY) => centerY - offsetY, Vector2.right, new Vector2(width, 0));
			SetTriangles(vertexHelper);
			SetTrianglesLastFirstVertex(vertexHelper);
		}

		private void SetConerVertex(VertexHelper vh,
		Func<float, float, float> setOffsetX,
		Func<float, float, float> setOffsetY,
		Vector3 direction, Vector3 conerBorderPos)
		{
			UIVertex vert = UIVertex.simpleVert;
			float angle = -45;
			if (roundDetails > 0)
			{
				angle = -90f / roundDetails;
			}
			else
			{
				direction = Vector2.zero;
			}
			for (int i = 0; i <= roundDetails; i++)
			{
				vert.color = color;
				Vector2 dir = Quaternion.Euler(0, 0, angle * i) * direction;
				Vector2 dif = dir * roundOffset;

				vert.position = conerBorderPos;
				vh.AddVert(vert);

				float x = setOffsetX(center.x, rectWidth / 2) + dif.x;
				float y = setOffsetY(center.y, rectHeight / 2) + dif.y;
				float xOffsetIn = -roundOffset * setOffsetX(0, 1);
				float yOffsetIn = -roundOffset * setOffsetY(0, 1);
				vert.position = new Vector2(x, y) + new Vector2(xOffsetIn, yOffsetIn);
				vh.AddVert(vert);

				vert.color = holeOutColor;
				float xOffsetOut = -(thiknesGradient + roundOffset) * setOffsetX(0, 1);
				float yOffsetOut = -(thiknesGradient + roundOffset) * setOffsetY(0, 1);
				vert.position = new Vector2(x, y) + new Vector2(xOffsetOut, yOffsetOut);
				vh.AddVert(vert);
			}
		}

		private void SetTriangles(VertexHelper vh)
		{
			for (int i = 0; i < vh.currentVertCount - 3; i += 3)
			{
				int index = i;
				vh.AddTriangle(index, index + 3, index + 4);
				vh.AddTriangle(index + 4, index + 1, index);
				vh.AddTriangle(index + 1, index + 4, index + 5);
				vh.AddTriangle(index + 5, index + 2, index + 1);
			}
		}

		private void SetTrianglesLastFirstVertex(VertexHelper vh)
		{
			int index2 = vh.currentVertCount - 3;
			vh.AddTriangle(index2, 0, 1);
			vh.AddTriangle(1, index2 + 1, index2);
			vh.AddTriangle(index2 + 1, 1, 2);
			vh.AddTriangle(2, index2 + 2, index2 + 1);
		}

		protected override void OnValidate()
		{
			base.OnValidate();
			CheckFields();
		}

		private void CheckFields()
		{
			if (roundDetails == 0)
			{
				roundRadius = 0;
			}
			float min = Mathf.Min(rectHeight, rectWidth);
			if (roundRadius * 2 > min)
			{
				roundRadius = min / 2;
			}
			if ((roundRadius + thiknesGradient) * 2 > min)
			{
				thiknesGradient = min / 2 - roundRadius;
			}
		}
	}
}
