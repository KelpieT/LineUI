using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LineUI
{
	[RequireComponent(typeof(CanvasRenderer))]
	[RequireComponent(typeof(RectTransform))]
	public class LineUI : Graphic
	{
		[SerializeField] private float thickness = 1f;
		[SerializeField] private int roundCount = 0;
		[SerializeField] private List<Vector2> screenPositions = new List<Vector2>();

		public RectTransform RectTr { get => rectTransform; }

		public void SetLine(List<Vector2> screenPositions)
		{
			this.screenPositions = screenPositions;
			SetVerticesDirty();
		}

		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
			DrawLine(vh);
		}

		private void DrawLine(VertexHelper vh)
		{
			UIVertex vert = UIVertex.simpleVert;
			vert.color = color;
			if (screenPositions.Count < 2)
				return;
			float lastAngle = 0;
			for (int i = 1; i < screenPositions.Count; i++)
			{
				Vector2 previousPos = screenPositions[i - 1];
				Vector2 currentPos = screenPositions[i];
				Vector2 dif = currentPos - previousPos;
				float angle = Vector2.SignedAngle(Vector2.right, dif);
				if (i > 1)
				{
					float anglePerRound = (angle - lastAngle) / roundCount;
					for (int j = 0; j < roundCount; j++)
					{
						float ang = lastAngle + anglePerRound * j;
						Vector2 roundOffset = (Vector2)(Quaternion.Euler(0, 0, ang) * Vector3.up * thickness);
						vert.position = previousPos - roundOffset;
						vh.AddVert(vert);

						vert.position = previousPos + roundOffset;
						vh.AddVert(vert);
					}
				}
				lastAngle = angle;

				Vector2 offset = (Vector2)(Quaternion.Euler(0, 0, angle) * Vector3.up * thickness);

				vert.position = previousPos - offset;
				vh.AddVert(vert);

				vert.position = previousPos + offset;
				vh.AddVert(vert);

				vert.position = currentPos - offset;
				vh.AddVert(vert);

				vert.position = currentPos + offset;
				vh.AddVert(vert);
			}
			for (int i = 0; i < vh.currentVertCount - 2; i += 2)
			{
				int index = i;
				vh.AddTriangle(index, index + 1, index + 3);
				vh.AddTriangle(index + 3, index + 2, index);
			}
		}
	}
}
