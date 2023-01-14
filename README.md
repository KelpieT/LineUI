# LineUI

Simple UI Line.
LineUI generate line mesh and render as ui element.
Line corners can be rounded.

## How to use:

1. 
    - Add prefab "CanvasLineUI" or "LineUI" to canvas 
    - Or you can create empty gameobject in canvas(it must be fully cover canvas with pivot(0,0)) and add component LineUI

2. Modify screenPositions in inspector (Line UI component) 
* Or set all screenPositions via script by method SetLine(List<Vector2> screenPositions)

- Can modify color, thickness and round detailing
