﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Backend.Core;
using Backend.Model;
using Backend.Model.Pieces;
using Backend.Utility;
using Type = Backend.Model.Pieces.Type;

namespace Backend.IA
{
    public class UciProcessController : PlayerControler
    {
        private Container _container;
        private Process _uciProcess;
        private string _search;

        public UciProcessController(Container container)
        {
            _container = container;
            _search = "go movetime 1000";
            _uciProcess = new Process
            {
                StartInfo =
                {
                    FileName = "Files/stockfish_64.exe",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    CreateNoWindow = true
                }
            };

            _uciProcess.Start();
            _uciProcess.StandardInput.WriteLine("uci");

            var output = "";
            while (output != "uciok")
            {
                output = _uciProcess.StandardOutput.ReadLine();
                Console.WriteLine(output);
            }
            _uciProcess.StandardInput.WriteLine("ucinewgame");
            Console.WriteLine("ucinewgame");
            _uciProcess.StandardInput.WriteLine("setoption name Threads value {0}", Environment.ProcessorCount);
        }

        public UciProcessController(Container container, string searchType, int skillLevel, int searchValue)
        {
            _container = container;
            _search = "go " + searchType + " " + searchValue;

            _uciProcess = new Process
            {
                StartInfo =
                {
                    FileName = "Files/stockfish_64.exe",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    CreateNoWindow = true
                }
            };

            _uciProcess.Start();
            _uciProcess.StandardInput.WriteLine("uci");


            var output = "";
            while (output != "uciok")
            {
                output = _uciProcess.StandardOutput.ReadLine();
                Console.WriteLine(output);
            }
            _uciProcess.StandardInput.WriteLine("ucinewgame");
            Console.WriteLine("ucinewgame");
            _uciProcess.StandardInput.WriteLine("setoption name Threads value {0}", Environment.ProcessorCount);
            _uciProcess.StandardInput.WriteLine("setoption name Skill Level value {0}", skillLevel);


        }

        public override void Play(Move move)
        {
            PlayAsync();
        }

        private async void PlayAsync()
        {
            Console.WriteLine(FenTranslator.FenNotation(_container));
            await _uciProcess.StandardInput.WriteLineAsync("position fen " + FenTranslator.FenNotation(_container));
            await _uciProcess.StandardInput.WriteLineAsync(_search);

            var input = new string(' ', 1);

            while (input == null || !input.Contains("bestmove"))
            {
                input = await _uciProcess.StandardOutput.ReadLineAsync();
                if (input != null)
                    Console.WriteLine(input);
            }

            if (!input.Contains("(none)"))
            {
                var startCoordinate = new Coordinate(input[9] - 'a', 7 - (input[10] - '1'));
                var targCoordinate = new Coordinate(input[11] - 'a', 7 - (input[12] - '1'));

                if (input.Length > 13 && input[13] != ' ')
                    switch (input[13])
                    {
                        case 'q':
                            Move(new Move(_container.Board.SquareAt(startCoordinate),
                                _container.Board.SquareAt(targCoordinate), Type.Pawn, Player.Color, Type.Queen));
                            break;
                        case 'r':
                            Move(new Move(_container.Board.SquareAt(startCoordinate),
                                _container.Board.SquareAt(targCoordinate), Type.Pawn, Player.Color, Type.Rook));
                            break;
                        case 'b':
                            Move(new Move(_container.Board.SquareAt(startCoordinate),
                                _container.Board.SquareAt(targCoordinate), Type.Pawn, Player.Color, Type.Bishop));
                            break;
                        case 'n':
                            Move(new Move(_container.Board.SquareAt(startCoordinate),
                                _container.Board.SquareAt(targCoordinate), Type.Pawn, Player.Color, Type.Knight));
                            break;
                    }
                else
                    Move(new Move(_container.Board.PieceAt(startCoordinate), _container.Board.SquareAt(targCoordinate)));
            }
        }

        public override void Move(Move move)
        {
            Player.Move(move);
        }

        public override void InvalidMove(List<string> reasonsList)
        {
            //throw new System.NotImplementedException();
        }

        public override List<Square> PossibleMoves(Piece piece)
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            StopAsync();
        }

        private async void StopAsync()
        {
            await _uciProcess.StandardInput.WriteLineAsync("stop");
        }
    }
}