using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XO
{
    public enum XOElement { None = 0, Cross = 1, Circle = -1 };

    public class XOField
    {
        public event Action<int, int, XOElement> OnTurn;
        public event Action<XOElement> OnGameOver;

        private const int rowCount = 3, colCount = 3;
        private XOElement[,] rawValues = new XOElement[rowCount, colCount];
        private XOElement lastTurn = XOElement.Circle;
        private XOElement winner = XOElement.None;

        private XOElement reverseElement(XOElement element)
        {
            // смена элемента на противоположный:
            // приводим в int, меняем знак, приводим обратно
            return (XOElement)(-(int)element);
        }

        public bool GameOver
        {
            get
            {
                return winner != XOElement.None;
            }
        }

        public bool CanTurn(int row, int col)
        {
            return rawValues[row, col] == XOElement.None && !GameOver;

        }

        public bool TryTurn(int row, int col)
        {

            if (!CanTurn(row, col))
            {
                return false;
            }

            rawValues[row, col] = reverseElement(lastTurn);
            lastTurn = rawValues[row, col];

            if (OnTurn != null)
            {
                OnTurn.Invoke(row, col, lastTurn);
            }

            Winner = checkWinner();
            return true;
        }
        public char[,] Field
        {
            get
            {
                char[,] result = new char[rowCount, colCount];
                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < colCount; j++)
                    {
                        if (rawValues[i, j] == XOElement.Cross)
                            result[i, j] = 'X';
                        else if (rawValues[i, j] == XOElement.Circle)
                            result[i, j] = 'O';
                        else
                            result[i, j] = '-';
                    }
                }
                return result;
            }
        } 
       
        public XOElement Winner
        {
            get { return winner; }
            set
            {
                winner = value;
                if (winner != XOElement.None)
                {
                    OnGameOver.Invoke(winner);
                }
            }
        }
        private XOElement checkRows()
        {
            for (int i = 0; i < rowCount; i++)
            {
                int sum = 0;
                for (int j = 0; j < colCount; j++)
                {
                    sum += (int)rawValues[i, j];
                }
                if (sum == 3)
                {
                    return XOElement.Cross;
                }
                if (sum == -3)
                {
                    return XOElement.Circle;
                }
            }
            return XOElement.None;
        }

        private XOElement checkWinner()
        {
            var winner = checkRows();
            if (winner != XOElement.None)
            {
                return winner;
            }
            winner = checkCols();
            if (winner != XOElement.None)
            {
                return winner;
            }
            winner = checkDiagonals();
            if (winner != XOElement.None)
            {
                return winner;
            }
            return XOElement.None;
        }
        private XOElement checkCols() 
        {
            for (int i = 0; i < colCount; i++)
            {
                int sum = 0;
                for (int j = 0; j < rowCount; j++)
                {
                    sum += (int)rawValues[j, i];
                }
                if (sum == 3)
                {
                    return XOElement.Cross;
                }
                if (sum == -3)
                {
                    return XOElement.Circle;
                }
            }
            return XOElement.None;


        }
        
        private XOElement checkDiagonals() 
        {
           int summ = (int)rawValues[0, 0] + (int)rawValues[1,1]+ (int)rawValues[2,2];
            if (summ == 3)
            {
                return XOElement.Cross;
            }
            if(summ == -3) 
            {
                return XOElement.Circle;

            }
            int summ1 = (int)rawValues[0, 2] + (int)rawValues[1, 1] + (int)rawValues[2, 0];
            if (summ1 == 3)
            {
                return XOElement.Cross;
            }
            if (summ1 == -3)
            {
                return XOElement.Circle;

            }
            return XOElement.None;
           
           


        }





    }

}
