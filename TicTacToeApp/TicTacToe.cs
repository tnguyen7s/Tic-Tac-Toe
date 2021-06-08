using System;
using System.Text;
class TicTacToe{
    /***
    declare static and non-static fields for the class
    ***/
    private static string gameInstruction = "Tic-Tac-Toe is traditionally a paper-and-pencil game for two players,\nX and O, who take turns marking the spaces in a 3×3 grid.\nThe User who succeeds in placing three of their marks in a diagonal,\nhorizontal, or vertical row is the winner.";
    private static int gridSize = 3;
    private char[][] grid;
    private char userSymbol;
    private char aiSymbol;
    /***
    define properties allowing public getters
    ***/
    public static string GameInstruction{
        get{ return TicTacToe.gameInstruction;}
    }
    public static int GridSize{
        get{ return TicTacToe.gridSize;}
    }
    public char UserSymbol{
        get{ return this.userSymbol;}
    }
    public char AiSymbol{
        get{ return this.aiSymbol;}
    }
    /***
    Constructor
    ***/
    public TicTacToe(char userSymbol)
    {
        this.grid = new char[TicTacToe.gridSize][];
        InitializeGrid();
        this.userSymbol = userSymbol;
        this.aiSymbol = (this.userSymbol == 'X')? 'O':'X';
        
    }
    /***
    To initialize grid with ' '
    ***/
    private void InitializeGrid()
    {
        for (int i = 0; i<TicTacToe.gridSize; i++)
        {
            this.grid[i] = new char[TicTacToe.gridSize];
            for (int j = 0; j<TicTacToe.gridSize; j++) this.grid[i][j] = ' ';
        }
    }
    /***
    toString(): print out the current state of the grid
    ***/
    public override string ToString()
    {
        StringBuilder s = new StringBuilder();
        for (int i = 0; i<TicTacToe.gridSize; i++)
        {
            for (int j = 0; j<TicTacToe.gridSize; j++)
            {
                s.Append(this.grid[i][j]);
                if (j+1<TicTacToe.gridSize) s.Append("|");
            }
            s.Append("\n");
            if (i+1<TicTacToe.gridSize) s.Append("-+-+-\n"); 
        }
        return s.ToString();
       
    }
    /***
    return the layout of the grid
    ***/
    public static string PrintGridLayout()
    {
        int count = 1;
        StringBuilder s = new StringBuilder();
        for (int i = 0; i<TicTacToe.gridSize; i++)
        {
            for (int j = 0; j<TicTacToe.gridSize; j++)
            {
                s.Append(count++);
                if (j+1<TicTacToe.gridSize) s.Append("|");
            }
            s.Append("\n");
            if (i+1<TicTacToe.gridSize) s.Append("-+-+-\n"); 
        }
        return s.ToString();
       
    }
    /***
    For each of user's turn, take their input and store into the grid
    If the input is invalid, throw an exception
    Param--- the number telling which square in the grid user want to play
    ***/
    public void UserPlay(int n)
    {
        if (n<1 || n>9)
        {
            throw new InvalidInputGameException("Number should be between 1 and 9.");
        }
        int i = n-1;
        int rowTh = i/TicTacToe.gridSize;
        int colTh = i - rowTh*TicTacToe.gridSize;
        if (this.grid[rowTh][colTh] != ' ')
        {
            throw new InvalidInputGameException("Square has been used.");
        }
        this.grid[rowTh][colTh] = this.UserSymbol;

    }
    /***
    Ai Turn to place a mark in the grid
    AI uses Minimax algorithm to find an optimal go, considering user playa optimally
    ***/
    public void AiPlay()
    {
        int squareN;
        Random rand = new Random();
        if (this.isEmpty()) squareN = rand.Next(1,9);
        else Minimax(out squareN, true);
        
        int row = (squareN - 1) / TicTacToe.GridSize;
        int col = squareN - 1 - row * TicTacToe.GridSize;
        this.grid[row][col] = this.AiSymbol;
    }
    private static int aiWin = 1;
    private static int aiLose = -1;
    private static int tie = 0;
    /***
    Minimax algorithm to find the optimal go for AI to play (maximize its evaluation value considering the other user plays optimally)
    Parra----bool AITurn: if (AITurn) maximize e, else if (userTurn) minimize e
    Return---int square: the square's number which is an optimal solution to go (1<<9)
          ---int: the evaluation value e
    ***/
    public int Minimax(out int square, bool AITurn)
    {
        /*
        stopping case
        */
        char winner;
        if (this.GameOver(out winner))
        {
            square = 0;
            if (winner == this.aiSymbol) return TicTacToe.aiWin;
            else if (winner == this.userSymbol) return TicTacToe.aiLose;
            else return TicTacToe.tie;
        }
        /*
        recursive case
        */
        int e= 0;
        int otherPlayerSquare = 0;
        int optimalGo = 0;
        //AI wants to maximize the evaluation value e (1>0>-1)
        if (AITurn)
        {
            int maxE = -2;
            for (int s=1; s <= TicTacToe.GridSize*TicTacToe.GridSize; s++)
            {
                int row = (s - 1) / TicTacToe.GridSize;
                int col = s - 1 - row * TicTacToe.GridSize;
                if (!this.isSquareOccupied(row, col))
                {
                    this.grid[row][col] = this.AiSymbol;
                    e = Minimax(out otherPlayerSquare,!AITurn);
                    if (e>maxE)
                    {
                        maxE = e;
                        optimalGo = s;
                    }
                    this.grid[row][col] = ' ';
                }
            }
            square = optimalGo;
            return maxE;
        }
        //the other player may play optimally by minimizing e (-1<0<1)
        else
        {
            int minE = 2;
            for (int s=1; s<=TicTacToe.GridSize*TicTacToe.GridSize; s++)
            {
                int row = (s - 1) / TicTacToe.gridSize;
                int col = s - 1 - row * TicTacToe.GridSize;
                if (!this.isSquareOccupied(row, col))
                {
                    this.grid[row][col] = this.UserSymbol;
                    e = Minimax(out otherPlayerSquare, !AITurn);
                    if (e < minE)
                    {
                        minE = e;
                        optimalGo = s; 
                    }
                    this.grid[row][col] = ' ';
                }
            }
            square = optimalGo;
            return minE;

        }
    }
    /***
    Utility method to check if a square has been played already
    ***/
    private bool isSquareOccupied(int row, int col)
    {
        return this.grid[row][col] != ' '; 
    }
    /***
    Check if the grid is still empty
    ***/
    private bool isEmpty()
    {
        for (int i=0; i<TicTacToe.GridSize; i++)
        {
            for (int j=0; j<TicTacToe.GridSize; j++)
            {
                if (isSquareOccupied(i, j))
                {
                    return false;
                }
            }
        }
        return true;
    }
    /***
    Check if the game is over
    Return--- boolean: over or not
          --- char: who wins
    ***/
    public bool GameOver(out char winner)
    {
        //Check rows
        for (int i = 0; i<TicTacToe.GridSize; i++)
        {
            if (this.grid[i][0] == this.grid[i][1] && this.grid[i][1] == this.grid[i][2] && this.grid[i][0] != ' ')
            {
                winner = this.grid[i][0];
                return true;
            }
        }
        //Check columns
        for (int j = 0; j < TicTacToe.GridSize; j++)
        {
           if (this.grid[0][j] == this.grid[1][j] && this.grid[1][j] == this.grid[2][j] && this.grid[0][j] != ' ')
           {
               winner = this.grid[0][j];
               return true;
           }
        }
        //check diagnols
        if (this.grid[0][0] != ' ' && this.grid[0][0] == this.grid[1][1] && this.grid[1][1] == this.grid[2][2]){
            winner = grid[0][0];
            return true;
        }

        if (this.grid[0][2] != ' ' && this.grid[0][2] == this.grid[1][1] && this.grid[1][1] == this.grid[2][0]){
            winner = grid[0][2];
            return true;
        }
        //check if there is an available spot to continue playing
        winner = ' ';
        for (int i = 0; i<TicTacToe.GridSize; i++)
        {
            for (int j = 0; j<TicTacToe.GridSize; j++)
            {
                if (this.grid[i][j] == ' ') return false;
            }
        }
        return true;
    }
    
}