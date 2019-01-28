using RestaUm.Enum;

namespace RestaUm.Model
{
    public class Board
    {
        public Piece[,] Pieces;

        public Board CreateNewSolitaireBoard()
        {
            this.Pieces = new Piece[7, 7];
            for (var x = 0; x < this.Pieces.GetLength(0); x++)
            {
                for (var y = 0; y < this.Pieces.GetLength(1); y++)
                {
                    this.Pieces[x, y] = this.ThisPiceIs(x, y);

                    if (this.FildCenter(x, y))
                    {
                        this.Pieces[x, y].StatusPiece = StatusPiece.Livre;
                    }
                }
            }
            return this;
        }

        private Piece ThisPiceIs(int x, int y)
        {
            return !this.FildIsNull(x, y) ? new Piece()
            {
                StatusPiece = StatusPiece.Activo,
                LocationX = x,
                LocationY = y
            }
            : new Piece()
            {
                StatusPiece = StatusPiece.Inactive,
                LocationX = x,
                LocationY = y
            };
        }

        private bool FildCenter(int x, int y)
        {
            return ((x == 3) && (y == 3));
        }

        private bool FildIsNull(int x, int y)
        {
            return ((x < 2 || x > 4) && (y < 2 || y > 4));
        }
    }
}
