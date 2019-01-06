using System;
using Xunit;
using RestaUm.Business;
using RestaUm.Enum;

namespace RestaUm.Test.Board
{
    public class BoardTest
    {
        private BoardBusiness _boardBusiness;
        public BoardTest()
        {
            this._boardBusiness = new BoardBusiness(new Model.Board());
        }

        [Fact]
        public void TesteDeveCriarUmTabuleiroValidoDeRestaUm()
        {
            Model.Board board = this._boardBusiness.CreateNew();
            Equals(null, board.Pieces[0, 0]);
        }

        [Fact]
        public void TesteDeveTerAPecaCentralInativaQuandoCriado()
        {
            Model.Board board = this._boardBusiness.CreateNew();
            Equals(StatusPiece.Inactive, board.Pieces[3, 3].StatusPiece);
        }
    }
}
