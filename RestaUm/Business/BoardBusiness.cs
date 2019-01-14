using System;
using RestaUm.Model;
using RestaUm.Enum;

namespace RestaUm.Business
{
    public class BoardBusiness
    {
        private Board _board;
        public BoardBusiness(Model.Board board)
        {
            this._board = board;
        }

        public Board CreateNew()
        {
            this.ConfigBoardSize();
            for (var x = 0; x <= this._board.SizeX; x++)
            {
                for (var y = 0; y <= this._board.SizeY; y++)
                {
                    this._board.Pieces[x, y] = !this.FildIsNull(x, y)
                        ? new Piece()
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

                    if (this.FildCenter(x, y))
                    {
                        this._board.Pieces[x, y].StatusPiece = StatusPiece.Livre;
                    }
                }
            }
            return this._board;
        }

        private void ConfigBoardSize()
        {
            this._board.SizeX = 7;
            this._board.SizeY = 7;
            this._board.Pieces = new Piece[(this._board.SizeX + 1), (this._board.SizeY + 1)];
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
