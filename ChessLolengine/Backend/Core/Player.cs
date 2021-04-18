﻿using System.Collections.Generic;
using Backend.Model;
using Backend.Model.Pieces;

namespace Backend.Core
{
    public class Player
    {
        public Color Color { get; internal set; }

        private PlayerControler _playerControler;

        public Game Game { get; set; }

        public Player(Color color, PlayerControler playerControler)
        {
            Color = color;
            _playerControler = playerControler;
        }

        /// <summary>
        /// Notifie le joueur que c'est à son tour de jouer et que le Game peut recevoir un mouvement de sa part.
        /// Tant que ce mouvement n'est pas valide, cette méthode est appelée.
        /// </summary>
        public void Play(Move move) => _playerControler.Play(move);

        public void Stop() => _playerControler.Stop();

        public List<Square> PossibleMoves(Piece piece) => Game.PossibleMoves(piece);

        public void Move(Move move)
        {
            MoveDone?.Invoke(this, move);
        }

        public delegate void MoveHandler(Player sender, Move move);
        public event MoveHandler MoveDone;
    }
}
