using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

enum GoalState { ODD, EVEN };
enum Direction { LEFT, RIGHT, UP, DOWN };
class Node
{
    //Variable
    public Node Pre;
    public int DistanceFromStart;
    public int[,] val;
    //Function
    public Node(int n)
    {
        DistanceFromStart = 9999;
        val = new int[n, n];
    }
    public Node(int[,] _val)
    {
        DistanceFromStart = 9999;
        this.val = _val;
    }
    public int HeuristicFunction()
    {
        int h = 0;
        //kiem tra o hien va o chuan
            for (int i = 0; i < val.GetLength(0); i++)
                for (int j = 0; j < val.GetLength(0); j++)
                    h = h + Math.Abs(val[i, j] / val.GetLength(0) - i) + Math.Abs(val[i, j] % val.GetLength(0) - j);
        return h;
    }
}
class puzzle
{

    LinkedList<Node> Open = new LinkedList<Node>();             //Open
    LinkedList<Node> Close = new LinkedList<Node>();             //Close
    public Stack<Direction> Way;
    public int count = 0;
    public puzzle(int[,] val)
    {
        Way = new Stack<Direction>();
        Node Initial = new Node(val);
        Initial.DistanceFromStart = 0;
        Open.AddFirst(Initial); 
        while (Open != null)
        {
            count++;
            //Chọn node nhỏ nhất
            LinkedListNode<Node> smallest = ChooseSmallestNode();
            //Chuyển "smallest" từ Open sang Close
            Open.Remove(smallest);
            Close.AddFirst(smallest);
            
            //Nếu node "smallest" là đích
            if (smallest.Value.HeuristicFunction() == 0)
            {
                while (smallest.Value.Pre != null)
                {
                    //duyệt ngược lại để lấy đường đi
                    Way.Push(Turn(smallest.Value.Pre, smallest.Value));
                    smallest.Value = smallest.Value.Pre;
                }
                break;
            }
            //Thêm các đỉnh kề của "smallest" vào O mà không thuộc C và không thuộc O vào O  
            AddAdjacentNodesToOpen(smallest.Value);
            if (count > 2000)
                break;
        }
    }
    private LinkedListNode<Node> ChooseSmallestNode()
    {
        LinkedListNode<Node> smallestNode = Open.First;
        LinkedListNode<Node> node = Open.First;
        while (node != null)
        {
            if (node.Value.DistanceFromStart + node.Value.HeuristicFunction() < smallestNode.Value.DistanceFromStart + smallestNode.Value.HeuristicFunction())
            {
                smallestNode = node;
            }
            node = node.Next;
        }
        return smallestNode;
    }
    private void AddAdjacentNodesToOpen(Node node)
    {
        //Tìm vị trí của ô trống (có giá trị bằng 0) trong "node"
        int col = 0, row = 0;
        for (int i = 0; i < node.val.GetLength(0)* node.val.GetLength(0); i++)
            if (node.val[i / node.val.GetLength(0), i % node.val.GetLength(0)] == 0)
            {
                row = i / node.val.GetLength(0);
                col = i % node.val.GetLength(0);
            }
        //Ô bên trên
        if (row > 0)
        {
            Node up = new Node(node.val.GetLength(0));
            //sao chep
            for (int i = 0; i < node.val.GetLength(0)* node.val.GetLength(0); i++)
            {
                up.val[i / node.val.GetLength(0), i % node.val.GetLength(0)] = node.val[i / node.val.GetLength(0), i % node.val.GetLength(0)];
            }

            up.val[row, col] = up.val[row - 1, col];
            up.val[row - 1, col] = 0;
            if (!Contain(Close, up) && !Contain(Open, up))
            {
                up.DistanceFromStart = node.DistanceFromStart + 1;
                up.Pre = node;
                Open.AddFirst(up);
            }
            else if (Contain(Open, up))
            {
                if (up.DistanceFromStart > node.DistanceFromStart + 1)
                {
                    up.DistanceFromStart = node.DistanceFromStart + 1;
                    up.Pre = node;
                }
            }
            else if (Contain(Close, up))
            {
                if (up.DistanceFromStart > node.DistanceFromStart + 1)
                {
                    Close.Remove(up);
                    up.DistanceFromStart = node.DistanceFromStart + 1;
                    up.Pre = node;
                    Open.AddFirst(up);
                }
            }
            


        }
        //Ô bên dưới
        if (row < node.val.GetLength(0)-1)
        {
            Node down = new Node(node.val.GetLength(0));
            for (int i = 0; i < node.val.GetLength(0)* node.val.GetLength(0); i++)
            {
                down.val[i / node.val.GetLength(0), i % node.val.GetLength(0)] = node.val[i / node.val.GetLength(0), i % node.val.GetLength(0)];
            }
            down.val[row, col] = down.val[row + 1, col];
            down.val[row + 1, col] = 0;
            if (!Contain(Close, down) && !Contain(Open, down))
            {
                down.DistanceFromStart = node.DistanceFromStart + 1;
                down.Pre = node;
                Open.AddFirst(new LinkedListNode<Node>(down));
            }
            else if (Contain(Open, down))
            {
                if (down.DistanceFromStart > node.DistanceFromStart + 1)
                {
                    down.DistanceFromStart = node.DistanceFromStart + 1;
                    down.Pre = node;
                }
            }
            else if (Contain(Close, down))
            {
                Close.Remove(down);
                down.DistanceFromStart = node.DistanceFromStart + 1;
                down.Pre = node;
                Open.AddFirst(down);
            }
        }
        //Ô bên trái 
        if (col > 0)
        {
            Node left = new Node(node.val.GetLength(0));
            for (int i = 0; i < node.val.GetLength(0)* node.val.GetLength(0); i++)
            {
                left.val[i / node.val.GetLength(0), i % node.val.GetLength(0)] = node.val[i / node.val.GetLength(0), i % node.val.GetLength(0)];
            }
            left.val[row, col] = left.val[row, col - 1];
            left.val[row, col - 1] = 0;
            if (!Contain(Close, left) && !Contain(Open, left))
            {
                left.DistanceFromStart = node.DistanceFromStart + 1;
                left.Pre = node;
                Open.AddFirst(new LinkedListNode<Node>(left));
            }
            else if (Contain(Open, left))
            {
                if (left.DistanceFromStart > node.DistanceFromStart + 1)
                {
                    left.DistanceFromStart = node.DistanceFromStart + 1;
                    left.Pre = node;
                }
            }
            else if (Contain(Close, left))
            {
                Close.Remove(left);
                left.DistanceFromStart = node.DistanceFromStart + 1;
                left.Pre = node;
                Open.AddFirst(left);
            }
        }
        //ô bên phải
        if (col < node.val.GetLength(0)-1)
        {
            Node right = new Node(node.val.GetLength(0));
            for (int i = 0; i < node.val.GetLength(0)* node.val.GetLength(0); i++)
            {
                right.val[i / node.val.GetLength(0), i % node.val.GetLength(0)] = node.val[i / node.val.GetLength(0), i % node.val.GetLength(0)];
            }
            right.val[row, col] = right.val[row, col + 1];
            right.val[row, col + 1] = 0;
            if (!Contain(Close, right) && !Contain(Open, right))
            {
                right.DistanceFromStart = node.DistanceFromStart + 1;
                right.Pre = node;
                Open.AddFirst(new LinkedListNode<Node>(right));
            }
            else if (Contain(Open, right))
            {
                if (right.DistanceFromStart > node.DistanceFromStart + 1)
                {
                    right.DistanceFromStart = node.DistanceFromStart + 1;
                    right.Pre = node;
                }
            }
            else if (Contain(Close, right))
            {
                Close.Remove(right);
                right.DistanceFromStart = node.DistanceFromStart + 1;
                right.Pre = node;
                Open.AddFirst(right);
            }
        }
    }
    private bool Compare(Node x, Node y)
    {
        for (int i = 0; i < x.val.GetLength(0)* x.val.GetLength(0); i++)
            if (x.val[i / x.val.GetLength(0), i % x.val.GetLength(0)] != y.val[i / x.val.GetLength(0), i % x.val.GetLength(0)])
                return false;
        return true;

    }
    private bool Contain(LinkedList<Node> nodes, Node node)
    {
        foreach (Node _node in nodes)
            if (Compare(node, _node))
                return true;
        return false;
    }
    private Direction Turn(Node Last, Node Next)
    {
        int i = 0, j = 0;
        //tìm hàng và cột chứa ô trống của last và next
        for (; i < Last.val.GetLength(0)* Last.val.GetLength(0); i++)
            if (Last.val[i / Last.val.GetLength(0), i % Last.val.GetLength(0)] == 0)
                break;
        for (; j < Last.val.GetLength(0)* Last.val.GetLength(0); j++)
            if (Next.val[j / Last.val.GetLength(0), j % Last.val.GetLength(0)] == 0)
                break;
        //xác định hướng đi
        if (i % Last.val.GetLength(0) == j % Last.val.GetLength(0) + 1)
            return Direction.LEFT;
        else if (i % Last.val.GetLength(0) == j % Last.val.GetLength(0) - 1)
            return Direction.RIGHT;
        else if (i / Last.val.GetLength(0) == j / Last.val.GetLength(0) + 1)
            return Direction.UP;
        else
            return Direction.DOWN;
    }

}

//kiểm tra với các tập close, open hay chưa
public class AI : MonoBehaviour
{
    bool notice = true;
    GameController GameMN;
    // Use this for initialization
    void Start()
    {

        GameObject GameManager = GameObject.Find("GameController");
        GameMN = GameManager.GetComponent<GameController>();
    }

    void OnMouseDown()
    {
        transform.localScale = new Vector4(0.1f, 0.1f);
    }
    void OnMouseUp()
    {
        transform.localScale = new Vector4(0.12f, 0.12f);
        
        puzzle p = new puzzle(GameMN.CurrentMatrix);
        //foreach (Direction d in p.Way)
        //{
        //    Process(d);
        //}

        if (p.count > 2000)
        {
            //Không giải được, Lệnh bên dưới tạo ra Dialog nhưng khi gặp lỗi không rõ khi build
            //EditorUtility.DisplayDialog("Notice", "Can't solve !", "OK, i'll play myselft");
        }
        else
        {
            if (notice)
            {
                //Giải được, Lệnh bên dưới tạo ra Dialog nhưng khi gặp lỗi không rõ khi build
                //EditorUtility.DisplayDialog("Notice", "Solve in " + Convert.ToString(p.Way.Count) + " Step ", "OK");
                notice = false;
            }
            Process(p.Way.Pop());
        }
    }
    void Process(Direction _d)
    {
        switch (_d)
        {
            case Direction.LEFT:
                GameMN.countStep += 1;
                GameMN.row = GameMN.rowBlank;
                GameMN.col = GameMN.colBlank - 1;
                GameMN.ProcessSoft();
                break;
            case Direction.RIGHT:
                GameMN.countStep += 1;
                GameMN.row = GameMN.rowBlank;
                GameMN.col = GameMN.colBlank + 1;
                GameMN.ProcessSoft();
                break;
            case Direction.UP:
                GameMN.countStep += 1;
                GameMN.row = GameMN.rowBlank - 1;
                GameMN.col = GameMN.colBlank;
                GameMN.ProcessSoft();
                break;
            case Direction.DOWN:
                GameMN.countStep += 1;
                GameMN.row = GameMN.rowBlank + 1;
                GameMN.col = GameMN.colBlank;
                GameMN.ProcessSoft();
                break;

        }
    }
}


