#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
#endregion

namespace WheelChairGameLibrary.Helpers
{
    /// <summary>
    /// Helper for reading input from keyboard and gamepad. This class tracks both
    /// the current and previous state of both input devices, and implements query
    /// methods for high level input actions such as "move up through the menu"
    /// or "pause the game".
    /// </summary>
    public class InputState
    {

        Keys p1HighPunch = Keys.W;
        Keys p1LowPunch = Keys.S;
        Keys p1HighKick = Keys.E;
        Keys p1LowKick = Keys.D;
        Keys p1Left = Keys.Left;
        Keys p1Right = Keys.Right;

        Buttons p1bHighPunch = Buttons.X;
        Buttons p1bLowPunch = Buttons.A;
        Buttons p1bHighKick = Buttons.Y;
        Buttons p1bLowKick = Buttons.B;
        Buttons p1bLeft = Buttons.DPadLeft;
        Buttons p1bRight = Buttons.DPadRight;

        Keys p2HighPunch = Keys.I;
        Keys p2LowPunch = Keys.K;
        Keys p2HighKick = Keys.O;
        Keys p2LowKick = Keys.L;
        Keys p2Left = Keys.N;
        Keys p2Right = Keys.M;

        Buttons p2bHighPunch = Buttons.X;
        Buttons p2bLowPunch = Buttons.A;
        Buttons p2bHighKick = Buttons.Y;
        Buttons p2bLowKick = Buttons.B;
        Buttons p2bLeft = Buttons.DPadLeft;
        Buttons p2bRight = Buttons.DPadRight;


        #region Fields

        public const int MaxInputs = 4;

        public readonly KeyboardState[] CurrentKeyboardStates;
        public readonly GamePadState[] CurrentGamePadStates;

        public readonly KeyboardState[] LastKeyboardStates;
        public readonly GamePadState[] LastGamePadStates;

        public readonly bool[] GamePadWasConnected;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructs a new input state.
        /// </summary>
        public InputState()
        {
            CurrentKeyboardStates = new KeyboardState[MaxInputs];
            CurrentGamePadStates = new GamePadState[MaxInputs];

            LastKeyboardStates = new KeyboardState[MaxInputs];
            LastGamePadStates = new GamePadState[MaxInputs];

            GamePadWasConnected = new bool[MaxInputs];
        }


        #endregion

        #region Public Methods


        /// <summary>
        /// Reads the latest state of the keyboard and gamepad.
        /// </summary>
        public void Update()
        {
            for (int i = 0; i < MaxInputs; i++)
            {
                LastKeyboardStates[i] = CurrentKeyboardStates[i];
                LastGamePadStates[i] = CurrentGamePadStates[i];

                CurrentKeyboardStates[i] = Keyboard.GetState((PlayerIndex)i);
                CurrentGamePadStates[i] = GamePad.GetState((PlayerIndex)i);

                // Keep track of whether a gamepad has ever been
                // connected, so we can detect if it is unplugged.
                if (CurrentGamePadStates[i].IsConnected)
                {
                    GamePadWasConnected[i] = true;
                }
            }
        }


        /// <summary>
        /// Helper for checking if a key was newly pressed during this update. The
        /// controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When a keypress
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsKeyPressed(Keys key, PlayerIndex? controllingPlayer,
                                            out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return (CurrentKeyboardStates[i].IsKeyDown(key) &&
                        LastKeyboardStates[i].IsKeyUp(key));
            }
            else
            {
                // Accept input from any player.
                return (IsKeyPressed(key, PlayerIndex.One, out playerIndex) ||
                        IsKeyPressed(key, PlayerIndex.Two, out playerIndex) ||
                        IsKeyPressed(key, PlayerIndex.Three, out playerIndex) ||
                        IsKeyPressed(key, PlayerIndex.Four, out playerIndex));
            }
        }


        /// <summary>
        /// Helper for checking if a button was newly pressed during this update.
        /// The controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When a button press
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsButtonPressed(Buttons button, PlayerIndex? controllingPlayer,
                                                     out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return (CurrentGamePadStates[i].IsButtonDown(button) &&
                        LastGamePadStates[i].IsButtonUp(button));
            }
            else
            {
                // Accept input from any player.
                return (IsButtonPressed(button, PlayerIndex.One, out playerIndex) ||
                        IsButtonPressed(button, PlayerIndex.Two, out playerIndex) ||
                        IsButtonPressed(button, PlayerIndex.Three, out playerIndex) ||
                        IsButtonPressed(button, PlayerIndex.Four, out playerIndex));
            }
        }

        public bool IsButtonReleased(Buttons button, PlayerIndex? controllingPlayer,
                                                     out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return (CurrentGamePadStates[i].IsButtonUp(button) &&
                        LastGamePadStates[i].IsButtonDown(button));
            }
            else
            {
                // Accept input from any player.
                return (IsButtonPressed(button, PlayerIndex.One, out playerIndex) ||
                        IsButtonPressed(button, PlayerIndex.Two, out playerIndex) ||
                        IsButtonPressed(button, PlayerIndex.Three, out playerIndex) ||
                        IsButtonPressed(button, PlayerIndex.Four, out playerIndex));
            }
        }

        public bool IsButtonDown(Buttons button, PlayerIndex? controllingPlayer,
                                                     out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return (CurrentGamePadStates[i].IsButtonDown(button));
            }
            else
            {
                // Accept input from any player.
                return (IsButtonPressed(button, PlayerIndex.One, out playerIndex) ||
                        IsButtonPressed(button, PlayerIndex.Two, out playerIndex) ||
                        IsButtonPressed(button, PlayerIndex.Three, out playerIndex) ||
                        IsButtonPressed(button, PlayerIndex.Four, out playerIndex));
            }
        }


        /// <summary>
        /// Checks for a "menu select" input action.
        /// The controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When the action
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsMenuSelect(PlayerIndex? controllingPlayer,
                                 out PlayerIndex playerIndex)
        {
            return IsKeyPressed(Keys.Space, controllingPlayer, out playerIndex) ||
                   IsKeyPressed(Keys.Enter, controllingPlayer, out playerIndex) ||
                   IsButtonPressed(Buttons.A, controllingPlayer, out playerIndex) ||
                   IsButtonPressed(Buttons.Start, controllingPlayer, out playerIndex);
        }

        public bool IsMenuEnter(PlayerIndex? controllingPlayer,
                                 out PlayerIndex playerIndex)
        {
            return 
                   IsKeyPressed(Keys.Enter, controllingPlayer, out playerIndex) ||

                   IsButtonPressed(Buttons.Start, controllingPlayer, out playerIndex);
        }


        /// <summary>
        /// Checks for a "menu cancel" input action.
        /// The controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When the action
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsMenuCancel(PlayerIndex? controllingPlayer,
                                 out PlayerIndex playerIndex)
        {
            return IsKeyPressed(Keys.Escape, controllingPlayer, out playerIndex) ||
                   IsButtonPressed(Buttons.B, controllingPlayer, out playerIndex) ||
                   IsButtonPressed(Buttons.Back, controllingPlayer, out playerIndex);
        }


        /// <summary>
        /// Checks for a "menu up" input action.
        /// The controllingPlayer parameter specifies which player to read
        /// input for. If this is null, it will accept input from any player.
        /// </summary>
        public bool IsMenuUp(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;

            if (CurrentGamePadStates[(int)controllingPlayer].IsConnected)
                return IsButtonPressed(Buttons.DPadUp, controllingPlayer, out playerIndex) ||
                   IsButtonPressed(Buttons.LeftThumbstickUp, controllingPlayer, out playerIndex);
            else
                return IsKeyPressed(Keys.Up, controllingPlayer, out playerIndex);
                   
        }

        public bool IsMenuRight(PlayerIndex? controllingPlayer)
        {


            PlayerIndex playerIndex;
            if (CurrentGamePadStates[(int)controllingPlayer].IsConnected)
                return
                   IsButtonPressed(Buttons.DPadRight, controllingPlayer, out playerIndex) ||
                   IsButtonPressed(Buttons.LeftThumbstickRight, controllingPlayer, out playerIndex);
            else                
                return IsKeyPressed(Keys.Right, controllingPlayer, out playerIndex);
        }

        public bool IsMenuLeft(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;

            if (CurrentGamePadStates[(int)controllingPlayer].IsConnected)
                return
                       IsButtonPressed(Buttons.DPadLeft, controllingPlayer, out playerIndex) ||
                       IsButtonPressed(Buttons.LeftThumbstickLeft, controllingPlayer, out playerIndex);
            else
                return IsKeyPressed(Keys.Left, controllingPlayer, out playerIndex);
        }


        /// <summary>
        /// Checks for a "menu down" input action.
        /// The controllingPlayer parameter specifies which player to read
        /// input for. If this is null, it will accept input from any player.
        /// </summary>
        public bool IsMenuDown(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;

            if (CurrentGamePadStates[(int)controllingPlayer].IsConnected)
                return
                       IsButtonPressed(Buttons.DPadDown, controllingPlayer, out playerIndex) ||
                       IsButtonPressed(Buttons.LeftThumbstickDown, controllingPlayer, out playerIndex);
            else
                return IsKeyPressed(Keys.Down, controllingPlayer, out playerIndex);
        }


        /// <summary>
        /// Checks for a "pause the game" input action.
        /// The controllingPlayer parameter specifies which player to read
        /// input for. If this is null, it will accept input from any player.
        /// </summary>
        public bool IsPauseGame(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;

            return IsKeyPressed(Keys.Escape, controllingPlayer, out playerIndex) ||
                   IsButtonPressed(Buttons.Back, controllingPlayer, out playerIndex) ||
                   IsButtonPressed(Buttons.Start, controllingPlayer, out playerIndex);
        }


        public bool IsKeyDown(Keys key, PlayerIndex? controllingPlayer,
                                            out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return CurrentKeyboardStates[i].IsKeyDown(key);
            }
            else
            {
                // Accept input from any player.
                return (IsKeyDown(key, PlayerIndex.One, out playerIndex) ||
                        IsKeyDown(key, PlayerIndex.Two, out playerIndex) ||
                        IsKeyDown(key, PlayerIndex.Three, out playerIndex) ||
                        IsKeyDown(key, PlayerIndex.Four, out playerIndex));
            }
        }



        public bool IsKeyReleased(Keys key, PlayerIndex? controllingPlayer,
                                            out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return CurrentKeyboardStates[i].IsKeyUp(key) && LastKeyboardStates[i].IsKeyDown(key);
            }
            else
            {
                // Accept input from any player.
                return (IsKeyReleased(key, PlayerIndex.One, out playerIndex) ||
                        IsKeyReleased(key, PlayerIndex.Two, out playerIndex) ||
                        IsKeyReleased(key, PlayerIndex.Three, out playerIndex) ||
                        IsKeyReleased(key, PlayerIndex.Four, out playerIndex));
            }
        }





        public bool IsHighPunchPressed(PlayerIndex playerIndex)
        {
            if (playerIndex == PlayerIndex.One)
            {
                if (CurrentGamePadStates[(int)playerIndex].IsConnected)
                {
                    return IsButtonPressed(p1bHighPunch, playerIndex, out playerIndex);
                }
                else
                {
                    return IsKeyPressed(p1HighPunch, playerIndex, out playerIndex);
                }
            }
            else
            {
                if (CurrentGamePadStates[(int)playerIndex].IsConnected)
                {
                    return IsButtonPressed(p2bHighPunch, playerIndex, out playerIndex);
                }
                else
                {
                    return IsKeyPressed(p2HighPunch, playerIndex, out playerIndex);
                }
            }
        }




        public bool IsLowPunchPressed(PlayerIndex playerIndex)
        {
            if (playerIndex == PlayerIndex.One)
            {
                if (CurrentGamePadStates[(int)playerIndex].IsConnected)
                {
                    return IsButtonPressed(p1bLowPunch, playerIndex, out playerIndex);
                }
                else
                {
                    return IsKeyPressed(p1LowPunch, playerIndex, out playerIndex);
                }
            }
            else
            {
                if (CurrentGamePadStates[(int)playerIndex].IsConnected)
                {
                    return IsButtonPressed(p2bLowPunch, playerIndex, out playerIndex);
                }
                else
                {
                    return IsKeyPressed(p2LowPunch, playerIndex, out playerIndex);
                }
            }
        }



        public bool IsHighKickPressed(PlayerIndex playerIndex)
        {
            if (playerIndex == PlayerIndex.One)
            {
                if (CurrentGamePadStates[(int)playerIndex].IsConnected)
                {
                    return IsButtonPressed(p1bHighKick, playerIndex, out playerIndex);
                }
                else
                {
                    return IsKeyPressed(p1HighKick, playerIndex, out playerIndex);
                }
            }
            else
            {
                if (CurrentGamePadStates[(int)playerIndex].IsConnected)
                {
                    return IsButtonPressed(p2bHighKick, playerIndex, out playerIndex);
                }
                else
                {
                    return IsKeyPressed(p2HighKick, playerIndex, out playerIndex);
                }
            }
        }



        public bool IsLowKickPressed(PlayerIndex playerIndex)
        {
            if (playerIndex == PlayerIndex.One)
            {
                if (CurrentGamePadStates[(int)playerIndex].IsConnected)
                {
                    return IsButtonPressed(p1bLowKick, playerIndex, out playerIndex);
                }
                else
                {
                    return IsKeyPressed(p1LowKick, playerIndex, out playerIndex);
                }
            }
            else
            {
                if (CurrentGamePadStates[(int)playerIndex].IsConnected)
                {
                    return IsButtonPressed(p2bLowKick, playerIndex, out playerIndex);
                }
                else
                {
                    return IsKeyPressed(p2LowKick, playerIndex, out playerIndex);
                }
            }
        }



        public bool IsLeftPressed(PlayerIndex playerIndex)
        {
            if (playerIndex == PlayerIndex.One)
            {
                if (CurrentGamePadStates[(int)playerIndex].IsConnected)
                {
                    return IsButtonPressed(p1bLeft, playerIndex, out playerIndex);
                }
                else
                {
                    return IsKeyPressed(p1Left, playerIndex, out playerIndex);
                }
            }
            else
            {
                if (CurrentGamePadStates[(int)playerIndex].IsConnected)
                {
                    return IsButtonPressed(p2bLeft, playerIndex, out playerIndex);
                }
                else
                {
                    return IsKeyPressed(p2Left, playerIndex, out playerIndex);
                }
            }
        }




        public bool IsLeftRelease(PlayerIndex playerIndex)
        {
            if (playerIndex == PlayerIndex.One)
            {
                if (CurrentGamePadStates[(int)playerIndex].IsConnected)
                {
                    return IsButtonReleased(p1bLeft, playerIndex, out playerIndex);
                }
                else
                {
                    return IsKeyReleased(p1Left, playerIndex, out playerIndex);
                }
            }
            else
            {
                if (CurrentGamePadStates[(int)playerIndex].IsConnected)
                {
                    return IsButtonReleased(p2bLeft, playerIndex, out playerIndex);
                }
                else
                {
                    return IsKeyReleased(p2Left, playerIndex, out playerIndex);
                }
            }
        }


        public bool IsLeftDown(PlayerIndex playerIndex)
        {
            if (playerIndex == PlayerIndex.One)
            {
                if (CurrentGamePadStates[(int)playerIndex].IsConnected)
                {
                    return IsButtonDown(p1bLeft, playerIndex, out playerIndex);
                }
                else
                {
                    return IsKeyDown(p1Left, playerIndex, out playerIndex);
                }
            }
            else
            {
                if (CurrentGamePadStates[(int)playerIndex].IsConnected)
                {
                    return IsButtonDown(p2bLeft, playerIndex, out playerIndex);
                }
                else
                {
                    return IsKeyDown(p2Left, playerIndex, out playerIndex);
                }
            }
        }



        //right


        public bool IsRightPressed(PlayerIndex playerIndex)
        {
            if (playerIndex == PlayerIndex.One)
            {
                if (CurrentGamePadStates[(int)playerIndex].IsConnected)
                {
                    return IsButtonPressed(p1bRight, playerIndex, out playerIndex);
                }
                else
                {
                    return IsKeyPressed(p1Right, playerIndex, out playerIndex);
                }
            }
            else
            {
                if (CurrentGamePadStates[(int)playerIndex].IsConnected)
                {
                    return IsButtonPressed(p2bRight, playerIndex, out playerIndex);
                }
                else
                {
                    return IsKeyPressed(p2Right, playerIndex, out playerIndex);
                }
            }
        }




        public bool IsRightRelease(PlayerIndex playerIndex)
        {
            if (playerIndex == PlayerIndex.One)
            {
                if (CurrentGamePadStates[(int)playerIndex].IsConnected)
                {
                    return IsButtonReleased(p1bRight, playerIndex, out playerIndex);
                }
                else
                {
                    return IsKeyReleased(p1Right, playerIndex, out playerIndex);
                }
            }
            else
            {
                if (CurrentGamePadStates[(int)playerIndex].IsConnected)
                {
                    return IsButtonReleased(p2bRight, playerIndex, out playerIndex);
                }
                else
                {
                    return IsKeyReleased(p2Right, playerIndex, out playerIndex);
                }
            }
        }


        public bool IsRightDown(PlayerIndex playerIndex)
        {
            if (playerIndex == PlayerIndex.One)
            {
                if (CurrentGamePadStates[(int)playerIndex].IsConnected)
                {
                    return IsButtonDown(p1bRight, playerIndex, out playerIndex);
                }
                else
                {
                    return IsKeyDown(p1Right, playerIndex, out playerIndex);
                }
            }
            else
            {
                if (CurrentGamePadStates[(int)playerIndex].IsConnected)
                {
                    return IsButtonDown(p2bRight, playerIndex, out playerIndex);
                }
                else
                {
                    return IsKeyDown(p2Right, playerIndex, out playerIndex);
                }
            }
        }


        #endregion
    }
}
