﻿using System;

namespace Backend.Model.Pieces
{
    [Serializable]
    public abstract class Piece
    {
        protected Piece(Color color, Square square)
        {
            Color = color;
            Square = square;
        }

        protected Piece(Color color)
        {
            Color = color;
            Square = null;
        }

        public Color Color { get; }
        public Square Square { get; set; }

        /// <summary>
        ///     HasMoved
        /// </summary>
        /// <value>
        ///     Return true if the piece has already moved
        /// </value>
        public bool HasMoved { get; set; } = false;

        public Type Type { get; set; }


        public abstract Piece Clone(Square square);
    }

    [Serializable]
    public enum Type
    {
        Bishop,
        King,
        Queen,
        Pawn,
        Knight,
        Rook
    }

    [Serializable]
    public enum Color
    {
        White,
        Black
    }
}