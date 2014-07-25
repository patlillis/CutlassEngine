﻿using System;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Input;

namespace Cutlass.Utilities
{
    /// <summary>
    /// Game settings, stored in a custom xml file. The reason for this is
    /// we want to be able to store our game data on the Xbox360 too.
    /// On the PC we could just use a Settings/config file and have all the
    /// code autogenerated for us, but this way it works both on PC and Xbox.
    /// Note: The default instance for the game settings is in this class,
    /// this way we get the same behaviour as for normal Settings files!
    /// </summary>
    public class GameSettingsManager
    {
        [Serializable]
        public class GameSettings
        {
            /// <summary>Player name</summary>
            public string PlayerName
            {
                get { return _PlayerName; }
                set
                {
                    if (_PlayerName != value)
                        _NeedSave = true;
                    _PlayerName = value;
                }
            }
            private string _PlayerName = "Player";

            /// <summary>Resolution width</summary>
            public int ResolutionWidth
            {
                get { return _ResolutionWidth; }
                set
                {
                    if (_ResolutionWidth != value)
                    {
                        _NeedSave = true;
                        ResolutionChangesToApply = true;
                    }
                    _ResolutionWidth = value;
                }
            }
            private int _ResolutionWidth = 0;

            /// <summary>Resolution height</summary>
            public int ResolutionHeight
            {
                get { return _ResolutionHeight; }
                set
                {
                    if (_ResolutionHeight != value)
                    {
                        _NeedSave = true;
                        ResolutionChangesToApply = true;
                    }
                    _ResolutionHeight = value;
                }
            }
            private int _ResolutionHeight = 0;

            /// <summary>Is Fullscreen</summary>
            public bool IsFullscreen
            {
                get { return _IsFullscreen; }
                set
                {
                    if (_IsFullscreen != value)
                    {
                        _NeedSave = true;
                        ResolutionChangesToApply = true;
                    }
                    _IsFullscreen = value;
                }
            }
            private bool _IsFullscreen = false;

            /// <summary>Is Borderless</summary>
            public bool IsBorderless
            {
                get { return _IsBorderless; }
                set
                {
                    if (_IsBorderless != value)
                    {
                        _NeedSave = true;
                        ResolutionChangesToApply = true;
                    }
                    _IsBorderless = value;
                }
            }
            private bool _IsBorderless = false;

            /// <summary>Locale</summary>
            public string Locale
            {
                get { return _Locale; }
                set
                {
                    if (_Locale != value)
                        _NeedSave = true;
                    _Locale = value;
                }
            }
            private string _Locale = "en-US";

            /// <summary>Devastating Insults</summary>
            public bool Insults
            {
                get { return _Insults; }
                set
                {
                    if (_Insults != value)
                        _NeedSave = true;
                    _Insults = value;
                }
            }
            private bool _Insults = false;

            /// <summary>Ocean Color</summary>
            public int OceanColor
            {
                get { return _OceanColor; }
                set
                {
                    if (_OceanColor != value)
                        _NeedSave = true;
                    _OceanColor = value;
                }
            }
            private int _OceanColor = 0;

            /// <summary>Sound volume</summary>
            public int SfxVolume
            {
                get { return _SfxVolume; }
                set
                {
                    if (_SfxVolume != value)
                        _NeedSave = true;
                    _SfxVolume = value;
                }
            }
            private int _SfxVolume = 80;

            /// <summary>Music volume</summary>
            public int MusicVolume
            {
                get { return _MusicVolume; }
                set
                {
                    if (_MusicVolume != value)
                        _NeedSave = true;
                    _MusicVolume = value;
                }
            }
            private int _MusicVolume = 60;

            /// <summary>Controller sensitivity</summary>
            public float ControllerSensitivity
            {
                get { return _ControllerSensitivity; }
                set
                {
                    if (_ControllerSensitivity != value)
                        _NeedSave = true;
                    _ControllerSensitivity = value;
                }
            }
            private float _ControllerSensitivity = 0.5f;

            /// <summary>Jump Key</summary>
            public Keys JumpKey
            {
                get { return _JumpKey; }
                set
                {
                    if (_JumpKey != value)
                        _NeedSave = true;
                    _JumpKey = value;
                }
            }
            private Keys _JumpKey = Keys.Space;


            /// <summary>Up Key</summary>
            public Keys UpKey
            {
                get { return _UpKey; }
                set
                {
                    if (_UpKey != value)
                        _NeedSave = true;
                    _UpKey = value;
                }
            }
            private Keys _UpKey = Keys.Up;

            /// <summary>Right Key</summary>
            public Keys RightKey
            {
                get { return _RightKey; }
                set
                {
                    if (_RightKey!= value)
                        _NeedSave = true;
                    _RightKey= value;
                }
            }
            private Keys _RightKey = Keys.Right;

            /// <summary>Down Key</summary>
            public Keys DownKey
            {
                get { return _DownKey; }
                set
                {
                    if (_DownKey != value)
                        _NeedSave = true;
                    _DownKey = value;
                }
            }
            private Keys _DownKey = Keys.Down;

            /// <summary>Left Key</summary>
            public Keys LeftKey
            {
                get { return _LeftKey; }
                set
                {
                    if (_LeftKey != value)
                        _NeedSave = true;
                    _LeftKey = value;
                }
            }
            private Keys _LeftKey = Keys.Left;

        }

        #region Default Settings

        /// <summary>Minimum resolution width (if none is set)</summary>
        public const int MinimumResolutionWidth = 1280;

        /// <summary>Minimum resolution height (if none is set)</summary>
        public const int MinimumResolutionHeight = 720;

        /// <summary>Filename used to store the game settings</summary>
        private const string _SettingsFilename = "config.xml";

        /// <summary>Default instance of the game settings</summary>
        public static GameSettings Default
        {
            get { return _DefaultInstance; }
        }
        private static GameSettings _DefaultInstance;

        /// <summary>Need to save the game settings file only if true</summary>
        private static bool _NeedSave = false;

        /// <summary>Whether device resolution changes need to be applied</summary>
        public static bool ResolutionChangesToApply = false;

        #endregion

        #region Initialization

        /// <summary>
        /// No public constructor! Create the game settings.
        /// </summary>
        private GameSettingsManager()
        { }

        /// <summary>
        /// Create game settings.  This constructor helps us to only load the
        /// GameSetting once, not again if GameSetting is recreated by
        /// the Deserialization process.
        /// </summary>
        public static void Initialize()
        {
            _DefaultInstance = new GameSettings();
            Load();
        }

        #endregion Initialization

        #region Load

        /// <summary>
        /// Load
        /// </summary>
        public static void Load()
        {
            _NeedSave = false;

            FileStream file = FileHelper.LoadGameContentFile(
                _SettingsFilename);

            if (file == null)
            {
                // We need a save, but wait to create the file after quitting.
                _NeedSave = true;
                return;
            }

            // If the file is empty, just create a new file with the default
            // settings.
            if (file.Length == 0)
            {
                // Close the file first.
                file.Close();

                // Check if there is a file in the game directory
                // to load the default game settings.
                file = FileHelper.LoadGameContentFile(_SettingsFilename);
                if (file != null)
                {
                    // Load everything into this class.
                    GameSettings loadedGameSetting =
                        (GameSettings)new XmlSerializer(typeof(GameSettings)).Deserialize(file);

                    if (loadedGameSetting != null)
                        _DefaultInstance = loadedGameSetting;

                    // Close the file.
                    file.Close();
                }

                // Save the user settings.
                _NeedSave = true;
                Save();
            }
            else
            {
                // Else load everything into this class with help of the
                // XmlSerializer.
                try
                {
                    GameSettings loadedGameSetting =
                        (GameSettings)new XmlSerializer(typeof(GameSettings)).Deserialize(file);

                    if (loadedGameSetting != null)
                        _DefaultInstance = loadedGameSetting;

                    // Close the file.
                    file.Close();
                }
                catch (System.InvalidOperationException)
                {
                    file.Close();

                    // Save the user settings.
                    _NeedSave = true;
                    Save();
                }
            }
        }

        #endregion

        #region Save

        /// <summary>
        /// Save
        /// </summary>
        public static void Save()
        {
            // No need to save if everything is up to date.
            if (!_NeedSave)
                return;

            _NeedSave = false;

            FileStream file = FileHelper.SaveGameContentFile(
                _SettingsFilename);

            // Save everything in this class with help from the XmlSerializer.
            new XmlSerializer(typeof(GameSettings)).Serialize(file, _DefaultInstance);

            // Close the file.
            file.Close();
        }

        /// <summary>
        /// Set the minimum graphics capabilities.
        /// </summary>
        public static void SetMinimumGraphics()
        {
            _DefaultInstance.ResolutionWidth = GameSettingsManager.MinimumResolutionWidth;
            _DefaultInstance.ResolutionHeight = GameSettingsManager.MinimumResolutionHeight;
            GameSettingsManager.Save();
        }

        #endregion
    }
}