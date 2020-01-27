using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace GameAuto
{
    public class MovementDecision
    {
        public const int BOARD_SIZE_W = 8;
        public const int BOARD_SIZE_H = 8;
        public const int DIRECTION = 4;

        public static int[,] g_AllocCharacters = new int[BOARD_SIZE_H, BOARD_SIZE_W] {
            { 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0},
        };

        public static int[,] g_TempCharacters = new int[BOARD_SIZE_H, BOARD_SIZE_W] {
            { 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0},
        };

        public static int[,,] g_ScoreByMovements = new int[BOARD_SIZE_H, BOARD_SIZE_W, DIRECTION];

        private static List<List<Point>>    g_LstIdenticals = new List<List<Point>>();
        private static List<List<int>>      g_LstIdenticalItems = new List<List<int>>();
        private static List<Point>          g_LstRemainLands = new List<Point>();

        public static bool CompareCharacter(int c, int k)
        {
            if (c == k) return true;
            else if (c == 1 && k == 2 || c == 2 && k == 1) return true;
            else if (c == 3 && k == 4 || c == 4 && k == 3) return true;
            else if (c == 5 && k == 6 || c == 6 && k == 5) return true;
            else if (c == 7 && k == 8 || c == 8 && k == 7) return true;
            else if (c == 9 && k == 10 || c == 10 && k == 9) return true;

            return false;
        }

        public static void GetEqualLinks(int[,] arrCharacters)
        {
            g_LstIdenticals.Clear();
            g_LstIdenticalItems.Clear();

            int i = 0, j = 0;

            // Horizontal Search                    
            for (; i < BOARD_SIZE_H; i++)
            {
                j = 0;
                while( j < BOARD_SIZE_W)
                {
                    int c = arrCharacters[i, j]; int k = arrCharacters[i, j];
                    int ii = 0;

                    List<Point> LstIdenticals = new List<Point>();
                    List<int> LstIdenticalItems = new List<int>();
                    LstIdenticals.Add(new Point(i, j)); LstIdenticalItems.Add(k);

                    while (true)
                    {
                        ii++;
                        if (j + ii >= BOARD_SIZE_W)
                            break;

                        k = arrCharacters[i, j + ii];
                        if (CompareCharacter(c, k))
                        {
                            LstIdenticals.Add(new Point(i, j + ii));
                            LstIdenticalItems.Add(k);
                        }
                        else break;
                    }

                    if (LstIdenticals.Count > 2)
                    {
                        g_LstIdenticals.Add(LstIdenticals);
                        g_LstIdenticalItems.Add(LstIdenticalItems);
                    }

                    j += ii;
                }
            }

            i = 0;
            // Vertical Search
            for ( j = 0; j < BOARD_SIZE_W; j ++)
            {
                while (i < BOARD_SIZE_H)
                {
                    int ii = 0;
                    int c = arrCharacters[i, j]; int k = arrCharacters[i, j];

                    List<Point> LstIdenticals = new List<Point>();
                    List<int> LstIdenticalItems = new List<int>();
                    LstIdenticals.Add(new Point(i, j)); LstIdenticalItems.Add(k);

                    while (true)
                    {
                        ii ++;
                        if (i + ii >= BOARD_SIZE_H)
                            break;

                        k = arrCharacters[i+ii, j];
                        if (CompareCharacter(c, k))
                        {
                            LstIdenticals.Add(new Point(i + ii, j));
                            LstIdenticalItems.Add(k);
                        }
                        else break;
                    }

                    if (LstIdenticals.Count > 2)
                    {
                        g_LstIdenticals.Add(LstIdenticals);
                        g_LstIdenticalItems.Add(LstIdenticalItems);
                    }

                    i += ii;
                }
            }
        }
    
        public static int CalcScores()
        {
            int k = 0; int nTotalSum = 0, nSum = 0;
            foreach (List<Point> Link in g_LstIdenticals)
            {
                nSum = 0;
                List<int> ListCharacter = g_LstIdenticalItems[k];
                
                int nCntOdd = 0, nCntEven = 0;
                foreach(int Charac in ListCharacter)
                {
                    // if it contains sea increase by 3 points.
                    if (Charac > 0 && Charac % 2 == 0) nCntEven++;
                    if (Charac % 2 == 1) nCntOdd ++;
                }

                if (nCntEven > 1 && Link.Count > 4)
                    nSum += 1500;

                if (nCntEven == 0 || nCntOdd == 0)
                    nSum += ListCharacter.Count;
                else
                    nSum += nCntOdd * 200 + ListCharacter.Count;

                nTotalSum += nSum;
                k++;
            }

            return nTotalSum;
        }
    
        public static void ApplyMovementToArray(ref int[,] array)
        {
            foreach(List<Point> Links in g_LstIdenticals)
            {
                foreach(Point pt in Links)
                {
                    array[pt.X, pt.Y] = 0;
                }
            }

            int j = 0, k = 0, nDelta = 1;
            for( int i = 0; i < BOARD_SIZE_W; i ++)
            {
                j = BOARD_SIZE_H - 1;
                while (j >= 0)
                {
                    if (array[j, i] == 0)
                    {
                        k = j - 1;
                        nDelta = 1;

                        while(k >= 0)
                        {
                            if (array[k, i] == (int)Global.CHARACTER_TYPE.CHAR_WOOD)
                            {
                                k --;
                                nDelta ++;
                                continue;
                            }

                            array[k+nDelta, i] = array[k, i];
                            k --;
                        }
                    }

                    j--;
                }
            }

            Random rnd = new Random();
            for (int i = 0; i < BOARD_SIZE_H; i++)
            {
                for (j = 0; j < BOARD_SIZE_W; j++)
                {
                    // still need to fill, then randomly fill it.
                    if (array[i, j] == 0)
                    {
                        array[i, j] = rnd.Next(1, 9);
                        if (array[i, j] % 2 == 0) array[i, j]++;
                    }
                }
            }
        }

        public static void RefreshRemainLandsList(int[,] array)
        {
            g_LstRemainLands.Clear();
            for( int i = 0; i < BOARD_SIZE_H; i ++)
            {
                for ( int j = 0; j < BOARD_SIZE_W; j ++ )
                {
                    if (array[i,j] % 2 == 1)
                    {
                        g_LstRemainLands.Add(new Point(i, j));
                    }
                }
            }
        }

        public static int TrySwapping()
        {
            GetEqualLinks(g_TempCharacters);
            //RefreshRemainLandsList(g_TempCharacters);

            int nScore = CalcScores();
            int nTotalScore = nScore;

            int nRepeatCnt = 0;
            while (nScore > 1 && nRepeatCnt < 2) // predict 2 times
            {
                ApplyMovementToArray(ref g_TempCharacters);
                GetEqualLinks(g_TempCharacters);
                nScore = CalcScores() / (nRepeatCnt + 2);
                nTotalScore += nScore;
                nRepeatCnt++;
            }

            return nTotalScore;
        }

        public static int TrySwapWithTop(int i, int j)
        {
            if (i < 1) return 0;
            if (g_AllocCharacters[i-1, j] == 11) // wood
                return 0;

            CopyCharactersToTemp();

            int nTemp = g_TempCharacters[i-1, j];
            g_TempCharacters[i-1, j] = g_TempCharacters[i, j];
            g_TempCharacters[i, j] = nTemp;

            return TrySwapping();
        }

        public static int TrySwapWithLeft(int i, int j)
        {
            if (j < 1) return 0;
            if (g_AllocCharacters[i, j-1] == 11) // wood
                return 0;

            CopyCharactersToTemp();

            int nTemp = g_TempCharacters[i, j-1];
            g_TempCharacters[i, j-1] = g_TempCharacters[i, j];
            g_TempCharacters[i, j] = nTemp;

            return TrySwapping();
        }

        public static int TrySwapWithRight(int i, int j)
        {
            if (j >= BOARD_SIZE_W-1) return 0;
            if (g_AllocCharacters[i, j+1] == 11) // wood
                return 0;

            CopyCharactersToTemp();

            int nTemp = g_TempCharacters[i, j+1];
            g_TempCharacters[i, j+1] = g_TempCharacters[i, j];
            g_TempCharacters[i, j] = nTemp;

            return TrySwapping();
        }

        public static int TrySwapWithBottom(int i, int j)
        {
            if (i >= BOARD_SIZE_H - 1) return 0;
            if (g_AllocCharacters[i+1, j] == 11) // wood
                return 0;

            CopyCharactersToTemp();

            int nTemp = g_TempCharacters[i+1, j];
            g_TempCharacters[i+1, j] = g_TempCharacters[i, j];
            g_TempCharacters[i, j]   = nTemp;

            return TrySwapping();
        }

        public static void CopyCharactersToTemp()
        {
            for (int i = 0; i < BOARD_SIZE_H; i++)
            {
                for (int j = 0; j < BOARD_SIZE_W; j++)
                {
                    g_TempCharacters[i, j] = g_AllocCharacters[i, j];
                }
            }
        }

        public static void Process()
        {
            for (int i = 0; i < BOARD_SIZE_H; i++)
            {
                for (int j = 0; j < BOARD_SIZE_W; j++)
                {
                    g_ScoreByMovements[i, j, 0] = g_ScoreByMovements[i, j, 1] = g_ScoreByMovements[i, j, 2] = g_ScoreByMovements[i, j, 3] = 0;
                    // not empty & not wood
                    if (g_AllocCharacters[i, j] != 0 && g_AllocCharacters[i, j] != 11)
                    {
                        g_ScoreByMovements[i, j, 0] = TrySwapWithTop(i, j);
                        g_ScoreByMovements[i, j, 1] = TrySwapWithLeft(i, j);
                        g_ScoreByMovements[i, j, 2] = TrySwapWithRight(i, j);
                        g_ScoreByMovements[i, j, 3] = TrySwapWithBottom(i, j);
                    }
                }
            }

            int nMaxI = 0, nMaxJ = 0, nMaxDirection = 0;
            int nMaxVal = 0;

            for (int i = 0; i < BOARD_SIZE_H; i++)
            {
                for (int j = 0; j < BOARD_SIZE_W; j++)
                {
                    for (int k = 0; k < DIRECTION; k++)
                    {
                        if (nMaxVal < g_ScoreByMovements[i, j, k])
                        {
                            nMaxVal = g_ScoreByMovements[i, j, k];
                            nMaxI = i; nMaxJ = j; nMaxDirection = k;
                        }
                    }
                }
            }

            if (nMaxVal < 1)
                return;

            EmulateMovement(nMaxI, nMaxJ, nMaxDirection);
        }

        public static void EmulateMovement(int nMaxI, int nMaxJ, int nMaxDirection)
        {
            int X = Global.g_rcROI.X + 10 + 217;
            int Y = Global.g_rcROI.Y + 39 + 39;
            Global.GetRatioCalcedValues(Global.g_rcROI.Width, Global.g_rcROI.Height, ref X, ref Y);

            int nStepX = Global.DEF_MAIN_BOARD_W / 8;
            int nStepY = Global.DEF_MAIN_BOARD_H / 8;

            Point pt = new Point(X + nMaxJ * nStepX+nStepX/2, Y + nMaxI * nStepY + nStepY/2);
            Point ptTarget = Point.Empty;

            if (nMaxDirection == 0)// up
                ptTarget = new Point(X + nMaxJ * nStepX + nStepX / 2, Y + (nMaxI - 1) * nStepY + nStepY / 2);
            else if (nMaxDirection == 1) // left
                ptTarget = new Point(X + (nMaxJ-1) * nStepX + nStepX / 2, Y + nMaxI * nStepY + nStepY / 2);
            else if (nMaxDirection == 2) // right
                ptTarget = new Point(X + (nMaxJ + 1) * nStepX + nStepX / 2, Y + nMaxI * nStepY + nStepY / 2);
            else if (nMaxDirection == 3) // bottom
                ptTarget = new Point(X + nMaxJ * nStepX + nStepX / 2, Y + (nMaxI+1) * nStepY + nStepY / 2);
            
            SendMouseEventToPoint(pt, ptTarget);
        }

        public static bool SendMouseEventToPoint(Point ptStart, Point ptTarget)
        {
            try
            {
                Global.MouseDownTo(ptStart);
                Global.MouseMoveToAndUp(ptTarget);
                Global.MouseMoveTo(new Point(10,100));

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
