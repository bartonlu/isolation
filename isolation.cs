using System;
using static System.Console;

namespace Bme121
{
    static class Program
    {
        static int[ , ] board = new int[ 6, 8 ]; // Board for game
        // 0 if tile removed
        // 1 if tile not removed and not occupied
        // 2 if occupied by Player A
        // 3 if occupied by Player B
        
        // Stores usernames
        static string namePlayerA = "Player A";
        static string namePlayerB = "Player B";
        
        // Stores coordinates of platforms
        static int rowPlatformA    = 2;
        static int columnPlatformA = 0;
        static int rowPlatformB    = 3;
        static int columnPlatformB = 7;
        
        // Stores current positions of Player A and B
        static int rowPlayerA = 2;
        static int colPlayerA = 0;
        static int rowPlayerB = 3;
        static int colPlayerB = 7;
        
        // Array of letters used to convert letters to corresponding numbers
        static string[ ] letters = { "a","b","c","d","e","f","g","h","i","j","k","l",
            "m","n","o","p","q","r","s","t","u","v","w","x","y","z"};
        
        // Runs game as Main method
        static void Main( )
        {
            // Prompts players for usernames
            namePlayerA = Usernames( "Player A" );
            namePlayerB = Usernames( "Player B" );
            
            // Prompts user for dimensions of board
            board = new int[ Dimensions("rows"), Dimensions("columns") ];
            
            // Sets entire board to unoccupied
            for ( int r = 0; r < board.GetLength(0); r++ )
                for ( int c = 0; c < board.GetLength(1); c++ )
                    board[ r,c ] = 1;
            
            // Prompts Player A for starting platform and sets platform as occupied by Player A 
            string platformA = StartingPlatform( "Player A" ).ToString();
            rowPlatformA     = Array.IndexOf( letters, platformA[0].ToString() );
            columnPlatformA  = Array.IndexOf( letters, platformA[2].ToString() );
            rowPlayerA       = rowPlatformA;
            colPlayerA       = columnPlatformA;
            board[ rowPlatformA, columnPlatformA ] = 2;
            
            // Prompts Player B for starting platform and sets platform as occupied by Player B
            string platformB = StartingPlatform( "Player B" ).ToString();
            rowPlatformB     = Array.IndexOf( letters, platformB[0].ToString() );
            columnPlatformB  = Array.IndexOf( letters, platformB[2].ToString() );
            rowPlayerB       = rowPlatformB;
            colPlayerB       = columnPlatformB;
            board[ rowPlatformB, columnPlatformB ] = 3;
            
            DrawGameBoard( );
            
            WriteLine( "This is your starting board. To make a move, enter coordinates in the form 'abcd'." );
            WriteLine( "'a' and 'b' represent the row and column you want to move your pawn to, respectively." );
            WriteLine( "'c' and 'd' represent the row and column of the tile you want to remove, respectively." );
            
            // Alternates between Player A or Player B to enter moves - does not check for winner
            while (true) {
                MakeMove("Player A");
                DrawGameBoard();
                MakeMove("Player B");
                DrawGameBoard();
            }
        }
        
        // Prompts players for usernames
        static string Usernames(string whichPlayer)
        {
            if (whichPlayer == "Player A") {
                Write("Enter your name [default Player A]: ");
                string namePlayerA = ReadLine();
                
                // If player presses enter, returns default username
                if (namePlayerA.Length == 0) return "Player A";
                else                         return namePlayerA;
            }
            else {
                Write("Enter your name [default Player B]: ");
                string namePlayerB = ReadLine();

                // If player presses enter, returns default username
                if (namePlayerB.Length == 0) return "Player B";
                else                         return namePlayerB;
            }
        }
        
        // Prompts user for number of rows if "rows" is passed as argument
        // Prompts user for number of columns if "columns" is passed as argument
        static int Dimensions(string rowsOrColumns)
        {
            int dimension; // variable is used to test the dimension for validity
            bool isValidDimension = false;
            
            if (rowsOrColumns == "rows")
                Write("Enter a number of rows between 4 and 26 inclusive [default 6]: ");
            else
                Write("Enter a number of columns between 4 and 26 inclusive [default 8]: ");
            
            string enteredDimension = ReadLine( );
            
            // Continues to prompt user until a valid dimension is inputted
            while (!isValidDimension)
            {
                // Checks if user pressed enter for default number of rows/columns
                if (enteredDimension.Length == 0 && rowsOrColumns == "rows")    return 6;
                if (enteredDimension.Length == 0 && rowsOrColumns == "columns") return 8;
                
                // Checks if the enteredDimension can be parsed into an integer
                if (!int.TryParse(enteredDimension, out dimension))
                {
                    Write("Sorry! That is not a valid integer. Please try again: ");
                    enteredDimension = ReadLine();
                }
                else if (dimension < 4 || dimension > 26)
                {
                    Write("Sorry! Dimensions can only be between 4 and 26 inclusive. Please try again: ");
                    enteredDimension = ReadLine();
                }
                else isValidDimension = true;
            }
            
            return int.Parse(enteredDimension);
        }
        
        // Sets the starting platforms for Player A and B
        static string StartingPlatform(string whichPlayer)
        {
            bool isValidPlatform = false;
            bool isValidFormat   = false;
            
            // Row and column variables used to store the user's input
            int row    = -1;
            int column = -1;
            
            if (whichPlayer == "Player A") {
                WriteLine($"{namePlayerA}, please enter your starting platform [default c,a]");
                Write("'c,a' means your default starting platform is on row c and column a: ");
            }
            else
                Write($"{namePlayerB}, please enter your starting platform [default d,h]: ");
            
            string platform = ReadLine();
            
            // Continues asking user for platform until platform is valid
            while(!isValidPlatform)
            {
                // Checks if user pressed enter for default platforms
                if ( platform.Length == 0 && whichPlayer == "Player A" ) return "c,a";
                if ( platform.Length == 0 && whichPlayer == "Player B"
                     && 7 <= board.GetLength(1) - 1 )                    return "d,h";
                
                // Checks if the move is of valid format
                if (platform.Length != 3) {
                    Write("Sorry! Please type your move in as three characters (e.g. b,d): ");
                    platform = ReadLine();
                }
                else if (    !(platform[0] >= 'a' && platform[0] <= 'z')
                          || !(platform[2] >= 'a' && platform[2] <= 'z') ) {
                    Write("Sorry, your coordinates aren't in lowercase letter form! Please try again: ");
                    platform = ReadLine();                    
                }
                else if ( platform[1] != ',') {
                    Write("Sorry, did you mean to put a comma between your numbers? Please try again: ");
                    platform = ReadLine();
                }
                // Else, the format is valid and the row and column are stored
                else {
                    row    = Array.IndexOf( letters, platform[0].ToString() );
                    column = Array.IndexOf( letters, platform[2].ToString() );
                    isValidFormat = true;
                }
                
                // Only runs after it has been determined that the format is valid 
                if (isValidFormat)
                {
                    // Checks if the platform is on the board or is the same platform as other player
                    if ( row > board.GetLength(0) - 1 || column > board.GetLength(1) - 1 ) {
                        Write("Sorry! That tile does not exist on the board. Please try again: ");
                        platform = ReadLine();
                    }
                    else if ( whichPlayer == "Player B" 
                              && row      == rowPlatformA 
                              && column   == columnPlatformA ) {
                        Write("Sorry! Your platform is the same as Player A. Please try again: ");
                        platform = ReadLine();
                    }
                    else isValidPlatform = true;
                    
                    isValidFormat = false; // Resets variable to allow format to be checked again
                }
            }
            return platform;
        }
        
        // Allows each player to make a move
        static void MakeMove(string whichPlayer)
        {
            // Variables used to store coordinates of pawn and removed tile move
            int a, b, c, d;
            a = b = c = d = -1;
            
            bool isValidMove = false;
            bool isValidFormat = false; // Valid format means written as 4 lowercase English characters
            bool isValidPawnMove = false;
            
            if (whichPlayer == "Player A") Write($"{namePlayerA}, enter your move: ");
            else                           Write($"{namePlayerB}, enter your move: ");
            
            string move = ReadLine();
            
            // Continuously asks the user for their move until their move is valid.
            while (!isValidMove)
            {
                // This if-else block checks if the format of the move is valid
                if (move.Length != 4) {
                    Write("Sorry, your move does not have exactly 4 coordinates! Please try again: ");
                    move = ReadLine();
                }
                else if (    !(move[0] >= 'a' && move[0] <= 'z')
                          || !(move[1] >= 'a' && move[1] <= 'z')
                          || !(move[2] >= 'a' && move[2] <= 'z')
                          || !(move[3] >= 'a' && move[3] <= 'z') ) {
                    Write("Sorry, your coordinates aren't in lowercase letter form! Please try again: ");
                    move = ReadLine();
                }
                // Else, stores the characters as numbers into variables a, b, c, d
                else {
                    a = Array.IndexOf( letters, move[0].ToString() );
                    b = Array.IndexOf( letters, move[1].ToString() );
                    c = Array.IndexOf( letters, move[2].ToString() );
                    d = Array.IndexOf( letters, move[3].ToString() );
                    isValidFormat = true;
                }
                
                // This if-else block checks if the move is valid for the board
                if (isValidFormat)
                {
                    // Checks if moves are on existent tiles and that both moves are not the same
                    if ( (    a > board.GetLength(0) - 1
                           || b > board.GetLength(1) - 1
                           || c > board.GetLength(0) - 1
                           || d > board.GetLength(1) - 1
                           || (a == c && b == d) ) 
                           && !isValidPawnMove            )
                    {
                        if (    a > board.GetLength(0) - 1 || b > board.GetLength(1) - 1 
                             || c > board.GetLength(0) - 1 || d > board.GetLength(1) - 1  ) {
                            Write("Sorry, some of your moves are on nonexistent tiles! Please try again: ");
                            move = ReadLine();
                        }
                        else if ( a == c && b == d ) {
                            Write("Sorry, your pawn move and removed tile move are the same! Please try again: ");
                            move = ReadLine();
                        }
                        else {}
                        
                        // Resets isValidFormat so that the move can be checked for format again
                        isValidFormat = false;
                    }
                    // Checks if pawn move is valid
                    else if ( !isValidPawnMove ) {
                        if ( whichPlayer == "Player A" && 
                             ( Math.Abs( rowPlayerA - a ) > 1 || Math.Abs( colPlayerA - b ) > 1 ) ) {
                            Write("Sorry, your pawn move is not on an adjacent tile! Please try again: ");
                            move = ReadLine();
                        }
                        else if ( whichPlayer == "Player B" &&
                                  ( Math.Abs( rowPlayerB - a ) > 1 || Math.Abs( colPlayerB - b ) > 1 ) ) {
                            Write("Sorry, your pawn move is not on an adjacent tile! Please try again: ");
                            move = ReadLine();
                        }
                        // If Player A is making a move, checks if Player B is occupying tile
                        else if ( whichPlayer == "Player A" && board[a,b] == 3 ) {
                            Write("Sorry, Player B is occupying your pawn move tile! Please try again: ");
                            move = ReadLine();
                        }
                        // If Player B is making a move, checks if Player A is occupying tile
                        else if ( whichPlayer == "Player B" && board[a,b] == 2 ) {
                            Write("Sorry, Player A is occupying your pawn move tile! Please try again: ");
                            move = ReadLine();
                        }
                        else if ( board[a,b] == 0 ) {
                            Write("Sorry, you are trying to put your pawn on a removed tile! Please try again: ");
                            move = ReadLine();
                        }
                        else isValidPawnMove = true;
                        
                        isValidFormat = false; // Resets variable
                    }
                    // Checks if removed tile is valid
                    else if ( board[c,d] != 1
                              || (c == rowPlatformA && d == columnPlatformA) 
                              || (c == rowPlatformB && d == columnPlatformB) )
                    {
                        // Checks if Player B is occupying the tile that Player A wants to remove
                        if ( whichPlayer == "Player A" && board[c,d] == 3 ) {
                            Write("Sorry, Player B is occupying the tile you want to remove! Please try again: ");
                            move = ReadLine();
                        }
                        // Checks if Player A is occupying the tile that Player B wants to remove
                        else if ( whichPlayer == "Player B" && board[c,d] == 2 ) {
                            Write("Sorry, Player A is occupying the tile you want to remove! Please try again: ");
                            move = ReadLine();
                        }
                        else if ( board[c,d] == 0 ) {
                            Write("Sorry, that tile has already been removed! Please try again: ");
                            move = ReadLine();
                        }
                        else if (    (c == rowPlatformA && d == columnPlatformA) 
                                  || (c == rowPlatformB && d == columnPlatformB) ) {
                            Write("Sorry, platform tiles cannot be removed! Please try again: ");
                            move = ReadLine();
                        }
                        else isValidMove = true;
                        
                        isValidFormat = false; // Resets variables
                        isValidPawnMove = false;
                    }
                    else isValidMove = true;
                }
            }
            
            // Moves pawn
            if (whichPlayer == "Player A") {
                board[rowPlayerA, colPlayerA] = 1; // Sets Player A's previous position to unoccupied
                board[a,b] = 2; // Moves Player A to position a,b
                
                // Updates coordinates of Player A
                rowPlayerA = a;
                colPlayerA = b;
            }
            else {
                board[rowPlayerB, colPlayerB] = 1; // Sets Player B's previous position to unoccupied
                board[a,b] = 3; // Moves Player B to position a,b
                
                // Updates coordinates of Player B
                rowPlayerB = a;
                colPlayerB = b;
            }
            
            board[c,d] = 0; // Removes tile
        }
        
        // Draws the game board
        static void DrawGameBoard( )
        {
            const string h  = "\u2500"; // horizontal line
            const string v  = "\u2502"; // vertical line
            const string tl = "\u250c"; // top left corner
            const string tr = "\u2510"; // top right corner
            const string bl = "\u2514"; // bottom left corner
            const string br = "\u2518"; // bottom right corner
            const string vr = "\u251c"; // vertical join from right
            const string vl = "\u2524"; // vertical join from left
            const string hb = "\u252c"; // horizontal join from below
            const string ha = "\u2534"; // horizontal join from above
            const string hv = "\u253c"; // horizontal vertical cross
            const string bb = "\u25a0"; // block
            
            // Draws the top coordinates
            Write( "   " );
            for( int c = 0; c < board.GetLength( 1 ); c ++ )
                Write( "  {0} ", letters[ c ] );
            WriteLine( );
            
            // Draws the top board boundary.
            Write( "   " );
            for( int c = 0; c < board.GetLength( 1 ); c ++ )
            {
                if( c == 0 ) Write( tl );
                Write( "{0}{0}{0}", h );
                if( c == board.GetLength( 1 ) - 1 ) Write( "{0}", tr ); 
                else                                Write( "{0}", hb );
            }
            WriteLine( );
            
            // Draws the board rows
            for( int r = 0; r < board.GetLength( 0 ); r ++ )
            {
                Write( " {0} ", letters[ r ] );
                
                // Draws the row contents.
                for( int c = 0; c < board.GetLength( 1 ); c ++ )
                {
                    if( c == 0 ) Write( v );
                    
                    // If the tile is unoccupied and not a platform tile, keeps tile blank
                    if( board[ r, c ] == 1 &&
                        ( !( r == rowPlatformA && c == columnPlatformA ) &&
                          !( r == rowPlatformB && c == columnPlatformB )    ) )      
                                                  Write( "{0}{1}", "   ", v );
                    else if( board[r, c] == 2 )   Write( "{0}{1}", " A ", v );
                    else if( board[r, c] == 3)    Write( "{0}{1}", " B ", v );
                    // If the tile is a platform tile, draws a block
                    else if( board[ r, c ] == 1 &&
                             ( ( r == rowPlatformA && c == columnPlatformA ) ||
                               ( r == rowPlatformB && c == columnPlatformB )    ) )
                                                  Write( " {0}{1}",  bb , v );
                    // If the  tile is removed, draws an 'X'
                    else                          Write( "{0}{1}", " X ", v );
                }
                WriteLine( );
                
                // Draws the boundary after the row
                if( r != board.GetLength( 0 ) - 1 )
                { 
                    Write( "   " );
                    for( int c = 0; c < board.GetLength( 1 ); c ++ )
                    {
                        if( c == 0 ) Write( vr );
                        Write( "{0}{0}{0}", h );
                        if( c == board.GetLength( 1 ) - 1 ) Write( "{0}", vl ); 
                        else                                Write( "{0}", hv );
                    }
                    WriteLine( );
                }
                else
                {
                    Write( "   " );
                    for( int c = 0; c < board.GetLength( 1 ); c ++ )
                    {
                        if( c == 0 ) Write( bl );
                        Write( "{0}{0}{0}", h );
                        if( c == board.GetLength( 1 ) - 1 ) Write( "{0}", br ); 
                        else                                Write( "{0}", ha );
                    }
                    WriteLine( );
                }
            }
        }
    }
}
