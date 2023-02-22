using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class GrassTileManager : MonoBehaviour
{
    // Singleton
    public static GrassTileManager instance;
    public GameObject ground;
    private static Tilemap tilemap;

    public delegate void DamageDealer(float dmg);
    public static event DamageDealer DealDamage;

    private int TMax = 16;
    private float spreadSpeed = 0.2f; // Seconds between 1 pixel larger circle is drawn 
    private Vector3 mousePosition;
    private Vector3 lastMousePosition;
    private Dictionary<float, bool[,]> pixelCircleCache = new Dictionary<float, bool[,]>();

    private void Awake()
    {
        tilemap = ground.GetComponent<Tilemap>();
        instance = this;

        for (int r = 2; r <= 30; r++)
        {
            pixelCircleCache.Add(r, BoxedCircle(r));
        }
    }

    private void FixedUpdate()
    {

        // Testing
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new Vector3(mousePosition.x, mousePosition.y, 0f);
        if (Input.GetKey(KeyCode.X))
        {
            // movement cooldown, wont repeat calls unless the 
            if (Vector3.Distance(mousePosition, lastMousePosition) >= .06f)
            {
                StartDeathCircle(mousePosition);
            }
            lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lastMousePosition = new Vector3(mousePosition.x, mousePosition.y, 0f);
        }
    }

    private IEnumerator KillPlants()
    {
        while (gameObject.activeSelf is true)
        {
            // Find the closest pixel cooridante to the mouse and put a mask there. 16 pixels per unit
            Vector2 pixelPoint = new Vector2(
                Mathf.Floor(transform.position.x * 16f),
                Mathf.Floor(transform.position.y * 16f)
            ) / 16f;

            yield return new WaitForSeconds(.2f);
        }
    }
    /// <summary>
    /// StartDeathCircle is called when the grass is attacked.
    /// </summary>
    /// <param name="pos">. 3D world coordinates of grass death center </param>
    /// starts coroutine
    public void StartDeathCircle(Vector3 pos)
    {
        // get the tile under the position vector
        Vector3Int tileCenter = tilemap.WorldToCell(pos);
        Vector3Int pixelCenter = Vector3Int.RoundToInt((pos - tileCenter) * TMax);

        StartCoroutine(DeathCircleOld(pixelCenter.x, pixelCenter.y, tileCenter.x, tileCenter.y));
    }

    private IEnumerator DeathCircles(Transform deathPoint)
    {
        // get the tile under the position vector
        Vector3Int tileCenter = tilemap.WorldToCell(deathPoint.position);
        Vector3Int pixelCenter = Vector3Int.RoundToInt((deathPoint.position - tileCenter) * TMax);

        int slowAtHalf = 2;

        // Create Circle 
        for (int r = 2; r < 30; r += slowAtHalf)
        {
            // Quadratic equation to make spread speed fast when the radius is small and slow when it is big
            if (r > 20)
                slowAtHalf = 1;
            else if (r > 10)
                slowAtHalf = 2;

            spreadSpeed = .2f;

            // 2D array with true values where a pixel circle should be
            // CAN BE CACHEd
            bool[,] circleLayout = pixelCircleCache[r];
        }
        yield break;
    }



    // input is pixel and tile coordinates
    private IEnumerator DeathCircleOld(int cpx, int cpy, int ctx, int cty)
    {
        // 1 pixel circle initlizers
        int x1 = 0, y1 = 0, x2 = 1, y2 = 1;
        int slowAtHalf = 3;
        
        // Create Circle 
        for (int r = 2; r < 30; r += slowAtHalf)
        {
            // Quadratic equation to make spread speed fast when the radius is small and slow when it is big
            if (r > 20)
                slowAtHalf = 1;
            else if (r > 10)
                slowAtHalf = 2;

            spreadSpeed = .2f;
          
            // 2D array with true values where a pixel circle should be
            // CAN BE CACHEd
            bool[,] circleLayout = pixelCircleCache[r];

            // 2D array of 1D array's of color codes  
            bool[,][] tileSet = CircToTileKey(circleLayout, cpx, cpy, TMax, ref x1, ref x2, ref y1, ref y2);

            // For each tile in tileSet, send tile key 
            for (int i = x1; i < x2; i++)
            {
                for (int j = y1; j < y2; j++)
                {
                    // Check if tile has ability to change color
                    if (tilemap.GetInstantiatedObject(new Vector3Int(ctx + i, cty + j, 0)) == null)
                        continue;
                    if (tilemap.GetInstantiatedObject(new Vector3Int(ctx + i, cty + j, 0)).GetComponent<RecolorPixel>() == null)
                        continue;

                    GameObject tile = tilemap.GetInstantiatedObject(new Vector3Int(ctx + i, cty + j, 0));

                    int a = i, b = j;

                    // Negative Index
                    if (i < 0)
                        a = i + x2 - x1;
                    if (j < 0)
                        b = j + y2 - y1;

                    tile.GetComponent<RecolorPixel>().MixDeadGrass(tileSet[a, b]);
                }
            }
            yield return new WaitForSeconds(spreadSpeed);
        }
        // Stops Coroutinesd
        yield break;
    }

    /// <summary>
    /// CircToTileKey is called once per life drain instance and the continouly for 60 seconds every 2 seconds.
    /// </summary>
    /// <param name="circleLayout">. 2D square array with true values in the encolsed circle. </param>
    /// <param name="cx"> Center X value of the circle in pixels from BL.</param>
    /// <param name="cy"> Center Y value of the circle in pixels from BL.</param>
    /// <param name="tileSize"> The length/width of the tile.</param>
    /// <returns> A 2D Array of 1D array's of color coordinates for tiles.</returns>
    private bool[,][] CircToTileKey(bool[,] circleLayout, int cx, int cy, int tileSize, ref int tileX1, ref int tileX2, ref int tileY1, ref int tileY2)
    {
        int diameterIndex = circleLayout.GetLength(0) - 1;  // index for iterating the boxed circle
        int radius = diameterIndex / 2;                     // distance to edge of circle NOT counting center pixel

        int x = 0, y = 0;                                   // Coordinates of the boxed circle
        int tileX = 0, tileY = 0;                           // Coordinates of the tiles

        #region Tile Set Size
        int x1 = cx - radius, x2 = cx + radius;
        int y1 = cy - radius, y2 = cy + radius;

        if (x1 < 0)
        {
            if (x1 == -1 * tileSize)
                tileX1 = x1 / tileSize;
            else
                tileX1 = x1 / tileSize - 1;
        }
        if (x2 >= tileSize)
        {
            tileX2 = x2 / tileSize + 1;
        }

        if (y1 < 0)
        {
            if (y1 == -1 * tileSize)
                tileY1 = y1 / tileSize;
            else
                tileY1 = y1 / tileSize - 1;
        }
        if (y2 >= tileSize)
        {
            tileY2 = y2 / tileSize + 1;
        }

        int tileSetX = tileX2 - tileX1;
        int tileSetY = tileY2 - tileY1;
        #endregion

        if (tileSetX == 0)
            tileSetX = 1;
        if (tileSetY == 0)
            tileSetY = 1;


        bool[,][] keySet = new bool[tileSetX, tileSetY][];
  
        if (diameterIndex == 0)     // Single Pixel Circle
        {
            x = cx;
            y = cy;
            // Set quadrant to (0,0)
            keySet[0, 0] = new bool[tileSize * tileSize];
            keySet[0, 0][x + (y * tileSize)] = circleLayout[0, 0];
        }

        else
        {
            // Can use 8 way symetry to fill the circle in and reduce O.
            for (int i = 0; i <= diameterIndex; i++)
            {
                for (int j = 0; j <= diameterIndex; j++)
                {   
                    // Coordinate switch from boxed circle to real tiles
                    x = cx - radius + j;
                    y = cy - radius + i;
                    tileX = 0;
                    tileY = 0;

                    // x tile
                    if (x >= tileSize)
                    {
                        tileX = x / tileSize;
                        x = x % tileSize;
                    }
                    else if (x < 0)
                    {
                        if (x == -1 * tileSize)
                            tileX = x / tileSize;
                        else
                            tileX = x / tileSize - 1;
                        x = x % tileSize;
                    }

                    // y tile
                    if (y >= tileSize)
                    {
                        tileY = y / tileSize;
                        y = y % tileSize;
                    }
                    else if (y < 0)
                    {
                        if (y == -1 * tileSize)
                            tileY = y / tileSize;
                        else
                            tileY = y / tileSize - 1;
                        y = y % tileSize;
                    }

                    if (x < 0)
                        x += tileSize;
                    if (y < 0)
                        y += tileSize;

                    if (tileX < 0)
                        tileX += tileSetX;
                    if (tileY < 0)
                        tileY += tileSetY;


                    if (keySet[tileX, tileY] == null)
                    {
                        keySet[tileX, tileY] = new bool[tileSize * tileSize];
                    }


                    keySet[tileX, tileY][x + (y * tileSize)] = circleLayout[i, j];
                }
            }
        }
        return keySet;
    }

    public bool[,] BoxedCircle(int radius)
    {
        // Returns a 2d square matrix with true values where the circle pixels should be.

        // Size is 2r - 1 because there needs to be acenter point.
        bool[,] layout = new bool[2 * radius - 1, 2 * radius - 1];
        int x = 0;
        int y = radius;
        int p = (5 - radius * 4) / 4;

        CirclePoints(radius, x, y, ref layout);
        while (x < y)
        {
            x++;
            if (p < 0)
            {
                p += 2 * x + 1;
            }
            else
            {
                y--;
                p += 2 * (x - y) + 1;
            }
            CirclePoints(radius, x, y, ref layout);
        }
        return layout;
    }

    private void CirclePoints(int rad, int x, int y, ref bool[,] layout)
    {
        int center = rad - 1;
        // Sets the cardinal points
        if (x == 0)
        {
            for (int i = -y + 1; i < y; i++)
            {
                // Set all points on the vertical crossection
                layout[center, (rad + i) - 1] = true;
                // Set all points on the horizontal crossection
                layout[(rad + i) - 1, center] = true;
            }
        }
        // last point
        else if (x == y)
        {
            for (int i = -y + 1; i < y; i++)
            {
                if (i == 0)
                {
                    layout[(rad - x) - 1, (rad + i) - 1] = true;
                }
                else
                {
                    layout[(rad - x) - 1, (rad + i) - 1] = true;
                    layout[(rad + x) - 1, (rad + i) - 1] = true;
                }
            }
        }
        else if (x < y)
        {

            for (int i = -y + 1; i < y; i++)
            {
                layout[(rad + x) - 1, (rad + i) - 1] = true;
                layout[(rad - x) - 1, (rad + i) - 1] = true;
                layout[(rad + i) - 1, (rad + x) - 1] = true;
                layout[(rad + i) - 1, (rad - x) - 1] = true;
            }
        }
    }

    
    private void CirclePointsOutline(int rad, int x, int y, ref bool[,] layout)
    {
        int center = rad - 1;
        int pointRadius = y - 1;
        // Sets the cardinal points
        if (x == 0)
        {
            layout[center, center + pointRadius] = true;
            layout[center, center - pointRadius] = true;
            layout[center + pointRadius, center] = true;
            layout[center - pointRadius, center] = true;
        }
        // last point
        else if (x == y)
        {
            layout[center + x, center + pointRadius] = true;
            layout[center + x, center - pointRadius] = true;
            layout[center - x, center + pointRadius] = true;
            layout[center - x, center - pointRadius] = true;

        }
        else if (x < y)
        {
            layout[center + x, center + pointRadius] = true;
            layout[center + x, center - pointRadius] = true;
            layout[center - x, center + pointRadius] = true;
            layout[center - x, center - pointRadius] = true;

            layout[center + pointRadius, center + x] = true;
            layout[center + pointRadius, center - x] = true;
            layout[center - pointRadius, center + x] = true;
            layout[center - pointRadius, center - x] = true;
        }
    }
}