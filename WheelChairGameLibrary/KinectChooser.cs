﻿//------------------------------------------------------------------------------
// <copyright file="KinectChooser.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace WheelChairGameLibrary
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.Kinect;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;


    /// <summary>
    /// This class will pick a kinect sensor if available.
    /// </summary>
    public class KinectChooser : GameObject
    {
        /// <summary>
        /// The status to string mapping.
        /// </summary>
        private readonly Dictionary<KinectStatus, string> statusMap = new Dictionary<KinectStatus, string>();

        /// <summary>
        /// The requested color image format.
        /// </summary>
        private readonly ColorImageFormat colorImageFormat;

        /// <summary>
        /// The requested depth image format.
        /// </summary>
        private readonly DepthImageFormat depthImageFormat;

        /// <summary>
        /// The chooser background texture.
        /// </summary>
        private Texture2D chooserBackground;

        /// <summary>
        /// The SpriteBatch used for rendering.
        /// </summary>
        private SpriteBatch spriteBatch;

        /// <summary>
        /// The font for rendering the state text.
        /// </summary>
        private SpriteFont font;

        /// <summary>
        /// Check if sensor is able to be used
        /// </summary>
        public bool IsAvailable
        {
            get
            {
                if (null == Sensor ||
                    false == Sensor.IsRunning ||
                    KinectStatus.Connected != Sensor.Status)
                    return false;
                else
                    return true;
            }
        }

        /// <summary>
        /// Initializes a new instance of the KinectChooser class.
        /// </summary>
        /// <param name="game">The related game object.</param>
        /// <param name="colorFormat">The desired color image format.</param>
        /// <param name="depthFormat">The desired depth image format.</param>
        public KinectChooser(GameEnhanced game, ColorImageFormat colorFormat, DepthImageFormat depthFormat)
            : base(game, "kinect")
        {
            this.colorImageFormat = colorFormat;
            this.depthImageFormat = depthFormat;

            KinectSensor.KinectSensors.StatusChanged += this.KinectSensors_StatusChanged;
            this.DiscoverSensor();

            this.statusMap.Add(KinectStatus.Connected, string.Empty);
            this.statusMap.Add(KinectStatus.DeviceNotGenuine, "Device Not Genuine");
            this.statusMap.Add(KinectStatus.DeviceNotSupported, "Device Not Supported");
            this.statusMap.Add(KinectStatus.Disconnected, "Required");
            this.statusMap.Add(KinectStatus.Error, "Error");
            this.statusMap.Add(KinectStatus.Initializing, "Initializing...");
            this.statusMap.Add(KinectStatus.InsufficientBandwidth, "Insufficient Bandwidth");
            this.statusMap.Add(KinectStatus.NotPowered, "Not Powered");
            this.statusMap.Add(KinectStatus.NotReady, "Not Ready");
        }

        /// <summary>
        /// Gets the selected KinectSensor.
        /// </summary>
        public KinectSensor Sensor { get; private set; }

        /// <summary>
        /// Gets the last known status of the KinectSensor.
        /// </summary>
        public KinectStatus LastStatus { get; private set; }

        /// <summary>
        /// This method initializes necessary objects.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            this.spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }

        /// <summary>
        /// This method renders the current state of the KinectChooser.
        /// </summary>
        /// <param name="gameTime">The elapsed game time.</param>
        public override void Draw(GameTime gameTime)
        {
            //skip rendering for now
            return;

            // If the spritebatch is null, call initialize
            if (this.spriteBatch == null)
            {
                this.Initialize();
            }

            // If the background is not loaded, load it now
            if (this.chooserBackground == null)
            {
                this.LoadContent();
            }

            // If we don't have a sensor, or the sensor we have is not connected
            // then we will display the information text
            if (this.Sensor == null || this.LastStatus != KinectStatus.Connected)
            {
                this.spriteBatch.Begin();

                // Render the background
                this.spriteBatch.Draw(
                    this.chooserBackground,
                    new Vector2(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height / 2),
                    null,
                    Color.White,
                    0,
                    new Vector2(this.chooserBackground.Width / 2, this.chooserBackground.Height / 2),
                    1,
                    SpriteEffects.None,
                    0);

                // Determine the text
                string txt = "Kinect Required";
                if (this.Sensor != null)
                {
                    txt = this.statusMap[this.LastStatus];
                }

                // Render the text
                Vector2 size = this.font.MeasureString(txt);
                this.spriteBatch.DrawString(
                    this.font,
                    txt,
                    new Vector2((Game.GraphicsDevice.Viewport.Width - size.X) / 2, (Game.GraphicsDevice.Viewport.Height / 2) + size.Y),
                    Color.White);
                this.spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        /// <summary>
        /// This method loads the textures and fonts.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            //TODO: change textures
            //this.chooserBackground = Game.Content.Load<Texture2D>("ChooserBackground");
            this.chooserBackground = Game.WhitePixel;
            //this.font = Game.Content.Load<SpriteFont>("Segoe16");
            this.font = Game.DefaultFont;
        }

        /// <summary>
        /// This method ensures that the KinectSensor is stopped before exiting.
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();

            // Always stop the sensor when closing down
            if (this.Sensor != null)
            {
                this.Sensor.Stop();
            }
        }

        /// <summary>
        /// This method will use basic logic to try to grab a sensor.
        /// Once a sensor is found, it will start the sensor with the
        /// requested options.
        /// </summary>
        private void DiscoverSensor()
        {
            // Grab any available sensor
            this.Sensor = KinectSensor.KinectSensors.FirstOrDefault();

            if (this.Sensor != null)
            {
                this.LastStatus = this.Sensor.Status;

                // If this sensor is connected, then enable it
                if (this.LastStatus == KinectStatus.Connected)
                {
                    try
                    {
                        this.Sensor.SkeletonStream.Enable();
                        this.Sensor.ColorStream.Enable(this.colorImageFormat);
                        this.Sensor.DepthStream.Enable(this.depthImageFormat);

                        try
                        {
                            this.Sensor.Start();
                        }
                        catch (IOException)
                        {
                            // sensor is in use by another application
                            // will treat as disconnected for display purposes
                            this.Sensor = null;
                        }
                    }
                    catch (InvalidOperationException)
                    {
                        // KinectSensor might enter an invalid state while
                        // enabling/disabling streams or stream features.
                        // E.g.: sensor might be abruptly unplugged.
                        this.Sensor = null;
                    }
                }
            }
            else
            {
                this.LastStatus = KinectStatus.Disconnected;
            }
        }

        /// <summary>
        /// This wires up the status changed event to monitor for 
        /// Kinect state changes.  It automatically stops the sensor
        /// if the device is no longer available.
        /// </summary>
        /// <param name="sender">The sending object.</param>
        /// <param name="e">The event args.</param>
        private void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            // If the status is not connected, try to stop it
            if (e.Status != KinectStatus.Connected)
            {
                e.Sensor.Stop();
            }

            this.LastStatus = e.Status;
            this.DiscoverSensor();
        }
    }
}
