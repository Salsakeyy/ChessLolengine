using System;
using Backend.Model;
using Backend.Model.Pieces;
using Type = Backend.Model.Pieces.Type;

namespace Backend.Utility
{
    public class FenTranslator
    {
        public static string FenNotation(Container container)
        {
            var board = container.Board;

            var result = "";
            for (var i = 0; i < board.Size; i++)
            {
                var emptySquareNumber = 0;
                for (var j = 0; j < board.Size; j++)
                {
                    var square = board.Squares[j, i];
                    if (square.Piece != null)
                    {
                        if (emptySquareNumber != 0)
                        {
                            result += emptySquareNumber;
                            emptySquareNumber = 0;
                        }
                        var c = ' ';

                        switch (square.Piece.Type)
                        {
                            case Type.Bishop:
                                c = 'b';
                                break;
                            case Type.King:
                                c = 'k';
                                break;
                            case Type.Queen:
                                c = 'q';
                                break;
                            case Type.Pawn:
                                c = 'p';
                                break;
                            case Type.Knight:
                                c = 'n';
                                break;
                            case Type.Rook:
                                c = 'r';
                                break;
                        }

                        result += square.Piece.Color == Color.White ? c.ToString().ToUpper() : c.ToString();
                    }
                    else
                    {
                        emptySquareNumber++;
                    }
                }
                if (emptySquareNumber != 0)
                    result += emptySquareNumber;
                result += '/';
            }

            result += ' ';

            result += container.Moves[container.Moves.Count - 1].PieceColor == Color.White ? 'b' : 'w';

            result += ' ';

            Piece blackRookQueen = null;
            Piece blackRookKing = null;
            Piece whiteRookQueen = null;
            Piece whiteRookKing = null;

            Piece blackKing = null;
            Piece whiteKing = null;

            Square enPassant = null;
            foreach (var square in board.Squares)
                if (square?.Piece?.Type == Type.King)
                    if (square.Piece.Color == Color.White)
                        whiteKing = square.Piece;
                    else
                        blackKing = square.Piece;
                else if (square?.Piece?.Type == Type.Rook)
                    if (square.X == 0)
                    {
                        if (square.Piece.Color == Color.White)
                            whiteRookQueen = square.Piece;
                        else
                            blackRookQueen = square.Piece;
                    }
                    else
                    {
                        if (square.Piece.Color == Color.White)
                            whiteRookKing = square.Piece;
                        else
                            blackRookKing = square.Piece;
                    }


                else if (square?.Piece?.Type == Type.Pawn)
                    if ((square.Piece as Pawn)?.EnPassant == true)
                        if (square?.Piece.Color == container.Moves[container.Moves.Count - 1].PieceColor)
                            enPassant =
                                board.Squares[square.X, square.Piece.Color == Color.White ? square.Y + 1 : square.Y - 1];

            //CastlingRule
            var bRq = !blackRookQueen?.HasMoved == true;
            var bRk = !blackRookKing?.HasMoved == true;
            var wRq = !whiteRookQueen?.HasMoved == true;
            var wRk = !whiteRookKing?.HasMoved == true;

            var wK = !whiteKing.HasMoved;
            var bK = !blackKing.HasMoved;

            if (wK)
            {
                if (wRk)
                    result += 'K';
                if (wRq)
                    result += 'Q';
            }
            if (bK)
            {
                if (bRk)
                    result += 'k';
                if (bRq)
                    result += 'q';
            }

            if (!(bK && (bRk || bRq))
                && !(wK && (wRk || wRq)))
                result += '-';

            result += ' ';

            //En passant

            if (enPassant != null)
                result += enPassant.ToString().ToLower();
            else
                result += '-';

            result += ' ';

            //Halfmove since last capture
            result += container.HalfMoveSinceLastCapture;

            result += ' ';

            result += (int) Math.Ceiling((double) (container.Moves.Count/2));

            return result;
        }
    }
}