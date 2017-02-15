using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace laba2_pyatnashki_
{
    public partial class Form1 : Form
    {
        int[,] boardExam = new int[,] { { 1, 2, 3, 4}, { 5, 6, 7, 8}, { 9, 10, 11, 12}, { 13, 14, 15, 0} };
        int[,] board;
        int x, y;
        int B_X
        {
            get { return board.GetLength(0); }
        }
        int B_Y
        {
            get { return board.GetLength(1); }
        }

        public Form1()
        {
            InitializeComponent();

            dgv.ColumnCount = 4;
            dgv.RowCount = 4;
            for (int i = 0; i < dgv.RowCount; i++)
                dgv.Rows[i].Height = (dgv.Height - 3) / dgv.Rows.Count;

        }

        bool CheckBoard()
        {
            int emptyCount = 0;
            for (int i = 0; i < dgv.RowCount; i++)
            {
                for (int j = 0; j < dgv.ColumnCount; j++)
                {
                    if (dgv.Rows[i].Cells[j].Value == null)
                        emptyCount++;
                }
            }
            if (emptyCount > 1)
            {
                MessageBox.Show("Заповніть пусті клітинки!");
                return false;
            }

            for (int i = 0; i < dgv.RowCount; i++)
            {
                for (int j = 0; j < dgv.ColumnCount; j++)
                {
                    if (emptyCount < 1)
                    {
                        MessageBox.Show("Одна клітинка має бути пустою!");
                        return false;
                    }
                    if ((Convert.ToInt16(dgv.Rows[i].Cells[j].Value) < 1 || Convert.ToInt16(dgv.Rows[i].Cells[j].Value) > 15) && dgv.Rows[i].Cells[j].Value != null)
                    {
                        MessageBox.Show("Введені некоректні дані. Числа мають бути в діапазоні [1; 15]");
                        return false;
                    }
                    for (int i1 = 0; i1 < dgv.RowCount; i1++)
                    {
                        for (int j1 = 0; j1 < dgv.ColumnCount; j1++)
                        {
                            if (Convert.ToInt16(dgv.Rows[i].Cells[j].Value) == Convert.ToInt16(dgv.Rows[i1].Cells[j1].Value) && i != i1 && j != j1 && Convert.ToInt16(dgv.Rows[i].Cells[j].Value) != 0 && Convert.ToInt16(dgv.Rows[i1].Cells[j1].Value) != 0)
                            {
                                MessageBox.Show("Числа не мають співпадати!");
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        int[,] ReadBoard()
        {
            int[,] board = new int[dgv.RowCount, dgv.ColumnCount];
            for (int i = 0; i < dgv.RowCount; i++)
            {
                for (int j = 0; j < dgv.ColumnCount; j++)
                {
                    if (dgv.Rows[i].Cells[j].Value == null)
                        board[i, j] = 0;
                    else
                        board[i, j] = Convert.ToInt16(dgv.Rows[i].Cells[j].Value);
                }
            }
            return board;
        }
        Point FindEmptyPoint(int[,] board)
        {
            for (int i = 0; i < B_X; i++)
            {
                for (int j = 0; j < B_Y; j++)
                {
                    if (board[i, j] == 0)
                    {
                        x = i;
                        y = j;
                        return new Point(i, j);
                    }
                }
            }
            return Point.Empty;
        }
        void Swap(int[,] board, int a, int b)
        {
            int x1 = 0, y1 = 0, x2 = 0, y2 = 0;
            for (int i = 0; i < B_X; i++)
            {
                for (int j = 0; j < B_Y; j++)
                {
                    if (board[i, j] == a)
                    {
                        x1 = i; y1 = j;
                    }
                    if (board[i, j] == b)
                    {
                        x2 = i; y2 = j;
                    }
                }
            }
            int tmp = board[x1, y1];
            board[x1, y1] = board[x2, y2];
            board[x2, y2] = tmp;
        }
        void Swap(int[,] board, int x1, int y1, int x2, int y2)
        {
            int tmp = board[x1, y1];
            board[x1, y1] = board[x2, y2];
            board[x2, y2] = tmp;
        }
        int Geth1(int[,] board)
        {
            int h1 = 0;
            for (int i = 0; i < B_X; i++)
            {
                for (int j = 0; j < B_Y; j++)
                {
                    if (board[i, j] != boardExam[i, j])
                        h1++;
                }
            }
            return h1;
        }
        void Copy(int[,] sourse, out int[,] dest)
        {
            dest = new int[sourse.GetLength(0), sourse.GetLength(1)];
            for (int i = 0; i < sourse.GetLength(0); i++)
            {
                for (int j = 0; j < sourse.GetLength(1); j++)
                {
                    dest[i, j] = sourse[i, j];
                }
            }
        }
        int[,] BFS(int[,] s, int[,] T)
        {
            Random rnd = new Random();
            List<int[,]> open = new List<int[,]>();
            List<int[,]> closed = new List<int[,]>();
            List<int[,]> children;
            List<int> evFunc = new List<int>();

            evFunc.Add(0);
            open.Add(s);
            while (true)
            {
                // 1
                int[,] X = open[0];
                // 2
                if (open.Count == 0)
                {
                    return null;
                }                
                // 3
                open.RemoveAt(0);evFunc.RemoveAt(0);
                // 4
                if (X.Equals(T))
                {
                    break;
                }
                // 5
                children = GenerateChild(X);
                // 6
                for (int k = 0; k < children.Count; k++)
                {
                    // 6.1
                    if (!open.Contains(children[k]) && !closed.Contains(children[k]))
                    {
                        open.Add(children[k]);
                        evFunc.Add(Geth1(children[k]));
                    }
                    // 6.2
                    if (open.Contains(children[k]))
                    {

                    }
                    // 6.3
                }
                //7
                closed.Add(X);
                //8
                for (int i = 0; i < open.Count; i++)
                {
                    for (int j = 0; j < open.Count - 1; j++)
                    {
                        if (evFunc[j + 1] < evFunc[j])
                        {
                            int[,] tmp = open[j];
                            open[j] = open[j + 1];
                            open[j + 1] = tmp;

                            int tmp1 = evFunc[j];
                            evFunc[j] = evFunc[j + 1];
                            evFunc[j + 1] = tmp1;
                        }
                    }
                }
            }
            return s;
        }
        List<int[,]> GenerateChild(int[,] X)
        {
            List<int[,]> children = new List<int[,]>();
            int x = FindEmptyPoint(X).X;
            int y = FindEmptyPoint(X).Y;
            int[,] stan = new int[1,1];
            if (x - 1 >= 0)
            {
                Copy(X, out stan);
                stan[x, y] = X[x - 1, y];
                stan[x - 1, y] = 0;
                children.Add(stan);
            }
            if (x + 1 < B_X)
            {
                Copy(X, out stan);
                stan[x, y] = X[x + 1, y];
                stan[x + 1, y] = 0;
                children.Add(stan);
            }
            if (y - 1 >= 0)
            {
                Copy(X, out stan);
                stan[x, y] = X[x, y - 1];
                stan[x, y - 1] = 0;
                children.Add(stan);
            }
            if (y + 1 < B_Y)
            {
                Copy(X, out stan);
                stan[x, y] = X[x, y + 1];
                stan[x, y + 1] = 0;
                children.Add(stan);
            }

            return children;
        }
        private void butStart_Click(object sender, EventArgs e)
        {
            if (!CheckBoard())
                return;
            board = ReadBoard();
            board = BFS(board, boardExam);
        }
    }
}
